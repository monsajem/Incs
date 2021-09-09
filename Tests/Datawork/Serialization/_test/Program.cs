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

        public struct str1
        {
            public int Value;
        }

        public static byte[] StructToBytes<t>(t value,int Size)
        {
            byte[] bytes = new byte[Size];
            System.Runtime.CompilerServices.Unsafe.As<byte, t>(ref bytes[0]) = value;
            return bytes;
        }
        public static t BytesToStruct<t>(byte[] value, int startIndex)
        {
            return System.Runtime.CompilerServices.Unsafe.ReadUnaligned<t>(ref value[startIndex]);
        }

        static void Main(string[] args)
        {

            var x = new int[2000,200].Serialize().Deserialize<int[,]>();

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
