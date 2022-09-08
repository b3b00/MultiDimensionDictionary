

using System.Collections.Concurrent;
using System.Collections.Generic;
using multiDimensionalDictionary;

namespace  multiDimensionalDictionary {


public class MultiDimensionalDictionary<K1, K2, K3, K4, K5, V>
    {
        protected ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, K3, K4, K5, V>> Data{ get;set; }

        public MultiDimensionalDictionary() : base()
        {
            Data = new ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, K3, K4, K5, V>>();
        }

#region ContainsKey
public bool ContainsKey(K1 k1) => Data.ContainsKey(k1);

public bool ContainsKey(K1 k1, K2 k2) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2);

public bool ContainsKey(K1 k1, K2 k2, K3 k3) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5);


#endregion

#region getKeys
public List<(K1, K2, K3, K4, K5)> GetKeys()
        {
            var keys = new List<(K1, K2, K3, K4, K5)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3, K4, K5)> subkeys = kvp.Value.GetKeys();
                foreach (var subkey in subkeys)
                {
        keys.Add((kvp.Key, subkey.Item1, subkey.Item2, subkey.Item3, subkey.Item4));}
            }
            return keys;
}
#endregion

#region Put
public void Put(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, V value)
        {

            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

DictionaryAssertions.AssertNotNull<K5>(k5, "k5");

DictionaryAssertions.AssertNotNull<V>(value, "value");



            var secondDimentionData = new MultiDimensionalDictionary<K2, K3, K4, K5, V>();

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1];
            }

            secondDimentionData.Put(k2, k3, k4, k5, value);

            Data[k1] = secondDimentionData;
        }
#endregion

#region Get
public MultiDimensionalDictionary<K2, K3, K4, K5, V> Get(K1 k1)
            {
                return Data[k1];
            }

public MultiDimensionalDictionary<K3, K4, K5, V> Get(K1 k1, K2 k2)
            {
                return Data[k1].Get(k2);
            }

public MultiDimensionalDictionary<K4, K5, V> Get(K1 k1, K2 k2, K3 k3)
            {
                return Data[k1].Get(k2, k3);
            }

public MultiDimensionalDictionary<K5, V> Get(K1 k1, K2 k2, K3 k3, K4 k4)
            {
                return Data[k1].Get(k2, k3, k4);
            }

public V Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5)
        {
DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

DictionaryAssertions.AssertNotNull<K5>(k5, "k5");




            if (Data.TryGetValue(k1, out MultiDimensionalDictionary<K2, K3, K4, K5, V> secondDimensionData))
            {
                return secondDimensionData.Get(k2, k3, k4, k5);
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


public void Remove(K1 k1, K2 k2, K3 k3)
{
Data[k1].Get(k2).Remove(k3);
}


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4)
{
Data[k1].Get(k2).Get(k3).Remove(k4);
}


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5)
{
Data[k1].Get(k2).Get(k3).Get(k4).Remove(k5);
}



#endregion
}

}
