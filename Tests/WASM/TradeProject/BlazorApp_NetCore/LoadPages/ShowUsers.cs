using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using static Monsajem_Client.App;
using WebAssembly.Browser.MonsajemDomHelpers;
using Monsajem_Incs.Resources.Person;
using Monsajem_Incs.Resources.Partials;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System;
using Monsajem_Incs.Serialization;
using System.Threading.Tasks;
using static Monsajem_Incs.UserControler.Publish;
using System.Xml;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        public class ShowUsersPage : Monsajem_Incs.Views.Page
        {
            public static string CurrentUser;

            public override string Address => "ShowUsers";

            protected override async Task Ready()
            {
                var Res = await App.RequestXml(@"php\AdminActions\GetOrders.php");
                MainElement.InnerHtml = "";
                var Rows = Res.GetElementsByTagName("Row");
                foreach (XmlNode Row in Rows)
                {
                    var User = new ShortView_html();
                    User.txt_Number.TextContent = Row["username"].InnerText;
                    User.Menu.OnClick += (c1, c2) =>
                    {
                        var Menu = new OptionsView_html();
                        Menu.btn_ShowShopList.OnClick+=(c1,c2)=>
                        {
                            js.GoBack();
                            CurrentUser = Row["username"].InnerText;
                            var Orders = Convert.FromBase64String(Row["orders"].InnerText).
                                            Deserialize<SelectedProduct[]>();
                            Data.SelectedProducts.Delete();
                            Data.SelectedProducts.Insert(Orders);
                            Data.SelectedProducts.ShowItems();
                        };
                        ShowModal(Menu.Main);
                    };
                    MainElement.AppendChild(User.main);
                }
            }
        }
    }
}
