using System;
using System.Threading;
using multiDimensionalDictionary;

namespace consoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            
             
            //  Test1b();
            //  Console.WriteLine("\n=========================\n");
            //  Test2b();
            //  Console.WriteLine("\n=========================\n");
            // Test3b();
            Console.WriteLine("\n=========================\n");
            Test4b();
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
                Console.Error.WriteLine(string.Format(messagePattern,t)+" KO");
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
            var dic = new ExpirationalMultiDimensionDictionary<string, string>(TimeSpan.FromSeconds(1));
            ;
            dic.Put("x", "y");
            AssertTrue(dic.ContainsKey("x"),"(1) {0} found before expiracy",1);
            Thread.Sleep(1200);
            AssertFalse(dic.ContainsKey("x"),"(1) {0} found after 1 expiracy",1);
            Console.WriteLine("Test #1 all is fine");
        }
        
        private static void Test2b()
        {
            var dic2 = new ExpirationalMultiDimensionDictionary<string, string, string>(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(2));

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
            var dic3 = new ExpirationalMultiDimensionDictionary<string, string, string,string>(TimeSpan.FromSeconds(8),
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
         
         private static void Test4b()
         {
             var dic4 = new ExpirationalMultiDimensionDictionary<string, string, string,string,string>(TimeSpan.FromSeconds(8),
                 TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1));
              DateTime start = DateTime.Now;  
             dic4.Put("w","x", "y", "z","a");
             AssertTrue(dic4.ContainsKey("w", "x", "y","z"),"(1,2,3,4) {0} found before 4 expiracy",4);
             
             Thread.Sleep(2000);
             Console.WriteLine("============================\nwait 2");
            Console.WriteLine($"\t elapsed : {DateTime.Now-start} ");
             // test if (1,2,3) still present
             AssertTrue(dic4.ContainsKey("w","x","y"),"(1,2,3) {0} found after 4 expiracy",4);
            
             // test if (1,2,3,4) missing
             AssertFalse(dic4.ContainsKey("w","x","y","z"),"(1,2,3,4) {0} found after 3 expiracy",4);
             
             Thread.Sleep(2000);
             Console.WriteLine("============================\nwait 2 more");
             Console.WriteLine($"\t elapsed : {DateTime.Now-start} ");
             // test if (1) still present
             AssertTrue(dic4.ContainsKey("w","x"),"(1,2) {0} found after 3 expiracy",4);
             // test if (1,2) absent
             AssertFalse(dic4.ContainsKey("w", "x"),"(1,2,3) {0} found after 3 expiracy",4);
            
             Thread.Sleep(3000);
             Console.WriteLine("============================\nwait 3 more");
             Console.WriteLine($"\t elapsed : {DateTime.Now-start} ");
             // test if (1) absent
             AssertFalse(dic4.ContainsKey("w"),"(1) {0} found after 1 expiracy",4);
            
             Console.WriteLine("Test #4 : all is fine.");
            
         }
        
       
    }
}