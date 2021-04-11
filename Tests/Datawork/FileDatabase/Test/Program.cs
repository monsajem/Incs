using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.DirectoryTable;
using System;
using System.Threading;
using System.Linq;
using static System.Environment;
using Monsajem_Incs.TimeingTester;

namespace Test
{
    public class Program
    {
        public class Person
        {
            public string Name;
            public string Describe;
            public PartOfTable<Product, string> Products_M_N;
            public PartOfTable<Product2, string> Products;
        }

        public class Product
        {
            public string Name;
            public string Describe;
            public PartOfTable<Product2, string> Product2;
            public PartOfTable<Person, string> Persons_M_N;
        }
        public class Product2
        {
            public string Name;
            public string Describe;
            public PartOfTable<Product, string>.RelationItem Product;
            public PartOfTable<Person, string>.RelationItem Persons_M_N;
        }

        static void Main(string[] args)
        {
            var Address = Environment.CurrentDirectory + "\\Cache";
            try { System.IO.File.Delete(Address); } catch { }
            Monsajem_Incs.Serialization.StreamCacheSerialize.Stream =
                new System.IO.FileStream(Address, System.IO.FileMode.CreateNew);
            Test();
            //PerformanceTestMixed();
            //PerformanceTest();
        }

        public static void Test()
        {
            //PerformanceTest();
            try { System.IO.File.Delete(CurrentDirectory + "/Persons/Data"); } catch { }
            try { System.IO.File.Delete(CurrentDirectory + "/Persons/PK"); } catch { }
            try { System.IO.File.Delete(CurrentDirectory + "/Products/Data"); } catch { }
            try { System.IO.File.Delete(CurrentDirectory + "/Products/PK"); } catch { }

            var Persons = new DirectoryTable<Person, string>(
                CurrentDirectory + "/Persons",
                (person) => person.Name, true, true);

            var Products = new DirectoryTable<Product, string>(
                CurrentDirectory + "/Products",
                (Product) => (string)Product.Name, true, true);

            //(Products.Relation((c) => c.Person_1_1), Persons.Relation((c) => c.Product_1_1)).Join();//1 to 1
            (Products.Relation((c) => c.Persons_M_N), Persons.Relation((c) => c.Products_M_N)).Join();//m to n
                                                                                                      //(Products.Relation((c) => c.Person_1_X), Persons.Relation((c) => c.Products_X_1)).Join();// 1 to n

            var X = Persons.AsEnumerable().ToArray();
            var Y = Products.AsEnumerable().ToArray();


            Persons.Delete();
            Products.Delete();


            Persons.Insert((c) => c.Name = "ali");

            Products.Insert((c) =>
            {
                c.Name = "p1";
            });

            Products.GetItem((c) => c.Name = "p1").Value.
                Persons_M_N.Accept("ali");//m to n

            //Products.Update((c) => c.Name = "p1", (c) =>
            //{
            //    c.Person_1_1.Key = "ali";
            //});//1 to 1
            //Products.Update((c) => c.Name = "p1",
            //    (c) => c.Person_1_X.Value = Persons.GetItem((c2) => c2.Name = "ali"));//n to 1
            //                                                                          //Persons.GetItem("ali").Value.
            //                                                                          //    Products_X_1.Accept(Products.GetItem(0));



            //Products.Delete();
            //Persons.Delete();

            Products.Update((c) => c.Name = "p2");
            Persons.Update((c) => c.Name = "ahmad");

            var Ips1 = Persons.First();
            var Ipr1 = Products.GetItem(0);

            //Products.Delete(0);
            //Products.Update((c) => c.Name = "p2");
            //Persons.Update((c) => c.Name = "ahmad");

            var Ipr = Products.GetItem(0);
            //var Ips = Persons.First();

            //Console.WriteLine(Ipr.Person_1_1.Value.Name);
            //Console.WriteLine(Ips.Product_1_1.Value.Name);

            //Console.WriteLine(Ips.Products_M_N.First().Name);
            //Console.WriteLine(Ipr.Persons_M_N.First().Name);

            X = Persons.AsEnumerable().ToArray();

            Console.ReadKey();
        }

        public static void Test2()
        {
            //PerformanceTest();
            System.IO.Directory.Delete(CurrentDirectory + "/Test2/", true);

            var Persons = new DirectoryTable<Person, string>(
                CurrentDirectory + "/Test2/Persons",
                (person) => person.Name, true, true);

            var Products = new DirectoryTable<Product, string>(
                CurrentDirectory + "/Test2/Products",
                (Product) => (string)Product.Name, true, true);

            var Products2 = new DirectoryTable<Product2, string>(
                CurrentDirectory + "/Test2/Products2",
                (Product) => (string)Product.Name, true, true);


            (Products.Relation((c) => c.Persons_M_N), Persons.Relation((c) => c.Products_M_N)).
                Join();//m to n



            (Products2.Relation((c) => c.Persons_M_N),
             Persons.Relation((c) => c.Products)).
                Join(Products, (c) => c.Persons_M_N, (c) => c.Product2);// 1 to n
            (Products2.Relation((c) => c.Product), Products.Relation((c) => c.Product2)).
                Join(Persons, (c) => c.Products_M_N, (c) => c.Products);// 1 to n

            RelationItemInfo.ReadyFill();

            Persons.Delete();
            Products.Delete();


            Persons.Insert((c) => c.Name = "ali");

            Products.Insert((c) =>
            {
                c.Name = "p1";
            });
            Products2.Insert((c) =>
            {
                c.Name = "p2";
            });

            Products.GetItem((c) => c.Name = "p1").Value.
                Persons_M_N.Accept("ali");//m to n
            Products.GetItem((c) => c.Name = "p1").Value.
                            Product2.Accept("p2");//m to n


            var Ips1 = Persons.First();
            var Ipr1 = Products.GetItem(0);

            //Products.Delete(0);
            //Products.Update((c) => c.Name = "p2");
            //Persons.Update((c) => c.Name = "ahmad");

            var Ipr = Products2.GetItem(0);
            //var Ips = Persons.First();

            //Console.WriteLine(Ipr.Person_1_1.Value.Name);
            //Console.WriteLine(Ips.Product_1_1.Value.Name);

            //Console.WriteLine(Ips.Products_M_N.First().Name);
            //Console.WriteLine(Ipr.Persons_M_N.First().Name);

            Console.ReadKey();
        }

        public class Person2
        {
            public string Name;
            public int id;
        }

        public class ActionOn :
            IComparable<ActionOn>
        {
            public bool Insert;
            public bool Update;
            public bool Delete;
            public int Id;

            public int CompareTo(ActionOn other)
            {
                var c = Id - other.Id;
                return c;
            }
        }

        public static void PerformanceTestMixed()
        {
            try
            {
                System.IO.Directory.Delete(CurrentDirectory + "/Persons2", true);
                System.IO.Directory.Delete(CurrentDirectory + "/Persons3", true);
            }
            catch { }
            var Persons2 = new DirectoryTable<Person2, string>(
                CurrentDirectory + "/Persons2",
                (person) => person.Name, false, true);
            var count = 1000000;
            Array<ActionOn> Actions = new Array<ActionOn>();
            for (int i = 0; i < count; i++)
            {
                var CurrentAction = new ActionOn() { Id = i };
                CurrentAction.Insert = true;
                //CurrentAction.Update = true;
                CurrentAction.Delete = true;
                Actions.Insert(CurrentAction);
            }

            Array<string> Inserted = new Array<string>();
            Random random = new Random();
            while (Actions.Length > 0)
            {
                var i = random.Next(0, Actions.Length - 1);
                var Ac = Actions[i];
                if (Ac.Insert)
                {
                    Persons2.Insert((c) => c.Name = Ac.Id.ToString());
                    Ac.Insert = false;
                }
                else if (Ac.Update)
                {
                    Persons2.Update((c) => c.Name = Ac.Id.ToString(), (c) => c.id = Ac.Id);
                    Ac.Update = false;
                }
                else
                {
                    Persons2.Delete((c) => c.Name = Ac.Id.ToString());
                    Actions.DeleteByPosition(i);
                }
            }

            Console.ReadKey();
        }

        public static void PerformanceTest()
        {
            try
            {
                System.IO.Directory.Delete(CurrentDirectory + "/Persons2", true);
                System.IO.Directory.Delete(CurrentDirectory + "/Persons3", true);
            }
            catch { }
            var Persons = new DirectoryTable<Person2, string>(
                CurrentDirectory + "/Persons2",
                (person) => person.Name, false, true);
            var Count = 1000000;

            var InsertTime = Timing.run(() =>
            {
                for (int i = Count - 1; i > -1; i--)
                {
                    Persons.Insert((c) => { c.Name = i.ToString(); c.id = i; });
                }
            });

            var Inserts_Per_Second = (int)(Count / InsertTime.TotalSeconds);
            var EveryInsert = (InsertTime.TotalSeconds / Count).ToString("0.##########");
            var EveryInsert_Milisecond = ((InsertTime.TotalSeconds / Count) * 1000).ToString("0.##########");

            var UpdateTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Persons.Update((c) => { c.Name = i.ToString(); c.id = i; }, (c) => { c.Name = i.ToString(); c.id = i; });
                }
            });
            var Update1_Per_Second = (int)(Count / UpdateTime.TotalSeconds);

            var UpdateTime2 = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Persons.Update(new Person2 () { Name = i.ToString(), id = i }, (c) => { c.Name = i.ToString(); c.id = i; });
                }
            });
            var Update2_Per_Second = (int)(Count / UpdateTime2.TotalSeconds);

            var GetTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    var c = Persons.GetItem(i);
                    if (c == null)
                        throw new Exception();
                }
            });
            var GetT_Per_Second = (int)(Count / GetTime.TotalSeconds);

            var DeleteTime = Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Persons.Delete(new Person2() { Name = i.ToString() });
                }
            });
            var Delete_Per_Second = (int)(Count / DeleteTime.TotalSeconds);

            Console.ReadKey();
        }
    }
}
