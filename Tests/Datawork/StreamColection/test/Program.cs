using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.StreamCollection;
using System.Threading;
using Monsajem_Incs.TimeingTester;

namespace test
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Data2
    {
        public int Pos;
        public string Name;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //PerformanceTest();
            SafeTest();
            //BugTest();
        }

        static void PerformanceTest()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);

            var FileStream = System.IO.File.Open(System.Environment.CurrentDirectory + "\\a.txt", System.IO.FileMode.Truncate);

            var SC = new StreamCollection<Data>()
            {
                Stream = FileStream
            };
            var Stream = new Monsajem_Incs.Database.Base.Table<Data, string>(
                SC, (c) => c.Name, false);

            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\b.txt", new byte[0]);

            //// warm up

            Stream.Insert(new Data() { Name = "0", Id = 0 });
            Stream.Delete();

            var Count = 1000000;

            var InsertTime = Timing.run(() =>
            {
                for (int i = Count - 1; i > -1; i--)
                {
                    Stream.Insert((c) => { c.Name = i.ToString(); c.Id = i; });
                }
            });

            var Inserts_Per_Second = (int)(Count / InsertTime.TotalSeconds);
            var EveryInsert = (InsertTime.TotalSeconds / Count).ToString("0.##########");
            var EveryInsert_Milisecond = ((InsertTime.TotalSeconds / Count) * 1000).ToString("0.##########");

            var UpdateTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Stream.Update((c) => { c.Name = i.ToString(); c.Id = i; }, (c) => { c.Name = i.ToString(); c.Id = i; });
                }
            });
            var Update1_Per_Second = (int)(Count / UpdateTime.TotalSeconds);

            var UpdateTime2 = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Stream.Update(new Data() { Name = i.ToString(), Id = i }, (c) => { c.Name = i.ToString(); c.Id = i; });
                }
            });
            var Update2_Per_Second = (int)(Count / UpdateTime2.TotalSeconds);

            var GetTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    var c = Stream.GetItem(i);
                    if (c == null)
                        throw new Exception();
                }
            });
            var GetT_Per_Second = (int)(Count / GetTime.TotalSeconds);

            var DeleteTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Stream.Delete(new Data() { Name = i.ToString() });
                }
            });
            var Delete_Per_Second = (int)(Count / DeleteTime.TotalSeconds);

            var stdb = SC.Serialize().Deserialize(SC);

            //FileStream.Flush();
            FileStream.Close();
            FileStream = System.IO.File.Open(System.Environment.CurrentDirectory + "\\a.txt", System.IO.FileMode.OpenOrCreate);

            var OldKeys = Stream.KeysInfo.Keys;

            Stream.KeysInfo.Keys = OldKeys;

            var data = Stream.GetItem(new Data() { Name = "ahmad" });

        }

        public class TestClass
        {
            public int count;
            public byte[] Bytes;
        }

        static StreamCollection<TestClass> Ar;
        static Random random = new Random();
        static void SafeTest()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);
            StreamLoger.Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\DebugLog", System.IO.FileMode.OpenOrCreate);
            Ar = new StreamCollection<TestClass>()
            {
                Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\a.txt", System.IO.FileMode.Truncate)
            };

            s:

            var Role = random.Next(0, 5);

            switch (Role)
            {
                case 0:
                    {
                        var Pos = random.Next(0, Ar.Length);
                        var Data = new TestClass() { Bytes = new byte[random.Next(0, 50)] };
                        Data.count = Data.Bytes.Length;
                        StreamLoger.run(() => {
                            Ar.Insert(Data, Pos);
                            var OldData = Ar[Pos];
                            if (OldData.count != Data.count|OldData.Bytes.Length!=Data.Bytes.Length)
                                throw new Exception();
                        });
                    }
                    break;
                case 1:
                    if (Ar.Length > 0)
                    {
                        var Pos = random.Next(0, Ar.Length - 1);
                        var Data = new TestClass() { Bytes = new byte[random.Next(0, 50)] };
                        Data.count = Data.Bytes.Length;
                        StreamLoger.run(() =>
                        {
                            Ar.SetItem(Pos, Data);
                            var OldData = Ar[Pos];
                            if (OldData.count != Data.count | OldData.Bytes.Length != Data.Bytes.Length)
                                throw new Exception();
                        });
                    }
                    break;
                case 2:
                    if (Ar.Length > 0)
                    {
                        var Pos = random.Next(0, Ar.Length);
                        StreamLoger.run(() =>
                        {
                            var InnerData = Ar[Pos];
                        });
                    }
                    break;
                case 3:
                    if (Ar.Length > 0)
                    {
                        var Pos = random.Next(0, Ar.Length-1);
                        StreamLoger.run(() =>
                        {
                            Ar.DeleteByPosition(Pos);
                        });
                    }
                    break;
                case 4:
                    StreamLoger.run(() =>
                        {
                            var newar = Ar.Serialize().Deserialize(Ar);
                            //newar.Info.Browse(newar);
                        });
                    break;
            }
            goto s;
        }


        static void BugTest()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);
            StreamLoger.Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\DebugLog",
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            Ar = new StreamCollection<TestClass>()
            {
                Stream = System.IO.File.Open(Environment.CurrentDirectory + "\\a.txt", System.IO.FileMode.Truncate)
            };

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