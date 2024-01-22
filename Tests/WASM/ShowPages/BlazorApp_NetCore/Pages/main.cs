using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.DirectoryTable;
using WebAssembly.Browser.DOM;
using static WASM_Global.Publisher;

namespace Monsajem_Client
{
    public class MainPage : Monsajem_Incs.Views.Page
    {
        public override string Address => "/";

        protected override async Task Ready()
        {
            
        }
    }
}