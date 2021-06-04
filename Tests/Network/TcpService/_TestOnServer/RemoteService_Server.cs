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
    public static class RemoteServiceTest
    {
        public static void Test()
        {

            var Server = new Server();
            Server.StartServicing(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989),
            (Link) =>
            {
                Link.RemoteServices(
                async (c)=>
                {
                    return "111";
                },
                async (c) =>
                {
                    return "222";
                },
                async (c) =>
                {
                    return "333";
                },
                async (c) =>
                {
                    return "44";
                });
            });

            Console.ReadKey();
        }
    }
}
