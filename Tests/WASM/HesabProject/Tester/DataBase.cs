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
          
        }

        static Maker GetMaker(Monsajem_Incs.Database.Register.Base.Register<object> Register)
        {
            Monsajem_Incs.Database.CDN.DataRegister.Register = Register;
            return new Maker();
        }

        protected override void _Load()
        {
            MakeDB(ref Persons, "Person", (c) => c.Name);
            //Persons = new Monsajem_Incs.Database.Stream.StreamTable<Person, string>
            //    ("Person", new System.IO.MemoryStream(), (c) => c.Name, false); 

            Persons.Events.Updated += (e) =>
            {

            };

            MakeDB(ref Transactions, "Products", (c) => c.Code);

    //        Transactions = new Monsajem_Incs.Database.Stream.StreamTable<Transaction,UInt32>
    //("Transactions", new System.IO.MemoryStream(), (c) => c.Code, false);

            (Persons.Relation((c) => c.Transactions),
             Transactions.Relation((c) => c.Person, (c) =>c.IsChild=true)).Join();
        }

        public override void ClearData()
        {
            throw new NotImplementedException();
        }

        public Table<Person, string> Persons;
        public Table<Transaction, UInt32> Transactions;

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
        public string Number;
        public string Name;
        public string Describe;

        public PartOfTable<Transaction, UInt32> Transactions;
    }

    public class Transaction
    {
        public UInt32 Code;
        public Int64 Value;
        public string Describe;
        public UInt16 LastPicId;
        public UInt16[] PicIds;

        public PartOfTable<Person, string>.RelationItem Person;
    }
}