using System.Threading.Tasks;
using Monsajem_Incs.Resources.Partials;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using static Monsajem_Client.App;
using Monsajem_Incs.Serialization;
using System;
using static Monsajem_Incs.UserControler.Publish;
using System.Net.Http;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {

        static WebAssembly.Browser.DOM.HTMLDivElement 
            MakeButton(string Text, Action Action, string ImgUri = null)
        {
            var btn = new Button_html();
            if (ImgUri == null)
            {
                btn.icon.Remove();
            }
            else
                btn.icon.Src = ImgUri;
            btn.txt.TextContent = Text;
            btn.icon.SetAttribute("alt", Text);
            btn.btn_view.OnClick += (c1, c2) => Action();
            return btn.btn_view;
        }

        public static void MakeMenus()
        {

            static void MenuAddBtn(string Text, Action Action, string ImgUri = null)
            {
                var btn = MakeButton(Text, Action, ImgUri);
                BasePage_html.Menu.AppendChild(btn);
            }

            BasePage_html.Menu.InnerHtml = "";
            if (App.IsUser == false)
            {
                MenuAddBtn("رفتن به خانه", () =>
                {
                    BasePage_html.Btn_MenuClose.Click();
                    new MainPage().Show();
                }, "./Files/home.png");

                MenuAddBtn("منتخب ها", () =>
                {
                    BasePage_html.Btn_MenuClose.Click();
                    BasePage_html.btn_basket.Click();
                }, "./Files/shopicon.png");

                BasePage_html.btn_basket.OnClick += (c1, c2) =>
                {
                    if (WASM_Global.Publisher.NavigationManager.Uri.EndsWith("SelectedProducts") == false)
                        Data.SelectedProducts.ShowItems();
                };
            }

            if (App.UserName != null)
            {

                MenuAddBtn("خروج از حساب", () =>
                {
                    App.UserName = null;
                    App.Password = null;
                    App.IsUser = true;
                    MakeMenus();
                    WASM_Global.Publisher.NavigationManager.NavigateTo(
                        WASM_Global.Publisher.NavigationManager.BaseUri);
                    BasePage_html.Btn_MenuClose.Click();
                }, "./Files/Logout.png");
            }
        }
    }
}