using System.Threading.Tasks;
using Monsajem_Incs.Resources.Partials;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using static Monsajem_Incs.UserControler.Publish;
using static Monsajem_Client.App;
using Monsajem_Incs.Serialization;
using System;
using System.Net.Http;
using System.Collections.Generic;
using WebAssembly.Browser.MonsajemDomHelpers;
using System.Linq;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        public static void GetReady()
        {
            var Loader = new OnLoadApp();
            Loader.Persons();
            Loader.Transactions();
            Monsajem_Incs.Views.Page.SubmitPage(App.MainElement,
                new MainPage(),
                new LoginPage());

            MakeMenus();
            Monsajem_Incs.Views.Page.LoadingPage+=(c)=>
            {
                if (UserName == null && c.Page.GetType() != typeof(LoginPage))
                    new LoginPage().Show();
            };
        }

        public class MainPage : Monsajem_Incs.Views.Page
        {
            public override string Address => "";

            protected override async Task Ready()
            {
                var View = new Monsajem_Incs.Resources.Manager.BasePage_html();
                if(IsUser==false)
                    View.btn_ShowPersons.OnClick += (c1, c2) => App.Data.Persons.ShowItems();
                else
                {
                    View.btn_ShowPersons.Remove();
                    View.MainCard.OnClick += (c1, c2) => App.Data.Persons.ShowItems();
                }
                var AllTranActions = Data.Transactions.Select((c) => c.Value.Value).ToArray();
                View.AcountingPosetive.InnerHtml = AddThousandSprator(AllTranActions.Where((c) => c > 0).Sum());
                View.AcountingNegative.InnerHtml = AddThousandSprator(AllTranActions.Where((c) => c < 0).Sum());
                View.AcountingSummary.InnerHtml = AddThousandSprator(AllTranActions.Sum());
                MainElement.ReplaceChilds(View.main);
            }
        }
    }
}