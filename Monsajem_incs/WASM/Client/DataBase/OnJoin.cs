using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;

namespace MonsajemData
{
    public abstract partial class DataBase<UserType>
    {

        private class OnJoined : RelationJoined
        {
            private void OnRelation<R1ValueType, R1KeyType, R2ValueType, R2KeyType>(
                Table<R1ValueType, R1KeyType>.RelationItemInfo<R2ValueType, R2KeyType> RLN)
                where R1KeyType:IComparable<R1KeyType>
                where R2KeyType:IComparable<R2KeyType>
            {
                var Info = Data._MakeFinder<R1ValueType, R1KeyType>(
                        FindDB(RLN.OwnerArray.GetHashCode()), 0,RLN.Link.Body.ToString());
                Info.ParentTableName = FindDB(RLN.LinkArray.GetHashCode());
                Info.GetTblOfRelation =
                           (c) => RLN.Field.Value(
                                    RLN.OwnerArray.GetItem((R1KeyType)c).Value);
            }

            private void OnRelation<R1ValueType, R1KeyType, R2ValueType, R2KeyType>(
                Table<R1ValueType, R1KeyType>.RelationTableInfo<R2ValueType, R2KeyType> RLN)
                where R1KeyType : IComparable<R1KeyType>
                where R2KeyType : IComparable<R2KeyType>
            {
                var Info = Data._MakeFinder<R2ValueType, R2KeyType>(
                        FindDB(RLN.OwnerArray.GetHashCode()), 0,RLN.Field.Field.Name);
                Info.ParentTableName = FindDB(RLN.LinkArray.GetHashCode());
                //Info.GetTblOfRelation =
                //           (c) => RLN.Field.Value(
                //                    RLN.OwnerArray.GetItem((R1KeyType)c).Value);
                //var Caption = (Caption) RLN.Field.Field.GetCustomAttributes(true).
                //                Where((c) => c.GetType() == typeof(Caption)).FirstOrDefault();
                //if (Caption == null)
                //    Caption = (Caption)typeof(R2ValueType).GetCustomAttributes(true).
                //                Where((c) => c.GetType() == typeof(Caption)).FirstOrDefault();
                //if (Caption == null)
                //    Caption = new Caption()
                //    {
                //        Name_Multy = RLN.Field.Field.Name,
                //        Name_Single = RLN.Field.Field.Name,
                //        Name_Multy_Unknown = RLN.Field.Field.Name,
                //        Name_Single_Unknown = RLN.Field.Field.Name,
                //    };
                //Info.Caption = Caption;
                //Info.RelationCaption = FindDB((FindDB(RLN.OwnerArray.GetHashCode()), "")).Caption;
                //UserControler.Partial.Page<R1ValueType,R1KeyType>.Relations.Insert((RLN.Field.Field.Name, Caption));
            
            }

            public override void OnJoin<R1ValueType, R1KeyType, R2ValueType, R2KeyType>((Table<R1ValueType, R1KeyType>.RelationItemInfo<R2ValueType, R2KeyType>, Table<R2ValueType, R2KeyType>.RelationItemInfo<R1ValueType, R1KeyType>) Relation)
            {
                OnRelation(Relation.Item1);
                OnRelation(Relation.Item2);
            }

            public override void OnJoin<R1ValueType, R1KeyType, R2ValueType, R2KeyType>((Table<R1ValueType, R1KeyType>.RelationTableInfo<R2ValueType, R2KeyType>, Table<R2ValueType, R2KeyType>.RelationItemInfo<R1ValueType, R1KeyType>) Relation)
            {
                OnRelation(Relation.Item1);
                OnRelation(Relation.Item2);
            }

            public override void OnJoin<R1ValueType, R1KeyType, R2ValueType, R2KeyType>((Table<R1ValueType, R1KeyType>.RelationItemInfo<R2ValueType, R2KeyType>, Table<R2ValueType, R2KeyType>.RelationTableInfo<R1ValueType, R1KeyType>) Relation)
            {
                OnRelation(Relation.Item1);
                OnRelation(Relation.Item2);
            }

            public override void OnJoin<R1ValueType, R1KeyType, R2ValueType, R2KeyType>((Table<R1ValueType, R1KeyType>.RelationTableInfo<R2ValueType, R2KeyType>, Table<R2ValueType, R2KeyType>.RelationTableInfo<R1ValueType, R1KeyType>) Relation)
            {
                OnRelation(Relation.Item1);
                OnRelation(Relation.Item2);
            }

            public override void OnJoin<R1ValueType, R1KeyType, R2ValueType, R2KeyType>(Table<R1ValueType, R1KeyType>.RelationTableInfo<R2ValueType, R2KeyType> Relation)
            {
                OnRelation(Relation);
            }

            public override void OnJoin<R1ValueType, R1KeyType, R2ValueType, R2KeyType>(Table<R1ValueType, R1KeyType>.RelationItemInfo<R2ValueType, R2KeyType> Relation)
            {
                OnRelation(Relation);
            }
        }
    }
}