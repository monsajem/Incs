using Monsajem_Incs.Collection.Array.Base;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using System;
using System.Threading.Tasks;

namespace Monsajem_Incs.Database.KeyValue.Base
{
    public class Table<ValueType, KeyType> :
        Monsajem_Incs.Database.Base.Table<ValueType, KeyType>,
        IDisposable
        where KeyType : IComparable<KeyType>
    {
        [Monsajem_Incs.Serialization.NonSerialized]
        private bool NeedToSave = true;
        private Action Save;

        public Table(
            Action<byte[]> SaveKeys,
            Func<byte[]> LoadKeys,
            Collection.IDictionary<KeyType, (ValueType Value, ulong UpdateCode)> Data,
            Func<ValueType, KeyType> GetKey, bool IsUpdateAble) :
            base(new DynamicArray<(ValueType, ulong)>(), new Register.MemoryRegister<ulong>(), GetKey, IsUpdateAble)
        {
            var OldData = LoadKeys();

            if (OldData != null)
            {
                var OldTable = OldData.Deserialize(this);
                KeysInfo.Keys = OldTable.KeysInfo.Keys;
            }

            var Ar = (DynamicArray<(ValueType Value, ulong UpdateCode)>)BasicActions.Items;

            Ar._GetItem = (Pos) => Data[KeysInfo.Keys[Pos]];

            Ar._SetItem = (Pos, Value) =>
            {
                var OldKey = KeysInfo.Keys[Pos];
                var NewKey = GetKey(Value.Value);
                if (OldKey.CompareTo(NewKey) != 0)
                {
                    _ = Data.Remove(OldKey);
                    Data.Add(NewKey, Value);
                }
                else
                    Data[OldKey] = Value;
            };
            Ar._DeleteByPosition = (c) =>
            {
                _ = Data.Remove(KeysInfo.Keys[c]);
                Ar.Length--;
            };
            Ar._insert = (Value, pos) =>
            {
                Data.Add(KeysInfo.Keys[pos], Value);
                Ar.Length++;
            };
            Ar.Length = KeysInfo.Keys.Length;

            Save = () =>
            {
                SaveKeys(this.Serialize());
            };

            if (true == true) //is fast Save
            {
                ((Action)(async () =>
                {
                save:
                    try
                    {
                        await Task.Delay(1000);
                    }
                    catch
                    {
                        goto save;
                    }
                    if (NeedToSave == true)
                    {
                        Save();
                        NeedToSave = false;
                    }
                    goto save;
                }))();

                Events.Inserted += (info) =>
                {
                    lock (this)
                    {
                        if (NeedToSave == false)
                            NeedToSave = true;
                    }
                };
                Events.Deleted += (info) =>
                {
                    lock (this)
                    {
                        if (NeedToSave == false)
                            NeedToSave = true;
                    }
                };
                KeyChanged += (info) =>
                {
                    lock (this)
                    {
                        if (NeedToSave == false)
                            NeedToSave = true;
                    }
                };
                Events.Updated += (info) =>
                {
                    lock (this)
                    {
                        if (NeedToSave == false)
                            NeedToSave = true;
                    }
                };
            }
            else
            {
                Runer.Run.OnClosedAllBlocks += () => Save();
            }
        }

        [Monsajem_Incs.Serialization.NonSerialized]
        private bool IsDisposed;
        public void Dispose()
        {
            if (IsDisposed == false)
            {
                IsDisposed = true;
                Save();
                System.GC.SuppressFinalize(this);
            }
        }
    }
}