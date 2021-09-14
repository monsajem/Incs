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
            Action<KeyType> OnUpdate = null,
            Action<KeyType> OnDelete = null)
            where KeyType : IComparable<KeyType>
        {
            await Table.SyncUpdate();
            var Views = new HTMLElement[Table.Length];
            var i = 0;

            foreach(var Key in Table.KeysInfo.Keys)
            {
                Views[i++] = Table.MakeShowView(Key,
                                    ()=>OnUpdate?.Invoke(Key),
                                    ()=>OnDelete?.Invoke(Key));
            }

            var Holder = new Monsajem_Incs.Resources.Base.Html.Div_html();

            foreach (var View in Views)
                Holder.Main.AppendChild(View);

            return Holder.Main;
        }
    }
}