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
    public static class DB_Test
    {
        public class Person
        {
            public int id;
            public string name;
            public PartOfTable<Person, string> frinds;
        }
        public static void Test()
        {
            try
            {
                System.IO.Directory.Delete(Environment.CurrentDirectory + "/DB", true);
            }
            catch{}

            var Table_Person = new Monsajem_Incs.Database.DirectoryTable.DirectoryTable<Person, string>(
               Environment.CurrentDirectory+"/DB", (c) => c.name, true,true);
            Table_Person.Relation((c) => c.frinds, (c) => c.IsUpdateAble = true).Join();

            Table_Person.Insert(new Person() { name = "ali" });
            Table_Person.Insert(new Person() { name = "reza" });

            //{
            //    Table_Person.Insert((c) => c.name = "ahmad");
            //    Table_Person.Insert((c) => c.name = "akbar");
            //    Table_Person.Delete("ahmad");
            //    Table_Person.Insert((c) => c.name = "ahmad");
            //    Table_Person["ali"].Value.frinds.Accept("ahmad");
            //    Table_Person["ali"].Value.frinds.Accept("akbar");

            //    var Ali1 = Table_Person.GetItem("ali").Value.frinds.UpdateAble["akbar"];
            //    var ahmad1 = Table_Person.UpdateAble["akbar"];

            //    Table_Person.Update("akbar", (c) => { });
            //    var Ali = Table_Person.GetItem("ali").Value.frinds.UpdateAble["akbar"];
            //    var ahmad = Table_Person.UpdateAble["akbar"];
            //}

            var Server = new Server();
            Server.StartServicing(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8989),
            (Link) =>
            {
                Link.SendUpdate(Table_Person);//1
                Link.SendUpdate(Table_Person);//2

                Table_Person.Insert((c) => c.name = "ahmad");
                Table_Person.Insert((c) => c.name = "akbar");

                Link.SendUpdate(Table_Person);//3

                Table_Person.Delete("ahmad");

                Link.SendUpdate(Table_Person);//4

                Table_Person.Insert((c) => c.name = "ahmad");

                Link.SendUpdate(Table_Person);//5

                var Ali = Table_Person.GetItem("ali").Value;
                var ahmad = Table_Person.GetItem("ahmad").Value;
                Ali.frinds.Accept("ahmad");
                Ali.frinds.Accept("akbar");
                Ali = Table_Person.GetItem("ali").Value;
                ahmad = Table_Person.GetItem("ahmad").Value;
                Link.SendUpdate(Ali.frinds);//6
                Link.SendUpdate(ahmad.frinds);//7
                Link.SendUpdate(Ali.frinds);//8
                Link.SendUpdate(ahmad.frinds);//9

                Ali = Table_Person.GetItem("ali").Value;
                Ali.frinds.Ignore("ahmad");
                Ali = Table_Person.GetItem("ali").Value;
                Link.SendUpdate(Ali.frinds);//10

            });

            Console.ReadKey();

        }
    }
}
