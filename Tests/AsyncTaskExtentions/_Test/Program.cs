using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.DelegateExtentions;
using static Monsajem_Incs.DelegateExtentions.Actions;

namespace _Test
{
    class Program
    {
        public static Action a;
        public static Action b;
        static void Main(string[] args)
        {
            new System.Threading.Thread(() =>
            {
                WaitForHandle(() => ref a, () => ref b).Wait(); ;

                Console.WriteLine("handled");
            }).Start();
            a.Invoke();

            Console.ReadKey();
        }
    }
}
