using Monsajem_Incs.Database.Base;
using System;

namespace MonsajemData
{

    public class User<UserType>
        where UserType : User<UserType>
    {
        public string ID;

        public Table<Permitions<UserType>, string>.RelationItem Permitions;
    }

    public class Permition
    {
        public string TableName;
        public string RelationName;
        public bool Read;
        public bool Insert;
        public bool Update;
        public bool Delete;
        public bool Accept;
        public bool Ignore;
    }

    public class Permitions<UserType>
        where UserType : User<UserType>
    {
        public Table<UserType, string>.RelationItem User;
        public Permition[] Accept;
    }

    public abstract class DataBase : DataBase<DataBase.User>
    {
        protected DataBase(TableMaker tableMaker) : base(tableMaker)
        { }
        public class User : User<User>
        { }
    }

    public abstract partial class DataBase<UserType>
        where UserType : User<UserType>
    {
        public readonly TableMaker tableMaker;
        private static DataBase<UserType> Data;
        protected DataBase(TableMaker tableMaker)
        {
            if (Data != null)
                throw new Exception("App Database Declared.");
            Data = this;
            this.tableMaker = tableMaker;
            Load();
        }

        public abstract uint Ver { get; }
        public abstract uint LastVer { get; set; }
        public abstract void ClearData();
        public abstract bool ISUpdateAble { get; }

        private void Load()
        {
            BeforeLoad();
            _Load();
            AfterLoad();
        }

        protected abstract void _Load();

        private void BeforeLoad()
        {
            if (Ver != LastVer)
            {
                ClearData();
                LastVer = Ver;
            }

            DefaultRelationConfigs.DefaultTableInfo = (c) => c.IsUpdateAble = ISUpdateAble;

            MakeDB(ref Users, "T_Users", (c) => c.ID);

            MakeDB(ref Permitions, "T_Permitions", (c) => c.User.Key.ToString());
            (Users.Relation((c) => c.Permitions), Permitions.Relation((c) => c.User)).Join();

            Users.Events.Inserted += ((UserType Value, Events<UserType>.ValueInfo[]) c) =>
            {
            };
        }

        private void AfterLoad()
        {
            RelationItemInfo.ReadyFill();
        }

        public Table<UserType, string> Users;
        public Table<Permitions<UserType>, string> Permitions;
    }
}