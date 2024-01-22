using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Collection.Array.Base;

namespace Monsajem_Incs.Database.CDN
{

    public static class DataRegister
    {
        public static Register.Base.Register<object> Register;
        public static bool AutoSave = true;

        private static bool IsLoaded;
        internal static (UpdateCodeRegister Register, DynoArray<(t,ulong)> Array) 
            GetData<t>(string Name)
        {
            if(IsLoaded==false)
            {
                if (Register == null)
                    throw new Exception("deafult register of object not set.");
                Register.Load();
                IsLoaded = true;
            }
            if(Register.Value == null)
                Register.Value = new DynoArray<(object Data, string Name)>();
            var Datas = Register.Value as DynoArray<(object Data, string Name)>;
            if (Datas.Comparer == System.Collections.Generic.Comparer<(object Data, string Name)>.Default||
                Datas.Comparer == null)
                Datas.Comparer = System.Collections.Generic.Comparer<(object Data, string Name)>.
                                    Create((c1, c2) =>
                                    {
                                        Console.WriteLine(c2.Name);
                                        return c1.Name.CompareTo(c2.Name);
                                    });

            var Pos = Datas.BinarySearch((default(object), Name));
            if (Pos.Index > -1)
                return ((UpdateCodeRegister Register, DynoArray<(t,ulong)> Array)) Pos.Value.Data;

            var Value = (new UpdateCodeRegister(), new DynoArray<(t, ulong)>());

            Datas.BinaryInsert((Value, Name));

            return Value;
        }

        internal class UpdateCodeRegister : Register.Base.Register<ulong>
        {
            [Serialization.NonSerialized]

            public UpdateCodeRegister()
            {}

            protected override ulong LoadData()
            {
                return Value;
            }

            protected override void SaveData(ulong Value)
            {
                if(DataRegister.AutoSave==true)
                    DataRegister.Register.Save();
            }
        }

        internal class DynoArray<ArrayType> :
            DynamicArray<ArrayType>,ISerializable<ArrayType[]>
        {
            private ArrayType[] Ar = new ArrayType[0];
            public DynoArray()
            {
                Ready();
            }

            private void Ready()
            {
                _DeleteByPosition = (Pos) =>
                {
                    Monsajem_Incs.Collection.Array.Extentions.DeleteByPosition(ref Ar, Pos);
                    Length--;
                    if (DataRegister.AutoSave == true)
                        DataRegister.Register.Save();
                };
                _insert = (Item, Pos) =>
                {
                    Monsajem_Incs.Collection.Array.Extentions.Insert(ref Ar, Item, Pos);
                    Length++;
                    if (DataRegister.AutoSave == true)
                        DataRegister.Register.Save();
                };
                _GetItem = (c) => Ar[c];
                _SetItem = (Pos, Value) =>
                {
                    Ar[Pos] = Value;
                    if (DataRegister.AutoSave == true)
                        DataRegister.Register.Save();
                };
                Length = Ar.Length;
            }

            public override (int Index, ArrayType Value) BinarySearch(ArrayType key, int minNum, int maxNum)
            {
                var Pos = Array.BinarySearch(Ar,minNum,maxNum,key,Comparer);
                if(Pos>-1)
                    return (Pos, Ar[Pos]);
                else
                    return (Pos, default);
            }

            public ArrayType[] GetData() => Ar;
            public void SetData(ArrayType[] Data)
            {
                Ar = Data;
                Ready();
            }
        }
    }

    public class CDNTable<ValueType, KeyType> :
        Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {
        public CDNTable(
            string TableName,
            Func<ValueType, KeyType> GetKey,
            bool SubmitTableName = true) :
            this(TableName,DataRegister.GetData<ValueType>(TableName),GetKey)
        {
            if (typeof(KeyType) == typeof(string))
            {
                KeysInfo.Keys.Comparer = (System.Collections.Generic.IComparer<KeyType>)StringComparer.InvariantCulture;
            }
            if(SubmitTableName)
                this.TableName = TableName;
            foreach (var Value in this.BasicActions.Items)
                KeysInfo.Keys.BinaryInsert(GetKey(Value.Value));
        }

        private CDNTable(string TableName,
                         (DataRegister.UpdateCodeRegister Register, DataRegister.DynoArray<(ValueType,ulong)> Array) Info,
                         Func<ValueType, KeyType> GetKey) :
            base(Info.Array,Info.Register,GetKey,false){}

    }
}