using Monsajem_Incs.Net.Base.Service;
using System;
using System.Linq;

namespace Monsajem_Incs.Database.Base
{
    public interface IFactualyData
    {
        object Parent { get; set; }
    }

    public partial class BasicActions<ValueType>
    {
        public Collection.Array.Base.IArray<(ValueType Value, ulong UpdateCode)> Items;
        public Register.Base.Register<ulong> UpdateCode;
    }

    public class Runer
    {
        [ThreadStatic]
        private static Monsajem_Incs.DynamicAssembly.RunOnceInBlock _Run;

        public static Monsajem_Incs.DynamicAssembly.RunOnceInBlock Run
        {
            get
            {
                _Run ??= new Monsajem_Incs.DynamicAssembly.RunOnceInBlock();
                return _Run;
            }
        }
    }

    public partial class Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {

        [Remotable]
        [Serialization.NonSerialized]
        public Func<ValueType, KeyType> GetKey;
        public KeyInfo KeysInfo = new();

        [Serialization.NonSerialized]
        public BasicActions<ValueType> BasicActions;
        [Serialization.NonSerialized]
        public Events<ValueType> Events;
        [Serialization.NonSerialized]
        public Action<ValueType> AutoFillRelations;
        [Serialization.NonSerialized]
        public SecurityEvents<ValueType> SecurityEvents;

        [Serialization.NonSerialized]
        private string _TableName;
        internal string TableName
        {
            get => _TableName;
            set
            {
                var Done = false;
                var OldName = _TableName;
                try
                {
                    _TableName = value;
                    if (GetType() != typeof(PartOfTable<ValueType, KeyType>))
                        TableFinder.AddTable(this);
                    Done = true;
                }
                finally
                {
                    if (Done == false)
                        _TableName = OldName;
                }
            }
        }

        protected int KeyPos;

        internal Table(BasicActions<ValueType> BasicActions,
                       Func<ValueType, KeyType> GetKey,
                       KeyType[] XNewKeys,
                       int KeyPos,
                       bool IsUnique,
                       bool IsUpdateAble)
        {
            this.GetKey = GetKey;
            this.BasicActions = BasicActions;
            this.KeyPos = KeyPos;

            KeysInfo.Keys = new Collection.Array.TreeBased.Array<KeyType>(XNewKeys);

            SecurityEvents = new SecurityEvents<ValueType>();
            Events = new Events<ValueType>();

            SecurityEvents.MakeKeys += (uf) =>
            {
                uf.Info[KeyPos].OldKey = this.GetKey(uf.Value);
            };

            if (IsUnique)
            {
                SecurityEvents.Updating += (uf) =>
                {
                    var MyInfo = uf.Info[KeyPos];
                    var MyValue = uf.Value;
                    var OldKey = (KeyType)MyInfo.OldKey;
                    var NewKey = this.GetKey(MyValue);
                    var OldPos = KeysInfo.Keys.BinarySearch(OldKey).Index;
                    var NewPos = 0;
                    if (OldKey.CompareTo(NewKey) != 0)
                    {
                        NewPos = KeysInfo.Keys.BinarySearch(NewKey).Index;
                        if (NewPos > -1)
                            throw new InvalidOperationException("Value be exist!");
                        NewPos *= -1;
                        NewPos -= 1;
                        KeyChanging?.Invoke(new KeyChangeInfo()
                        {
                            NewKey = NewKey,
                            OldKey = OldKey,
                            Value = MyValue
                        });
                    }
                    else
                        NewPos = OldPos;
                    MyInfo.OldPos = OldPos;
                    MyInfo.Key = NewKey;
                    MyInfo.Pos = NewPos;
                };
            }
            else
            {
                SecurityEvents.Updating += (uf) =>
                {
                    var MyInfo = uf.Info[KeyPos];
                    var MyValue = uf.Value;
                    var OldKey = (KeyType)MyInfo.OldKey;
                    var NewKey = this.GetKey(MyValue);
                    var OldPos = KeysInfo.Keys.BinarySearch(OldKey).Index;
                    var NewPos = 0;
                    if (OldKey.CompareTo(NewKey) != 0)
                    {
                        NewPos = KeysInfo.Keys.BinarySearch(NewKey).Index;
                        if (NewPos < 0)
                        {
                            NewPos *= -1;
                            NewPos -= 1;
                        }
                        KeyChanging?.Invoke(new KeyChangeInfo()
                        {
                            NewKey = NewKey,
                            OldKey = OldKey,
                            Value = MyValue
                        });
                    }
                    else
                        NewPos = OldPos;
                    MyInfo.OldKey = OldKey;
                    MyInfo.OldPos = OldPos;
                    MyInfo.Key = NewKey;
                    MyInfo.Pos = NewPos;
                };
            }

            if (KeyPos == 0)
            {
                Events.Updated += (uf) =>
                {
                    var MyInfo = uf.Info[KeyPos];
                    var MyValue = uf.Value;
                    var OldKey = (KeyType)MyInfo.OldKey;
                    var NewKey = (KeyType)MyInfo.Key;
                    var OldPos = MyInfo.OldPos;
                    var NewPos = MyInfo.Pos;
                    if (OldPos < NewPos)
                    {
                        NewPos -= 1;
                    }

                    if (OldPos != NewPos)
                    {
                        var OldValue = this.BasicActions.Items.Pop(OldPos);
                        this.BasicActions.Items.Insert((MyValue, OldValue.UpdateCode), NewPos);
                    }
                    else
                    {
                        var OldValue = this.BasicActions.Items[OldPos];
                        this.BasicActions.Items[OldPos] = (MyValue, OldValue.UpdateCode);
                    }

                    if (OldKey.CompareTo(NewKey) != 0)
                    {
                        KeysInfo.Keys.DeleteByPosition(OldPos);
                        KeysInfo.Keys.Insert(NewKey, NewPos);
                        KeyChanged?.Invoke(new KeyChangeInfo()
                        {
                            NewKey = NewKey,
                            OldKey = OldKey,
                            Value = MyValue
                        });
                    }
                };
            }
            else
            {
                Events.Updated += (uf) =>
                {
                    var MyInfo = uf.Info[KeyPos];
                    var OldKey = (KeyType)MyInfo.OldKey;
                    var NewKey = (KeyType)MyInfo.Key;
                    var OldPos = MyInfo.OldPos;
                    var NewPos = MyInfo.Pos;
                    if (OldPos < NewPos)
                    {
                        NewPos -= 1;
                    }
                    if (OldKey.CompareTo(NewKey) != 0)
                    {
                        KeysInfo.Keys.DeleteByPosition(OldPos);
                        KeysInfo.Keys.Insert(NewKey, NewPos);
                        KeyChanging?.Invoke(new KeyChangeInfo()
                        {
                            NewKey = NewKey,
                            OldKey = OldKey,
                            Value = uf.Value
                        });
                    }
                };
            }

            if (IsUnique)
            {
                SecurityEvents.Inserting += (info) =>
                {
                    var NewKey = this.GetKey(info.Value);
                    var NewPos = KeysInfo.Keys.BinarySearch(NewKey).Index;
                    if (NewPos > -1)
                        throw new InvalidOperationException("Value be exist!");
                    NewPos *= -1;
                    NewPos -= 1;
                    var MyInfo = info.Info[KeyPos];
                    MyInfo.Key = NewKey;
                    MyInfo.Pos = NewPos;
                };
            }
            else
            {
                SecurityEvents.Inserting += (info) =>
                {
                    var NewKey = this.GetKey(info.Value);
                    var NewPos = KeysInfo.Keys.BinarySearch(NewKey).Index;
                    if (NewPos < 0)
                    {
                        NewPos *= -1;
                        NewPos -= 1;
                    }
                    var MyInfo = info.Info[KeyPos];
                    MyInfo.Key = NewKey;
                    MyInfo.Pos = NewPos;
                };
            }

            if (KeyPos == 0)
            {
                Events.Inserted += (info) =>
                {
                    var MyInfo = info.Info[KeyPos];
                    var Pos = MyInfo.Pos;
                    KeysInfo.Keys.Insert((KeyType)MyInfo.Key, Pos);
                    this.BasicActions.Items.Insert((info.Value, 0), Pos);
                };
            }
            else
            {
                Events.Inserted += (info) =>
                {
                    var MyInfo = info.Info[KeyPos];
                    KeysInfo.Keys.Insert((KeyType)MyInfo.Key, MyInfo.Pos);
                };
            }

            SecurityEvents.Deleting += (info) =>
            {
                var MyInfo = info.Info[KeyPos];
                //if (MyInfo.Key==null)
                //{
                var OldKey = this.GetKey(info.Value);
                var OldPos = KeysInfo.Keys.BinarySearch(OldKey).Index;
                MyInfo.Key = OldKey;
                MyInfo.Pos = OldPos;
                //}
            };

            if (KeyPos == 0)
            {
                Events.Deleted += (info) =>
                {
                    var Pos = info.Info[KeyPos].Pos;
                    this.BasicActions.Items.DeleteByPosition(Pos);
                    KeysInfo.Keys.DeleteByPosition(Pos);
                };
            }
            else
            {
                Events.Deleted += (info) =>
                {
                    KeysInfo.Keys.DeleteByPosition(info.Info[KeyPos].Pos);
                };
            }



            if (typeof(ValueType).GetInterfaces().Where((c) =>
                c == typeof(IFactualyData)).Count() > 0)
            {
                Events.loading += (NewValue) =>
                {
                    ((IFactualyData)NewValue).Parent = this;
                };
                Events.Saving += (NewValue) =>
                {
                    ((IFactualyData)NewValue).Parent = null;
                };
            }

            if (IsUpdateAble)
            {
                ReadyForUpdateAble();
                UpdateAble = new UpdateAbles<KeyType>(BasicActions.UpdateCode, BasicActions.Items.Select((c) => (GetKey(c.Value), c.UpdateCode)));
                UpdateAble.OnChanged += (c) =>
                {
                    var Pos = GetPosition(c.Key);
                    var Info = BasicActions.Items[Pos];
                    Info.UpdateCode = c.UpdateCode;
                    BasicActions.Items[Pos] = Info;
                };
            }
            IgnoreUpdateAble_pos = UpdateAbles<KeyType>.IgnoreUpdateAble_Len;
            UpdateAbles<KeyType>.IgnoreUpdateAble_Len++;

            IUpdateOrInsert = (OldKey, NewCreator) =>
            {
                return PositionOf(OldKey) > -1 ? Update(OldKey, NewCreator) : Insert(NewCreator).value;
            };
        }

        internal Table() { }

        public Table(
            Collection.Array.Base.IArray<(ValueType Value, ulong UpdateCode)> Items,
            Register.Base.Register<ulong> UpdateCodeRegister,
            Func<ValueType, KeyType> GetKey,
            bool IsUpdateAble) :
            this(new BasicActions<ValueType>()
            {
                Items = Items,
                UpdateCode = UpdateCodeRegister,
            }, GetKey, new KeyType[0], 0, true, IsUpdateAble)
        { }


        public override string ToString()
        {
            return "Table " + typeof(ValueType).ToString();
        }

    }
}
