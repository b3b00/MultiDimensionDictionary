

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace multiDimensionalDictionary
{

    [ExcludeFromCodeCoverage]
    public class ExpirationalMultiDimensionDictionary<K1, K2, K3, K4, K5, K6, K7, K8, V>
    {
        TimeSpan ExpirationSpan1;
        TimeSpan ExpirationSpan2;
        TimeSpan ExpirationSpan3;
        TimeSpan ExpirationSpan4;
        TimeSpan ExpirationSpan5;
        TimeSpan ExpirationSpan6;
        TimeSpan ExpirationSpan7;
        TimeSpan ExpirationSpan8;



        public ExpirationalMultiDimensionDictionary(TimeSpan expiration1, TimeSpan expiration2, TimeSpan expiration3,
            TimeSpan expiration4, TimeSpan expiration5, TimeSpan expiration6, TimeSpan expiration7,
            TimeSpan expiration8)
        {
            ExpirationSpan1 = expiration1;
            ExpirationSpan2 = expiration2;
            ExpirationSpan3 = expiration3;
            ExpirationSpan4 = expiration4;
            ExpirationSpan5 = expiration5;
            ExpirationSpan6 = expiration6;
            ExpirationSpan7 = expiration7;
            ExpirationSpan8 = expiration8;
            Data =
                new ConcurrentDictionary<K1, (DateTime date,
                    ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, K6, K7, K8, V> subData)>();
        }

        public ExpirationalMultiDimensionDictionary(long expiration1, long expiration2, long expiration3,
            long expiration4, long expiration5, long expiration6, long expiration7, long expiration8) : this(
            TimeSpan.FromMilliseconds(expiration1), TimeSpan.FromMilliseconds(expiration2),
            TimeSpan.FromMilliseconds(expiration3), TimeSpan.FromMilliseconds(expiration4),
            TimeSpan.FromMilliseconds(expiration5), TimeSpan.FromMilliseconds(expiration6),
            TimeSpan.FromMilliseconds(expiration7), TimeSpan.FromMilliseconds(expiration8))
        {
        }

        protected ConcurrentDictionary<K1, (DateTime date,
            ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, K6, K7, K8, V> subData)> Data { get; set; }

        public ExpirationalMultiDimensionDictionary() : base()
        {
            Data =
                new ConcurrentDictionary<K1, (DateTime date,
                    ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, K6, K7, K8, V> subData)>();
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

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2, k3, k4);
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2, k3, k4, k5);
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2, k3, k4, k5, k6);
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2, k3, k4, k5, k6, k7);
        }

        public bool ContainsKey(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8)
        {
            Invalidate();
            return Data.ContainsKey(k1) && Data[k1].subData.ContainsKey(k2, k3, k4, k5, k6, k7, k8);
        }


        #endregion

        #region getKeys

        public List<(K1, K2, K3, K4, K5, K6, K7, K8)> GetKeys()
        {
            Invalidate();
            var keys = new List<(K1, K2, K3, K4, K5, K6, K7, K8)>();
            foreach (var kvp in Data)
            {
                List<(K2, K3, K4, K5, K6, K7, K8)> subkeys = kvp.Value.subData.GetKeys();
                foreach (var subkey in subkeys)
                {
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2, subkey.Item3, subkey.Item4, subkey.Item5,
                        subkey.Item6, subkey.Item7));
                }
            }

            return keys;
        }

        #endregion

        #region Put

        public void Put(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8, V value)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

            DictionaryAssertions.AssertNotNull<K5>(k5, "k5");

            DictionaryAssertions.AssertNotNull<K6>(k6, "k6");

            DictionaryAssertions.AssertNotNull<K7>(k7, "k7");

            DictionaryAssertions.AssertNotNull<K8>(k8, "k8");

            DictionaryAssertions.AssertNotNull<V>(value, "value");



            var secondDimentionData = new ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, K6, K7, K8, V>(
                ExpirationSpan2, ExpirationSpan3, ExpirationSpan4, ExpirationSpan5, ExpirationSpan6, ExpirationSpan7,
                ExpirationSpan8);

            if (Data.ContainsKey(k1))
            {
                secondDimentionData = Data[k1].subData;
            }

            secondDimentionData.Put(k2, k3, k4, k5, k6, k7, k8, value);

            Data[k1] = (DateTime.Now, secondDimentionData);
        }

        #endregion

        #region Get

        public ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, K6, K7, K8, V> Get(K1 k1)
        {
            return Data[k1].subData;
        }

        public ExpirationalMultiDimensionDictionary<K3, K4, K5, K6, K7, K8, V> Get(K1 k1, K2 k2)
        {
            return Data[k1].subData.Get(k2);
        }

        public ExpirationalMultiDimensionDictionary<K4, K5, K6, K7, K8, V> Get(K1 k1, K2 k2, K3 k3)
        {
            return Data[k1].subData.Get(k2, k3);
        }

        public ExpirationalMultiDimensionDictionary<K5, K6, K7, K8, V> Get(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            return Data[k1].subData.Get(k2, k3, k4);
        }

        public ExpirationalMultiDimensionDictionary<K6, K7, K8, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5)
        {
            return Data[k1].subData.Get(k2, k3, k4, k5);
        }

        public ExpirationalMultiDimensionDictionary<K7, K8, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6)
        {
            return Data[k1].subData.Get(k2, k3, k4, k5, k6);
        }

        public ExpirationalMultiDimensionDictionary<K8, V> Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7)
        {
            return Data[k1].subData.Get(k2, k3, k4, k5, k6, k7);
        }

        public V Get(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8)
        {
            Invalidate();
            DictionaryAssertions.AssertNotNull<K1>(k1, "k1");

            DictionaryAssertions.AssertNotNull<K2>(k2, "k2");

            DictionaryAssertions.AssertNotNull<K3>(k3, "k3");

            DictionaryAssertions.AssertNotNull<K4>(k4, "k4");

            DictionaryAssertions.AssertNotNull<K5>(k5, "k5");

            DictionaryAssertions.AssertNotNull<K6>(k6, "k6");

            DictionaryAssertions.AssertNotNull<K7>(k7, "k7");

            DictionaryAssertions.AssertNotNull<K8>(k8, "k8");




            if (Data.TryGetValue(k1, out var secondDimensionData))
            {
                return secondDimensionData.subData.Get(k2, k3, k4, k5, k6, k7, k8);
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


        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4)
        {
            Invalidate();
            Data[k1].subData.Get(k2).Get(k3).Remove(k4);
        }


        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5)
        {
            Invalidate();
            Data[k1].subData.Get(k2).Get(k3).Get(k4).Remove(k5);
        }


        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6)
        {
            Invalidate();
            Data[k1].subData.Get(k2).Get(k3).Get(k4).Get(k5).Remove(k6);
        }


        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7)
        {
            Invalidate();
            Data[k1].subData.Get(k2).Get(k3).Get(k4).Get(k5).Get(k6).Remove(k7);
        }


        public void Remove(K1 k1, K2 k2, K3 k3, K4 k4, K5 k5, K6 k6, K7 k7, K8 k8)
        {
            Invalidate();
            Data[k1].subData.Get(k2).Get(k3).Get(k4).Get(k5).Get(k6).Get(k7).Remove(k8);
        }



        #endregion


        public void _Remove(K1 k1)
        {
            (DateTime date, ExpirationalMultiDimensionDictionary<K2, K3, K4, K5, K6, K7, K8, V> subData) ignore =
                default;
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
