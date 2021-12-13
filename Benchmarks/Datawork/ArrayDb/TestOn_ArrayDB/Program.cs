using System.Linq;
using Monsajem_Incs.Database;
using Monsajem_Incs.Database.Base;
using System;
using System.Threading.Tasks;
using Monsajem_Incs.Collection.Array.ArrayBased.Hyper;

namespace TestOn_ArrayDB
{
    class Program
    {
        public class Person
        {
            public string Name;
            public int id;
            public PartOfTable<Product, string>.RelationItem Product;
            public PartOfTable<Product, string> Products;
            public PartOfTable<Product, string> Products2;
        }

        public class Product
        {
            public string Name;
            public PartOfTable<Person, string>.RelationItem Person;
            public PartOfTable<Person, string>.RelationItem Person2;
            public PartOfTable<Person, string> Persons;
        }

        static void Main(string[] args)
        {
            //SafeTest();
            PerformanceTest();
        }

        public static void SimpleTest()
        {
            var Persons = new ArrayTable<Person, string>(
                (person) => person.Name, false);

            Persons.Insert((c) => c.Name = "babak");
            Persons.Insert((c) => c.Name = "aal");
            Persons.Insert((c) => c.Name = "ali");

            Persons.Update((c) => c.Name = "aal", (c) => c.Name = "bba");
            Persons.Update((c) => c.Name = "babak", (c) => c.Name = "abl");

            Console.ReadKey();
        }

        //public static void HoleTest()
        //{
        //    var Persons = DbMaker.Make<Person, string>(
        //        (person) => person.Name,false);
        //    var PersonById = Persons.MakeUniqueKey((c) => c.id);
        //    var Products = DbMaker.Make<Product, string>(
        //        (Product) => Product.Name,false);
        //    Products.AddRelation((c) => c.Person, Persons, (c) => c.Product);//1 to 1
        //    Products.AddRelation((c) => c.Persons,false, Persons, (c) => c.Products,false);//m to n
        //    Products.AddRelation((c) => c.Person2, Persons, (c) => c.Products2,false);// 1 to n

        //    Persons.Delete();
        //    Products.Delete();

        //    Products.Insert(new Product() { Name = "p1" });

        //    Products.Update((c) => c.Name = "p1", (c) => c.Persons.Insert(new Person() { Name = "ali", id = 12 }));//m to n
        //    Products.Update((c) => c.Name = "p1", (c) => c.ToString());
        //    Products.Update((c) => c.Name = "p1", (c) => c.Person.Value = Persons.GetItem(new Person() { Name = "ali" }));//1 to 1
        //    Products.Update((c) => c.Name = "p1", (c) => c.Person2.Value = Persons.GetItem(new Person() { Name = "ali" }));//n to 1

        //    var Ipr = Products.First();
        //    var Ips = Persons.First();

        //    Console.WriteLine(Ipr.Person.Value.Name);
        //    Console.WriteLine(Ips.Product.Value.Name);

        //    Console.WriteLine(Ips.Products.First().Name);
        //    Console.WriteLine(Ipr.Persons.First().Name);

        //    PersonById.Insert(new Person() { Name = "ala", id = 13 });

        //    //Persons.Delete(new Person() { Name = "ahmad" });

        //    var psid = PersonById.GetItem(new Person() { id = 12 });

        //    Console.ReadKey();
        //}

        public static void PerformanceTest()
        {
            var Persons = new ArrayTable<Person, string>(
                (person) => person.Name, false);
            var Count = 1000000;
            var InsertTime = Monsajem_Incs.TimeingTester.Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Persons.Insert((c) => c.Name = i.ToString());
                }
            });

            var UpdateTime = Monsajem_Incs.TimeingTester.Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Persons.Update((c) => c.Name = i.ToString(), (c) => c.id = i);
                }
            });

            var DeleteTime = Monsajem_Incs.TimeingTester.Timing.run(() =>
            {
                for (int i = 0; i < Count; i++)
                {
                    Persons.Delete((c) => c.Name = i.ToString());
                }
            });

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
            public Action Insert;
            public Action Update;
            public Action Delete;
            public int Id;

            public int CompareTo(ActionOn other)
            {
                var c = Id - other.Id;
                return c;
            }
        }
        public static void SafeTest()
        {
            var Persons = new ArrayTable<Person2, string>(
                 (person) => person.Name, false);
            var count = 100000;
            Persons.ToArray();
            Array<ActionOn> Actions = new Array<ActionOn>();

            for (int i = 0; i < count; i++)
            {
                var Id = i;
                var CurrentAction = new ActionOn() { Id = Id };
                CurrentAction.Insert = () =>
                  Persons.Insert((c) => c.Name = Id.ToString());
                CurrentAction.Update = () =>
                  Persons.Update((c) => c.Name = Id.ToString(), (c) => c.id = Id);
                CurrentAction.Delete = () =>
                  Persons.Delete((c) => c.Name = Id.ToString());
                Actions.Insert(CurrentAction);
            }


            Random random = new Random();
            while (Actions.Length > 0)
            {
                var i = random.Next(0, Actions.Length - 1);
                var Ac = Actions[i];
                if (Ac.Insert != null)
                {
                    Ac.Insert();
                    Ac.Insert = null;
                }
                else if (Ac.Update != null)
                {
                    Ac.Update();
                    Ac.Update = null;
                }
                else
                {
                    Ac.Delete();
                    Actions.DeleteByPosition(i);
                }
            }
            Console.ReadKey();
        }
    }
}
