using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.TimeingTester;
using static System.Runtime.Serialization.FormatterServices;
using System.Buffers;

namespace _test
{
    public class q
    {
        public object sQ ="aaa";
        public Delegate a;
        public q q1;
        public IEnumerable<string> str = new string[] { "a", "b" };
        public string[] str2 = new string[] { "a", "b" };
        public byte[] Bytes = new byte[] {25,234,22};
        public int q2;
        public int q3;
    }

    class Program
    {
        static void Main(string[] args)
        {
            object obj = 12;
            var D = obj.Serialize();

            q q1 = MakeQ();


            var Sa = q1.Serialize((c)=>
            {
                //if (c == typeof(string))
                //    throw new Exception();
            });

            var Da = Sa.Deserialize(q1);

            var Len = 1000000;

            var STime =
            Timing.run(() =>
            {
                for (int i = 0; i < Len; i++)
                {
                    Sa = q1.Serialize();
                }
            });

            var DTime =
            Timing.run(() =>
            {
                for (int i = 0; i < Len; i++)
                {
                    Da = Sa.Deserialize<q>();
                }
            });

            Console.WriteLine("S Time: " + STime.ToString());
            Console.WriteLine("D Time: " + DTime.ToString());
            Console.ReadKey();
        }

        public static q MakeQ()
        {
            string gt = "aasa";
            q q1 = new q();
            //q1.a = (Func<string>)(() => q1.ToString() + gt.ToString());
            q1.q1 = q1;
            q1.a = (Action)(() => Console.WriteLine(gt+"aas"));
            return q1;
        }
    }
}
