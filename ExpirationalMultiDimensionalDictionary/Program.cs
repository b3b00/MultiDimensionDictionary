using System;
using System.Threading;

namespace ExpirationalMultiDimensionalDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            
             
             Test1b();
             Console.WriteLine("\n=========================\n");
             Test2b();
             Console.WriteLine("\n=========================\n");
            Test3b();
        }

        static void AssertExpiration(bool expected, bool actual, string messagePattern, int errorCode)
        {
            string t = actual ? "" : "not";
            if (actual == expected)
            {
                Console.WriteLine($"Test #{errorCode} : "+string.Format(messagePattern,t)+" OK");
            }
            else
            {
                Console.WriteLine(string.Format(messagePattern,t)+" OK");
                Environment.Exit(errorCode);
            }
        }
        
        static void AssertTrue(bool test, string messagePattern, int errorCode)
        {
            AssertExpiration(true,test,messagePattern,errorCode);
        }
        
        static void AssertFalse(bool test, string messagePattern, int errorCode)
        {
            AssertExpiration(false,test,messagePattern,errorCode);
        }


        private static void Test1b()
        {
            var dic = new MultiDimensionalDictionary<string, string>(TimeSpan.FromSeconds(1));
            ;
            dic.Put("x", "y");
            AssertTrue(dic.ContainsKey("x"),"(1) {0} found before expiracy",1);
            Thread.Sleep(1200);
            AssertFalse(dic.ContainsKey("x"),"(1) {0} found after 1 expiracy",1);
            Console.WriteLine("Test #1 all is fine");
        }

        private static void Test1()
        {
            var dic = new MultiDimensionalDictionary<string, string>(TimeSpan.FromSeconds(3));
            dic.Put("x", "y");
            var c = dic.ContainsKey("x");
            if (c)
            {
                Console.WriteLine("Test1 : first found before expiracy OK");
                Thread.Sleep(4000);
                c = dic.ContainsKey("x");
                if (!c)
                {
                    Console.WriteLine("Test1 : first not found after expiracy OK");
                }
                else
                {
                    Console.Error.WriteLine("Test1 : first  found after (1) expiracy KO");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.Error.WriteLine("Test1 : first not found before (1) expiracy KO");
                Environment.Exit(1);
            }
            Console.WriteLine("Test1 : All is fine");
        }

        
        
        
        
        
        private static void Test2b()
        {
            var dic2 = new MultiDimensionalDictionary<string, string, string>(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(2));

            dic2.Put("w","x", "y");
            var c = dic2.ContainsKey("w", "x");
            AssertTrue(dic2.ContainsKey("w", "x"),"(1,2) {0} found before expiracy",2);
            
            Thread.Sleep(3000);
            AssertTrue(dic2.ContainsKey("w"),"(1) found after 2 expiracy",2);
            AssertFalse(dic2.ContainsKey("w","x"),"(1,2) {0} found after 2 expiracy",2);
            
            Thread.Sleep(2000);
            AssertFalse(dic2.ContainsKey("w"),"(1) {0} found after 2 expiracy",2);
            
            Console.WriteLine("Test #2 : all is fine.");
            
        }
        
         private static void Test3b()
        {
            var dic3 = new MultiDimensionalDictionary<string, string, string,string>(TimeSpan.FromSeconds(8),
                TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2));

            dic3.Put("w","x", "y", "z");
            var c = dic3.ContainsKey("w", "x", "y");
            AssertTrue(dic3.ContainsKey("w", "x", "y"),"(1,2,3) {0} found before expiracy",3);
            Thread.Sleep(3000);
            
            // test if (1,2) still present
            AssertTrue(dic3.ContainsKey("w","x"),"Test3 (1,2) found after 3 expiracy",3);
            
            // test if (1,2,3) missing
            AssertFalse(dic3.ContainsKey("w","x","y"),"(1,2,3) {0} found after 3 expiracy",3);
            
            Thread.Sleep(3000);
            // test if (1) still present
            AssertTrue(dic3.ContainsKey("w"),"Test3 (1) {0} found after 2 expiracy",3);
            // test if (1,2) absent
            AssertFalse(dic3.ContainsKey("w", "x"),"Test3 (1,2) {0} found after 2 expiracy",3);
            
            Thread.Sleep(3000);
            // test if (1) absent
            AssertFalse(dic3.ContainsKey("w"),"Test3 (1) {0} found after 1 expiracy",3);
            
            Console.WriteLine("Test #3 : all is fine.");
            
        }
        
       
    }
}