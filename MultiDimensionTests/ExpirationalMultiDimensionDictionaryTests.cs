using System;
using System.Threading;
using multiDimensionalDictionary;
using Xunit;

namespace MultiDimensionTests
{
    public class ExpirationalMultiDimensionDictionaryTests
    {
         private void AssertExpiration(bool expected, bool actual, string messagePattern, int errorCode)
        {
            string t = actual ? "" : "not";
            if (actual != expected)
            {
                Assert.True(false,string.Format(messagePattern,t)+" KO");
                
            }
        }
        
        private void AssertTrue(bool test, string messagePattern, int errorCode)
        {
            AssertExpiration(true,test,messagePattern,errorCode);
        }
        
        private void AssertFalse(bool test, string messagePattern, int errorCode)
        {
            AssertExpiration(false,test,messagePattern,errorCode);
        }


        [Fact]
        private void Test1()
        {
            var dic = new ExpirationalMultiDimensionDictionary<string, string>(TimeSpan.FromSeconds(1));
            ;
            dic.Put("x", "y");
            AssertTrue(dic.ContainsKey("x"),"(1) {0} found before expiracy",1);
            Thread.Sleep(1200);
            AssertFalse(dic.ContainsKey("x"),"(1) {0} found after 1 expiracy",1);
        }
        
        [Fact]
        private void Test2()
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
            
        }
        
        [Fact]
         public void Test3()
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
            
        }
    }
}