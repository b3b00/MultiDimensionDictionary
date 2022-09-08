using System.Text;

namespace DictionaryGenerator;

public class ExpirationalGenerator
{

    private static string GenerateTypeParameters(int start, int count, bool withValue = true)
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
    
    private static string GenerateParametersDeclaration(int start, int count)
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

    private static string GenerateParameters(int start, int count, bool withValue = false)
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

    private static string GenerateUsingsAndNamespace()
    {
        var header = $@"

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace multiDimensionalDictionary {{";
        return header;
    }


    private static string GenerateExpirations(int count)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 1; i <= count; i++)
        {
            builder.AppendLine($"TimeSpan ExpirationSpan{i};");
        }

        return builder.ToString();
    }


    private static string GenerateConstructorMillis(int count)
    {
        var builder = new StringBuilder();
        builder.Append("public ExpirationalMulti(");
        for (int i = 1; i <= count; i++)
        {
            builder.Append($"long expiration{i}");
            if (i < count)
            {
                builder.Append(", ");
            }
        }

        builder.Append(") : this(");
        for (int i = 1; i <= count; i++)
        {
            builder.Append($"TimeSpan.FromMilliseconds(expiration{i})");
            if (i < count)
            {
                builder.Append(", ");
            }
        }

        builder.Append(")");
        builder.Append(@"
{
            }");
        return builder.ToString();
    }
    
    private static string GenerateConstructorTimeSpan(int count)
    {
        var builder = new StringBuilder();
        builder.Append("public ExpirationalMulti(");
        for (int i = 1; i <= count; i++)
        {
            builder.Append($"TimeSpan expiration{i}");
            if (i < count)
            {
                builder.Append(", ");
            }
        }

        builder.AppendLine(") ");
        builder.Append("{");
        for (int i = 1; i <= count; i++)
        {
            builder.Append($"ExpirationSpan{i} = expiration{i};");
        }
        builder.Append("}");
        return builder.ToString();
    }

    private static string GenerateConstructors(int count)
    {
        return $@"
{GenerateConstructorTimeSpan(count)}

{GenerateConstructorMillis(count)}";
    }
    
    private static string GenerateClassDeclarationGenerateHeader(int count)
    {
        var header = $@"

public class ExpirationalMulti<{GenerateTypeParameters(1, count)}>
    {{
        {GenerateExpirations(count)}

        {GenerateConstructors(count)}

        protected ConcurrentDictionary<K1, (DateTime date, ExpirationalMulti<{GenerateTypeParameters(2, count)}> subData)> Data{{ get;set; }}

        public ExpirationalMulti() : base()
        {{
            Data = new ConcurrentDictionary<K1, (DateTime date, ExpirationalMulti<{GenerateTypeParameters(2, count)}> subData)>();
        }}

        
";
        
        
        
        return header;
    }

    private static string GenerateContains(int count)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 1; i < count + 1; i++)
        {
            builder.AppendLine(GenerateContainsKey(count, i));
            builder.AppendLine();
        }

        return builder.ToString();
    }



    private static string GenerateContainsKey(int count, int level)
    {
        if (level == 1)
        {
            return "public bool ContainsKey(K1 k1) => Data.ContainsKey(k1);";
        }
        else
        {
            return
                $"public bool ContainsKey({GenerateParametersDeclaration(1, level)}) => Data.ContainsKey(k1) && Data[k1].subData.ContainsKey({GenerateParameters(2, level)});";
        }
    }

    private static string GenerateGetKeys(int count)
    {

        var parameterTypes = count == 2 ? "K2" : $"({GenerateTypeParameters(2, count, false)})";
        
        var get = $@"public List<({GenerateTypeParameters(1, count, false)})> GetKeys()
        {{
            var keys = new List<({GenerateTypeParameters(1, count, false)})>();
            foreach (var kvp in Data)
            {{
                List<{parameterTypes}> subkeys = kvp.Value.subData.GetKeys();
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


    private static string GenerateAssert(string type, string name)
    {
        return $@"DictionaryAssertions.AssertNotNull<{type}>({name}, ""{name}"");";
    }
    
    private static string GenerateAsserts(int count, bool withValue = true)
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

    private static string GenerateExpirationParameters(int start, int count)
    {
        StringBuilder builder = new StringBuilder();

        for (int i = start; i < count + 1; i++)
        {
            builder.Append($"ExpirationSpan{i}");
            if (i < count)
            {
                builder.Append(", ");
            }
        }

        return builder.ToString();
    }
    
    private static string GeneratePut(int count)
    {
        var put = $@"public void Put({GenerateParametersDeclaration(1, count)}, V value)
        {{

            {GenerateAsserts(count)}

            var secondDimentionData = new ExpirationalMulti<{GenerateTypeParameters(2,count,true)}>({GenerateExpirationParameters(2,count)});

            if (Data.ContainsKey(k1))
            {{
                secondDimentionData = Data[k1].subData;
            }}

            secondDimentionData.Put({GenerateParameters(2,count,true)});

            Data[k1] = (DateTime.Now, secondDimentionData);
        }}";
            return put;
    }

    private static string GenerateGet(int count, int level)
    {
        if (level == count)
        {
            var get = $@"public V Get({GenerateParametersDeclaration(1, level)})
        {{
{GenerateAsserts(count, false)}

            if (Data.TryGetValue(k1, out var secondDimensionData))
            {{
                return secondDimensionData.subData.Get({GenerateParameters(2, count, false)});
            }}

            throw new KeyNotFoundException();
                }}";
            return get;
        }

        if (level == 1)
        {
            return $@"public ExpirationalMulti<{GenerateTypeParameters(2,count, true)}> Get(K1 k1)
            {{
                return Data[k1].subData;
            }}";
        }
        
        return $@"public ExpirationalMulti<{GenerateTypeParameters(level+1,count,true)}> Get({GenerateParametersDeclaration(1,level)})
            {{
                return Data[k1].subData.Get({GenerateParameters(2,level,false)});
            }}";
    }

    private static string GenerateGets(int count)
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
            .AppendLine("#region ContainsKey")
            .AppendLine(GenerateContains(count))
            .AppendLine("#endregion")
            .AppendLine()
            .AppendLine("#region getKeys")
            .AppendLine(GenerateGetKeys(count))
            .AppendLine("#endregion")
            .AppendLine()
            .AppendLine("#region Put")
            .AppendLine(GeneratePut(count))
            .AppendLine("#endregion")
            .AppendLine()
            .AppendLine("#region Get")
            .AppendLine(GenerateGets(count))
            .AppendLine("#endregion")
            .AppendLine("#region Remove")
            .AppendLine(GenerateRemoves(count))
            .AppendLine("#endregion")
            .AppendLine(GenerateInvalidate(count))
            .AppendLine("}");
        return builder.ToString();
    }

    public static string GenerateRemove(int count, int level)
    {
        if (level == 1)
        {
            return @"public new void Remove(K1 k1)
    {
        Data.Remove(k1, out var ignore);
    }";
        }
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"public void Remove({GenerateParametersDeclaration(1, level)})")
                .AppendLine("{")
                .Append("Data[k1].subData");
            for (int i = 2; i < level; i++)
            {
                builder.Append($".Get(k{i})");
            }

            builder.AppendLine($".Remove(k{level});")
                .AppendLine("}");
            return builder.ToString();
        }
    }

    
    
    public static string GenerateRemoves(int count)
     {
        StringBuilder builder = new StringBuilder();
        for (int i = 1; i < count + 1; i++)
        {
            builder.AppendLine(GenerateRemove(count, i));
            builder.AppendLine();
        }

        return builder.ToString();
    }
    
    public static string GenerateInvalidate(int count)
    {
        string invalidate = $@"

public void _Remove(K1 k1)
        {{
            (DateTime date, ExpirationalMulti<{GenerateTypeParameters(2,count,true)}> subData) ignore = default;
            Data.TryRemove(k1, out ignore);
        }}

public void Invalidate()
        {{
            DateTime now = DateTime.Now;
            List<K1> toRemove = new List<K1>();
            foreach (var entry in Data)
            {{
                var age = now - entry.Value.date;
                if (age > ExpirationSpan1)
                {{
                    toRemove.Add(entry.Key);
                }}
            }}

            if (toRemove.Any())
            {{
                foreach (var k1 in toRemove)
                {{
                    _Remove(k1);
                }}
            }}
        }}";
        return invalidate;
    }
    
    public static string Generate(int count)
    {
        if (count <= 1)
        {
            throw new InvalidOperationException("NO ! You can't do so !");
        }
        StringBuilder builder = new StringBuilder();
        builder
            .AppendLine(GenerateUsingsAndNamespace())
        .AppendLine(GenerateClass(count))
        .AppendLine("}");

        
        
        return builder.ToString();
    }

}