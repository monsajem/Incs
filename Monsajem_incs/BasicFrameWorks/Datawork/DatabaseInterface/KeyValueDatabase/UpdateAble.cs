using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
using Monsajem_Incs.Serialization;

namespace Monsajem_Incs.Database.Base
{
    public partial class UpdateAble<ValueType, KeyType>
    {
        [ThreadStaticAttribute]
        public static DynamicAssembly.RunOnceInBlock[] IgnoreUpdateAble;
        public static int IgnoreUpdateAble_Len;

        public void Update()
        {
            UpdateCode += 1;
        }

        public void Update(int Position)
        {
            UpdateCode += 1;
                UpdateCodes[Position] = UpdateCode;
        }

        public void Insert(int Position)
        {
            UpdateCode += 1;
                ArrayExtentions.ArrayExtentions.Insert(
                    ref UpdateCodes, UpdateCode, Position);
        }

        public void Delete(int Position)
        {
            UpdateCode += 1;
                ArrayExtentions.ArrayExtentions.DeleteByPosition(
                    ref UpdateCodes, Position);
        }

        public void ChangePosition(int OldPosition,int NewPosition)
        {
            UpdateCode += 1;
                ArrayExtentions.ArrayExtentions.DeleteByPosition(
                    ref UpdateCodes, OldPosition);
                ArrayExtentions.ArrayExtentions.Insert(
                    ref UpdateCodes, UpdateCode, NewPosition);
        }

        public ulong Get(int Position)
        {
            return UpdateCodes[Position];
        }

        public ulong UpdateCode;
        public ulong[] UpdateCodes= new ulong[0];
    }

    public partial class Table<ValueType, KeyType>
    {
        [Serialization.NonSerialized]
        public Action<UpdateAble<ValueType, KeyType>> UpdateAbleChanged;


        [Serialization.NonSerialized]
        internal int IgnoreUpdateAble_pos;

        [Serialization.NonSerialized]
        public DynamicAssembly.RunOnceInBlock IgnoreUpdateAble
        {
            get
            {
                if (UpdateAble<ValueType, KeyType>.IgnoreUpdateAble == null)
                    UpdateAble<ValueType, KeyType>.IgnoreUpdateAble =
                        new DynamicAssembly.RunOnceInBlock[UpdateAble<ValueType, KeyType>.IgnoreUpdateAble_Len];
                else if (UpdateAble<ValueType, KeyType>.IgnoreUpdateAble.Length < UpdateAble<ValueType, KeyType>.IgnoreUpdateAble_Len)
                    System.Array.Resize(ref UpdateAble<ValueType, KeyType>.IgnoreUpdateAble, 
                        UpdateAble<ValueType, KeyType>.IgnoreUpdateAble_Len);

                var MY_UpdateAble =
                     UpdateAble<ValueType, KeyType>.IgnoreUpdateAble[IgnoreUpdateAble_pos];
                if (MY_UpdateAble == null)
                {
                    MY_UpdateAble = new DynamicAssembly.RunOnceInBlock();
                    UpdateAble<ValueType, KeyType>.IgnoreUpdateAble[IgnoreUpdateAble_pos] = MY_UpdateAble;

                }
                return MY_UpdateAble;
            }
        }

        protected UpdateAble<ValueType, KeyType> _UpdateAble;
        public UpdateAble<ValueType, KeyType> UpdateAble
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
                        if(IgnoreUpdateAble.BlockLengths==0)
                        {
                            _UpdateAble.Insert(info.Info[KeyPos].Pos);
                        }
                    };

                    Events.Deleted += (info) =>
                    {
                        if (IgnoreUpdateAble.BlockLengths ==0)
                        {
                            _UpdateAble.Delete(info.Info[KeyPos].Pos);
                        }
                    };

                    Events.Updated += (info) =>
                    {
                        if (IgnoreUpdateAble.BlockLengths == 0)
                        {
                            var OldPos = info.Info[KeyPos].OldPos;
                            var NewPos = info.Info[KeyPos].Pos;
                            if (OldPos < NewPos)
                            {
                                NewPos -= 1;
                            }
                            _UpdateAble.ChangePosition(OldPos,NewPos);
                        }
                    };
                }
            };
        }
    }
}