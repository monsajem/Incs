using Monsajem_Incs.Database.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monsajem_Client
{
    public class DataBase : MonsajemData.DataBase
    {
        public DataBase(
            Monsajem_Incs.Database.Register.Base.Register<object> Register) : base(GetMaker(Register))
        {
            App.Data = this;
        }

        static Maker GetMaker(Monsajem_Incs.Database.Register.Base.Register<object> Register)
        {
            Monsajem_Incs.Database.CDN.DataRegister.Register = Register;
            return new Maker();
        }

        protected override void _Load()
        {
            MakeDB(ref Groups, "Groups", (c) => c.Name);
            (Groups.Relation((c) => c.GroupParents, (c) => c.IsUpdateAble = ISUpdateAble),
             Groups.Relation((c) => c.GroupChilds, (c) => c.IsUpdateAble = ISUpdateAble)).Join();

            Groups.Events.Updated += (e) =>
            {
                var Info = e.Info[0];
                if (Info.OldKey.ToString() != Info.Key.ToString())
                {
                    Func<Task> DBTask = null;
                    DBTask = async () =>
                    {
                        await App.RenameFile(
                                    "/GroupImages/" + Info.OldKey,
                                    "/GroupImages/" + Info.Key);
                        await App.RenameFile(
                                    "/GroupImages_ORG/" + Info.OldKey,
                                    "/GroupImages_ORG/" + Info.Key);
                        App.TasksAfterUploadDB -= DBTask;
                    };
                    App.TasksAfterUploadDB += DBTask;
                }
            };

            MakeDB(ref Products, "Products", (c) => c.ProductName);
            (Products.Relation((c) => c.GroupParents, (c) => c.IsUpdateAble = ISUpdateAble),
             Groups.Relation((c) => c.ProductChilds, (c) => c.IsUpdateAble = ISUpdateAble)).Join();

            Products.Events.Updated += (e) =>
            {
                var Info = e.Info[0];
                if (Info.OldKey.ToString() != Info.Key.ToString())
                {
                    Func<Task> DBTask = null;
                    DBTask = async () =>
                    {
                        await App.RenameFile(
                                    "/ProductImages/" + Info.OldKey,
                                    "/ProductImages/" + Info.Key);
                        await App.RenameFile(
                                    "/ProductImages_ORG/" + Info.OldKey,
                                    "/ProductImages_ORG/" + Info.Key);
                        App.TasksAfterUploadDB -= DBTask;
                    };
                    App.TasksAfterUploadDB += DBTask;
                }
            };


            MakeDB(ref SelectedProducts, "SelectedProducts", (c) => c.ProductName);

            SelectedProducts.Events.Inserted += (c) => App.SelectedProductsChanged();
            SelectedProducts.Events.Updated += (c) => App.SelectedProductsChanged();
            SelectedProducts.Events.Deleted += (c) => App.SelectedProductsChanged();
        }

        public override void ClearData()
        {
            throw new NotImplementedException();
        }

        public Table<ProductGroup, string> Groups;
        public Table<Product, string> Products;
        public Table<SelectedProduct, string> SelectedProducts;

        public override uint Ver => 1;

        public override uint LastVer { get => 1; set { } }

        public override bool ISUpdateAble => false;

        private class Maker : MonsajemData.TableMaker
        {
            public override Monsajem_Incs.Database.Base.Table<ValueType, KeyType> MakeDB<ValueType, KeyType>(string Name, Func<ValueType, KeyType> GetKey)
            {
                var res = new Monsajem_Incs.Database.CDN.CDNTable<ValueType, KeyType>(Name, GetKey);
                return res;
            }
        }
    }

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
        public int Price;

        public PartOfTable<ProductGroup, string> GroupParents;

        public Calculate_wall.Calculator.UniverseSummary Summary;
    }

    public class SelectedProduct

    {
        public double Count;
        public string PersonDescribe;
        public string ProductName;
    }
}