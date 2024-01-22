using Monsajem_Incs.Database.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using Monsajem_Client;

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
            MakeDB(ref Users_Calc, "UserCalc", (c) => c.Name);
            MakeDB(ref Products, "Products", (c) => c.ProductName);
            MakeDB(ref ProductsHaving, "ProductsHaving", (c) => c.ID);
            MakeDB(ref Transactions, "UserCalc", (c) => c.ID);
            MakeDB(ref Users_Calc, "UserCalc", (c) => c.Name);
            MakeDB(ref Users_Calc, "UserCalc", (c) => c.Name);




        }

        public override void ClearData()
        {
            throw new NotImplementedException();
        }

        public Table<Person, string> Users_Calc;
        public Table<Product, string> Products;
        public Table<ProductHaving, UInt32> ProductsHaving;
        public Table<Transaction, UInt32> Transactions;
        public Table<ShopProduct, UInt32> ShopProducts;
        public Table<SellProduct, UInt32> SellProducts;
        public Table<ShopAction, UInt32> ShopActions;
        public Table<SellAction, UInt32> SellActions;
        public Table<GetMoneyFromUser, UInt32> GetMoneyFromUsers;
        public Table<GetMoneyToUser, UInt32> GetMoneyToUsers;

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

    public class Person
    {
        public string Name;
        public string Describe;
        public string PhoneNumber;
    }

    public class Product
    {
        public string ProductName;
        public string Describe;
    }

    public class ProductHaving
    {
        public UInt32 ID;
        public PartOfTable<Product, string> Product;
        public UInt32 Value;
        public UInt32 Price;
        public string Describe;
    }

    public class Action
    {
        public string Name;
        public string Describe;
    }

    public class Transaction
    {
        public UInt32 ID;
        public DateTime SubmitDate;
        public DateTime ActionDate;
        public DateTime CalculateDate;

        public string Describe;
        public int TotalMoneyShouldGet;
        public PartOfTable<Person, string> WithUser;

        public PartOfTable<ShopProduct, UInt32>.RelationItem ShopProduct;
        public PartOfTable<SellProduct, UInt32>.RelationItem SellProduct;
        public PartOfTable<ShopAction, UInt32>.RelationItem ShopAction;
        public PartOfTable<SellAction, UInt32>.RelationItem SellAction;
        public PartOfTable<GetMoneyFromUser, UInt32>.RelationItem GetMoneyFromUser;
        public PartOfTable<GetMoneyToUser, UInt32>.RelationItem GetMoneyToUser;
    }

    public class ShopProduct
    {
        public PartOfTable<Transaction, UInt32>.RelationItem Transaction;
        public PartOfTable<Product, string>.RelationItem Product;
        public UInt32 Value;
        public UInt32 Price;
    }

    public class SellProduct
    {
        public PartOfTable<Transaction, UInt32>.RelationItem Transaction;
        public PartOfTable<Product, string>.RelationItem Product;
        public UInt32 Value;
        public UInt32 Price;
    }

    public class ShopAction
    {
        public PartOfTable<Transaction, UInt32> Transaction;
        public PartOfTable<Action, string> Action;
    }

    public class SellAction
    {
        public PartOfTable<Transaction, UInt32> Transaction;
        public PartOfTable<Action, string> Action;
    }

    public class GetMoneyFromUser
    {
        public PartOfTable<Transaction, UInt32> Transaction;
    }

    public class GetMoneyToUser
    {
        public PartOfTable<Transaction, UInt32> Transaction;
    }
}