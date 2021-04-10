using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Net.WebTest;
namespace Test
{
    public class Tester
    {
        
        public static void Client()
        {
            Console.Title = "Client";

            System.Threading.Thread.Sleep(2000);
            new Client().Connect(
                new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989),async (Link) =>
                {
                    string a = "aaa";
                    int b = 123;
                   var s = await Link.RunOnOtherSide(() => a+b);

                    Console.WriteLine(s);
                    //await Link.RunOnOtherSide((sv) =>'An existing connection was forcibly closed by the remote host'
                    //{
                    //    sv.SendData<string>("aaa");
                    //});

                    //var str = await Link.GetData<string>();

                }).GetAwaiter().GetResult();
            Console.ReadKey();
        }

        public static void Server()
        {
            Console.Title = "Server";

            var Server = new Server();
            Server.StartServicing(8989,
            (Link) =>
            {
                Link.RunRecivedAction();
                Console.WriteLine("Done");
            });

            new Server().StartServicing(8999,
            (Link) =>
            {
                Link.RunRecivedAction();
                Console.WriteLine("Done");
            });

            Console.ReadKey();
        }
    }
}