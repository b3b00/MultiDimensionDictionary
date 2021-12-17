using System;

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

    }
}