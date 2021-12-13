using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Collection;
using System.Threading;
using Monsajem_Incs.TimeingTester;

namespace _test
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PerformanceTest();
        }

        static void PerformanceTest()
        {
            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\a.txt", new byte[0]);

            var FileStream = System.IO.File.Open(System.Environment.CurrentDirectory + "\\a.txt", System.IO.FileMode.Truncate);

            var Stream = new StreamCollection<Data>(FileStream);

            System.IO.File.WriteAllBytes(System.Environment.CurrentDirectory + "\\b.txt", new byte[0]);

            //// warm up

            Stream.Insert(new Data() { Name = "0", Id = 0 });
            Stream.DeleteByPosition(0);

            var Count = 4000000;

            var InsertTime = Timing.run(() =>
            {
                for (int i =0;i<Count;i++)
                {
                    Stream.Insert(new Data(){ Name = i.ToString(), Id = i},i);
                }
            });

            var Inserts_Per_Second = (int)(Count / InsertTime.TotalSeconds);
            var EveryInsert = (InsertTime.TotalSeconds / Count).ToString("0.##########");
            var EveryInsert_Milisecond = ((InsertTime.TotalSeconds / Count) * 1000).ToString("0.##########");

            var UpdateTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Stream[i]=new Data(){ Name = i.ToString(),Id = i };
                }
            });
            var Update1_Per_Second = (int)(Count / UpdateTime.TotalSeconds);

            var GetTimeByPosition = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    var c = Stream[i];
                    if (c == null)
                        throw new Exception();
                }
            });
            var Get_Position_Per_Second = (int)(Count / GetTimeByPosition.TotalSeconds);

            var DeleteTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Stream.DeleteByPosition(0);
                }
            });
            var Delete_Per_Second = (int)(Count / DeleteTime.TotalSeconds);
        }
    }
}