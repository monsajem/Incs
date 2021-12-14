using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monsajem_Incs.TimeingTester;
using Monsajem_Incs.Serialization;
using static Monsajem_Incs.Collection.Array.Extentions;
using System.Runtime.InteropServices;

using Monsajem_Incs.Convertors;

namespace Test
{
    class Program
    {
        public static Monsajem_Incs.Collection.Array.Base.IArray<string> Ar_Check;
        public static string[] Ar_True = new string[0];

        public static bool Check_SR_DR = false;

        public static (Func<int, object> Get, Action<int, object> Set) Convertor<t>(t[] ar)
        {
            return ((pos) => ar[pos], (pos, value) => ar[pos] =(t)value);
        }

        static Span<T> AsSpan<T>(T[,] values)
        {
            return MemoryMarshal.CreateSpan(ref values[0, 0], values.Length).ToArray();
        }

        static void Main(string[] args)
        {
            //var xar = new Monsajem_Incs.Collection.Array.TreeBased.Array<string>();

            //xar.ChangedNextSequence = (c) =>
            //{
            //    c.ToString();
            //};


            //xar.Insert("0", 0);
            //xar.Insert("1", 1);
            //xar.Insert("2", 2);
            //xar.Insert("3", 3);
            //xar.Insert("4", 4);
            //xar.Insert("5", 5);
            //xar.Insert("6", 6);
            //xar.Insert("7", 7);

            //xar.DeleteByPosition(3);

            //ReadPerformaceTest();
            //PerformaceTest();
            //try
            //{

            System.IO.File.Delete(Environment.CurrentDirectory + "\\DB");

            var Sr = System.IO.File.Open(Environment.CurrentDirectory + "\\DB", System.IO.FileMode.OpenOrCreate);

            var ar = new Monsajem_Incs.Collection.StreamCollection<string>(Sr);

            Ar_Check = ar;
                SafeTest();
                SafeTestBinary();
            //}
            //catch
            //{

            //}
            BugTest();
        }

        static void ReadPerformaceTest()
        {
            var Count = 1000000;
            var ar1 = new Monsajem_Incs.Collection.Array.TreeBased.Array<int>();
            for (int i =0; i < Count; i++)
                ar1.Insert(i, i);

            for (int i = 0; i < Count; i++)
                if (ar1[i] != i)
                    throw new Exception();

            Console.WriteLine(ar1.AsEnumerable().ToArray().Length);
            var ar2 = new SortedSet<int>();
            for (int i = 0; i < Count; i++)
                ar2.Add(i);
            var ar3 = new LinkedList<int>();
            for (int i = 0; i < Count; i++)
                ar3.AddLast(i);

            var Res = 0;
            
            var t1 = Timing.run(() =>
            {
                var _Res = 0;
                var ar = ar1;
                foreach (var item in ar)
                    _Res += item;
                Res = _Res;
            });

            var t2 = Timing.run(() =>
            {
                var _Res = 0;
                var ar = ar2;
                foreach (var item in ar)
                    _Res += item;
                Res = _Res;
            });

            var t3 = Timing.run(() =>
            {
                var _Res = 0;
                var ar = ar3;
                var Item = ar3.First;
                while(Item!=null)
                {
                    _Res += Item.Value;
                    Item = Item.Next;
                }
                Res = _Res;
            });
            Console.WriteLine(t1.ToString());
            Console.WriteLine(t2.ToString());
            Console.WriteLine(t3.ToString());
            Console.ReadKey();
        }

        static void PerformaceTest()
        {
            var Count = 1000000;
            //var ar1 = new Monsajem_Incs.Array.DynamicSize.Array<int>();

            //var t1 = Monsajem_Incs.TimeingTester.Tester.run(() =>
            //{
            //    for (int i = Count; i > 0; i--)
            //    {
            //        ar1.Insert(i, 0);
            //    }
            //    ar1.ToArray().ToString();
            //});

            //ar1 = new Monsajem_Incs.Array.DynamicSize.Array<int>();

            //var t2 = Monsajem_Incs.TimeingTester.Tester.run(() =>
            //{
            //    for (int i = Count; i > 0; i--)
            //    {
            //        ar1.Insert(i, 0);
            //    }
            //    ar1.ToArray().ToString();
            //});

            var ar2 = new Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize.Array<int>();
            ar2.Insert(12, 17, 18);
            ar2.Insert(25, 23, 22);
            var q = ar2.AsEnumerable().ToArray();
            q.First();
            q.First();
            var q1 = ar2.FromTo(0, 3);
            var t3 = Timing.run(() =>
            {
                for (int i = Count; i > 0; i--)
                {
                    ar2.Insert(i, 0);
                }
            });

            //var t5 = Timing.run(() =>
            //{
            //    for (int i = Count; i > 0; i--)
            //    {
            //        ar2 = new Monsajem_Incs.Array.Hyper.Array<int>();
            //        var newCount = Count -= 1000;
            //        if (newCount < 0)
            //            return;
            //        for (; i > newCount; i--)
            //        {
            //            ar2.Insert(i);
            //        }
            //    }
            //});


            var t4 = Timing.run(() =>
            {
                for (int i = Count; i > 0; i--)
                {
                    ar2.DeleteByPosition(0);
                }
            });
        }

        static Random random = new Random();
        static void SafeTest()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);
            StreamLoger.Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\DebugLog", System.IO.FileMode.OpenOrCreate);

        s:

            var Role = random.Next(0, 3);

            switch (Role)
            {
                case 0:
                    {
                        var Pos = random.Next(0, Ar_Check.Length);
                        var Data = random.Next(int.MinValue, int.MaxValue).ToString();
                        StreamLoger.run(() =>
                        {
                            Ar_Check.Insert(Data, Pos);
                            Insert(ref Ar_True, Data, Pos);
                            if (Ar_Check.Length != Ar_True.Length)
                                throw new Exception();
                            DebugAr();
                        });
                    }
                    break;
                case 1:
                    if (Ar_Check.Length > 0)
                    {
                        var Pos = random.Next(0, Ar_Check.Length - 1);
                        var Data = random.Next(int.MinValue, int.MaxValue).ToString();
                        StreamLoger.run(() =>
                        {
                            Ar_Check[Pos] = Data;
                            Ar_True[Pos] = Data;
                            DebugAr();
                        });
                    }
                    break;
                case 2:
                    if (Ar_Check.Length > 0)
                    {
                        var Pos = random.Next(0, Ar_Check.Length - 1);
                        StreamLoger.run(() =>
                        {
                            Ar_Check.DeleteByPosition(Pos);
                            DeleteByPosition(ref Ar_True, Pos);
                            if (Ar_Check.Length != Ar_True.Length)
                                throw new Exception();
                            DebugAr();
                        });
                    }
                    break;
            }
            goto s;
        }
        static void SafeTestBinary()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);
            StreamLoger.Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\DebugLog", System.IO.FileMode.OpenOrCreate);

        s:

            var Role = random.Next(0, 3);

            switch (Role)
            {
                case 0:
                    {
                        var Data = random.Next(int.MinValue, int.MaxValue).ToString();
                        StreamLoger.run(() =>
                        {
                            var LastLen = Ar_Check.Length;
                            Ar_Check.BinaryInsert(Data);
                            BinaryInsert(ref Ar_True, Data);
                            if (Ar_Check.Length != LastLen + 1)
                                throw new Exception();
                            DebugAr();
                        });
                    }
                    break;
                case 1:
                    if (Ar_Check.Length > 0)
                    {
                        var Pos = random.Next(0, Ar_Check.Length - 1);
                        StreamLoger.run(() =>
                        {
                            var LastLen = Ar_Check.Length;
                            Ar_Check.BinaryDelete(Ar_Check[Pos]);
                            BinaryDelete(ref Ar_True,Ar_True[Pos]);
                            if (Ar_Check.Length != LastLen - 1)
                                throw new Exception();
                            DebugAr();
                        });
                    }
                    break;
                case 2:
                    if (Ar_Check.Length > 0)
                    {
                        var Pos = random.Next(0, Ar_Check.Length - 1);
                        StreamLoger.run(() =>
                        {
                            var LastLen = Ar_Check.Length;
                            var CheckPos = Ar_Check.BinarySearch(Ar_Check[Pos]);
                            var TruePos = System.Array.BinarySearch(Ar_True,Ar_True[Pos]);
                            if (CheckPos.Index!=TruePos)
                                throw new Exception();
                            if(CheckPos.Value!=Ar_True[Pos])
                                throw new Exception();
                            DebugAr();
                        });
                    }
                    break;
            }
            goto s;
        }

        static void DebugAr()
        {
            static void CompareAr(string[] Ar_True, Monsajem_Incs.Collection.Array.Base.IArray<string> Ar_Check)
            {
                if (Ar_Check.AsEnumerable().Count() != Ar_Check.Length)
                    throw new Exception();

                int x = 0;
                foreach (var Item in Ar_Check)
                {
                    if (Item != Ar_True[x])
                        throw new Exception();
                    if (Item != Ar_Check[x])
                        throw new Exception();
                    x++;
                }
            }

            if (Check_SR_DR)
            {
                var Sr = Ar_Check.Serialize().Deserialize(Ar_Check);
                CompareAr(Ar_True, Sr);
            }

            CompareAr(Ar_True, Ar_Check);


        }



        static void BugTest()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);
            StreamLoger.Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\DebugLog",
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);

            StreamLoger.DebugStream((c) =>
            {
                if (c.IsSafe)
                    c.Action();
                else
                    c.Action();
            });
        }
    }
}
