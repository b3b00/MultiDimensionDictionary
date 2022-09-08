using System;
using multiDimensionalDictionary;
using NFluent;
using Xunit;

namespace MultiDimensionTests
{
    public class MultiDimentsionDictionaryTests
    {
        [Fact]
        static void Test1()
        {
            MultiDimensionalDictionary<int, string> oneDimDic = new MultiDimensionalDictionary<int, string>();


            for (int i = 0; i < 5; i++)
            {
                oneDimDic.Put(i, $"{i}");
            }


            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, 4);
                var v = oneDimDic.Get(d1);
                Check.That(v).IsEqualTo(d1.ToString());
            }

            var keys = oneDimDic.GetKeys();
            Check.That(keys).CountIs(5);
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
                Check.That(v).IsEqualTo($"{d1}.{d2}");
            }

            var keys = twoDimDic.GetKeys();
            Check.That(keys).CountIs(25);
        }

        [Fact]
        static void Test3()
        {
            MultiDimensionalDictionary<int, int, int, string> threeDimDic =
                new MultiDimensionalDictionary<int, int, int, string>();


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
                Check.That(v).IsEqualTo($"{d1}.{d2}.{d3}");
            }

            var keys = threeDimDic.GetKeys();
            Check.That(keys).CountIs(125);
        }

        [Fact]
        static void Test4()
        {
            MultiDimensionalDictionary<int, int, int, int, string> fourDimDic =
                new MultiDimensionalDictionary<int, int, int, int, string>();

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
                Check.That(v).IsEqualTo($"{d1}.{d2}.{d3}.{d4}");
            }

            var keys = fourDimDic.GetKeys();
            Check.That(keys).CountIs(625);
        }

        [Fact]
        static void Test5()
        {
            MultiDimensionalDictionary<int, int, int, int, int, string> fiveDimDic =
                new MultiDimensionalDictionary<int, int, int, int, int, string>();

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
                Check.That(v).IsEqualTo($"{d1}.{d2}.{d3}.{d4}.{d5}");
            }

            var keys = fiveDimDic.GetKeys();
            Check.That(keys).CountIs(3125);
        }

        [Fact]
        static void Test10()
        {
            MultiDimensionalDictionary<int, int, int, int, int, int, int, int, int, int, string> fiveDimDic =
                new MultiDimensionalDictionary<int, int, int, int, int, int, int, int, int, int, string>();
            int count = 2;
            for (int k1 = 0; k1 < count; k1++)
            {
                for (int k2 = 0; k2 < count; k2++)
                {
                    for (int k3 = 0; k3 < count; k3++)
                    {
                        for (int k4 = 0; k4 < count; k4++)
                        {
                            for (int k5 = 0; k5 < count; k5++)
                            {
                                for (int k6 = 0; k6 < count; k6++)
                                {
                                    for (int k7 = 0; k7 < count; k7++)
                                    {
                                        for (int k8 = 0; k8 < count; k8++)
                                        {
                                            for (int k9 = 0; k9 < count; k9++)
                                            {
                                                for (int k10 = 0; k10 < count; k10++)
                                                {
                                                    fiveDimDic.Put(k1, k2, k3, k4, k5, k6, k7, k8, k9, k10,
                                                        $"{k1}.{k2}.{k3}.{k4}.{k5}.{k6}.{k7}.{k8}.{k9}.{k10}");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int d1 = rnd.Next(0, count-1);
                int d2 = rnd.Next(0, count-1);
                int d3 = rnd.Next(0, count-1);
                int d4 = rnd.Next(0, count-1);
                int d5 = rnd.Next(0, count-1);
                int d6 = rnd.Next(0, count-1);
                int d7 = rnd.Next(0, count-1);
                int d8 = rnd.Next(0, count-1);
                int d9 = rnd.Next(0, count-1);
                int d10 = rnd.Next(0, count-1);
                var v = fiveDimDic.Get(d1, d2, d3, d4, d5, d6, d7, d8, d9, d10);
                Check.That(v).IsEqualTo($"{d1}.{d2}.{d3}.{d4}.{d5}.{d6}.{d7}.{d8}.{d9}.{d10}");
            }

            var keys = fiveDimDic.GetKeys();
            Check.That(keys).CountIs((int)Math.Pow(count, 10));
        }


        [Fact]
        static void Test1Remove()
        {
            MultiDimensionalDictionary<int, string> oneDimDic = new MultiDimensionalDictionary<int, string>();

            oneDimDic.Put(1, "1");
            Check.That(oneDimDic).ContainsKey(1);

            oneDimDic.Remove(1);

            Check.That(oneDimDic).Not.ContainsKey(1);
        }

        [Fact]
        static void Test2Remove()
        {
            MultiDimensionalDictionary<int, int, string> twoDimDic = new MultiDimensionalDictionary<int, int, string>();

            twoDimDic.Put(1, 2, "2");
            Check.That(twoDimDic).ContainsKey(1, 2);

            twoDimDic.Remove(1);

            Check.That(twoDimDic).Not.ContainsKey(1, 2);
        }

        [Fact]
        static void Test3Remove()
        {
            MultiDimensionalDictionary<int, int, int, string> threeDimDic =
                new MultiDimensionalDictionary<int, int, int, string>();

            threeDimDic.Put(1, 2, 3, "3");
            Check.That(threeDimDic).ContainsKey(1, 2, 3);

            threeDimDic.Remove(1, 2, 3);

            Check.That(threeDimDic).Not.ContainsKey(1, 2, 3);
        }

        [Fact]
        static void Test4Remove()
        {
            MultiDimensionalDictionary<int, int, int, int, string> fourDimDic =
                new MultiDimensionalDictionary<int, int, int, int, string>();

            fourDimDic.Put(1, 2, 3, 4, "4");
            Check.That(fourDimDic).ContainsKey(1, 2, 3, 4);

            fourDimDic.Remove(1, 2, 3, 4);

            Check.That(fourDimDic).Not.ContainsKey(1, 2, 3, 4);
        }

        [Fact]
        static void Test5Remove()
        {
            MultiDimensionalDictionary<int, int, int, int, int, string> fiveDimDic =
                new MultiDimensionalDictionary<int, int, int, int, int, string>();

            fiveDimDic.Put(1, 2, 3, 4, 5, "5");
            Check.That(fiveDimDic).ContainsKey(1, 2, 3, 4, 5);

            fiveDimDic.Remove(1, 2, 3, 4, 5);

            Check.That(fiveDimDic).Not.ContainsKey(1, 2, 3, 4, 5);
        }
        
        [Fact]
        static void Test10Remove()
        {
            MultiDimensionalDictionary<int, int, int, int, int, int, int, int, int, int, string> tenDimDic =
                new MultiDimensionalDictionary<int, int, int, int, int, int, int, int, int, int, string>();

            tenDimDic.Put(1, 2, 3, 4, 5,6,7,8,9,10, "10");
            Check.That(tenDimDic).ContainsKey(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            tenDimDic.Remove(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Check.That(tenDimDic).Not.ContainsKey(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}