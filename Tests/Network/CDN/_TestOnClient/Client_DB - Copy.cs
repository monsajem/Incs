using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Net.Tcp;
using System;
using System.Linq;
using System.Net;

namespace _TestOnClient
{
    public class ProductGroup
    {
        public string Name;
        public string Describe;

        public byte[] Image;
        public PartOfTable<ProductGroup, string> GroupChilds;
        public PartOfTable<ProductGroup, string> GroupParents;
        public PartOfTable<Product, string> ProductChilds;
    }

    public class Product
    {
        public string ProductName;
        public string LongDescribe;
        public string ShortDescribe;

        public PartOfTable<ProductGroup, string> GroupParents;
    }

    public class SimpleData
    {
        public string Name;
        public object Data;
    }
    public static class DB_Test
    {
        private static string DBAddress;
        private static bool ISUpdateAble;


        public class DB
        {
            private static void MakeDB<ValueType, KeyType>
                (ref Table<ValueType, KeyType> TBL, string Name, Func<ValueType, KeyType> GetKey)
                where KeyType : IComparable<KeyType>
            {
                TBL = new Monsajem_Incs.Database.KeyValue.DirBased.Table<ValueType, KeyType>(
                                     DBAddress + Name, GetKey, ISUpdateAble);
            }

            public DB MakeDB()
            {
                try { System.IO.Directory.Delete(DBAddress, true); } catch { }

                MakeDB(ref SimpleDatas, "KeyValueDatas", (c) => c.Name);

                MakeDB(ref Groups, "Groups", (c) => c.Name);
                (Groups.Relation((c) => c.GroupParents, (c) => c.IsUpdateAble = ISUpdateAble),
                 Groups.Relation((c) => c.GroupChilds, (c) => c.IsUpdateAble = ISUpdateAble)).Join();

                MakeDB(ref Products, "Products", (c) => c.ProductName);
                (Products.Relation((c) => c.GroupParents, (c) => c.IsUpdateAble = ISUpdateAble),
                 Groups.Relation((c) => c.ProductChilds, (c) => c.IsUpdateAble = ISUpdateAble)).Join();

                return this;
            }
            public Table<ProductGroup, string> Groups;
            public Table<Product, string> Products;
            public Table<SimpleData, string> SimpleDatas;
        }

        public static void Test()
        {
            ISUpdateAble = true;
            DBAddress = Environment.CurrentDirectory + "..\\..\\..\\..\\..\\CDN_Data\\DB\\";
            var ServerDB = new DB().MakeDB();

            ISUpdateAble = false;
            DBAddress = Environment.CurrentDirectory + "\\DB\\";
            var ClientDB = new DB().MakeDB();
            var Link = new Uri("http://localhost:1098/DB/");

            {
                ServerDB.Groups.Insert((c) => c.Name = "Root");
                //ClientDB.Groups.Insert((c) => c.Name = "Root");
                System.Threading.Thread.Sleep(2000);

                Link.GetUpdate(ClientDB.Products).Wait();
                Link.GetUpdate(ClientDB.Groups).Wait();

                ServerDB.Products.Insert((c) => c.ProductName = "Product");
                var Product = ServerDB.Products["Product"].Value;
                ServerDB.Groups["Root"].Value.ProductChilds.Accept("Product");

                System.Threading.Thread.Sleep(2000);

                Link.GetUpdate(ClientDB.Groups, "Root", (c) => c.ProductChilds).Wait();

                System.Threading.Thread.Sleep(2000);
                Link.GetUpdate(ClientDB.Products).Wait();
                Link.GetUpdate(ClientDB.Groups).Wait();
                Link.GetUpdate(ClientDB.Groups, "Root", (c) => c.ProductChilds).Wait();
                //Products.Insert((c) => c.ProductName = "Product2");
                //Products["Product2"].Value.Game.Insert((c) => c.Game = new byte[10]);
                //Groups["Root"].Value.ProductChilds.Accept("Product2");
            }
        }
    }
}