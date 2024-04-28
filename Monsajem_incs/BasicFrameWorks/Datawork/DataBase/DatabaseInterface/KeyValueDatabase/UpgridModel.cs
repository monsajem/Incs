using System;
using System.Linq;
using System.Reflection;
using static System.Runtime.Serialization.FormatterServices;
namespace Monsajem_Incs.Database.Base
{
    public partial class DatabaseUpgrider
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


        public static void UpgridDatabase(
            object OldDataBase,
            object NewDatabase)
        {
            var Fields = new (FieldInfo OldField, FieldInfo NewField)[0];

            {

                static bool IsAssignableToGenericType(Type givenType, Type genericType)
                {
                    var interfaceTypes = givenType.GetInterfaces();

                    foreach (var it in interfaceTypes)
                    {
                        if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                            return true;
                    }

                    if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                        return true;

                    Type baseType = givenType.BaseType;
                    return baseType == null ? false : IsAssignableToGenericType(baseType, genericType);
                }

                var OldFields = GetFields(OldDataBase.GetType());
                var NewFields = GetFields(NewDatabase.GetType());
                foreach (var OldField in OldFields)
                {
                    var NewField = NewFields.Where((c) => c.Name == OldField.Name)
                                            .Where((c) => c.FieldType.IsGenericType == true)
                                            .Where((c) => c.FieldType.GetGenericTypeDefinition() == typeof(Table<,>))
                                            .FirstOrDefault();
                    if (NewField != null)
                    {
                        Collection.Array.Extentions.Insert(ref Fields, (OldField, NewField));
                    }
                }
            }

            var UpgradeValues_Generic = typeof(DatabaseUpgrider).GetMethod(nameof(DatabaseUpgrider.UpgridTableValues));
            if (UpgradeValues_Generic == null)
                throw new Exception("UpgradeValues not Found!");

            var UpgradeRelations_Generic = typeof(DatabaseUpgrider).GetMethod(nameof(DatabaseUpgrider.UpgridTableRelations));
            if (UpgradeRelations_Generic == null)
                throw new Exception("UpgradeValues not Found!");


            foreach (var FieldInfo in Fields)
            {
                var Upgrader = UpgradeValues_Generic.
                        MakeGenericMethod(
                            FieldInfo.OldField.FieldType.GetGenericArguments()[0],
                            FieldInfo.OldField.FieldType.GetGenericArguments()[1],
                            FieldInfo.NewField.FieldType.GetGenericArguments()[0]);
                _ = Upgrader.Invoke(null,
                    new object[]{
                        FieldInfo.OldField.GetValue(OldDataBase),
                        FieldInfo.NewField.GetValue(NewDatabase),null});
            }

            foreach (var FieldInfo in Fields)
            {
                var Upgrader = UpgradeRelations_Generic.
                        MakeGenericMethod(
                            FieldInfo.OldField.FieldType.GetGenericArguments()[0],
                            FieldInfo.OldField.FieldType.GetGenericArguments()[1],
                            FieldInfo.NewField.FieldType.GetGenericArguments()[0]);
                _ = Upgrader.Invoke(null,
                    new object[]{
                        FieldInfo.OldField.GetValue(OldDataBase),
                        FieldInfo.NewField.GetValue(NewDatabase),null});
            }
        }

        public static void UpgridTableValues<ValueType, KeyType, NewValueType>(
            Table<ValueType, KeyType> Oldtbl,
            Table<NewValueType, KeyType> NewTbl,
            Action<(ValueType Old, NewValueType New)> Upgrid = null)
            where KeyType : IComparable<KeyType>
        {
            var Fields = new (FieldInfo OldField, FieldInfo NewField)[0];

            {

                static bool IsAssignableToGenericType(Type givenType, Type genericType)
                {
                    var interfaceTypes = givenType.GetInterfaces();

                    foreach (var it in interfaceTypes)
                    {
                        if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                            return true;
                    }

                    if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                        return true;

                    Type baseType = givenType.BaseType;
                    return baseType == null ? false : IsAssignableToGenericType(baseType, genericType);
                }

                var OldFields = GetFields(typeof(ValueType));
                var NewFields = GetFields(typeof(NewValueType));
                foreach (var OldField in OldFields)
                {
                    var NewField = NewFields.Where((c) => c.Name == OldField.Name).FirstOrDefault();
                    if (NewField != null &&
                       IsAssignableToGenericType(NewField.FieldType, typeof(Table<,>)) == false &&
                       IsAssignableToGenericType(NewField.FieldType, typeof(Table<,>.RelationItem)) == false)
                    {
                        if (OldField.FieldType == NewField.FieldType)
                            Collection.Array.Extentions.Insert(ref Fields, (OldField, NewField));
                    }
                }
            }
            for (int i = 0; i < Oldtbl.KeysInfo.Keys.Length; i++)
            {
                var Item = Oldtbl.BasicActions.Items[i].Value;
                var NewItem = (NewValueType)GetUninitializedObject(typeof(NewValueType));
                foreach (var Field in Fields)
                {
                    Field.NewField.SetValue(NewItem, Field.OldField.GetValue(Item));
                }
                Upgrid?.Invoke((Item, NewItem));
                _ = NewTbl.Insert(NewItem);
            }
        }

        public static void UpgridTableRelations<ValueType, KeyType, NewValueType>(
            Table<ValueType, KeyType> Oldtbl,
            Table<NewValueType, KeyType> NewTbl,
            Action<(ValueType Old, NewValueType New)> Upgrid = null)
            where KeyType : IComparable<KeyType>
        {
            var Fields = new Action<(object OldValue, object NewValue)>[0];

            {
                var OldFields = GetFields(typeof(ValueType));
                var NewFields = GetFields(typeof(NewValueType));
                foreach (var OldField in OldFields)
                {
                    var NewField = NewFields.Where((c) => c.Name == OldField.Name).FirstOrDefault();
                    if (NewField != null)
                    {
                        if (OldField.FieldType.IsGenericType &&
                                NewField.FieldType.IsGenericType)
                        {
                            var OldFieldGeneric = OldField.FieldType.GetGenericTypeDefinition();
                            var NewFieldGeneric = NewField.FieldType.GetGenericTypeDefinition();

                            if (OldFieldGeneric == typeof(PartOfTable<,>) &&
                                NewFieldGeneric == typeof(PartOfTable<,>) &&
                                OldField.FieldType.GetGenericArguments()[1] ==
                                NewField.FieldType.GetGenericArguments()[1])
                            {
                                Collection.Array.Extentions.Insert(ref Fields, (c) =>
                                {
                                    var NewItemPartTable = (dynamic)NewField.GetValue(c.NewValue);
                                    var OldItemPartTable = (dynamic)OldField.GetValue(c.OldValue);
                                    if (OldItemPartTable != null)
                                        NewItemPartTable.KeysInfo.Keys = OldItemPartTable.KeysInfo.Keys;
                                });
                            }
                            else if (OldField.FieldType.GetGenericTypeDefinition() == typeof(Table<,>.RelationItem) &&
                                     NewField.FieldType.GetGenericTypeDefinition() == typeof(Table<,>.RelationItem) &&
                                     OldField.FieldType.GetGenericArguments()[1] ==
                                     NewField.FieldType.GetGenericArguments()[1])
                            {
                                Collection.Array.Extentions.Insert(ref Fields, (c) =>
                                {
                                    var NewItem = (dynamic)NewField.GetValue(c.NewValue);
                                    var OldItem = (dynamic)OldField.GetValue(c.OldValue);
                                    NewItem.Key = OldItem.Key;
                                });
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < Oldtbl.KeysInfo.Keys.Length; i++)
            {
                var Item = Oldtbl.BasicActions.Items[i].Value;
                var NewItem = NewTbl[Oldtbl.GetKey(Item)].Value;
                foreach (var Field in Fields)
                {
                    Field((Item, NewItem));
                }
                Upgrid?.Invoke((Item, NewItem));
                NewTbl.Update(NewItem);
            }
        }
    }
}