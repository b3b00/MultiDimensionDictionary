using System.Text;

namespace DictionaryGenerator;

public class Generator
{

    public static string GenerateTypeParameters(int start, int count, bool withValue = true)
    {
        StringBuilder builder = new StringBuilder();
        
        for (int i = start; i < count + 1; i++)
        {
            builder.Append($"K{i}");
            if (i < count) 
            {
                builder.Append(", ");
            }
        }

        if (withValue)
        {
            builder.Append(", V");
        }        
        return builder.ToString();
    }
    
    public static string GenerateParametersDeclaration(int start, int count)
    {
        StringBuilder builder = new StringBuilder();

        for (int i = start; i < count + 1; i++)
        {
            builder.Append($"K{i} k{i}");
            if (i < count)
            {
                builder.Append(", ");
            }
        }

        return builder.ToString();
    }

    public static string GenerateParameters(int start, int count, bool withValue = false)
    {
        StringBuilder builder = new StringBuilder();

        for (int i = start; i < count + 1; i++)
        {
            builder.Append($"k{i}");
            if (i < count)
            {
                builder.Append(", ");
            }
        }

        if (withValue)
        {
            builder.Append(", value");
        }
        return builder.ToString();
    }

    public static string GenerateUsingsAndNamespace()
    {
        var header = $@"

using System.Collections.Concurrent;
using System.Collections.Generic;
using multiDimensionalDictionary;

namespace  multiDimensionalDictionary {{";
        return header;
    }
    
    public static string GenerateClassDeclarationGenerateHeader(int count)
    {
        var header = $@"

public class Multi<{GenerateTypeParameters(1, count)}>
    {{
        protected ConcurrentDictionary<K1, Multi<{GenerateTypeParameters(2, count)}>> Data{{ get;set; }}

        public Multi() : base()
        {{
            Data = new ConcurrentDictionary<K1, Multi<{GenerateTypeParameters(2, count)}>>();
        }}";
        return header;
    }

    public static string GenerateContains(int count)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 1; i < count + 1; i++)
        {
            builder.AppendLine(GenerateContainsKey(count, i));
            builder.AppendLine();
        }

        return builder.ToString();
    }



    public static string GenerateContainsKey(int count, int level)
    {
        if (level == 1)
        {
            return "public bool ContainsKey(K1 k1) => Data.ContainsKey(k1);";
        }
        else
        {
            return
                $"public bool ContainsKey({GenerateParametersDeclaration(1, level)}) => Data.ContainsKey(k1) && Data[k1].ContainsKey({GenerateParameters(2, level)});";
        }
    }

    public static string GenerateGetKeys(int count)
    {

        var parameterTypes = count == 2 ? "K2" : $"({GenerateTypeParameters(1, count, false)})";
        
        var get = $@"public List<({GenerateTypeParameters(1, count, false)})> GetKeys()
        {{
            var keys = new List<({GenerateTypeParameters(1, count, false)})>();
            foreach (var kvp in Data)
            {{
                List<{parameterTypes}> subkeys = kvp.Value.GetKeys();
                foreach (var subkey in subkeys)
                {{
        keys.Add((kvp.Key, ";
        for (int i = 1; i < count; i++)
        {
            if (count == 2)
            {
                get += "subkey";
            }
            else
            {
                get += $"subkey.Item{i}";
            }

            if (i < count-1)
            {
                get += ", ";
            }
        }

        get += "));";

        get+=$@"}}
            }}
            return keys;
}}";
        return get;
    }


    public static string GenerateAssert(string type, string name)
    {
        return $@"DictionaryAssertions.AssertNotNull<{type}>({name}, ""{name}"");";
    }
    
    public static string GenerateAsserts(int count, bool withValue = true)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 1; i < count + 1; i++)
        {
            builder.AppendLine(GenerateAssert($"K{i}", $"k{i}"));
            builder.AppendLine();
        }

        if (withValue)
        {
            builder.AppendLine(GenerateAssert($"V", "value"));
        }
        builder.AppendLine();
        return builder.ToString();
    }
    
    public static string GeneratePut(int count)
    {
        var put = $@"public void Put({GenerateParametersDeclaration(1, count)}, V value)
        {{

            {GenerateAsserts(count)}

            var secondDimentionData = new Multi<{GenerateTypeParameters(2,count,true)}>();

            if (Data.ContainsKey(k1))
            {{
                secondDimentionData = Data[k1];
            }}

            secondDimentionData.Put({GenerateParameters(2,count,true)});

            Data[k1] = secondDimentionData;
        }}";
            return put;
    }

    public static string GenerateGet(int count, int level)
    {
        if (level == count)
        {
            var get = $@"public V Get({GenerateParametersDeclaration(1, level)})
        {{
{GenerateAsserts(count, false)}

            if (Data.TryGetValue(k1, out Multi<{GenerateTypeParameters(2, count, true)}> secondDimensionData))
            {{
                return secondDimensionData.Get({GenerateParameters(level-1, count, false)});
            }}

            throw new KeyNotFoundException();
                }}";
            return get;
        }

        if (level == 1)
        {
            return $@"public Multi<{GenerateTypeParameters(2,count, true)}> Get(K1 k1)
            {{
                return Data[k1];
            }}";
        }
        
        return $@"public Multi<{GenerateTypeParameters(level+1,count,true)}> Get({GenerateParametersDeclaration(1,level)})
            {{
                return Data[k1].Get({GenerateParameters(level,count-1,false)});
            }}";
    }

    public static string GenerateGets(int count)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 1; i < count + 1; i++)
        {
            builder.AppendLine(GenerateGet(count, i));
            builder.AppendLine();
        }

        return builder.ToString();
    }


    public static string GenerateClass(int count)
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine(GenerateClassDeclarationGenerateHeader(count))
            .AppendLine()
            .AppendLine(GenerateContains(count))
            .AppendLine()
            .AppendLine(GenerateGetKeys(count))
            .AppendLine()
            .AppendLine(GeneratePut(count))
            .AppendLine()
            .AppendLine(GenerateGets(count))
            .AppendLine("}");
        return builder.ToString();
    }
    
    public static string Generate(int count)
    {
        StringBuilder builder = new StringBuilder();
        builder
            .AppendLine(GenerateUsingsAndNamespace())
        .AppendLine(GenerateClass(count))
        .AppendLine("}");

        
        
        return builder.ToString();
    }

}