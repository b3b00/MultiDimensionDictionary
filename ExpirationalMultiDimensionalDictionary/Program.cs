using System;
using System.Threading;

namespace ExpirationalMultiDimensionalDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            // var dic = new MultiDimensionalDictionary<string, string>(TimeSpan.FromSeconds(3));
            // dic.Put("x","y");
            // var c = dic.ContainsKey("x");
            // if (c)
            // {
            //     Thread.Sleep(4000);
            //     c = dic.ContainsKey("x");
            //     if (!c)
            //     {
            //         Console.WriteLine("K1 OK"); 
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("something's wrong");
            // }

            var dic2 = new MultiDimensionalDictionary<string, string, string>(TimeSpan.FromSeconds(6),
                TimeSpan.FromSeconds(3));
            
            dic2.Put("x","y","z");
            var c = dic2.ContainsKey("x", "y");
            if (c)
            {
                Console.WriteLine("step1 OK");
                Thread.Sleep(4000);
                c = dic2.ContainsKey("x");
                if (c)
                {
                    Console.WriteLine("step2 OK");
                    c = dic2.ContainsKey("x", "y");
                    if (!c)
                    {
                        Console.WriteLine("step3 OK");
                        Thread.Sleep(2000);
                        c = dic2.ContainsKey("x");
                        if (!c)
                        {
                            Console.WriteLine("K2 OK");
                        }
                        else
                        {
                            Console.WriteLine("final step KO");
                        }
                    }
                    else
                    {
                        Console.WriteLine("step3 KO");
                    }
                }
                else
                {
                    Console.WriteLine("step2 KO");
                }
            }
            else
            {
                Console.WriteLine("step1 KO");
            }
        }
    }
}