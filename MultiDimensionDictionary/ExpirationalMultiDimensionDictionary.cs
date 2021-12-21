using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{
    public class ExpirationalMultiDimensionDictionary<K1, V>
    {

        private TimeSpan ExpirationSpan;
        protected ConcurrentDictionary<K1, (DateTime date,V value)> Data { get; set; }

        public ExpirationalMultiDimensionDictionary(TimeSpan expirationSpan)
        {
            ExpirationSpan = expirationSpan;
            Data = new ConcurrentDictionary<K1, (DateTime,V)>();
        }

        public void Invalidate()
        {
            DateTime now = DateTime.Now;
            List<K1> toRemove = new List<K1>();
            foreach (var entry in Data)
            {
                var age = now - entry.Value.date;
                if (age > ExpirationSpan)
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

        public List<K1> GetKeys()
        {
            Invalidate();
            return Data.Keys.ToList<K1>();
        }

        public void Put(K1 k1, V value)
        {
            Invalidate();
            Data[k1] = (DateTime.Now, value);
        }

        public V Get(K1 k1)
        {
            Invalidate();
            return Data[k1].value;
        }

        public void Remove(K1 k1)
        {
            Invalidate();
            _Remove(k1);
        }
        
        public void _Remove(K1 k1)
        {
            (DateTime,V) ignore;
            Data.TryRemove(k1, out ignore);
        }

        public void Clear()
        {
            Data.Clear();
        }


    }




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

    public class ExpirationalMultiDimensionDictionary<K1, K2, K3, K4, V> //: ExpirationalMultiDimensionDictionary<K1, K2, ExpirationalMultiDimensionDictionary<K3, K4, V>>
    {
        protected ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, K4, V> data)>  Data { get; set; }
    
        TimeSpan ExpirationSpan1;
        TimeSpan ExpirationSpan2;
        TimeSpan ExpirationSpan3;
        TimeSpan ExpirationSpan4;
        
        public ExpirationalMultiDimensionDictionary(TimeSpan expirationSpan1, TimeSpan expirationSpan2, TimeSpan expirationSpan3, TimeSpan expirationSpan4) 
        {
            Data = new ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, K4, V> data)>();
            ExpirationSpan1 = expirationSpan1;
            ExpirationSpan2 = expirationSpan2;
            ExpirationSpan3 = expirationSpan3;
            ExpirationSpan4 = expirationSpan4;
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

        public new bool ContainsKey(K1 k1)
        {
            Invalidate();
          return Data.ContainsKey(k1);  
        } 
        
        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            bool containsK1 = Data.ContainsKey(k1);
            bool containsChildren = Data[k1].data.ContainsKey(k2, k3, k4); 
            return  containsK1 && containsChildren;
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            var containsKey = Data.ContainsKey(k1);
            var containsChildren = Data[k1].data.ContainsKey(k2, k3);
            return containsKey && containsChildren;
        }

        public bool ContainsKey(K1 k1, K2 k2)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].data.ContainsKey(k2);
        }
    
    
        public List<(K1, K2, K3, K4)> GetKeys()
        {
            Invalidate();
            List<(K1, K2, K3, K4)> keys = new List<(K1, K2, K3, K4)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3, K4)> subkeys = kvp.Value.data.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2, subkey.Item3));
                }
            }
            return keys;
        }
    
        public void Put(K1 k1, K2 k2, K3 k3, K4 k4, V value)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");
    
            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
    
            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");
    
            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");
    
            DictionaryAssertions.AssertNotNull<V>(value, "value");
    
            var secondDimentionData = new ExpirationalMultiDimensionDictionary<K2, K3, K4, V>(ExpirationSpan2,ExpirationSpan3,ExpirationSpan4);
    
            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1].data;
            }
    
            secondDimentionData.Put(k2, k3, k4, value); ;
    
            Data[k1] = (DateTime.Now, secondDimentionData);
        }
    
    
        public ExpirationalMultiDimensionDictionary<K2, K3, K4, V> Get(K1 k1)
        {
            Invalidate();
            return Data[k1].data;
        }
    
        public ExpirationalMultiDimensionDictionary<K3, K4, V> Get(K1 k1, K2 k2)
        {
            Invalidate();
            return Data[k1].data.Get(k2);
        }
    
        public ExpirationalMultiDimensionDictionary<K4, V> Get(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            return Data[k1].data.Get(k2).Get(k3);
        }
    
        public V Get(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");
            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");
            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");
    
            (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, K4, V> data) secondDimensionData;
            if (Data.TryGetValue(k1, out secondDimensionData))
            {
                return secondDimensionData.data.Get(k2, k3, k4);
            }
    
            throw new KeyNotFoundException();
        }
    
        public  void Remove(K1 k1)
        {
            Invalidate();
            _Remove(k1);
        }
        
        public void _Remove(K1 k1)
        {
            (DateTime date, ExpirationalMultiDimensionDictionary<K2,K3,K4, V> subData) ignore = default;
            Data.TryRemove(k1, out ignore);
        }
    
        public void Remove(K1 k1, K2 k2)
        {
            Invalidate();
            Data[k1].data.Remove(k2);
        }
    
        public void Remove(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            Data[k1].data.Get(k2).Remove(k3);
        }
    
        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            Data[k1].data.Get(k2).Get(k3).Remove(k4);
        }
        
        public void Clear()
        {
            Data.Clear();
        }
    }


    public class ExpirationalMultiDimensionDictionary<K1, K2, K3, K4, K5, V> 
    {
        protected ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, V> data)>  Data { get; set; }
        TimeSpan ExpirationSpan1;
        TimeSpan ExpirationSpan2;
        TimeSpan ExpirationSpan3;
        TimeSpan ExpirationSpan4;
        TimeSpan ExpirationSpan5;
        
        public ExpirationalMultiDimensionDictionary(TimeSpan expirationSpan1, TimeSpan expirationSpan2, TimeSpan expirationSpan3, TimeSpan expirationSpan4, TimeSpan expirationSpan5) 
        {
            Data = new ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, V> data)>();
            ExpirationSpan1 = expirationSpan1;
            ExpirationSpan2 = expirationSpan2;
            ExpirationSpan3 = expirationSpan3;
            ExpirationSpan4 = expirationSpan4;
            ExpirationSpan5 = expirationSpan5;
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
        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5) {
            Invalidate();
            bool containsK1 = Data.ContainsKey(k1);
            bool containsChildren = Data[k1].data.ContainsKey(k2, k3, k4, k5);
            return  containsK1 && containsChildren;
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            bool containsK1 = Data.ContainsKey(k1);
            bool containsChildren = Data[k1].data.ContainsKey(k2, k3, k4); 
            return  containsK1 && containsChildren;
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            var containsKey = Data.ContainsKey(k1);
            var containsChildren = Data[k1].data.ContainsKey(k2, k3);
            return containsKey && containsChildren;
        }

        public bool ContainsKey(K1 k1, K2 k2)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].data.ContainsKey(k2);
        }
    
    
        public List<(K1, K2, K3, K4, K5)> GetKeys()
        {
            Invalidate();
            List<(K1, K2, K3, K4, K5)> keys = new List<(K1, K2, K3, K4, K5)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3, K4, K5)> subkeys = kvp.Value.data.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2, subkey.Item3, subkey.Item4));
                }
            }
            return keys;
        }
    
        public void Put(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, V value)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");
            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");
            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");
            DictionaryAssertions.AssertNotNull<K5>(k5, "k5");
            DictionaryAssertions.AssertNotNull<V>(value, "value");

            var secondDimentionData = new ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, V>(ExpirationSpan2,ExpirationSpan3,ExpirationSpan4, ExpirationSpan5);

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1].data;
            }
            secondDimentionData.Put(k2, k3, k4, k5, value); ;
            Data[k1] = (DateTime.Now, secondDimentionData);
        }
        public ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, V> Get(K1 k1)
        {
            Invalidate();
            return Data[k1].data;
        }

        public ExpirationalMultiDimensionDictionary<K3, K4, K5, V> Get(K1 k1, K2 k2)
        {
            Invalidate();
            return Data[k1].data.Get(k2);
        }

        public ExpirationalMultiDimensionDictionary<K4, K5, V> Get(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");
            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");
            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");
            return Data[k1].data.Get(k2).Get(k3);
        }
        
        public ExpirationalMultiDimensionDictionary<K5, V> Get(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            return Data[k1].data.Get(k2).Get(k3).Get(k4);
        }
        public void Remove(K1 k1)
        {
            Invalidate();
            _Remove(k1);
        }
        
        public void _Remove(K1 k1)
        {
            (DateTime date, ExpirationalMultiDimensionDictionary<K2,K3,K4,K5, V> subData) ignore = default;
            Data.TryRemove(k1, out ignore);
        }
    
        public void Remove(K1 k1, K2 k2)
        {
            Invalidate();
            Data[k1].data.Remove(k2);
        }
    
        public void Remove(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            Data[k1].data.Get(k2).Remove(k3);
        }
        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            Data[k1].data.Get(k2).Get(k3).Remove(k4);
        }
        public void Clear()
        {
            Data.Clear();
        }
    }
}