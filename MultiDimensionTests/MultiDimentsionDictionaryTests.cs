using System;
using multiDimensionalDictionary;
using Xunit;

namespace MultiDimensionTests
{
    public class MultiDimentsionDictionaryTests
    {

[Fact]
static void Test1() {
    MultiDimensionalDictionary<int,string> oneDimDic = new MultiDimensionalDictionary<int, string>();
   

            for (int i = 0; i < 5; i++)
            {
                    oneDimDic.Put(i, $"{i}");
            }


            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, 4);
                var v = oneDimDic.Get(d1);
                Assert.Equal($"{d1}",v);
            }

            var keys = oneDimDic.GetKeys();
            Assert.Equal<int>(5,keys.Count);
}

        [Fact]
        static void Test2()
        {
            MultiDimensionalDictionary<int, int, string> twoDimDic = new MultiDimensionalDictionary<int, int, string>();


            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    twoDimDic.Put(i, j, $"{i}.{j}");
                }
            }


            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, 4);
                int d2 = rnd.Next(0, 4);
                var v = twoDimDic.Get(d1, d2);
                Assert.Equal($"{d1}.{d2}",v);
            }

            var keys = twoDimDic.GetKeys();
            Assert.Equal<int>(25,keys.Count);

        }

        [Fact]
        static void Test3()
        {
            MultiDimensionalDictionary<int, int, int, string> threeDimDic = new MultiDimensionalDictionary<int, int, int, string>();


            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        threeDimDic.Put(i, j, k, $"{i}.{j}.{k}");
                    }
                }
            }


            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, 4);
                int d2 = rnd.Next(0, 4);
                int d3 = rnd.Next(0, 4);
                var v = threeDimDic.Get(d1, d2, d3);
                Assert.Equal($"{d1}.{d2}.{d3}", v);
            }

            var keys = threeDimDic.GetKeys();
            Assert.Equal<int>(125,keys.Count);
        }

        [Fact]
        static void Test4()
        {
            MultiDimensionalDictionary<int, int, int, int, string> fourDimDic = new MultiDimensionalDictionary<int, int, int, int, string>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            fourDimDic.Put(i, j, k, l, $"{i}.{j}.{k}.{l}");
                        }
                    }

                }
            }


            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, 4);
                int d2 = rnd.Next(0, 4);
                int d3 = rnd.Next(0, 4);
                int d4 = rnd.Next(0, 4);
                var v = fourDimDic.Get(d1, d2, d3, d4);
                Assert.Equal($"{d1}.{d2}.{d3}.{d4}",v);
            }

            var keys = fourDimDic.GetKeys();
            Assert.Equal<int>(625, keys.Count);



        }

        [Fact]
        static void Test5()
        {
            MultiDimensionalDictionary<int, int, int, int, int, string> fiveDimDic = new MultiDimensionalDictionary<int, int, int, int, int, string>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            for (int m = 0; m < 5; m++)
                            {
                                fiveDimDic.Put(i, j, k, l, m, $"{i}.{j}.{k}.{l}.{m}");
                            }
                        }
                    }

                }
            }




            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, 4);
                int d2 = rnd.Next(0, 4);
                int d3 = rnd.Next(0, 4);
                int d4 = rnd.Next(0, 4);
                int d5 = rnd.Next(0, 4);
                var v = fiveDimDic.Get(d1, d2, d3, d4, d5);
                Assert.Equal($"{d1}.{d2}.{d3}.{d4}.{d5}",v);
            }

            var keys = fiveDimDic.GetKeys();
            Assert.Equal<int>(3125, keys.Count);
        }
    }
}