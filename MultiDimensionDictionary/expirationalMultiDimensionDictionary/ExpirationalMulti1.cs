using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{
    public class ExpirationalMultiDimensionDictionary<K1, V>
    {

        private TimeSpan ExpirationSpan;
        protected ConcurrentDictionary<K1, (DateTime date, V value)> Data { get; set; }


        public ExpirationalMultiDimensionDictionary(long expiration1MMillis) : this(
            TimeSpan.FromMilliseconds(expiration1MMillis))
        {

        }

        public ExpirationalMultiDimensionDictionary(TimeSpan expirationSpan)
        {
            ExpirationSpan = expirationSpan;
            Data = new ConcurrentDictionary<K1, (DateTime, V)>();
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
            (DateTime, V) ignore;
            Data.TryRemove(k1, out ignore);
        }

        public void Clear()
        {
            Data.Clear();
        }


    }





}