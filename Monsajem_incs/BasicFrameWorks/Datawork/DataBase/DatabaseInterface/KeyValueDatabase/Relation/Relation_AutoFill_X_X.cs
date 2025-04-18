﻿using System;
using System.Linq.Expressions;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {

        public static void Join<ValueType, KeyType, RelationValueType, RelationKeyType, FillerValueType, FillerKeyType>(
            this ValueTuple<Table<RelationValueType, RelationKeyType>.RelationItemInfo<ValueType, KeyType>,
                            Table<ValueType, KeyType>.RelationTableInfo<RelationValueType, RelationKeyType>> Relation,
            Table<FillerValueType, FillerKeyType> Filler,
            Expression<Func<FillerValueType, PartOfTable<RelationValueType, RelationKeyType>>> SourceLink,
            Expression<Func<FillerValueType, PartOfTable<ValueType, KeyType>>> DestintionLink)
            where KeyType : IComparable<KeyType>
            where RelationKeyType : IComparable<RelationKeyType>
            where FillerKeyType : IComparable<FillerKeyType>
        {
            Relation.Join();
            Relation.Item2.Fill(Filler, SourceLink, DestintionLink);
        }
        public static void Join<ValueType, KeyType, RelationValueType, RelationKeyType, FillerValueType, FillerKeyType>(
            this ValueTuple<Table<RelationValueType, RelationKeyType>.RelationItemInfo<ValueType, KeyType>,
                            Table<ValueType, KeyType>.RelationTableInfo<RelationValueType, RelationKeyType>> Relation,
            Table<FillerValueType, FillerKeyType> Filler,
            Expression<Func<FillerValueType, PartOfTable<ValueType, KeyType>>> DestintionLink,
            Expression<Func<FillerValueType, PartOfTable<RelationValueType, RelationKeyType>>> SourceLink)
            where KeyType : IComparable<KeyType>
            where RelationKeyType : IComparable<RelationKeyType>
            where FillerKeyType : IComparable<FillerKeyType>
        {
            Relation.Join();
            Relation.Item2.Fill(Filler, SourceLink, DestintionLink);
        }

        public static void Join<ValueType, KeyType, RelationValueType, RelationKeyType, FillerValueType, FillerKeyType>(
            this ValueTuple<Table<RelationValueType, RelationKeyType>.RelationTableInfo<ValueType, KeyType>,
                            Table<ValueType, KeyType>.RelationItemInfo<RelationValueType, RelationKeyType>> Relation,
            Table<FillerValueType, FillerKeyType> Filler,
            Expression<Func<FillerValueType, PartOfTable<ValueType, KeyType>>> DestintionLink,
            Expression<Func<FillerValueType, PartOfTable<RelationValueType, RelationKeyType>>> SourceLink)
            where KeyType : IComparable<KeyType>
            where RelationKeyType : IComparable<RelationKeyType>
            where FillerKeyType : IComparable<FillerKeyType>
        {
            Relation.Join();
            Relation.Item1.Fill(Filler, DestintionLink, SourceLink);
        }
        public static void Join<ValueType, KeyType, RelationValueType, RelationKeyType, FillerValueType, FillerKeyType>(
            this ValueTuple<Table<RelationValueType, RelationKeyType>.RelationTableInfo<ValueType, KeyType>,
                            Table<ValueType, KeyType>.RelationItemInfo<RelationValueType, RelationKeyType>> Relation,
            Table<FillerValueType, FillerKeyType> Filler,
            Expression<Func<FillerValueType, PartOfTable<RelationValueType, RelationKeyType>>> SourceLink,
            Expression<Func<FillerValueType, PartOfTable<ValueType, KeyType>>> DestintionLink)
            where KeyType : IComparable<KeyType>
            where RelationKeyType : IComparable<RelationKeyType>
            where FillerKeyType : IComparable<FillerKeyType>
        {
            Relation.Join();
            Relation.Item1.Fill(Filler, DestintionLink, SourceLink);
        }

        public static void Join<ValueType, KeyType, RelationValueType, RelationKeyType, FillerValueType, FillerKeyType>(
            this ValueTuple<Table<RelationValueType, RelationKeyType>.RelationTableInfo<ValueType, KeyType>,
                            Table<ValueType, KeyType>.RelationTableInfo<RelationValueType, RelationKeyType>> Relation,
            Table<FillerValueType, FillerKeyType> Filler,
            Expression<Func<FillerValueType, PartOfTable<RelationValueType, RelationKeyType>>> SourceLink,
            Expression<Func<FillerValueType, PartOfTable<ValueType, KeyType>>> DestintionLink)
            where KeyType : IComparable<KeyType>
            where RelationKeyType : IComparable<RelationKeyType>
            where FillerKeyType : IComparable<FillerKeyType>
        {
            Relation.Join();
            Relation.Item2.Fill(Filler, SourceLink, DestintionLink);
        }
        public static void Join<ValueType, KeyType, RelationValueType, RelationKeyType, FillerValueType, FillerKeyType>(
            this ValueTuple<Table<RelationValueType, RelationKeyType>.RelationTableInfo<ValueType, KeyType>,
                            Table<ValueType, KeyType>.RelationTableInfo<RelationValueType, RelationKeyType>> Relation,
            Table<FillerValueType, FillerKeyType> Filler,
            Expression<Func<FillerValueType, PartOfTable<ValueType, KeyType>>> DestintionLink,
            Expression<Func<FillerValueType, PartOfTable<RelationValueType, RelationKeyType>>> SourceLink)
            where KeyType : IComparable<KeyType>
            where RelationKeyType : IComparable<RelationKeyType>
            where FillerKeyType : IComparable<FillerKeyType>
        {
            Relation.Join(Filler, SourceLink, DestintionLink);
        }
    }

}
