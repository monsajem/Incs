﻿using Monsajem_Incs.Serialization;
using System;
using static Monsajem_Incs.Database.Base.Runer;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Database.Base
{
    public partial class Events<ValueType>
    {
        public Action<ValueType> loading;
    }

    public partial class Table<ValueType, KeyType>
    {

        public class ValueInfo
        {
            public ValueType Value;
            public ulong UpdateCode;
            public int Pos;
            public KeyType Key;
            public Table<ValueType, KeyType> Parent;
            public static implicit operator ValueType(ValueInfo ValueInfo)
            {
                return ValueInfo.Value;
            }
        }

        public ValueInfo this[int Position]
        {
            get => GetItem(Position);
        }
        public ValueInfo this[KeyType Key]
        {
            get => GetItem(Key);
        }
        public ValueInfo this[ValueType Key]
        {
            get => GetItem(Key);
        }

        public ValueInfo GetItem(int Position, bool MakeCopy = false)
        {
            lock (this)
            {
                using (Run.UseBlock())
                {
                    var Item = BasicActions.Items[Position];
                    if (MakeCopy)
                        Item = Item.Serialize().Deserialize(Item);
                    Events.loading?.Invoke(Item.Value);
                    return new ValueInfo()
                    {
                        Value = Item.Value,
                        UpdateCode = Item.UpdateCode,
                        Pos = Position,
                        Parent = this
                    };
                }
            }
        }

        public ValueInfo GetItem(KeyType Key, bool MakeCopy = false)
        {
            lock (this)
            {
                using (Run.UseBlock())
                {
                    return GetItem(GetPosition(Key), MakeCopy);
                }
            }
        }

        public int GetPosition(KeyType Key)
        {
            lock (this)
            {
                using (Run.UseBlock())
                {
                    var Pos = KeysInfo.Keys.BinarySearch(Key).Index;
                    return Pos > -1 ? Pos : throw new ArgumentOutOfRangeException("Key", Key, "Key Not Exist");
                }
            }
        }

        public void GetItem(KeyType Key, Action<ValueInfo> Action)
        {
            var Value = GetItem(Key);
            Action(Value);
        }

        public ValueInfo GetEqualOrNextItem(KeyType Key)
        {
            lock (this)
            {
                var position = KeysInfo.Keys.BinarySearch(Key).Index;
                if (position < 0)
                {
                    position *= -1;
                    if (position > KeysInfo.Keys.Length)
                        throw new Exception("NotFound");
                    else
                        position--;
                }
                return GetItem(position);
            }
        }

        public ValueInfo GetEqualOrBeforeItem(KeyType Key)
        {
            lock (this)
            {
                var position = KeysInfo.Keys.BinarySearch(Key).Index;
                if (position < 0)
                {
                    position *= -1;
                    if (position == 1)
                        throw new Exception("NotFound");
                    else
                        position -= 2;
                }
                return GetItem(position);
            }
        }

        public ValueInfo GetItem(ValueType Value)
        {
            return GetItem(GetKey(Value));
        }

        public ValueInfo GetItemOrDefaultItem(KeyType Key)
        {
            lock (this)
            {
                ValueInfo Result = null;

                var Position = KeysInfo.Keys.BinarySearch(Key).Index;
                Result = Position > -1 ? GetItem(Position) : null;

                return Result;
            }
        }

        public PartOfTable<ValueType, KeyType> GetItems(KeyType[] Keys)
        {
            return new PartOfTable<ValueType, KeyType>
                (Keys, this);
        }

        public PartOfTable<ValueType, KeyType> GetElseItems(KeyType[] Keys)
        {
            var NewKeys = KeysInfo.Keys.MakeSameNew();
            NewKeys.BinaryDelete(Keys);

            return new PartOfTable<ValueType, KeyType>
                (NewKeys.ToArray(), this);
        }

        public PartOfTable<ValueType, KeyType> GetElseItems(Table<ValueType, KeyType> Items)
        {
            return GetElseItems(Items.KeysInfo.Keys.ToArray());
        }

        public ValueType GetOrInserItem(ValueType Value)
        {
            lock (this)
            {
                ValueType Result = default;


                var Key = GetKey(Value);
                var Position = KeysInfo.Keys.BinarySearch(Key).Index;
                if (Position > -1)
                {
                    Result = GetItem(Position).Value;
                }
                else
                {
                    _ = Insert(Value);
                    Result = this[Value].Value;
                }

                return Result;
            }

        }

        public ValueInfo GetItem(Action<ValueType> ValueAction)
        {
            var Value = (ValueType)GetUninitializedObject(typeof(ValueType));
            ValueAction(Value);
            return GetItem(Value);
        }

        public ValueType GetOrInserItem(Action<ValueType> ValueAction)
        {
            var Value = (ValueType)GetUninitializedObject(typeof(ValueType));
            ValueAction(Value);
            return GetOrInserItem(Value);
        }
    }
}