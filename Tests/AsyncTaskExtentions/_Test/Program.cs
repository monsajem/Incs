using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Test
{
    class Program
    {
        public static Action a;
        public static Action b;
        static void Main(string[] args)
        {
            var Tasks = new Func<Task>[10];

            for(int i=0;i<Tasks.Length;i++)
            {
                var x = i;
                Tasks[i]= async ()=>
                {
                    Console.WriteLine(x);
                };
            }

            Monsajem_Incs.Async.Task_EX.StartWait(3, Tasks).Wait();
        }
    }
}
