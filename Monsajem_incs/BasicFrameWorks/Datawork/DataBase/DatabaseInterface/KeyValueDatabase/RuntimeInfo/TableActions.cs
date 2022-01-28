using Monsajem_Incs.Net.Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Views.Maker.Database;
using static Monsajem_Client.Network;
using Monsajem_Incs.Convertors;

namespace Monsajem_Incs.Database.Base
{
    public class TableFinders
    {
        public interface TableActions
        {
            public string ConvertKeyToString(object Key);
            public object ConvertStringToKey(string Key);
            public Task Insert(object Data);
            public Task Update(object Key, object Data);
            public Task Delete(object Key);

            public Task SendUpdate(IAsyncOprations Client);
            public Task GetUpdate(IAsyncOprations Client);
            public Task SyncUpdate();

            public HTMLElement MakeShowViewForItem(
                   object Key,
                   Action<(TableActions TableInfo, object Key)> OnUpdate = null,
                   Action<(TableActions TableInfo, object Key)> OnDelete = null);
            public Task<HTMLElement> MakeShowViewForItems(
                    Action<(TableActions TableInfo, object Key)> OnUpdate = null,
                    Action<(TableActions TableInfo, object Key)> OnDelete = null);

            public HTMLElement MakeEditViewForItem(
                    object Key,
                    Action Done = null);
            public HTMLElement MakeInsertView(
                    Action Done = null);
        }


    }
}
