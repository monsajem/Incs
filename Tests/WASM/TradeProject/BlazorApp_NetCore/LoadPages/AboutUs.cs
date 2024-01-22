using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Views.Maker.Database;
using static Monsajem_Client.App;
using WebAssembly.Browser.MonsajemDomHelpers;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Resources.Partials;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System;
using Monsajem_Incs.Serialization;
using System.Threading.Tasks;
using static Monsajem_Incs.UserControler.Publish;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        public class AboutUsPage : Monsajem_Incs.Views.Page
        {
            public override string Address => "AboutUs";

            protected override async Task Ready()
            {
                var Content = new Monsajem_Incs.Resources.AboutUs_html();
                Content.CopyRight.InnerText = "(c) Monsajem_incs " + DateTime.UtcNow.Year.ToString();
                MainElement.ReplaceChilds(Content.Main);
            }
        }
    }
}
