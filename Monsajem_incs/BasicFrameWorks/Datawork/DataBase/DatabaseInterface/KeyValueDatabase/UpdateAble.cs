using System;
using System.Collections.Generic;

namespace Monsajem_Incs.Database.Base
{
    public partial class UpdateAble<KeyType>
        where KeyType : IComparable<KeyType>
    {
        public ulong UpdateCode;
        public KeyType Key;

        public static IComparer<UpdateAble<KeyType>> CompareKey = new _CompareKey();
        private class _CompareKey : IComparer<UpdateAble<KeyType>>
        {
            public int Compare(UpdateAble<KeyType> x, UpdateAble<KeyType> y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }

        public static IComparer<UpdateAble<KeyType>> CompareCode = new _CompareCode();
        private class _CompareCode : IComparer<UpdateAble<KeyType>>
        {
            public int Compare(UpdateAble<KeyType> x, UpdateAble<KeyType> y)
            {
                return x.UpdateCode.CompareTo(y.UpdateCode);
            }
        }

        public override string ToString()
        {
            return "Key: " + Key.ToString() + " UpdateCode:" + UpdateCode;
        }
    }
    public partial class UpdateAbles<KeyType>
        where KeyType : IComparable<KeyType>
    {
        [ThreadStaticAttribute]
        public static DynamicAssembly.RunOnceInBlock[] IgnoreUpdateAble;
        public static int IgnoreUpdateAble_Len;

        public Register.Base.Register<ulong> UpdateCode;
        public Monsajem_Incs.Collection.Array.Base.IArray<UpdateAble<KeyType>> UpdateCodes;
        public Monsajem_Incs.Collection.Array.Base.IArray<UpdateAble<KeyType>> UpdateKeys;

        [Serialization.NonSerialized]
        public Action<UpdateAble<KeyType>> OnChanged;


        public UpdateAbles(
            Register.Base.Register<ulong> UpdateCode,
            IEnumerable<(KeyType Key, ulong UpdateCode)> OldCodes = null)
        {
            this.UpdateCode = UpdateCode;
            UpdateCode.Load();
            var UpdateCodes = new Collection.Array.TreeBased.Array<UpdateAble<KeyType>>
            {
                Comparer = UpdateAble<KeyType>.CompareCode
            };
            var UpdateKeys = new Collection.Array.TreeBased.Array<UpdateAble<KeyType>>
            {
                Comparer = UpdateAble<KeyType>.CompareKey
            };

            if (OldCodes != null)
                foreach (var OldCode in OldCodes)
                {
                    var Update = new UpdateAble<KeyType>()
                    { Key = OldCode.Key, UpdateCode = OldCode.UpdateCode };

                    _ = UpdateCodes.BinaryInsert(Update);
                    _ = UpdateKeys.BinaryInsert(Update);
                }
            this.UpdateCodes = UpdateCodes;
            this.UpdateKeys = UpdateKeys;
        }

        public void Clear()
        {
            UpdateCode.Save(0);
            UpdateKeys.Clear();
            UpdateCodes.Clear();
        }

        public UpdateAble<KeyType> this[KeyType Key]
        {
            get
            {
                var Place = UpdateKeys.BinarySearch(new UpdateAble<KeyType>() { Key = Key });
                return Place.Index < 0 ? null : Place.Value;
            }
        }

        public UpdateAble<KeyType> this[ulong Code]
        {
            get
            {
                var Place = System.Array.BinarySearch(UpdateCodes,
                    new UpdateAble<KeyType>() { UpdateCode = Code },
                    UpdateAble<KeyType>.CompareCode);
                return Place < 0 ? null : UpdateCodes[Place];
            }
        }

        public bool IsExist(ulong Code)
        {
            var Place = UpdateKeys.BinarySearch(new UpdateAble<KeyType>() { UpdateCode = Code });
            return Place.Index >= 0;
        }

        public void Update()
        {
            UpdateCode.Value += 1;
            UpdateCode.Save();
        }

        public void Insert(KeyType Key)
        {
            UpdateCode.Value += 1;
            UpdateCode.Save();
            Insert(Key, UpdateCode);
        }

        public void Insert(KeyType Key, ulong UpdateCode)
        {
            var Update = new UpdateAble<KeyType>() { Key = Key, UpdateCode = UpdateCode };
            var Place = UpdateKeys.BinarySearch(Update).Index;
            if (Place >= 0)
            {
                _Changed(Key, Key, UpdateCode, Place);
                return;
            }
            _Insert(Key, UpdateCode);
        }
        private void _Insert(KeyType Key, ulong UpdateCode)
        {
            var Update = new UpdateAble<KeyType>() { Key = Key, UpdateCode = UpdateCode };
            _ = UpdateCodes.BinaryInsert(Update);
            _ = UpdateKeys.BinaryInsert(Update);
            OnChanged?.Invoke(Update);
        }

        public void Delete(KeyType Key)
        {
            UpdateCode.Value += 1;
            UpdateCode.Save();
            DeleteDontUpdate(Key);
        }
        public void DeleteDontUpdate(KeyType Key)
        {
            var Place = UpdateCodes.BinarySearch(new UpdateAble<KeyType>() { Key = Key });
            if (Place.Index < 0)
                return;
            _ = UpdateKeys.BinaryDelete(Place.Value);
        }

        public void Changed(KeyType Old, KeyType New)
        {
            UpdateCode.Value += 1;
            UpdateCode.Save();
            Changed(Old, New, UpdateCode);
        }

        public void Changed(KeyType Old, KeyType New, ulong UpdateCode)
        {
            var OldPlace = UpdateKeys.BinarySearch(new UpdateAble<KeyType>() { Key = Old }).Index;
            if (OldPlace < 0)
            {
                _Insert(New, UpdateCode);
                return;
            }
            _Changed(Old, New, UpdateCode, OldPlace);
        }
        private void _Changed(KeyType Old, KeyType New, ulong UpdateCode, int OldPlace_key)
        {
            var OldUpdate = UpdateKeys[OldPlace_key];
            var NewUpdate = new UpdateAble<KeyType>() { Key = New, UpdateCode = UpdateCode };

            _ = UpdateKeys.BinaryUpdate(OldUpdate, NewUpdate);
            _ = UpdateCodes.BinaryUpdate(OldUpdate, NewUpdate);
            OnChanged?.Invoke(NewUpdate);
        }
    }

    public partial class Table<ValueType, KeyType>
    {
        [Serialization.NonSerialized]
        public Action<UpdateAbles<KeyType>> UpdateAbleChanged;


        [Serialization.NonSerialized]
        internal int IgnoreUpdateAble_pos;

        public DynamicAssembly.RunOnceInBlock IgnoreUpdateAble
        {
            get
            {
                if (UpdateAbles<KeyType>.IgnoreUpdateAble == null)
                    UpdateAbles<KeyType>.IgnoreUpdateAble =
                        new DynamicAssembly.RunOnceInBlock[UpdateAbles<KeyType>.IgnoreUpdateAble_Len];
                else if (UpdateAbles<KeyType>.IgnoreUpdateAble.Length < UpdateAbles<KeyType>.IgnoreUpdateAble_Len)
                    System.Array.Resize(ref UpdateAbles<KeyType>.IgnoreUpdateAble,
                        UpdateAbles<KeyType>.IgnoreUpdateAble_Len);

                var MY_UpdateAble =
                     UpdateAbles<KeyType>.IgnoreUpdateAble[IgnoreUpdateAble_pos];
                if (MY_UpdateAble == null)
                {
                    MY_UpdateAble = new DynamicAssembly.RunOnceInBlock();
                    UpdateAbles<KeyType>.IgnoreUpdateAble[IgnoreUpdateAble_pos] = MY_UpdateAble;
                }
                return MY_UpdateAble;
            }
        }

        [Serialization.NonSerialized]
        private Action<KeyType> UpdateAbleChanged_InRelation;

        internal UpdateAbles<KeyType> _UpdateAble;
        public UpdateAbles<KeyType> UpdateAble
        {
            get => _UpdateAble;
            set
            {
                _UpdateAble = value;
                UpdateAbleChanged?.Invoke(_UpdateAble);
            }
        }

        protected void ReadyForUpdateAble()
        {
            UpdateAbleChanged += (_UpdateAble) =>
            {
                if (_UpdateAble != null)
                {
                    Events.Inserted += (info) =>
                    {
                        if (IgnoreUpdateAble.BlockLengths == 0)
                        {
                            _UpdateAble.Insert((KeyType)info.Info[KeyPos].Key);
                        }
                    };

                    Events.Deleted += (info) =>
                    {
                        if (IgnoreUpdateAble.BlockLengths == 0)
                        {
                            _UpdateAble.Delete((KeyType)info.Info[KeyPos].Key);
                        }
                    };

                    Events.Updated += (info) =>
                    {
                        if (IgnoreUpdateAble.BlockLengths == 0)
                        {
                            var OldPos = (KeyType)info.Info[KeyPos].OldKey;
                            var NewPos = (KeyType)info.Info[KeyPos].Key;
                            _UpdateAble.Changed(OldPos, NewPos);
                        }
                    };
                }
            };
        }
    }
}