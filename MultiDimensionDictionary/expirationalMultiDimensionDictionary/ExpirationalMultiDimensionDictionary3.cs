using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{
    public class ExpirationalMultiDimensionDictionary<K1, K2, K3, V>
    {
    
        TimeSpan ExpirationSpan1;
        TimeSpan ExpirationSpan2;
        TimeSpan ExpirationSpan3;
        
        protected ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> data)> Data { get; set; }
    
        public ExpirationalMultiDimensionDictionary(TimeSpan expirationSpan1, TimeSpan expirationSpan2, TimeSpan expirationSpan3) : base()
        {
            ExpirationSpan1 = expirationSpan1;
            ExpirationSpan2 = expirationSpan2;
            ExpirationSpan3 = expirationSpan3;
            Data = new ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> data)>();
        }

        public ExpirationalMultiDimensionDictionary(long expiration1MMillis, long expiration2Millis,
            long expiration3Millis) : this (TimeSpan.FromMilliseconds(expiration1MMillis), TimeSpan.FromMilliseconds(expiration2Millis), TimeSpan.FromMilliseconds(expiration3Millis))
        {
            
        }
        
        public void Invalidate()
        {
            DateTime now = DateTime.Now;
            List<K1> toRemove = new List<K1>();
            foreach (var entry in Data)
            {
                var age = now - entry.Value.date;
                if (age > ExpirationSpan1)
                {
                    toRemove.Add(entry.Key);
                }
            }

            if (toRemove.Any())
            {
                foreach (var k1 in toRemove)
                {
                    _Remove(k1);
                }
            }
        }
        
        public void _Remove(K1 k1)
        {
            (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> data) ignore;
            Data.TryRemove(k1, out ignore);
        }

        public bool ContainsKey(K1 k1)
        {
            Invalidate();
            return Data.ContainsKey(k1);
        }
        
        public bool ContainsKey(K1 k1, K2 k2)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].data.ContainsKey(k2);
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].data.ContainsKey(k2, k3);
        }

        public List<(K1, K2, K3)> GetKeys()
        {
            Invalidate();
            List<(K1, K2, K3)> keys = new List<(K1, K2, K3)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3)> subkeys = kvp.Value.data.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2));
                }
            }
            return keys;
        }
    
        public void Put(K1 k1, K2 k2, K3 k3, V value)
        {
            Invalidate();
            
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");
    
            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
    
            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");
    
            DictionaryAssertions.AssertNotNull<V>(value, "value");
    
            var secondDimentionData = new ExpirationalMultiDimensionDictionary<K2, K3, V>(ExpirationSpan2,ExpirationSpan3);
    
    
    
            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1].data;
            }
    
            secondDimentionData.Put(k2, k3, value);
    
            Data[k1] = (DateTime.Now, secondDimentionData);
        }
    
        public ExpirationalMultiDimensionDictionary<K2, K3, V> Get(K1 k1)
        {
            Invalidate();
            return Data[k1].data;
        }
    
        public ExpirationalMultiDimensionDictionary<K3, V> Get(K1 k1, K2 k2)
        {
            Invalidate();
            return Data[k1].data.Get(k2);
        }
    
        public V Get(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");
            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

            if (Data.TryGetValue(k1, out (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> data) secondDimensionData))
            {
                return secondDimensionData.data.Get(k2, k3);
            }
            throw new KeyNotFoundException();
        }

        public void Remove(K1 k1)
        {
            Invalidate();
            _Remove(k1);
        }
    
        public void Remove(K1 k1, K2 k2)
        {
            Invalidate();
            Data[k1].data.Remove(k2);
        }
    
        public void Remove(K1 k1, K2 k2, K3 k3)
        {
            Data[k1].data.Get(k2).Remove(k3);
        }
    
        public void Clear()
        {
            Data.Clear();
        }
    }

}