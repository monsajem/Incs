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

            protected override async Task Ready()
            {
                SendCodeReady();
            }

            protected async Task SendCodeReady()
            {
                var Elements = new Register_html();
                Elements.lable.TextContent = "شماره تلفن خود را وارد کنید";
                Elements.Done.Value = "ادامه";
                Elements.Done.OnClick += async (c1, c2) =>
                {
                    if(Elements.txt_input.Value!="09381556657")
                    {

                        ShowDangerMessage("شما عضویتی ندارید");
                        var ShowHint = new Hint_Person_html();

                        {
                            var msg = new Danger_html();
                            msg.txt_Success.TextContent = "متاسفیم، به نظر می‌رسد شما عضو سامانه نیستید";
                            ShowHint.Body.AppendChild(msg.txt_Success);
                            ShowHint.Body.AppendChild("<br />");
                        }

                        {
                            var msg = new Danger_html();
                            msg.txt_Success.TextContent = "در صورتی که فکر مینید ما اشتباه میکنیم لطفا با پشتیانی تماس بگیرید";
                            ShowHint.Body.AppendChild(msg.txt_Success);
                            ShowHint.Body.AppendChild("<br />");
                        }

                        {
                            var msg = new Danger_html();
                            msg.txt_Success.TextContent = "تلفن پشتیبانی 09381556657";
                            ShowHint.Body.AppendChild(msg.txt_Success);
                            ShowHint.Body.AppendChild("<br />");
                        }

                        MainElement.ReplaceChilds(ShowHint.Main);
                        return;
                    }
                    using (var httpClient = new HttpClient())
                    {
                        ShowAction("در حال ارسال کد فعال ساز");
                        try
                        {
                            var Res = await RequestAsString(@"php/SendUserCode.php",
                                (c) =>
                                {
                                    c.Add(new StringContent(Elements.txt_input.Value), "UserName");
                                });
                            if (Res == "Send.")
                            {
                                ShowSuccessMessage("کد فعال سازی ارسال شد");
                                HideAction();
                                RegisterCodeReady(Elements.txt_input.Value);
                            }
                            else
                            {
                                ShowDangerMessage("خطا، لطفا مجدد تلاش کنید");
                                HideAction();
                            }
                        }
                        catch (Exception ex)
                        {
                            HideAction();
                        }
                    }
                };
                MainElement.ReplaceChilds(Elements.Main);
                Elements.txt_input.Focus();
            }

            protected async Task RegisterCodeReady(string PhoneNumber)
            {
                var Elements = new Register_html();
                Elements.lable.TextContent = "کد فعال سازی را وارد کنید";
                Elements.Done.Value = "فعال سازی";
                Elements.Done.OnClick += async (c1, c2) =>
                {
                    using (var httpClient = new HttpClient())
                    {
                        ShowAction("در حال تایید کد فعال سازی");
                        try
                        {
                            var Res = await RequestAsString(@"php/GetPassword.php",
                                (c) =>
                                {
                                    c.Add(new StringContent(PhoneNumber), "Username");
                                    c.Add(new StringContent(Elements.txt_input.Value), "Password");
                                });
                            if (Res.StartsWith("Correct."))
                            {
                                Res = Res.Substring(8);
                                if(Res.StartsWith("User."))
                                {
                                    IsUser = true;
                                    Res = Res.Substring(5);
                                }
                                else if(Res.StartsWith("Admin."))
                                {
                                    IsUser = false;
                                    Res = Res.Substring(6);
                                }
                                ShowSuccessMessage("کد فعال سازی تایید شد\nشما وارد شدید");
                                App.UserName = PhoneNumber;
                                App.Password = Res;
                                HideAction();
                                js.GoBack();
                                MakeMenus();
                            }
                            else if(Res.StartsWith("NotRegister."))
                            {
                                ShowDangerMessage("درخواست شما منقضی شد، مجدد تلاش کنید");
                                HideAction();
                                SendCodeReady();
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
                };
                MainElement.ReplaceChilds(Elements.Main);
                Elements.txt_input.Focus();
            }
        }
    }
}
