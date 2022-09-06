using System;
using NFluent;
using NFluent.Extensibility;

namespace multiDimensionalDictionary
{
    public static class DictionaryAssertions
    {
        public static void AssertNotNull<T>(T val, string name)
        {
            if (val == null)
            {
                throw new ArgumentNullException(name);
            }
        }
        
        public static ICheckLink<ICheck<MultiDimensionalDictionary<K, V>>> ContainsKey<K,V>(this ICheck<MultiDimensionalDictionary<K, V>> context, K key) 
        {
            ExtensibilityHelper.BeginCheck(context)
                .FailWhen(sut => !sut.ContainsKey(key), "dictionary does not contains key {expected}.")
                .DefineExpectedValue($"{key.ToString()}")
                .OnNegate("dictionary contains key {expected}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
        
        public static ICheckLink<ICheck<MultiDimensionalDictionary<K1,K2, V>>> ContainsKey<K1,K2,V>(this ICheck<MultiDimensionalDictionary<K1,K2, V>> context, K1 key1, K2 key2) 
        {
            ExtensibilityHelper.BeginCheck(context)
                .FailWhen(sut => !sut.ContainsKey(key1, key2), "dictionary does not contains key {expected}.")
                .DefineExpectedValue($"{key1.ToString()}.{key2.ToString()}")
                .OnNegate("dictionary contains key {expected}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
        
        public static ICheckLink<ICheck<MultiDimensionalDictionary<K1,K2,K3, V>>> ContainsKey<K1,K2,K3,V>(this ICheck<MultiDimensionalDictionary<K1,K2,K3, V>> context, K1 key1, K2 key2, K3 key3) 
        {
            ExtensibilityHelper.BeginCheck(context)
                .FailWhen(sut => !sut.ContainsKey(key1, key2, key3), "dictionary does not contains key {expected}.")
                .DefineExpectedValue($"{key1.ToString()}.{key2.ToString()}.{key3.ToString()}")
                .OnNegate("dictionary contains key {expected}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
        
        public static ICheckLink<ICheck<MultiDimensionalDictionary<K1,K2,K3,K4, V>>> ContainsKey<K1,K2,K3,K4,V>(this ICheck<MultiDimensionalDictionary<K1,K2,K3,K4,V>> context, K1 key1, K2 key2, K3 key3, K4 key4) 
        {
            ExtensibilityHelper.BeginCheck(context)
                .FailWhen(sut => !sut.ContainsKey(key1, key2, key3, key4), "dictionary does not contains key {expected}.")
                .DefineExpectedValue($"{key1.ToString()}.{key2.ToString()}.{key3.ToString()}.{key4.ToString()}")
                .OnNegate("dictionary contains key {expected}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
        
        public static ICheckLink<ICheck<MultiDimensionalDictionary<K1,K2,K3,K4,K5, V>>> ContainsKey<K1,K2,K3,K4,K5,V>(this ICheck<MultiDimensionalDictionary<K1,K2,K3,K4,K5,V>> context, K1 key1, K2 key2, K3 key3, K4 key4, K5 key5) 
        {
            ExtensibilityHelper.BeginCheck(context)
                .FailWhen(sut => !sut.ContainsKey(key1, key2, key3, key4,key5), "dictionary does not contains key {expected}.")
                .DefineExpectedValue($"{key1.ToString()}.{key2.ToString()}.{key3.ToString()}.{key4.ToString()}.{key5.ToString()}")
                .OnNegate("dictionary contains key {expected}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
    }
}