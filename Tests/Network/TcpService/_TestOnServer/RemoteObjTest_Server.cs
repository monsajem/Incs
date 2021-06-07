using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;
using System.Threading;

namespace TestOnServer
{
    public static class RemoteObjTest
    {
        public class RemoteObj
        {
            static int i;
            static Random RND = new Random();
            [Remotable]
            public Action Ac = () => Console.WriteLine("aaa");
            [Remotable]
            public Func<string> Func= () => "alireza";
            [Remotable]
            public Func<int,Task<int>> TaskFunc =
                async (x) => {
                    //Console.WriteLine("S"+ ++i);
                    await Task.Delay(RND.Next(1800, 2000));
                    //Console.WriteLine("E"+i);
                    return x;
                };
            [Remotable]
            public Func<Task> TaskAc = async () =>
            {
                 await Task.Delay(RND.Next(100, 1000));
                 Console.WriteLine(i++);
            };
        }

        public static void Test()
        {

            var Server = new Server();
            Server.StartServicing(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989),
            (Link) =>
            {
                var obj = new RemoteObj();
                Link.Remote(obj,async (c)=>
                {
                    //c.Ac();
                    //var q = c.Func();

                    //{

                    //    var task = new Task[10];
                    //    for (int i = 0; i < 10; i++)
                    //        task[i] = c.TaskAc();

                    //    for (int i = 0; i < 10; i++)
                    //        await task[i];
                    //}

                    {
                        var task = new Thread[10];
                        for (int i = 0; i < 10; i++)
                        {
                            task[i] = new Thread(() => c.TaskFunc(i).Wait());
                            task[i].Start();
                        }

                        for (int i = 0; i < 10; i++)
                            task[i].Join();
                    }

                }).Wait();
            });

            Console.ReadKey();
        }
    }
}
