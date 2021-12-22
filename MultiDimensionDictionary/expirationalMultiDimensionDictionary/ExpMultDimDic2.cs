using System;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{

    
    
    public class ExpMultDimDic<K1,K2,V> : IMultiDimensionalDictionary<K1,K2,V>
    {

        private MultiDimensionalDictionary<K1, DatedItem<K2>, DatedItem<V>> Data { get; set; } 
        
        TimeSpan ExpirationSpan1;
        TimeSpan ExpirationSpan2;
        
        public ExtMultDimDic(TimeSpan expirationSpan1, TimeSpan expirationSpan2)
        {
            ExpirationSpan1 = expirationSpan1;
            ExpirationSpan2 = expirationSpan2;
            Data = new MultiDimensionalDictionary<K1, DatedItem<K2>, DatedItem<V>>();
        }

        
        public void Invalidate()
        {
            DateTime now = DateTime.Now;
            List<K1> toRemove = new List<K1>();
            foreach (var key in Data.GetKeys())
            {
                var value = Data.Get(key.Item1);
                if (value.Age > ExpirationSpan1)
                {
                    toRemove.Add(key);
                }
            }

            if (toRemove.Any())
            {
                foreach (var k1 in toRemove)
                {
                    Data.Remove(k1);
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
            return Data.ContainsKey(k1) && Data.Get(k1).Data.ContainsKey(k2);
        }

        public List<(K1, K2)> GetKeys()
        {
            Invalidate();
            List<(K1, K2)> keys = new List<(K1, K2)>();
            var firstLevelKeys = Data.GetKeys();
            foreach (var k1 in firstLevelKeys)
            {
                var subkeys = Get(k1).GetKeys();
                foreach (var k2 in subkeys)
                {
                    keys.Add((k1,k2));
                }
            }

            return keys;
        }

        public void Put(K1 k1, K2 k2, V value)
        {
            Invalidate();
            Data.Put(k1,k2,new DatedItem<V>(value));
        }

        public MultiDimensionalDictionary<K2, V> Get(K1 k1)
        {
            throw new NotImplementedException();
        }

        public V Get(K1 k1, K2 k2)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
    
  
}