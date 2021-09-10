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
            where KeyType:IComparable<KeyType>
        {
            await Remote(
            async (c) =>
            {
                var TableName = await c.GetData<string>();
                var Table = TableFinder.FindTable(TableName).Table as Table<ValueType, KeyType>;
                await c.SendUpdate(Table);
            },
            async (c) =>
            {
                await c.SendData(Table.TableName);
                await c.GetUpdate(Table);
            });
        }

        public static HTMLElement MakeEditView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table, KeyType Key)
            where KeyType : IComparable<KeyType>
        {
            static async Task RemoteTable(
                string TableName,
                KeyType Key,
                ValueType Value)
            {
               await Remote(() => 
               {
                   var Table = TableFinder.FindTable(TableName).Table as Table<ValueType, KeyType>;
                   if (Table == null)
                       return;
                   Table.Update(Key,(c)=>
                   {
                       Table.MoveRelations(c, Value);
                       return Value;
                   });
               });
            }

            return null;
        }

        public static HTMLElement MakeShowView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table, KeyType Key)
            where KeyType : IComparable<KeyType>
        {
            static async Task RemoteTable(
                string TableName,
                KeyType Key,
                ValueType Value)
            {
                await Remote(() =>
                {
                    var Table = TableFinder.FindTable(TableName).Table as Table<ValueType, KeyType>;
                    if (Table == null)
                        return;
                    Table.Update(Key, (c) =>
                    {
                        Table.MoveRelations(c, Value);
                        return Value;
                    });
                });
            }

            var Value = Table[Key].Value;
            return (Table,Value).MakeView();
        }

        public static RegisterEditor<ValueType, KeyType> RegisterEdit<ValueType, KeyType>(this Table<ValueType, KeyType> Table)
            where KeyType :IComparable<KeyType>
            => new RegisterEditor<ValueType, KeyType>() { Table = Table };

        public class RegisterEditor<ValueType, KeyType>
            where KeyType:IComparable<KeyType>
        {
            internal Table<ValueType, KeyType> Table;

            public void SetDefault<ViewType>(
                Action<(ViewType View,ValueType Value)> FillView = null,
                Func<ViewType, HTMLElement> GetMain = null,
                Func<(ViewType View,ValueType OldValue), ValueType> FillValue = null,
                Action<(ViewType View, Action Edited)> SetEdited = null)
                where ViewType : new()
            {
                ValueTypes.EditItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>
                    .Default_FillViewByValue =
                    (c) =>
                    {
                        ValueTypes.EditItemMaker<ValueType, ViewType>.Default_FillViewByValue
                            ((c.View, c.Value.Value));
                    };

                var _FillView = default(Action<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) Value)>);
                if (FillView != null)
                    _FillView = (c) => FillView((c.View, c.Value.Value));


                ValueTypes.EditItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>
                    .Default_MakeValueFromView =
                    (c) =>
                    {
                        var result = ValueTypes.EditItemMaker<ValueType, ViewType>.Default_MakeValueFromView
                            ((c.View,c.OldValue.Value));
                        return (c.OldValue.Table, result);
                    };

                var _FillValue = default(Func<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) OldValue), (Table<ValueType, KeyType> Table, ValueType Value)>);
                if (FillValue != null)
                    _FillValue = (c) =>(c.OldValue.Table, FillValue((c.View, c.OldValue.Value)));


                ValueTypes.EditItemMaker.MakeDefault
                    <(Table<ValueType, KeyType> Table, ValueType Key), ViewType>
                        (_FillView, GetMain, _FillValue, SetEdited);
            }
        }

        public static RegisterViewer<ValueType, KeyType> RegisterView<ValueType, KeyType>(this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
            => new RegisterViewer<ValueType, KeyType>() { Table = Table };

        public class RegisterViewer<ValueType, KeyType>
            where KeyType : IComparable<KeyType>
        {
            internal Table<ValueType, KeyType> Table;

            public void SetDefault<ViewType>(
                Action<(ViewType View, ValueType Value)> FillView = null,
                Func<ViewType, HTMLElement> GetMain = null,
                Action<(ViewType View, Action Edited)> SetEdited = null,
                Action<(ViewType View, Action Edit)> RegisterEdit = null,
                Action<(ViewType View, Action Delete)> RegisterDelete = null)
                where ViewType : new()
            {
                var ValueViewMaker = ValueTypes.ViewItemMaker<ValueType, ViewType>.Default;
                var TableViewMaker = ValueTypes.ViewItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>.Default;

                TableViewMaker.Default_FillView =
                    (c) =>
                    {
                        ValueViewMaker.Default_FillView((c.View, c.Value.Value));
                    };

                var _FillView = default(Action<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) Value)>);
                if (FillView != null)
                    _FillView = (c) => FillView((c.View, c.Value.Value));

                ValueTypes.ViewItemMaker.SetView
                    <(Table<ValueType, KeyType> Table, ValueType Key), ViewType>
                    (FillView:_FillView,
                     GetMain:GetMain,
                     RegisterEdit:RegisterEdit,
                     RegisterDelete:RegisterDelete);
            }
        }
    }
}