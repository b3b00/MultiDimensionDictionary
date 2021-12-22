using System;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary
{

    public class DatedItem<T> : IEquatable<T>
    {
        private IEquatable<T> _equatableImplementation;

        public DatedItem(T data)
        {
            Date = DateTime.Now;
            Data = data;
        }

        public DateTime Date { get; set; }

        public T Data { get; set; }

        public TimeSpan Age => DateTime.Now - Date;



        public bool Equals(T other)
        {
            return Data.Equals(other);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
        
        public static implicit operator T(DatedItem<T> datedItem) => datedItem.Data;
        public static explicit operator DatedItem<T>(T value) => new DatedItem<T>(value);
    }

    public class ExpMultDimDic<K1,V> : IMultiDimensionalDictionary<K1,V>
    {

        private MultiDimensionalDictionary<K1, DatedItem<V>> Data;
        
        public TimeSpan ExpirationSpan { get; set; }
        public ExpMultDimDic(TimeSpan expirationSpan)
        {
            ExpirationSpan = expirationSpan;
            Data = new MultiDimensionalDictionary<K1, DatedItem<V>>();
        }

        
        public void Invalidate()
        {
            DateTime now = DateTime.Now;
            List<K1> toRemove = new List<K1>();
            foreach (var key in Data.GetKeys())
            {
                var value = Data.Get(key);
                if (value.Age > ExpirationSpan)
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

        public List<K1> GetKeys()
        {
            Invalidate();
            return Data.GetKeys();
        }

        public void Put(K1 k1, V value)
        {
            Invalidate();
            Data.Put(k1, new DatedItem<V>(value));
        }

        public V Get(K1 k1)
        {
            Invalidate();
            return Data.Get(k1).Data;
        }

        public void Remove(K1 k1)
        {
            Invalidate();
            Data.Remove(k1);
        }

        public void Clear()
        {
            Data.Clear();
        }
    }
    
  
}