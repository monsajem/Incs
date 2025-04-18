﻿using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Views.Maker.ValueTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using static Monsajem_Incs.WasmClient.Network;

namespace Monsajem_Incs.Views.Maker.Database
{
    public static partial class EditItemMaker
    {
        public static async Task SyncUpdate<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            if (Table == null)
                throw new Exception("Table is null!");

            switch (Table)
            {
                case PartOfTable<ValueType, KeyType>:
                    var PartTable = Table as PartOfTable<ValueType, KeyType>;
                    await Remote(
                            async (c) =>
                            {
                                var Info = await c.GetData<(string TableName, object Key, string RealtionName)>();
                                await TableFinder.FindTable(Info.TableName)
                                                 .FindRelation(Info.RealtionName)
                                                 .SendUpdate(Info.Key, c);
                            },
                            async (c) =>
                            {
                                _ = await c.SendData((
                                            (string)PartTable.HolderTable.Table.TableName,
                                            PartTable.HolderTable.Key,
                                            PartTable.TableName));
                                _ = await c.GetUpdate(Table);
                            });
                    break;

                case Table<ValueType, KeyType>:
                    await Remote(
                            async (c) =>
                            {
                                var TableName = await c.GetData<string>();
                                await TableFinder.FindTable(TableName).SendUpdate(c);
                            },
                            async (c) =>
                            {
                                _ = await c.SendData(Table.TableName);
                                _ = await c.GetUpdate(Table);
                            });
                    break;
            }
        }

        public static HTMLElement MakeEditView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table, KeyType Key,
            Action Done = null)
            where KeyType : IComparable<KeyType>
        {
            var Value = Table[Key].Value;
            var TableInfo = TableFinder.FindTable(Table.TableName);
            return (Table, Value).MakeEditView(
                   (c) =>
                   {
                       TableInfo.Update(Key, c.NewValue.Value);
                       Done?.Invoke();
                   });
        }

        public static HTMLElement MakeInsertView<ValueType, KeyType>(
                this Table<ValueType, KeyType> Table,
                Action Done = null)
            where KeyType : IComparable<KeyType>
        {
            var PartTable = Table as PartOfTable<ValueType, KeyType>;

            if (PartTable == null)
            {
                var TableInfo = TableFinder.FindTable(Table.TableName);
                return (Table, Value: default(ValueType)).MakeEditView(
                       (c) =>
                       {
                           TableInfo.Insert(c.NewValue.Value);
                           Done?.Invoke();
                       });
            }
            else
            {
                var TableInfo = TableFinder.FindTable((string)PartTable.HolderTable.Table.TableName);
                var RelationInfo = TableInfo.FindRelation(Table.TableName);
                return (Table, Value: default(ValueType)).MakeEditView(
                       (c) =>
                       {
                           RelationInfo.Insert(PartTable.HolderTable.Key, c.NewValue.Value);
                           Done?.Invoke();
                       });
            }
        }

        public static HTMLElement MakeShowView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table, KeyType Key,
            Action<(TableFinder.TableInfo TableInfo, KeyType Key)> OnUpdate = null,
            Action<(TableFinder.TableInfo TableInfo, KeyType Key)> OnDelete = null)
            where KeyType : IComparable<KeyType>
        {
            var Value = Table[Key].Value;
            var TableInfo = TableFinder.FindTable(Table.TableName);
            return (Table, Value).MakeView(
                                    OnEdit: () => OnUpdate?.Invoke((TableInfo, Key)),
                                    OnDelete: () => OnDelete?.Invoke((TableInfo, Key)));
        }


        public static void RegisterView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table,
            Action<RegisterViewes<ValueType, KeyType>> Actions)
            where KeyType : IComparable<KeyType>
            => Actions.Invoke(new RegisterViewes<ValueType, KeyType>() { Table = Table });

        public class RegisterViewes<ValueType, KeyType>
            where KeyType : IComparable<KeyType>
        {
            internal Table<ValueType, KeyType> Table;

            public class Inputs_View<ViewType>
                where ViewType : new()
            {
                public Action<(ViewType View, ValueType Value, Table<ValueType, KeyType> Table)> FillView;
                public Func<ViewType, HTMLElement> GetMain;
                public Action<(ViewType View, Action Edit)> RegisterEdit;
                public Action<(ViewType View, Action Delete)> RegisterDelete;
                public Func<Table<ValueType, KeyType>, HTMLElement> MakeHolder
                {
                    set
                    {
                        HolderViewItemMaker<Table<ValueType, KeyType>>.MakeHolder = value;
                    }
                }

                public static Func<(Table<ValueType, KeyType> Table, HTMLElement[] Views), HTMLElement> FillHolder
                {
                    set
                    {
                        HolderViewItemMaker<Table<ValueType, KeyType>>.FillHolder = value;
                    }
                }
            }

            public void SetView<ViewType>(Action<Inputs_View<ViewType>> FillInputs = null)
                where ViewType : new()
            {
                var ValueViewItemMaker = new ValueTypes.ViewItemMaker<ValueType, ViewType>();
                var TableViewItemMaker = new ValueTypes.ViewItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>();
                ValueViewItemMaker.SetAsDefault();
                TableViewItemMaker.SetAsDefault();
                var Inputs = new Inputs_View<ViewType>();
                FillInputs?.Invoke(Inputs);


                TableViewItemMaker.Default_FillView =
                    (c) =>
                    {
                        ValueViewItemMaker.Default_FillView((c.View, c.Value.Value));
                    };

                var _FillView = default(Action<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) Value)>);
                if (Inputs.FillView != null)
                    _FillView = (c) => Inputs.FillView((c.View, c.Value.Value, c.Value.Table));

                ValueTypes.ViewItemMaker.SetView
                    <(Table<ValueType, KeyType>, ValueType), ViewType>
                    ((c) =>
                    {
                        c.FillView = _FillView;
                        c.GetMain = Inputs.GetMain;
                        c.RegisterEdit = Inputs.RegisterEdit;
                        c.RegisterDelete = Inputs.RegisterDelete;
                    });
            }


            public class Inputs_Edit<ViewType>
                where ViewType : new()
            {
                public Action<(ViewType View, ValueType Value, Table<ValueType, KeyType> Table)> FillView;
                public Func<ViewType, HTMLElement> GetMain;
                public Func<(ViewType View, ValueType OldValue, Table<ValueType, KeyType> Table), ValueType> FillValue;
                public Action<(ViewType View, Action Edited)> SetEdited;
            }

            public void SetEdit<ViewType>(Action<Inputs_Edit<ViewType>> FillInputs = null)
                where ViewType : new()
            {
                var Inputs = new Inputs_Edit<ViewType>();
                FillInputs?.Invoke(Inputs);

                ValueTypes.EditItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>
                    .Default_FillViewByValue =
                    (c) =>
                    {
                        ValueTypes.EditItemMaker<ValueType, ViewType>.Default_FillViewByValue
                            ((c.View, c.Value.Value));
                    };

                var _FillView = default(Action<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) Value)>);
                if (Inputs.FillView != null)
                    _FillView = (c) => Inputs.FillView((c.View, c.Value.Value, c.Value.Table));


                ValueTypes.EditItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>
                    .Default_MakeValueFromView =
                    (c) =>
                    {
                        var result = ValueTypes.EditItemMaker<ValueType, ViewType>.Default_MakeValueFromView
                            ((c.View, c.OldValue.Value));
                        return (c.OldValue.Table, result);
                    };

                var _FillValue = default(Func<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) OldValue), (Table<ValueType, KeyType> Table, ValueType Value)>);
                if (Inputs.FillValue != null)
                    _FillValue = (c) => (c.OldValue.Table, Inputs.FillValue((c.View, c.OldValue.Value, c.OldValue.Table)));


                ValueTypes.EditItemMaker.MakeDefault
                    <(Table<ValueType, KeyType> Table, ValueType Key), ViewType>
                        ((c) =>
                        {
                            c.FillView = _FillView;
                            c.GetMain = Inputs.GetMain;
                            c.FillValue = _FillValue;
                            c.SetEdited = Inputs.SetEdited;
                        });
            }

            public void SetSelector(
                Func<(IEnumerable<ValueType> Values, string Query), IEnumerable<ValueType>> SelectorItems)
            {
                TableFinder.FindTable(Table, Table.TableName).SelectorItems = SelectorItems;
            }

        }
    }
}