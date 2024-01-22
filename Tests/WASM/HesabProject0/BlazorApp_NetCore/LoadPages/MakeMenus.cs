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

            static void MenuAddBtn(string Text, Action Action,string ImgUri=null)
            {
                var btn = MakeButton(Text,Action,ImgUri);
                BasePage_html.Menu.AppendChild(btn);
            }

            BasePage_html.Menu.InnerHtml = "";
            if (App.IsUser == false)
            {
                MenuAddBtn("درج", () =>
                    {
                        Shower.ShowInsertForCurrentURL();
                        BasePage_html.Btn_MenuClose.Click();
                    });

                MenuAddBtn("ذخیره اطلاعات در سرور",async () =>
                {
                    Data.SelectedProducts.Delete();
                    await UploadFile(Register.Value.Serialize(), "/DB");
                    await UploadFile(System.Text.Encoding.UTF8.GetBytes(
                        DateTime.UtcNow.ToShortDateString() + DateTime.UtcNow.ToLongTimeString()),
                        "/ChachedInfo.txt");
                    if(App.TasksAfterUploadDB!=null)
                        await App.TasksAfterUploadDB.Invoke();
                    BasePage_html.Btn_MenuClose.Click();
                    ShowSuccessMessage("ثبت شد");
                });

                MenuAddBtn("آماده سازی برای جستجوگر ها", async () =>
                {
                    await MakeSearchIndex();
                    BasePage_html.Btn_MenuClose.Click();
                    ShowSuccessMessage("ثبت شد");
                });
            }
            else
            {
                if(App.UserName==null)
                {
                    MenuAddBtn("ورود به حساب", () =>
                    {
                        BasePage_html.Btn_MenuClose.Click();
                        new LoginPage().Show();
                    }, "./Files/Login.png");
                }

                MenuAddBtn("رفتن به خانه", () =>
                {
                    BasePage_html.Btn_MenuClose.Click();
                    new MainPage().Show();
                }, "./Files/home.png");

                MenuAddBtn("سبد خرید", () =>
                {
                    BasePage_html.Btn_MenuClose.Click();
                    BasePage_html.btn_basket.Click();
                }, "./Files/shopicon.png");

                MenuAddBtn("درباره ما", () =>
                {
                    BasePage_html.Btn_MenuClose.Click();
                    new AboutUsPage().Show();
                }, "./Files/us.png");

                BasePage_html.btn_basket.OnClick += (c1, c2) =>
                {
                    if (WASM_Global.Publisher.NavigationManager.Uri.EndsWith("SelectedProducts") == false)
                        Data.SelectedProducts.ShowItems();
                };
            }

            BasePage_html.img_Search.OnClick += (c1, c2) =>
            {
                var GetMessageView = new GetMessage_SingleLine_html();
                GetMessageView.btn_send.Value = "جستجو کن";
                GetMessageView.txt_Prompt.TextContent = "چه چیزی احتیاج دارین؟";
                GetMessageView.Image.Remove();
                GetMessageView.txt_message.Placeholder = "متن جستجو";
                GetMessageView.btn_send.OnClick += async (c1, c2) =>
                {
                    var Message = GetMessageView.txt_message.Value;
                    Message = Message.Trim();
                    if (Message == "")
                    {
                        ShowDangerMessage("لطفا کالای مورد نظر خود را وارد کنید");
                        return;
                    }
                    js.GoBack();
                    await Task.Delay(1);
                    Data.Products.ShowItems(Message);
                    Data.Products.MakeShowView();
                };
                ShowModal(GetMessageView.Main);
            };

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