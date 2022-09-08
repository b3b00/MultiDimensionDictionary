

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



        public ExpirationalMultiDimensionDictionary(TimeSpan expiration1, TimeSpan expiration2, TimeSpan expiration3)
        {
            ExpirationSpan1 = expiration1;
            ExpirationSpan2 = expiration2;
            ExpirationSpan3 = expiration3;
            Data =
                new ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> subData
                    )>();
        }

        public ExpirationalMultiDimensionDictionary(long expiration1, long expiration2, long expiration3) : this(
            TimeSpan.FromMilliseconds(expiration1), TimeSpan.FromMilliseconds(expiration2),
            TimeSpan.FromMilliseconds(expiration3))
        {
        }

        protected ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> subData)>
            Data { get; set; }

        public ExpirationalMultiDimensionDictionary() : base()
        {
            Data =
                new ConcurrentDictionary<K1, (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> subData
                    )>();
        }




        #region ContainsKey

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

        public bool ContainsKey(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2, k3);
        }


        #endregion

        #region getKeys

        public List<(K1, K2, K3)> GetKeys()
        {
            Invalidate();
            var keys = new List<(K1, K2, K3)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3)> subkeys = kvp.Value.subData.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2));
                }
            }

            return keys;
        }

        #endregion

        #region Put

        public void Put(K1 k1, K2 k2, K3 k3, V value)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

            DictionaryAssertions.AssertNotNull<V>(value, "value");



            var secondDimentionData =
                new ExpirationalMultiDimensionDictionary<K2, K3, V>(ExpirationSpan2, ExpirationSpan3);

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1].subData;
            }

            secondDimentionData.Put(k2, k3, value);

            Data[k1] = (DateTime.Now, secondDimentionData);
        }

        #endregion

        #region Get

        public ExpirationalMultiDimensionDictionary<K2, K3, V> Get(K1 k1)
        {
            return Data[k1].subData;
        }

        public ExpirationalMultiDimensionDictionary<K3, V> Get(K1 k1, K2 k2)
        {
            return Data[k1].subData.Get(k2);
        }

        public V Get(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");




            if (Data.TryGetValue(k1, out var secondDimensionData))
            {
                return secondDimensionData.subData.Get(k2, k3);
            }

            throw new KeyNotFoundException();
        }


        #endregion

        #region Remove

        public new void Remove(K1 k1)
        {
            Invalidate();
            Data.Remove(k1, out var ignore);
        }

        public void Remove(K1 k1, K2 k2)
        {
            Invalidate();
            Data[k1].subData.Remove(k2);
        }


        public void Remove(K1 k1, K2 k2, K3 k3)
        {
            Invalidate();
            Data[k1].subData.Get(k2).Remove(k3);
        }



        #endregion


        public void _Remove(K1 k1)
        {
            (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, V> subData) ignore = default;
            Data.TryRemove(k1, out ignore);
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
    }

}
