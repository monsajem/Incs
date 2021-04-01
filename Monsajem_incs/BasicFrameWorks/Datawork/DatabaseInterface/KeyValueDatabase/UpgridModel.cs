using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using static System.Runtime.Serialization.FormatterServices;
using System.Reflection;
namespace Monsajem_Incs.Database.Base
{
    public partial class Upgrider
    {
        internal static FieldInfo[] GetFields(Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            var fiels = typeToReflect.GetFields(bindingFlags);
            if (filter != null)
                fiels = fiels.Where((c) => filter(c)).ToArray();
            if (typeToReflect.BaseType != null)
            {
                Collection.Array.Extentions.Insert(ref fiels, GetFields(typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate));
            }
            return fiels;
        }

        public static void Upgrid<ValueType, KeyType,NewValueType>(
            Table<ValueType,KeyType> Oldtbl,
            Table<NewValueType, KeyType> NewTbl,
            Action<(ValueType Old, NewValueType New)> Upgrid=null)
            where KeyType:IComparable<KeyType>
        {
            var Fields=new (FieldInfo OldField, FieldInfo NewField)[0];
            
            {
                var OldFields = GetFields(typeof(ValueType));
                var NewFields = GetFields(typeof(NewValueType));
                foreach(var OldField in OldFields)
                {
                    var NewField = NewFields.Where((c) => c.Name == OldField.Name).FirstOrDefault();
                    if(NewField != null)
                    {
                        if(OldField.FieldType==NewField.FieldType)
                            Collection.Array.Extentions.Insert(ref Fields,(OldField,NewField));
                    }
                }
            }
            foreach (var Item in Oldtbl.BasicActions)
            {
                var NewItem = (NewValueType)GetUninitializedObject(typeof(NewValueType));
                foreach(var Field in Fields)
                {
                    Field.NewField.SetValue(NewItem,Field.OldField.GetValue(Item));
                }
                Upgrid?.Invoke((Item.Value, NewItem));
                NewTbl.BasicActions.Remove(Item.Key);
                NewTbl.BasicActions.Add(Item.Key,NewItem);
            }

            //if (tbl.KeysInfo.Keys.Length > 0)
            //{
            //    tbl.UpdateAble.UpdateCode++;
            //    tbl.UpdateAble.UpdateCodes = new ulong[tbl.KeysInfo.Keys.Length];
            //    for (int i = 0; i < tbl.KeysInfo.Keys.Length; i++)
            //    {
            //        tbl.UpdateAble.UpdateCodes[i] = tbl.UpdateAble.UpdateCode;
            //    }
            //    tbl.Update(tbl.First());
            //}
        }
    }
}