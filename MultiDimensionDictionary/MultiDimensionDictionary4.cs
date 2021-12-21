using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{
    public class MultiDimensionalDictionary<K1, K2, K3, K4, V> : MultiDimensionalDictionary<K1, K2, MultiDimensionalDictionary<K3, K4, V>>
    {
        protected new ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, K3, K4, V>> Data { get; set; }

        public MultiDimensionalDictionary() : base()
        {
            Data = new ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, K3, K4, V>>();
        }

        public new bool ContainsKey(K1 k1) => Data.ContainsKey(k1);

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4);
        public new List<(K1, K2, K3, K4)> GetKeys()
        {
            List<(K1, K2, K3, K4)> keys = new List<(K1, K2, K3, K4)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3, K4)> subkeys = kvp.Value.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2, subkey.Item3));
                }
            }
            return keys;
        }

        public void Put(K1 k1, K2 k2, K3 k3, K4 k4, V value)
        {
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

            DictionaryAssertions.AssertNotNull<V>(value, "value");

            var secondDimentionData = new MultiDimensionalDictionary<K2, K3, K4, V>();

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1];
            }

            secondDimentionData.Put(k2, k3, k4, value); ;
            Data[k1] = secondDimentionData;
        }

        public new MultiDimensionalDictionary<K2, K3, K4, V> Get(K1 k1)
        {
            return Data[k1];
        }

        public new MultiDimensionalDictionary<K3, K4, V> Get(K1 k1, K2 k2)
        {
            return Data[k1].Get(k2);
        }

        public MultiDimensionalDictionary<K4, V> Get(K1 k1, K2 k2, K3 k3)
        {
            return Data[k1].Get(k2).Get(k3);
        }

        public V Get(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

            if (Data.TryGetValue(k1, out MultiDimensionalDictionary<K2, K3, K4, V> secondDimensionData))
            {
                return secondDimensionData.Get(k2, k3, k4);
            }

            throw new KeyNotFoundException();
        }

        public new void Remove(K1 k1)
        {
            MultiDimensionalDictionary<K2, K3, K4, V> ignore = null;
            Data.Remove(k1, out ignore);
        }

        public void Remove(K1 k1, K2 k2)
        {
            Data[k1].Remove(k2);
        }

        public void Remove(K1 k1, K2 k2, K3 k3)
        {
            Data[k1].Get(k2).Remove(k3);
        }

        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Data[k1].Get(k2).Get(k3).Remove(k4);
        }

        public void Clear()
        {
            Data.Clear();
        }
    }

}