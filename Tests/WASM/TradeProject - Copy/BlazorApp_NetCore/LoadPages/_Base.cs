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
using Monsajem_Incs.Resources.Manager;

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

        public static async Task SaveDataOnServer()
        {
            var Data = Calculate_wall.Calculator.KeysNames.Serialize();
            ShowAction("Saving Data On Server...");
            while (true)
            {
                try
                {
                    await SafeRun.Safe(async () =>
                    await App.UploadFile(Data,"DataStored"));
                    break;
                }
                catch{}
            }
            HideAction();
            ShowSuccessMessage("Save Success");
        }

        public class MainPage : Monsajem_Incs.Views.Page
        {
            public override string Address => "";

            protected override async Task Ready()
            {
                if ((UserName == null || IsUser == true) && IsLocal == false)
                {
                    new OnLoadApp.LoginPage().Show();
                    return;
                }
                var View = new Monsajem_Incs.Resources.Manager.BasePage_html();
                View.btn_download.OnClick+=async (c1,c2)=>
                {
                   await SafeRun.Safe(async ()=>
                    await Calculate_wall.Calculator.GetDataFromServer());
                    ShowSuccessMessage("Download Success");
                };
                View.btn_save.OnClick += async (c1, c2) =>
                {
                    await SaveDataOnServer();
                };
                View.btn_Reload.OnClick += async (c1, c2) =>
                {
                    await SafeRun.Safe(async () =>
                     Calculate_wall.Calculator.KeysNames =
                     (await js.LoadBytesFromBaseURL("DataStored?" + DateTime.UtcNow.ToString())).Deserialize(
                         Calculate_wall.Calculator.KeysNames));
                    Data.Products.Delete();
                    foreach (var Key in Calculate_wall.Calculator.KeysNames)
                    {
                        Data.Products.Insert((c) =>
                        {
                            c.ProductName = Key.Name;
                            c.Summary = Key.Summary;
                        });
                    }
                    ShowSuccessMessage("load Success");
                };

                View.btn_select_PerDay.OnClick += async (c1, c2) =>
                {
                    var Selector = new SelectDayLen_html();
                    ShowModal(Selector.main);
                };

                View.btn_ShowAVG.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.RateAvg);
                    Data.Products.MakeShowView();
                    Data.Products.ShowItems();
                };
                View.btn_ShowAVG_D.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[10]);
                    Data.Products.ShowItems();
                };
                View.btn_ShowAVG_W.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[11]);
                    Data.Products.ShowItems();
                };
                View.btn_ShowAVG_M.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[12]);
                    Data.Products.ShowItems();
                };

                View.btn_D_RSI.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[0]);
                    Data.Products.ShowItems();
                };
                View.btn_D_DEMA.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[1]);
                    Data.Products.ShowItems();
                };
                View.btn_D_CRSI.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[2]);
                    Data.Products.ShowItems();
                };

                View.btn_W_RSI.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[3]);
                    Data.Products.ShowItems();
                };
                View.btn_W_DEMA.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[4]);
                    Data.Products.ShowItems();
                };
                View.btn_W_CRSI.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[5]);
                    Data.Products.ShowItems();
                };

                View.btn_M_RSI.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[6]);
                    Data.Products.ShowItems();
                };
                View.btn_M_DEMA.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[7]);
                    Data.Products.ShowItems();
                };
                View.btn_M_CRSI.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[8]);
                    Data.Products.ShowItems();
                };

                View.btn_ByDown.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.Rate.Rates[9]);
                    Data.Products.ShowItems();
                };

                View.btn_ByUp.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.OrderBy((c) => c.Summary.PercentUp+c.Summary.PercentDown);
                    Data.Products.ShowItems();
                };

                View.D_Before_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.Where((c)=>c.Summary.Info_1.Close.DEMA_Cross_Closing==true)
                                            .Where((c)=>c.Summary.Info_1.Close.DEMA_Cross<=0)
                                            .OrderByDescending((c) => c.Summary.Info_1.Close.DEMA_Cross);
                    Data.Products.ShowItems();
                };

                View.W_Before_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.Where((c) => c.Summary.Info_7.Close.DEMA_Cross_Closing == true)
                                            .Where((c) => c.Summary.Info_7.Close.DEMA_Cross <= 0)
                                            .OrderByDescending((c) => c.Summary.Info_7.Close.DEMA_Cross);
                    Data.Products.ShowItems();
                };

                View.M_Before_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.Where((c) => c.Summary.Info_30.Close.DEMA_Cross_Closing == true)
                                            .Where((c) => c.Summary.Info_30.Close.DEMA_Cross <= 0)
                                            .OrderByDescending((c) => c.Summary.Info_30.Close.DEMA_Cross);
                    Data.Products.ShowItems();
                };

                View.D_After_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.Where((c) => c.Summary.Info_1.Close.DEMA_Cross_Closing == false)
                                            .Where((c) => c.Summary.Info_1.Close.DEMA_Cross >= 0)
                                            .OrderBy((c) => c.Summary.Info_1.Close.DEMA_Cross);
                    Data.Products.ShowItems();
                };

                View.W_After_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.Where((c) => c.Summary.Info_7.Close.DEMA_Cross_Closing == false)
                                            .Where((c) => c.Summary.Info_7.Close.DEMA_Cross >= 0)
                                            .OrderBy((c) => c.Summary.Info_7.Close.DEMA_Cross);
                    Data.Products.ShowItems();
                };

                View.M_After_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    OrderProducts = (c) => c.Where((c) => c.Summary.Info_30.Close.DEMA_Cross_Closing == false)
                                            .Where((c) => c.Summary.Info_30.Close.DEMA_Cross >= 0)
                                            .OrderBy((c) => c.Summary.Info_30.Close.DEMA_Cross);
                    Data.Products.ShowItems();
                };

                MainElement.ReplaceChilds(View.main);
            }
        }
    }
}