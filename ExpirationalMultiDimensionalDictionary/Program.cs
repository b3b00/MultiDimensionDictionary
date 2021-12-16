using System;
using System.Threading;

namespace ExpirationalMultiDimensionalDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var dic = new MultiDimensionalDictionary<string, string>(TimeSpan.FromSeconds(3));
            dic.Put("x","y");
            var c = dic.ContainsKey("x");
            if (c)
            {
                Thread.Sleep(4000);
                c = dic.ContainsKey("x");
            }
            else
            {
                Console.WriteLine("something's wrong");
            }
        }
    }
}