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

namespace _test
{
    public class q
    {
        public Delegate a;
        public q q1;
        public IEnumerable<string> str = new string[] { "a", "b" };
        public string[] str2 = new string[] { "a", "b" };
        public int q2;
        public int q3;
    }

    class Program
    {
        static void Main(string[] args)
        {
            q q1 = MakeQ();


            var Sa = q1.Serialize();

            var Da = Sa.Deserialize(q1);


            var STime =
            Timing.run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    Sa = q1.Serialize();
                }
            });

            var DTime =
            Timing.run(() =>
            {
                for (int i = 0; i < 1000000; i++)
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
