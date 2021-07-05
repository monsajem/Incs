using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;
using static TestOnServer.DB_Test;
 
namespace TestOnClient
{
    public static class DB_Test
    {
        public static void Test()
        {
            try
            {
                System.IO.Directory.Delete(Environment.CurrentDirectory + "/DB", true);
            }
            catch { }

            var Person_Table = new Monsajem_Incs.Database.DirectoryTable.DirectoryTable<Person, string>(
               Environment.CurrentDirectory + "/DB", (c) => c.name, false, true);
            Person_Table.Relation((c) => c.frinds,(c)=>c.IsUpdateAble =false).Join();

            System.Threading.Thread.Sleep(1000);
           new Client().Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989),
            (Link) =>
            {
                Link.GetUpdate(Person_Table);//1 // get Items
                Link.GetUpdate(Person_Table);//2 // get Items
                Link.GetUpdate(Person_Table);//3 // New Items
                Link.GetUpdate(Person_Table);//4 // Delete Items
                Link.GetUpdate(Person_Table);//5 // New Items
                var Ali = Person_Table.GetItem("ali").Value;
                Link.GetUpdate(Ali.frinds);//6
                var ahmad = Person_Table.GetItem("ahmad").Value;
                Link.GetUpdate(ahmad.frinds);//7
                Ali = Person_Table.GetItem("ali").Value;
                Link.GetUpdate(Ali.frinds);//8
                ahmad = Person_Table.GetItem("ahmad").Value;
                Link.GetUpdate(ahmad.frinds);//9
                Ali = Person_Table.GetItem("ali").Value;
                Link.GetUpdate(Ali.frinds);//10
            });
        }
    }
}