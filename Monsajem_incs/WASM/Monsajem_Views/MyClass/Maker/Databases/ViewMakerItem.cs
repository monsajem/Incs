using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Collection.Array;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Views.Maker.ValueTypes;
using static Monsajem_Client.SafeRun;
using static Monsajem_Client.Network;

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
                                await c.SendData((
                                            (string)PartTable.HolderTable.Table.TableName,
                                            PartTable.HolderTable.Key,
                                            PartTable.TableName));
                                await c.GetUpdate(Table);
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
                                await c.SendData(Table.TableName);
                                await c.GetUpdate(Table);
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
                   async (c) =>
                   {
                       await TableInfo.Update(Key, c.NewValue.Value);
                       Done?.Invoke();
                   });
        }

        public static HTMLElement MakeInsertView<ValueType, KeyType>(
                this Table<ValueType, KeyType> Table,
                Action Done = null)
            where KeyType : IComparable<KeyType>
        {
            var TableInfo = TableFinder.FindTable(Table.TableName);
            return (Table, Value: default(ValueType)).MakeEditView(
                   async (c) =>
                   {
                       await TableInfo.Insert(c.NewValue.Value);
                       Done?.Invoke();
                   });
        }

        public static HTMLElement MakeInsertView<ValueType, KeyType>(
                this PartOfTable<ValueType, KeyType> Table,
                Action Done = null)
            where KeyType : IComparable<KeyType>
        {
            var TableInfo = TableFinder.FindTable((string)Table.HolderTable.Table.TableName);
            var RelationInfo = TableInfo.FindRelation(Table.TableName);
            return (Table, Value: default(ValueType)).MakeEditView(
                   async (c) =>
                   {
                       await RelationInfo.Insert(Table.HolderTable.Key, c.NewValue.Value);
                       Done?.Invoke();
                   });
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

        public static RegisterEditor<ValueType, KeyType> RegisterEdit<ValueType, KeyType>(this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
            => new RegisterEditor<ValueType, KeyType>() { Table = Table };

        public class RegisterEditor<ValueType, KeyType>
            where KeyType : IComparable<KeyType>
        {
            internal Table<ValueType, KeyType> Table;


            public class Inputs<ViewType>
                where ViewType : new()
            {
                public Action<(ViewType View, ValueType Value)> FillView;
                public Func<ViewType, HTMLElement> GetMain;
                public Func<(ViewType View, ValueType OldValue), ValueType> FillValue;
                public Action<(ViewType View, Action Edited)> SetEdited;
            }


            public void SetDefault<ViewType>(Action<Inputs<ViewType>> FillInputs = null)
                where ViewType : new()
            {
                var Inputs = new Inputs<ViewType>();
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
                    _FillView = (c) => Inputs.FillView((c.View, c.Value.Value));


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
                    _FillValue = (c) => (c.OldValue.Table, Inputs.FillValue((c.View, c.OldValue.Value)));


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
        }

        public static RegisterViewer<ValueType, KeyType> RegisterView<ValueType, KeyType>(this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
            => new RegisterViewer<ValueType, KeyType>() { Table = Table };

        public class RegisterViewer<ValueType, KeyType>
            where KeyType : IComparable<KeyType>
        {
            internal Table<ValueType, KeyType> Table;

            public class Inputs<ViewType>
                where ViewType : new()
            {
                public Action<(ViewType View, ValueType Value)> FillView;
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

            public void SetDefault<ViewType>(Action<Inputs<ViewType>> FillInputs = null)
                where ViewType : new()
            {
                var ValueViewItemMaker = ValueTypes.ViewItemMaker<ValueType, ViewType>.Default;
                var TableViewItemMaker = ValueTypes.ViewItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>.Default;
                var Inputs = new Inputs<ViewType>();
                FillInputs?.Invoke(Inputs);


                TableViewItemMaker.Default_FillView =
                    (c) =>
                    {
                        ValueViewItemMaker.Default_FillView((c.View, c.Value.Value));
                    };

                var _FillView = default(Action<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) Value)>);
                if (Inputs.FillView != null)
                    _FillView = (c) => Inputs.FillView((c.View, c.Value.Value));

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
        }
    }
}