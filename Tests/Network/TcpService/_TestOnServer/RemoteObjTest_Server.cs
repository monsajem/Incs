using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;

namespace TestOnServer
{
    public static class RemoteObjTest
    {
        public class RemoteObj
        {
            [Remotable]
            public Action Ac= () => Console.WriteLine("aaa");
            [Remotable]
            public Func<string> Func= () => "alireza";
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
                    c.Ac();
                    var q = c.Func();
                }).Wait();
            });

            Console.ReadKey();
        }
    }
}
