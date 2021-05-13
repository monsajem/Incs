using System;
using MonsajemData;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.KeyValue;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Net.Web;
using Monsajem_Incs.Net.Base.Service;
using WebAssembly.Browser.DOM;
using System.Reflection;
using System.Threading.Tasks;
using static WASM_Global.Publisher;

namespace MonsajemData
{

    [Caption(Name_Single = "فرد", Name_Multy = "افراد", Name_Single_Unknown = "فردی", Name_Multy_Unknown = "افرادی")]
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
        where UserType:User<UserType>
    {
        public Table<UserType, string>.RelationItem User;
        public Permition[] Accept;
    }

    public abstract class DataBase:DataBase<DataBase.User>
    {
        protected DataBase(TableMaker tableMaker):base(tableMaker)
        {}
        public class User:User<User>
        {}
    }

    public abstract partial class DataBase<UserType>
        where UserType:User<UserType>
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

        public abstract bool MakeView { get; }
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

            if(MakeView)
            {
                DataBaseInfo.FindDB = FindDB;
                DataBaseInfo.FindDB_Hash = FindDB;
                RelationJoined.Make = () => new OnJoined();
                DataBaseInfo.UpdatePermitions = GetPermitionsUpdate;
            }
            DataBaseInfo.ClearDatabase = ClearData;


            DefaultRelationConfigs.DefaultTableInfo = (c) => c.IsUpdateAble = ISUpdateAble;

            MakeDB(ref Users, "T_Users", (c) => c.ID);

            MakeDB(ref Permitions, "T_Permitions", (c) => c.User.Key.ToString());
            (Users.Relation((c) => c.Permitions), Permitions.Relation((c) => c.User)).Join();

            Users.Events.Inserted += ((UserType Value, Events<UserType>.ValueInfo[]) c) =>
            {
                Permitions.Insert((p) =>
                {
                    p.User.Key = c.Value.ID;
                    p.Accept = new Permition[DBS_N.Length];
                    for (int i = 0; i < DBS_N.Length; i++)
                    {
                        var DB = DBS_N[i];
                        p.Accept[i] = new Permition
                        {
                            TableName = DB.Info.TableName,
                            RelationName = DB.Info.RelationName
                        };
                    }
                });
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