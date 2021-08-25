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
            var TableInfo = new TableInfo() {TableName = Table.TableName,Table = Table};
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

            private static HashSet<RelationInfo> Relations = new HashSet<RelationInfo>();

            public void AddRelation(
                string RelationName,
                Func<object, object> GetRelationByKey)
            {
                var RelationInfo = new RelationInfo() { RelationName = RelationName,
                                                        GetRelationByKey = GetRelationByKey};
                if (Relations.Contains(RelationInfo) == true)
                    throw new Exception($"Relation with name '{RelationName}' is exist at '{TableName}' of TableFinder.");
                else
                    Relations.Add(RelationInfo);
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

        public class RelationInfo : IEquatable<RelationInfo>
        {
            public string RelationName;
            public Func<object,object> GetRelationByKey;

            public bool Equals(RelationInfo other)
            {
                return RelationName == other.RelationName;
            }

            public override int GetHashCode()
            {
                return RelationName.GetHashCode();
            }
        }
    }
}
