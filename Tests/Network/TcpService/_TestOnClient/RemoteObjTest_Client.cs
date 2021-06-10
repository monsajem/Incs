using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;

namespace TestOnClient
{
    public static class RemoteObjectTest
    {
        public static void Test()
        {

            new Client().Connect(
                new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989), (Link) =>
                {
                     Link.Remote(new TestOnServer.RemoteObjTest.RemoteObj(),
                         async()=>
                         {
                             var Q = Link.GetData<string>();
                             Link.SendData("String");
                         }).Wait();
                });
            
        }
    }
}
