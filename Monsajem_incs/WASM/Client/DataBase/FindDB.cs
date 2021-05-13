using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using static Monsajem_Client.Network;

namespace MonsajemData
{
    public class DataBaseInfo
    {
        public string TableName;
        public string RelationName;
        public string ParentTableName;
        public object Key;
        public int HashCode;
        public object Tbl;
        public Func<object, object> GetTblOfRelation;
        public Func<Task<HTMLDivElement>> ReadyShow;
        public Func<Task<HTMLDivElement>> ReadyAccept;
        public Func<Task<HTMLDivElement>> ReadyIgnore;
        public Func<Task<HTMLDivElement>> Readyinsert;
        public Func<object, Task<HTMLDivElement>> ReadyUpdate;
        public Func<(string RelationName, object Key), DataBaseInfo> GetRelation;
        public Caption RelationCaption;
        public Caption Caption;
        public class DataBaseInfo_name :
        IComparable<DataBaseInfo_name>
        {
            public DataBaseInfo Info;
            public int CompareTo(DataBaseInfo_name other)
            {
                var Result = Info.TableName.CompareTo(other.Info.TableName);
                if (Result == 0)
                    return Info.RelationName.CompareTo(other.Info.RelationName);
                return Result;
            }
        }
        public class DataBaseInfo_Hash :
            IComparable<DataBaseInfo_Hash>
        {
            public DataBaseInfo Info;
            public int CompareTo(DataBaseInfo_Hash other)
            {
                return Info.HashCode.CompareTo(other.Info.HashCode);
            }
        }

        public static Func<(string TableName, string RelationName), DataBaseInfo> FindDB;
        public static Func<int, string> FindDB_Hash;
        public static Func<Task> UpdatePermitions;
        public static Func<string,Permition[]> GetPermitions;
        public static Action ClearDatabase;
    }

    public abstract partial class DataBase<UserType>
    {
        public static Array<DataBaseInfo.DataBaseInfo_name> AllDataBases { get => DBS_N; }
        protected static Array<DataBaseInfo.DataBaseInfo_name> DBS_N =
            new Array<DataBaseInfo.DataBaseInfo_name>(10);
        protected static Array<DataBaseInfo.DataBaseInfo_Hash> DBS_H =
            new Array<DataBaseInfo.DataBaseInfo_Hash>(10);

        public class DataBaseInfo<ValueType, KeyType>:DataBaseInfo
            where KeyType : IComparable<KeyType>
        {
            public UserControler.Partial.Page<ValueType, KeyType>.ShowPage Show_PG;
            public UserControler.Partial.Page<ValueType, KeyType>.insertPage Insert_PG;
            public UserControler.Partial.Page<ValueType, KeyType>.UpdatePage Update_PG;
        }
        public static DataBaseInfo FindDB((string TableName, string RelationName) Info)
        {
            var Pos = DBS_N.BinarySearch(new DataBaseInfo.DataBaseInfo_name()
            {
                Info = new DataBaseInfo()
                {
                    TableName = Info.TableName,
                    RelationName = Info.RelationName
                }
            }).Index;
            var DB = DBS_N[Pos].Info;
            return DB;
        }
        public static string FindDB(int Hash)
        {
            var Pos = DBS_H.BinarySearch(new DataBaseInfo.DataBaseInfo_Hash()
            {
                Info = new DataBaseInfo() { HashCode = Hash }
            }).Index;
            return DBS_H[Pos].Info.TableName;
        }

        private DataBaseInfo<ValueType,KeyType> _MakeFinder<ValueType, KeyType>(
            string Name, int Hash, string RelationName)
            where KeyType : IComparable<KeyType>
        {
            var Show_PG = new UserControler.Partial.Page<ValueType, KeyType>.ShowPage();
            var Insert_PG = new UserControler.Partial.Page<ValueType, KeyType>.insertPage();
            var Update_PG = new UserControler.Partial.Page<ValueType, KeyType>.UpdatePage();
            var Select_Add_PG = new UserControler.Partial.Page<ValueType, KeyType>.SelectPageForAdd();
            var Select_Drop_PG = new UserControler.Partial.Page<ValueType, KeyType>.SelectPageForDrop();
            var Info = new DataBaseInfo<ValueType,KeyType>()
            {
                Show_PG=Show_PG,
                Insert_PG=Insert_PG,
                Update_PG=Update_PG,
                RelationName = RelationName,
                HashCode = Hash,
                TableName = Name,
                ParentTableName=Name,
                ReadyShow = Show_PG.Ready,
                Readyinsert = Insert_PG.Ready,
                ReadyUpdate = Update_PG.Ready,
                ReadyAccept =Select_Add_PG.Ready,
                ReadyIgnore = Select_Drop_PG.Ready
            };
            Show_PG.DBInfo = Info;
            Insert_PG.DBInfo = Info;
            Update_PG.DBInfo = Info;
            Select_Add_PG.DBInfo = Info;
            Select_Drop_PG.DBInfo = Info;
            DBS_N.BinaryInsert(new DataBaseInfo.DataBaseInfo_name()
            {
                Info = Info
            });
            DBS_H.BinaryInsert(new DataBaseInfo.DataBaseInfo_Hash()
            {
                Info = Info
            });
            return Info;
        }

        private async Task GetPermitionsUpdate()
        {
            await Remote((c) =>
            {
                c.SendUpdate(Users);
                c.SendUpdate(Permitions);
            },
            async (c) =>
            {
                await c.GetUpdate(Users);
                await c.GetUpdate(Permitions);
            });
        }


    }
}