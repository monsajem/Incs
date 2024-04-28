using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Views.Maker.ValueTypes;
using System;
using System.Linq;
using WebAssembly.Browser.DOM;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Monsajem_Incs.Views.Maker.Database
{
    public static partial class EditItemMaker
    {

        public static HTMLElement MakeShowView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table,
            string Query = null,
            Action<(TableFinder.TableInfo TableInfo, KeyType Key)> OnUpdate = null,
            Action<(TableFinder.TableInfo TableInfo, KeyType Key)> OnDelete = null)
            where KeyType : IComparable<KeyType>
        {
            HTMLElement[] Views = new HTMLElement[0];
            string TableName;
            var PartTable = Table as PartOfTable<ValueType, KeyType>;
            TableName = PartTable != null ? PartTable.Parent.TableName : Table.TableName;

            var TableInfo = TableFinder.FindTable(Table, TableName);
            foreach (var Value in TableInfo.SelectorItems((Table.Select((c) => c.Value), Query)))
            {
                var Key = Table.GetKey(Value);
                var View = (Table, Value).MakeView(
                () => OnUpdate?.Invoke((TableInfo, Key)),
                () => OnDelete?.Invoke((TableInfo, Key)));
                Insert(ref Views, View);
            }
            return HolderViewItemMaker<Table<ValueType, KeyType>>.FillHolder((Table, Views));
        }
    }
}