// See https://aka.ms/new-console-template for more information

using System.Text;
using DictionaryGenerator;

for (int i = 2; i <= 10; i++)
{

    string g = Generator.Generate(i);

    File.WriteAllText(
        $@"C:\Users\olduh\dev\MultiDimensionDictionary\MultiDimensionDictionary\multiDimentsionDictionary\multi{i}.cs", g);
}



