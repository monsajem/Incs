﻿using System;
using static Monsajem_Incs.Database.Base.Runer;

namespace Monsajem_Incs.Database.Base
{
    public partial class Table<ValueType, KeyType>
    {
        public void AddRelation<To, ToKeyType>(
            RelationTableInfo<To, ToKeyType> ThisRelation,
            Table<To, ToKeyType>.RelationTableInfo<ValueType, KeyType> ThatRelation)
            where ToKeyType : IComparable<ToKeyType>
        {
            var RelationName = ThisRelation.Link.Body.ToString() + ThatRelation.Link.Body.ToString();
            _AddRelation(RelationName, ThisRelation, ThatRelation);
            ThisRelation.LinkArray._AddRelation(RelationName, ThatRelation, ThisRelation);
        }

        public void AddRelation(
            RelationTableInfo<ValueType, KeyType> RelationLink)
        {
            var RelationName = RelationLink.Link.Body.ToString();
            _AddRelation(RelationName, RelationLink, RelationLink);
        }
        public void AddRelation(
            RelationItemInfo<ValueType, KeyType> RelationLink)
        {
            var RelationName = RelationLink.Link.Body.ToString();
            _AddRelation(RelationName, RelationLink, RelationLink);
        }

        public void AddRelation(
            RelationTableInfo<ValueType, KeyType> Relation1,
            RelationTableInfo<ValueType, KeyType> Relation2)
        {
            var RelationName = Relation1.Link.Body.ToString() + Relation2.Link.Body.ToString();
            _AddRelation(RelationName, Relation1, Relation2);
            _AddRelation(RelationName, Relation2, Relation1);
        }

        private void _AddRelation<To, ToKeyType>(
            string RelationName,
            RelationTableInfo<To, ToKeyType> ThisRelation,
            Table<To, ToKeyType>.RelationTableInfo<ValueType, KeyType> ThatRelation)
            where ToKeyType : IComparable<ToKeyType>
        {
#if TRACE
            Console.WriteLine("@ "+ this.GetType().Namespace + this.GetType().Name + " _AddRelation_X_X");
#endif

            _AddRelationForLoading(RelationName,
                ThisRelation,
                Accepted: (Key, AcceptedKey, PartTable) =>
                {
#if TRACE
                    Console.WriteLine("@ " + this.GetType().Namespace + this.GetType().Name + " _AddRelation_X_X >> Accepted");
#endif
                    using (IgnoreUpdateAble.UseBlock())
                    {
                        PartTable.SaveToParent();
                    }


                    using (ThisRelation.LinkArray.IgnoreUpdateAble.UseBlock())
                    {
                        ThisRelation.LinkArray.Update(AcceptedKey,
                        (c) => ThatRelation.Field.Value(c).Accept(Key));
                    }

                },
                Ignored: (Key, IgnoredKey, PartTable) =>
                {
#if TRACE
                    Console.WriteLine("@ " + this.GetType().Namespace + this.GetType().Name + " _AddRelation_X_X >> Ignored");
#endif
                    using (IgnoreUpdateAble.UseBlock())
                    {
                        PartTable.SaveToParent();
                    }


                    using (ThisRelation.LinkArray.IgnoreUpdateAble.UseBlock())
                    {
                        ThisRelation.LinkArray.Update(IgnoredKey,
                             (c) => ThatRelation.Field.Value(c).Ignore(Key));
                    }

                });

            KeyChanged += (info) =>
            {
#if TRACE
                Console.WriteLine("@ " + this.GetType().Namespace + this.GetType().Name + " _AddRelation_X_X >> KeyChanged");
#endif
                if (Run.Use(RelationName))
                {
                    using (ThisRelation.LinkArray.IgnoreUpdateAble.UseBlock())
                    {
                        var ThisValue = ThisRelation.Field.Value(info.Value);
                        foreach (var ThatValue in ThisValue)
                        {
                            var ThatRelationTbl = ThatRelation.Field.Value(ThatValue);
                            ThatRelationTbl.KeysInfo.Keys.BinaryDelete(info.OldKey);
                            ThatRelationTbl.KeysInfo.Keys.BinaryInsert(info.NewKey);
                            ThisRelation.LinkArray.Update(ThatValue);
                        }
                    }
                    Run.EndUse(RelationName);
                }
            };

            Events.Deleted += (info) =>
            {
#if TRACE
                Console.WriteLine("@ " + this.GetType().Namespace + this.GetType().Name + " _AddRelation_X_X >> Deleted");
#endif
                if (Run.Use(RelationName))
                {
                    using (ThisRelation.LinkArray.IgnoreUpdateAble.UseBlock())
                    {
                        ThisRelation.LinkArray.Update(ThisRelation.Field.Value(info.Value),
                                 (c) => ThatRelation.Field.Value(c).Ignore(
                                     (KeyType)info.Info[ThatRelation.Field.Value(c).KeyPos].Key));
                    }
                    Run.EndUse(RelationName);
                }
            };

            var RelationName_Update = "U" + ThisRelation.Link.ToString() + RelationName;
            if (ThisRelation.IsUpdateAble)
            {
                Events.Updated += (info) =>
                {
#if TRACE
                    Console.WriteLine("@ " + this.GetType().Namespace + this.GetType().Name + " _AddRelation_X_X >> Updated");
#endif
                    if (IgnoreUpdateAble.BlockLengths == 0)
                        if (Run.Use(RelationName_Update))
                        {
                            using (ThisRelation.LinkArray.IgnoreUpdateAble.UseBlock())
                            {
                                var ThisValue = ThisRelation.Field.Value(info.Value);
                                foreach (var ThatValue in ThisValue)
                                {
                                    ThisRelation.LinkArray.Update(ThatValue, (c) =>
                                    {
                                        var MyInfo = info.Info[KeyPos];
                                        ThatRelation.Field.Value(c).UpdateAble.Changed(
                                            (KeyType)MyInfo.OldKey, (KeyType)MyInfo.OldKey);
                                    });
                                }
                            }
                        }
                };
            }
            if (ThisRelation.ClearRelationOnSendUpdate)
                _ClearRelation(ThisRelation.Link);
            if (ThatRelation.ClearRelationOnSendUpdate)
                ThisRelation.LinkArray._ClearRelation(ThatRelation.Link);
        }
    }
}