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

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        public static void GetReady()
        {
            var Loader = new OnLoadApp();
            Loader.Group();
            Loader.Product();
            Loader.ProductBasket();
            Monsajem_Incs.Views.Page.SubmitPage(App.MainElement,
                new MainPage(),
                new LoginPage(),
                new AboutUsPage(),
                new ShowUsersPage());

            MakeMenus();
        }

        public class MainPage : Monsajem_Incs.Views.Page
        {
            public override string Address => "";

            protected override async Task Ready()
            {
                if(App.IsUser)
                {
                    App.Data.Groups.GetItem("Root").Value.GroupChilds.ShowItems();
                }
                else
                {
                    var View = new Monsajem_Incs.Resources.Manager.BasePage_html();
                    View.btn_Root.OnClick += (c1, c2) => App.Data.Groups["Root"].Value.GroupChilds.ShowItems();
                    View.btn_Products.OnClick += (c1, c2) => App.Data.Products.ShowItems();
                    View.btn_Groups.OnClick += (c1, c2) => App.Data.Groups.ShowItems();
                    View.btn_Orders.OnClick+= async (c1,c2)=>
                    {
                        new ShowUsersPage().Show();
                    };

                    View.btn_AddUser.OnClick+=(c1,c2)=>
                    {
                        var View = new GetMessage_SingleLine_html();
                        View.btn_send.Value = "افزودن کاربر";
                        View.Image.Remove();
                        View.txt_Prompt.TextContent = "شماره تلفن را وارد کنید";
                        View.txt_message.Placeholder = "شماره";

                        View.btn_send.OnClick+= async (c1,c2)=>
                        {
                            var Res = await RequestWithLogin(App.ActionUri + @"SubmitUser.php", (c) =>
                            {
                                c.Add(new StringContent(""), "Pass_Submit");
                                c.Add(new StringContent(View.txt_message.Value), "username_Submit");
                            });

                            if (Res == "Done.")
                            {
                                ShowSuccessMessage("کاربر ذخیره شد");
                                js.GoBack();
                            }                        
                        };

                        ShowModal(View.Main);
                    };

                    View.btn_SendSms_One.OnClick += (c1, c2) =>
                    {
                        
                    };

                    View.btn_SendSms_All.OnClick += (c1, c2) =>
                    {
                        var View = new GetMessage_html();
                        View.btn_send.Value = "ارسال پیام به همه";
                        View.Image.Remove();
                        View.txt_Prompt.TextContent = "متن را وارد کنید";

                        View.btn_send.OnClick += async (c1, c2) =>
                        {
                            var Res = await RequestWithLogin(App.ActionUri + @"SendSmsToAll.php", (c) =>
                            {
                                c.Add(new StringContent(View.txt_message.Value), "TextSms");
                            });

                            if (Res == "Send.")
                            {
                                ShowSuccessMessage("پیام ارسال شد");
                                js.GoBack();
                            }
                        };

                        ShowModal(View.Main);
                    };

                    MainElement.ReplaceChilds(View.main);
                }
            }
        }
    }
}