using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{
    public class ExpirationalMultiDimensionDictionary<K1, K2, V>
    {
        protected ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, V> subData)> Data { get; set; }

        TimeSpan ExpirationSpan1;
        TimeSpan ExpirationSpan2;
        
        public ExpirationalMultiDimensionDictionary(TimeSpan expirationSpan1, TimeSpan expirationSpan2)
        {
            ExpirationSpan1 = expirationSpan1;
            ExpirationSpan2 = expirationSpan2;
            Data = new ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, V> subData)>();
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


        public bool ContainsKey(K1 k1)
        {
            Invalidate();
            return Data.ContainsKey(k1);
        }

        public bool ContainsKey(K1 k1, K2 k2)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2);
        }
        

        public List<(K1, K2)> GetKeys()
        {
            List<(K1, K2)> keys = new List<(K1, K2)>();
            foreach (var kvp in Data)
            {
                List<K2> subkeys = kvp.Value.subData.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey));
                }
            }
            return keys;
        }

        public void Put(K1 k1, K2 k2, V value)
        {
            
            Invalidate();
            
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<V>(value, "value");

            ExpirationalMultiDimensionDictionary<K2, V> secondDimentionData = new ExpirationalMultiDimensionDictionary<K2, V>(ExpirationSpan2);



            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1].subData;
            }

            if (secondDimentionData.ContainsKey(k2))
            {
                throw new ArgumentException($"dictionary already contains key {k1};{k2}");
            }
            else
            {
                secondDimentionData.Put(k2, value);
            }

            Data[k1] = (DateTime.Now, secondDimentionData);
        }

        public ExpirationalMultiDimensionDictionary<K2, V> Get(K1 k1)
        {
            Invalidate();
            return Data[k1].subData;
        }

        public V Get(K1 k1, K2 k2)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
            
            if (Data.TryGetValue(k1, out (DateTime date, ExpirationalMultiDimensionDictionary<K2, V> subData) secondDimensionData ))
            {

                if (secondDimensionData.subData.ContainsKey(k2))
                {
                    var value = secondDimensionData.subData.Get(k2);
                    return value;
                }
            }

            throw new KeyNotFoundException();
        }

        public void Remove(K1 k1)
        {
            Invalidate();
            _Remove(k1);
        }

        public void _Remove(K1 k1)
        {
            (DateTime date, ExpirationalMultiDimensionDictionary<K2, V> subData) ignore = default;
            Data.TryRemove(k1, out ignore);
        }

        public void Remove(K1 k1, K2 k2)
        {
            Invalidate();
            Data[k1].subData.Remove(k2);
        }

        public void Clear()
        {
            Data.Clear();
        }
    }
}