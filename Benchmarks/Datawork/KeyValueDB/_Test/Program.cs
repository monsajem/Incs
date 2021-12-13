using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database;
using static System.IO.Directory;
using Monsajem_Incs.Serialization;
using System.Reflection;

namespace _Test
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

        public static void TestLocalStorage()
        {
            var tbl = new Monsajem_Incs.Database.KeyValue.DirBased.Table<Person, string>(
                Environment.CurrentDirectory + "\\tbl_Peson", (c) => c.Name, true);
            tbl.Insert((c) => c.Name = "ali");
            var Result2 = tbl[0].Value.Name;
        }

        static void Main(string[] args)
        {
            TestLocalStorage();
            System.Threading.Thread.Sleep(2000);
        }
    }
}