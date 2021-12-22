using System.Collections.Generic;

namespace multiDimensionalDictionary
{
    public interface IMultiDimensionalDictionary<K1, K2, V>
    {
        bool ContainsKey(K1 k1); 
        bool ContainsKey(K1 k1, K2 k2);
        List<(K1, K2)> GetKeys();
        void Put(K1 k1, K2 k2, V value);
        MultiDimensionalDictionary<K2, V> Get(K1 k1);
        V Get(K1 k1, K2 k2);
        void Clear();
    }
}