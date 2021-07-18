using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.DirectoryTable;
using WebAssembly.Browser.DOM;
using static WASM_Global.Publisher;

namespace Monsajem_Client
{
    public class FileData
    {
        public static string MainFilesAddess;
        private string DirName;
        private string FileName;

        public FileData(string DirName, string FileName)
        {
            System.IO.Directory.CreateDirectory($"{MainFilesAddess}\\{DirName}");
            this.DirName = DirName;
            this.FileName = FileName;
        }

        public string NetAddress { get => $"{App.Client}{MainFilesAddess}/{DirName}/{FileName}"; }
        public void Save(byte[] Data)
        {
            System.IO.File.WriteAllBytes(
                    $"{MainFilesAddess}\\{DirName}\\{FileName}", Data);
        }
        public byte[] Load()=>System.IO.File.ReadAllBytes($"{MainFilesAddess}\\{DirName}\\{FileName}");
        
        public void Delete()
        {
            try
            {
                System.IO.File.Delete(
                    $"{MainFilesAddess}\\{DirName}\\{FileName}");
            }
            catch { }
        }
    }

    public abstract class DataBase : MonsajemData.DataBase
    {
        public DataBase(MonsajemData.TableMaker Maker,
            string MainFilesAddess) : base(Maker) 
        {
            App.Data = this;
            FileData.MainFilesAddess = MainFilesAddess;
        }
        protected override void _Load()
        {
            MakeDB(ref SimpleDatas, "KeyValueDatas", (c) => c.Name);

            MakeDB(ref Groups, "Groups", (c) => c.Name);
            (Groups.Relation((c) => c.GroupParents, (c) => c.IsUpdateAble = ISUpdateAble),
             Groups.Relation((c) => c.GroupChilds, (c) => c.IsUpdateAble = ISUpdateAble)).Join();

            MakeDB(ref Products, "Products", (c) => c.ProductName);
            (Products.Relation((c) => c.GroupParents, (c) => c.IsUpdateAble = ISUpdateAble),
             Groups.Relation((c) => c.ProductChilds, (c) => c.IsUpdateAble = ISUpdateAble)).Join();
        }

        public Table<ProductGroup, string> Groups;
        public Table<Product, string> Products;
        public Table<SimpleData, string> SimpleDatas;
    }

    internal class ClientDataBase : DataBase
    {
        public ClientDataBase(Storage WebStorage) : 
            base(new Maker() { WebStorage = WebStorage }, "/Files/") 
        {}

        private static Storage WebStorage;
        public override bool MakeView => true;
        public override bool ISUpdateAble => false;
        public override uint Ver => 2;
        public override uint LastVer
        {
            get
            {
                try
                {
                    return uint.Parse(WebStorage.GetItem("DV"));
                }
                catch
                {
                    LastVer = 1;
                    return 1;
                }
            }
            set => WebStorage.SetItem("DV", value.ToString());
        }

        public override void ClearData()
        {
            WebStorage.Clear();
        }

        protected class Maker : MonsajemData.TableMaker
        {
            public Storage WebStorage
            {
                get => ClientDataBase.WebStorage;
                set => ClientDataBase.WebStorage = value;
            }
            public override Monsajem_Incs.Database.Base.Table<ValueType, KeyType> MakeDB<ValueType, KeyType>(string Name, Func<ValueType, KeyType> GetKey)
            {
                return new Monsajem_Incs.Database.KeyValue.WebStorageBased.Table<ValueType, KeyType>(Name, GetKey, false,WebStorage);
            }
        }
    }

    public class ProductGroup
    {
        public string Name;
        public string Describe;

        public FileData ImageFile { get => new FileData("GroupImages",Name); }

        public PartOfTable<ProductGroup, string> GroupChilds;
        public PartOfTable<ProductGroup, string> GroupParents;
        public PartOfTable<Product, string> ProductChilds;
    }

    public class Product
    {
        public string ProductName;
        public string LongDescribe;
        public string ShortDescribe;

        public FileData ImageFile { get => new FileData("GameImages",ProductName); }
        public FileData GameFile { get => new FileData("Games", ProductName); }

        public PartOfTable<ProductGroup, string> GroupParents;
    }

    public class SimpleData
    {
        public string Name;
        public object Data;
    }
}