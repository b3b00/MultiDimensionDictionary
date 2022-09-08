// See https://aka.ms/new-console-template for more information

using System.Text;
using DictionaryGenerator;

for (int i = 2; i <= 6; i++)
{

    string g = Generator.Generate(i);

    File.WriteAllText(
        $@"C:\Users\olduh\dev\MultiDimensionDictionary\MultiDimensionDictionary\multiDimentsionDictionary\multi{i}.cs", g);
}
// g = Generator.GenerateGet(3, 2);
// Console.WriteLine(g);
// g = Generator.GenerateGet(3, 3);
// Console.WriteLine(g);




