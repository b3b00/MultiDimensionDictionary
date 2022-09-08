using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{
    public class MultiDimensionalDictionary<K1, V>
    {
        protected ConcurrentDictionary<K1, V> Data { get; set; }

        public MultiDimensionalDictionary()
        {
            Data = new ConcurrentDictionary<K1, V>();
        }

        public bool ContainsKey(K1 k1) => Data.ContainsKey(k1);

        public List<K1> GetKeys() => Data.Keys.ToList<K1>();

        public void Put(K1 k1, V value)
        {
            Data[k1] = value;
        }

        public V Get(K1 k1)
        {
            return Data[k1];
        }

        public void Remove(K1 k1)
        {
            Data.Remove(k1, out V ignore);
        }

        public void Clear()
        {
            Data.Clear();
        }
    }

}