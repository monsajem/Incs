//using Monsajem_Incs.Database.Base;
//using Monsajem_Incs.Net.Tcp;
//using System;
//using System.Linq;
//using System.Net;

//namespace TestOnClient
//{
//    public class Person
//    {
//        public int id;
//        public string name;
//        public PartOfTable<Person, string> frinds;
//    }
//    public static class DB_Test
//    {
//        public static void Test()
//        {
//            {
//                var CDNAddress = Environment.CurrentDirectory + "..\\..\\..\\..\\CDN_Data\\Person";
//                try { System.IO.Directory.Delete(CDNAddress, true); } catch { }

//                var Person_Table = new Monsajem_Incs.Database.KeyValue.DirBased.Table<Person, string>(
//                                CDNAddress, (c) => c.name, true);
//                Person_Table.Relation((c) => c.frinds, (c) => c.IsUpdateAble = true).Join();

//                Person_Table.Insert(new Person() { name = "ali" });
//                Person_Table.Insert(new Person() { name = "reza" });
//                Person_Table.Insert((c) => c.name = "ahmad");
//                Person_Table.Insert((c) => c.name = "akbar");
//                Person_Table["ali"].Value.frinds.Accept("ahmad");
//                Person_Table["ali"].Value.frinds.Accept("akbar");
//                System.Threading.Thread.Sleep(2000);
//            }

//            {
//                var Link = new Uri("http://localhost:1098");
//                try
//                {
//                    System.IO.Directory.Delete(Environment.CurrentDirectory + "/Person", true);
//                }catch{}

//                var TBL_Persons = new Monsajem_Incs.Database.DirectoryTable.DirectoryTable<Person, string>(
//                   Environment.CurrentDirectory + "/Person", (c) => c.name, false, true);
//                TBL_Persons.Relation((c) => c.frinds, (c) => c.IsUpdateAble = false).Join();

//                Link.GetUpdate(TBL_Persons).Wait();

//                Link.GetUpdate(TBL_Persons,"ali",(c)=>c.frinds).Wait();
//                var Ali = TBL_Persons.GetItem("ali").Value;
//            }
//        }
//    }
//}