

using System.Collections.Concurrent;
using System.Collections.Generic;
using multiDimensionalDictionary;

namespace  multiDimensionalDictionary {


public class MultiDimensionalDictionary<K1, K2, V>
    {
        protected ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, V>> Data{ get;set; }

        public MultiDimensionalDictionary() : base()
        {
            Data = new ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, V>>();
        }

#region ContainsKey
public bool ContainsKey(K1 k1) => Data.ContainsKey(k1);

public bool ContainsKey(K1 k1, K2 k2) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2);


#endregion

#region getKeys
public List<(K1, K2)> GetKeys()
        {
            var keys = new List<(K1, K2)>();
            foreach (var kvp in Data)
            {
                List<K2> subkeys = kvp.Value.GetKeys();
                foreach (var subkey in subkeys)
                {
        keys.Add((kvp.Key, subkey));}
            }
            return keys;
}
#endregion

#region Put
public void Put(K1 k1, K2 k2, V value)
        {

            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

DictionaryAssertions.AssertNotNull<V>(value, "value");



            var secondDimentionData = new MultiDimensionalDictionary<K2, V>();

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1];
            }

            secondDimentionData.Put(k2, value);

            Data[k1] = secondDimentionData;
        }
#endregion

#region Get
public MultiDimensionalDictionary<K2, V> Get(K1 k1)
            {
                return Data[k1];
            }

public V Get(K1 k1, K2 k2)
        {
DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

DictionaryAssertions.AssertNotNull<K2>(k2, "k2");




            if (Data.TryGetValue(k1, out MultiDimensionalDictionary<K2, V> secondDimensionData))
            {
                return secondDimensionData.Get(k2);
            }

            throw new KeyNotFoundException();
                }


#endregion
#region Remove
public void Remove(K1 k1)
    {
        Data.Remove(k1, out var ignore);
    }

public void Remove(K1 k1, K2 k2)
{
Data[k1].Remove(k2);
}



#endregion
}

}
