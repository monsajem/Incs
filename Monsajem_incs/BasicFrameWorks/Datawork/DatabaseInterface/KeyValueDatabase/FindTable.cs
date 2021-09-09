using Monsajem_Incs.Net.Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Database.Base
{
    internal class TableFinder
    {
        private static HashSet<TableInfo> Tables = new HashSet<TableInfo>();

        public static void AddTable<ValueType,KeyType>(Table<ValueType,KeyType> Table)
            where KeyType:IComparable<KeyType>
        {
            var TableInfo = new TableInfo<KeyType,ValueType>() {TableName = Table.TableName,Table = Table};
            if (Tables.Contains(TableInfo) == true)
                throw new Exception($"Table with name '{Table.TableName}' is exist at TableFinder.");
            else
                Tables.Add(TableInfo);
        }
        public static TableInfo FindTable(string TableName)
        {
            var TableInfo = new TableInfo() { TableName = TableName };
            if (Tables.TryGetValue(TableInfo,out TableInfo) == false)
                throw new Exception($"Table with name '{TableName}' is not found at TableFinder.");
            else
                return TableInfo;
        }

        public class TableInfo:IEquatable<TableInfo>
        {
            public string TableName;
            public object Table;

            private HashSet<RelationInfo> Relations = new HashSet<RelationInfo>();

            public virtual Task SendUpdate(IAsyncOprations Client)
            {
                throw new Exception("Not impelemented.");
            }
            public virtual Task GetUpdate(IAsyncOprations Client)
            {
                throw new Exception("Not impelemented.");
            }

            public RelationInfo<HolderKeyType, RelationKeyType, RelationValueType>
                AddRelation<HolderKeyType, RelationKeyType, RelationValueType>(
                string RelationName)
                where HolderKeyType:IComparable<HolderKeyType>
                where RelationKeyType : IComparable<RelationKeyType>
            {
                var RelationInfo = new RelationInfo<HolderKeyType, RelationKeyType, RelationValueType>() 
                                        { RelationName = RelationName };
                if (Relations.Contains(RelationInfo) == true)
                    throw new Exception($"Relation with name '{RelationName}' is exist at '{TableName}' of TableFinder.");
                else
                    Relations.Add(RelationInfo);
                return RelationInfo;
            }
            public RelationInfo FindRelation(string RelationName)
            {
                var RelationInfo = new RelationInfo() { RelationName = RelationName };
                if (Relations.TryGetValue(RelationInfo, out RelationInfo) == false)
                    throw new Exception($"Relation with name '{RelationName}' is not found at '{TableName}' of TableFinder.");
                else
                    return RelationInfo;
            }

            public bool Equals(TableInfo other)
            {
                return TableName == other.TableName;
            }

            public override int GetHashCode()
            {
                return TableName.GetHashCode();
            }
        }

        public class TableInfo<KeyType,ValueType>:TableInfo
            where KeyType:IComparable<KeyType>
        {
            public override async Task SendUpdate(IAsyncOprations Client)
            {
                var Table = this.Table as Table<ValueType, KeyType>;
                await Client.SendUpdate(Table);
            }
            public override async Task GetUpdate(IAsyncOprations Client)
            {
                var Table = this.Table as Table<ValueType, KeyType>;
                await Client.GetUpdate(Table);
            }
        }

        public class RelationInfo : IEquatable<RelationInfo>
        {
            public string RelationName;

            public virtual object GetRelationTableByKey(object HolderKey)
            {
                throw new Exception("Not impelemented.");
            }
            public virtual Task SendUpdate(object HolderKey, IAsyncOprations Client) 
            {
                throw new Exception("Not impelemented.");
            }
            public virtual Task GetUpdate(object HolderKey, IAsyncOprations Client) 
            {
                throw new Exception("Not impelemented.");
            }

            public bool Equals(RelationInfo other)
            {
                return RelationName == other.RelationName;
            }

            public override int GetHashCode()
            {
                return RelationName.GetHashCode();
            }
        }

        public class RelationInfo<HolderKeyType,RelationKeyType,RelationValueType> : RelationInfo
            where HolderKeyType : IComparable<HolderKeyType>
            where RelationKeyType : IComparable<RelationKeyType>
        {

            public override async Task SendUpdate(object HolderKey, IAsyncOprations Client)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await Client.SendUpdate(Table);
            }

            public override async Task GetUpdate(object HolderKey, IAsyncOprations Client)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await Client.GetUpdate(Table);
            }

            public override object GetRelationTableByKey(object HolderKey)
            {
                return GetterRealtionByKey((HolderKeyType)HolderKey);
            }

            public Func<HolderKeyType, PartOfTable<RelationValueType,RelationKeyType>> 
                GetterRealtionByKey = (c)=> throw new Exception("Not impelemented.");
        }
    }
}
