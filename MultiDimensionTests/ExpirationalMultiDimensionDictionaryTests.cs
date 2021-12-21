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

         [Fact]
         public void Test4()
         {
             var dic4 = new ExpirationalMultiDimensionDictionary<string, string, string,string,string>(TimeSpan.FromSeconds(8),
                 TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1));
             dic4.Put("w","x", "y", "z","a");
             AssertTrue(dic4.ContainsKey("w", "x", "y","z"),"(w,x,y,z) {0} found before 4 expiracy",4);

             Thread.Sleep(2000);
            AssertTrue(dic4.ContainsKey("w","x","y"),"(w,x,y) {0} found after @4 expiracy",4);
            AssertFalse(dic4.ContainsKey("w","x","y","z"),"(w,x,y,z) {0} found after @4 expiracy",4);

             Thread.Sleep(2000);
             AssertTrue(dic4.ContainsKey("w", "x"), "(w,x) {0} found after @3 expiracy", 4);
             AssertFalse(dic4.ContainsKey("w", "x","y"), "(w,x,y) {0} found after @3 expiracy", 4);

             Thread.Sleep(2000);
             AssertTrue(dic4.ContainsKey("w"), "(w) {0} found after @2 expiracy", 4);
             AssertFalse(dic4.ContainsKey("w", "x"), "(w,x) {0} found after @2 expiracy", 4);

             Thread.Sleep(2000);
             AssertFalse(dic4.ContainsKey("w"), "(w) {0} found after @2 expiracy", 4);
         }

         [Fact]
         public void Test5()
         {
             var dic5 = new ExpirationalMultiDimensionDictionary<string, string, string, string, string, string>(
                 TimeSpan.FromSeconds(9), TimeSpan.FromSeconds(7),
                 TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1));
             dic5.Put("v", "w", "x", "y", "z", "a");
             AssertTrue(dic5.ContainsKey("v", "w", "x", "y", "z"), "(v,w,x,y,z) {0} found before 5 expiracy", 5);

             Thread.Sleep(2000);
             AssertTrue(dic5.ContainsKey("v", "w", "x", "y"), "(v,w,x,y) {0} found after 5 expiracy", 5);
             AssertFalse(dic5.ContainsKey("v", "w", "x", "y", "z"), "(v,w,x,y,z) {0} after before 5 expiracy", 5);

             Thread.Sleep(2000);
             AssertTrue(dic5.ContainsKey("v", "w", "x"), "(v,w,x) {0} found after 4 expiracy", 5);
             AssertFalse(dic5.ContainsKey("v", "w", "x", "y"), "(v,w,x,y) {0} found after 4 expiracy", 5);

             Thread.Sleep(2000);
             AssertTrue(dic5.ContainsKey("v", "w"), "(v,w) {0} found after 3 expiracy", 5);
             AssertFalse(dic5.ContainsKey("v", "w", "x"), "(v,w,x) {0} found after 3 expiracy", 5);

             Thread.Sleep(2000);
             AssertTrue(dic5.ContainsKey("v"), "(v,w) {0} found after 2 expiracy", 5);
             AssertFalse(dic5.ContainsKey("v", "w"), "(v,w,x) {0} found after 2 expiracy", 5);

             Thread.Sleep(2000);
             AssertFalse(dic5.ContainsKey("v"), "(v) {0} found after 1 expiracy", 5);
         }
    }
}