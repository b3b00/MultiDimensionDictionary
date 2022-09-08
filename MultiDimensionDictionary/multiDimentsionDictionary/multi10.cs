

using System.Collections.Concurrent;
using System.Collections.Generic;
using multiDimensionalDictionary;

namespace  multiDimensionalDictionary {


public class MultiDimensionalDictionary<K1, K2, K3, K4, K5, K6, K7, K8, K9, K10, V>
    {
        protected ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, K3, K4, K5, K6, K7, K8, K9, K10, V>> Data{ get;set; }

        public MultiDimensionalDictionary() : base()
        {
            Data = new ConcurrentDictionary<K1, MultiDimensionalDictionary<K2, K3, K4, K5, K6, K7, K8, K9, K10, V>>();
        }

#region ContainsKey
public bool ContainsKey(K1 k1) => Data.ContainsKey(k1);

public bool ContainsKey(K1 k1, K2 k2) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2);

public bool ContainsKey(K1 k1, K2 k2, K3 k3) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5, k6);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5, k6, k7);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5, k6, k7, k8);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5, k6, k7, k8, k9);

public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9, K10 k10) => Data.ContainsKey(k1) && Data[k1].ContainsKey(k2, k3, k4, k5, k6, k7, k8, k9, k10);


#endregion

#region getKeys
public List<(K1, K2, K3, K4, K5, K6, K7, K8, K9, K10)> GetKeys()
        {
            var keys = new List<(K1, K2, K3, K4, K5, K6, K7, K8, K9, K10)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3, K4, K5, K6, K7, K8, K9, K10)> subkeys = kvp.Value.GetKeys();
                foreach (var subkey in subkeys)
                {
        keys.Add((kvp.Key, subkey.Item1, subkey.Item2, subkey.Item3, subkey.Item4, subkey.Item5, subkey.Item6, subkey.Item7, subkey.Item8, subkey.Item9));}
            }
            return keys;
}
#endregion

#region Put
public void Put(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9, K10 k10, V value)
        {

            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

DictionaryAssertions.AssertNotNull<K5>(k5, "k5");

DictionaryAssertions.AssertNotNull<K6>(k6, "k6");

DictionaryAssertions.AssertNotNull<K7>(k7, "k7");

DictionaryAssertions.AssertNotNull<K8>(k8, "k8");

DictionaryAssertions.AssertNotNull<K9>(k9, "k9");

DictionaryAssertions.AssertNotNull<K10>(k10, "k10");

DictionaryAssertions.AssertNotNull<V>(value, "value");



            var secondDimentionData = new MultiDimensionalDictionary<K2, K3, K4, K5, K6, K7, K8, K9, K10, V>();

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1];
            }

            secondDimentionData.Put(k2, k3, k4, k5, k6, k7, k8, k9, k10, value);

            Data[k1] = secondDimentionData;
        }
#endregion

#region Get
public MultiDimensionalDictionary<K2, K3, K4, K5, K6, K7, K8, K9, K10, V> Get(K1 k1)
            {
                return Data[k1];
            }

public MultiDimensionalDictionary<K3, K4, K5, K6, K7, K8, K9, K10, V> Get(K1 k1, K2 k2)
            {
                return Data[k1].Get(k2);
            }

public MultiDimensionalDictionary<K4, K5, K6, K7, K8, K9, K10, V> Get(K1 k1, K2 k2, K3 k3)
            {
                return Data[k1].Get(k2, k3);
            }

public MultiDimensionalDictionary<K5, K6, K7, K8, K9, K10, V> Get(K1 k1, K2 k2, K3 k3, K4 k4)
            {
                return Data[k1].Get(k2, k3, k4);
            }

public MultiDimensionalDictionary<K6, K7, K8, K9, K10, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5)
            {
                return Data[k1].Get(k2, k3, k4, k5);
            }

public MultiDimensionalDictionary<K7, K8, K9, K10, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6)
            {
                return Data[k1].Get(k2, k3, k4, k5, k6);
            }

public MultiDimensionalDictionary<K8, K9, K10, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7)
            {
                return Data[k1].Get(k2, k3, k4, k5, k6, k7);
            }

public MultiDimensionalDictionary<K9, K10, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8)
            {
                return Data[k1].Get(k2, k3, k4, k5, k6, k7, k8);
            }

public MultiDimensionalDictionary<K10, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9)
            {
                return Data[k1].Get(k2, k3, k4, k5, k6, k7, k8, k9);
            }

public V Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9, K10 k10)
        {
DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

DictionaryAssertions.AssertNotNull<K5>(k5, "k5");

DictionaryAssertions.AssertNotNull<K6>(k6, "k6");

DictionaryAssertions.AssertNotNull<K7>(k7, "k7");

DictionaryAssertions.AssertNotNull<K8>(k8, "k8");

DictionaryAssertions.AssertNotNull<K9>(k9, "k9");

DictionaryAssertions.AssertNotNull<K10>(k10, "k10");




            if (Data.TryGetValue(k1, out MultiDimensionalDictionary<K2, K3, K4, K5, K6, K7, K8, K9, K10, V> secondDimensionData))
            {
                return secondDimensionData.Get(k2, k3, k4, k5, k6, k7, k8, k9, k10);
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


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6)
{
Data[k1].Get(k2).Get(k3).Get(k4).Get(k5).Remove(k6);
}


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7)
{
Data[k1].Get(k2).Get(k3).Get(k4).Get(k5).Get(k6).Remove(k7);
}


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8)
{
Data[k1].Get(k2).Get(k3).Get(k4).Get(k5).Get(k6).Get(k7).Remove(k8);
}


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9)
{
Data[k1].Get(k2).Get(k3).Get(k4).Get(k5).Get(k6).Get(k7).Get(k8).Remove(k9);
}


public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, K9 k9, K10 k10)
{
Data[k1].Get(k2).Get(k3).Get(k4).Get(k5).Get(k6).Get(k7).Get(k8).Get(k9).Remove(k10);
}



#endregion
}

}
