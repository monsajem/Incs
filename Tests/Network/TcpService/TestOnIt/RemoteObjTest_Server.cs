using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.ArrayDb;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;

namespace TestOnServer
{
    public static class RemoteObjTest
    {
        public class RemoteObj
        {
            public Action Ac= () => Console.WriteLine("aaa");
            public Func<string> Func= () => "alireza";
        }

        public static void Test()
        {

            var Server = new Server();
            Server.StartServicing(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989),
            (Link) =>
            {
                var obj = new RemoteObj();
                Link.Remote(obj,(c)=>
                {
                    c.Ac();
                    var q = c.Func();
                });
            });

            Console.ReadKey();
        }
    }
}
