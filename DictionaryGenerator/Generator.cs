using System.Text;

namespace DictionaryGenerator;

public class Generator
{

    public static string GenerateTypeParameters(int start, int count, bool withValue = true)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<");
        for (int i = start; i < count + 1; i++)
        {
            builder.Append($"K{i}");
            if (i < count && withValue) 
            {
                builder.Append(", ");
            }
        }

        if (withValue)
        {
            builder.Append(",V");
        }
        builder.Append(">");
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

    public static string GenerateParameters(int start, int count)
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

        return builder.ToString();
    }

    public static string GenerateHeader(int count)
    {
        var header = $@"public class MultiDimensionalDictionary<{GenerateTypeParameters(1, count)}>
    {{
        protected ConcurrentDictionary<K1, MultiDimensionalDictionary> Data MultiDimensionalDictionary<{GenerateTypeParameters(2, count)}> {{ get;set; }}

        public MultiDimensionalDictionary() : base()
        {{
            Data = new ConcurrentDictionary<K1, MultiDimensionalDictionary<{GenerateTypeParameters(2, count)}>>();
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
                $"public bool ContainsKey({GenerateParametersDeclaration(1, level)}) => Data.ContainsKey(k1) && Data[k1].ContainsKey({GenerateParameters(level, count-level)})";
        }
    }

    public static string GenerategetKeys(int count)
    {
        var get = $@"public List<({GenerateTypeParameters(1,count)})> GetKeys()
        {{
            List<({GenerateTypeParameters(1,count)})> keys = new List<({GenerateTypeParameters(1,count)})>();
            foreach (var kvp in Data)
            {{
                List<(K2, K3)> subkeys = kvp.Value.GetKeys();
                foreach (var subkey in subkeys)
                {{
                    keys.Add((kvp.Key, subkey.Item1, subkey.Item2));
                }}
            }}
            return keys;
}}";
        return get;
    }


    public static string Generate(int count)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(GenerateHeader(count))
            .AppendLine()
            .AppendLine(GenerateContains(count))
            .AppendLine()
            .AppendLine(GenerategetKeys(count))
            .AppendLine()
            .AppendLine("}");

        return builder.ToString();
    }

}