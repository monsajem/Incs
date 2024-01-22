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
        public class LoginPage : Monsajem_Incs.Views.Page
        {
            public override string Address => "Login";

            private Monsajem_Incs.Resources.Login_html Main;

            protected override async Task Ready()
            {
                Main = new Login_html();
                Main.btn_Login.OnClick += async (c1, c2) =>
                {
                    if(IsLocalUrl)
                    {
                        UserName = Main.txt_Username.Value.Trim();
                        Password = Main.txt_Password.Value.Trim();
                        IsUser = false;
                    }
                    else
                    {
                        using (var httpClient = new HttpClient())
                        {
                            ShowAction("در حال بررسی");
                            try
                            {
                                var Res = await Request(@"php/Login.php",
                                    (c) =>
                                    {
                                        c.Add(new StringContent(Main.txt_Username.Value.Trim()), "Username");
                                        c.Add(new StringContent(Main.txt_Password.Value.Trim()), "Password");
                                    });
                                if (Res.StartsWith("LogedIn."))
                                {
                                    Res = Res.Substring(8);
                                    if (Res.StartsWith("User."))
                                    {
                                        IsUser = true;
                                        Res = Res.Substring(5);
                                    }
                                    else if (Res.StartsWith("Admin."))
                                    {
                                        IsUser = false;
                                        Res = Res.Substring(6);
                                    }
                                    ShowSuccessMessage("شما وارد شدید");
                                    App.UserName = Main.txt_Username.Value.Trim();
                                    App.Password = Main.txt_Password.Value.Trim();
                                    HideAction();
                                    MakeMenus();
                                    await Task.Delay(1);
                                    js.GoBack();
                                }
                                else if (Res.StartsWith("NotRegister."))
                                {
                                    ShowDangerMessage("نام کاربری یا پسورد اشتباه است");
                                    HideAction();
                                }
                                else
                                {
                                    ShowDangerMessage("خطا، لطفا مجدد تلاش کنید");
                                    HideAction();
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowDangerMessage("خطا، لطفا مجدد تلاش کنید");
                                HideAction();
                            }
                        }
                    }
                   
                };
                MainElement.ReplaceChilds(Main.Main);
            }
        }
    }
}
