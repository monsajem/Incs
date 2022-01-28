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
        public static async Task<HTMLElement> MakeShowView<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table,
            Action<(TableFinder.TableInfo TableInfo,KeyType Key)> OnUpdate = null,
            Action<(TableFinder.TableInfo TableInfo, KeyType Key)> OnDelete = null)
            where KeyType : IComparable<KeyType>
        {
            await Table.SyncUpdate();
            var Views = new HTMLElement[Table.Length];
            var i = 0;
            var TableInfo = TableFinder.FindTable(Table.TableName);
            foreach (var Key in Table.KeysInfo.Keys)
            {
                var Value = Table[Key].Value;
                Views[i++] = (Table, Value).MakeView(
                () => OnUpdate?.Invoke((TableInfo,Key)),
                () => OnDelete?.Invoke((TableInfo, Key)));
            }
            return HolderViewItemMaker<Table<ValueType,KeyType>>.FillHolder((Table,Views));
        }
    }
}