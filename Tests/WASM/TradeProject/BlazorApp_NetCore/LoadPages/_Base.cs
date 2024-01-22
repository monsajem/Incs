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
using static Calculate_wall.Calculator;

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
                    ShowSelectPerDay();
                };
                MainElement.ReplaceChilds(View.main);
            }

            static Func<UniverseSummary, UniverseSummary.Info_HLC[]> SelectPerDay;
            public static void ShowSelectPerDay()
            {
                var Selector = new SelectDayLen_html();

                SelectPerDay = null;

                Selector.btn_1day.OnClick += async (c1, c2) =>
                {
                    SelectPerDay = (c) => new UniverseSummary.Info_HLC[] { c.Info_1 };
                    await js.GoBack();
                };


                Selector.btn_7day.OnClick += async (c1, c2) =>
                {
                    SelectPerDay = (c) => new UniverseSummary.Info_HLC[] { c.Info_7 };
                    await js.GoBack();
                };


                Selector.btn_15day.OnClick += async (c1, c2) =>
                {
                    SelectPerDay = (c) => new UniverseSummary.Info_HLC[] { c.Info_15 };
                    await js.GoBack();
                };


                Selector.btn_30day.OnClick += async (c1, c2) =>
                {
                    SelectPerDay = (c) => new UniverseSummary.Info_HLC[] { c.Info_30 };
                    await js.GoBack();
                };

                ShowModal(Selector.main,()=>
                {
                    if (SelectPerDay != null)
                        ShowSelectHLC();
                });
            }


            static Func<UniverseSummary.Info_HLC[], UniverseSummary.Info[]> SelectHLC;
            public static void ShowSelectHLC()
            {
                var Selector = new Select_LHC_html();

                SelectHLC = null;

                Selector.btn_High.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.Select((c)=> c.High).ToArray();
                    await js.GoBack();
                };


                Selector.btn_Low.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.Select((c) => c.Low).ToArray();
                    await js.GoBack();
                };


                Selector.btn_Close.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.Select((c) => c.Close).ToArray();
                    await js.GoBack();
                };

                //----------------------------

                Selector.btn_HC.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.SelectMany((c) => 
                                    new UniverseSummary.Info[] { c.High, c.Close }).ToArray();
                    await js.GoBack();
                };

                Selector.btn_LC.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.SelectMany((c) =>
                                    new UniverseSummary.Info[] { c.Low, c.Close }).ToArray();
                    await js.GoBack();
                };

                Selector.btn_LH.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.SelectMany((c) =>
                                    new UniverseSummary.Info[] { c.Low, c.High }).ToArray();
                    await js.GoBack();
                };

                Selector.btn_HLC.OnClick += async (c1, c2) =>
                {
                    SelectHLC = (c) => c.SelectMany((c) =>
                                    new UniverseSummary.Info[] { c.Low, c.High,c.Close }).ToArray();
                    await js.GoBack();
                };

                ShowModal(Selector.main,()=>
                {
                    if(SelectHLC!=null)
                        ShowSelectInfo();
                });
            }

            static Func<UniverseSummary.Info[], int> SelectInfo;
            public static void ShowSelectInfo()
            {
                var Selector = new Select_Info_html();

                SelectInfo = null;

                Action ShowData = () =>
                {
                    OrderProducts = (c) => c.OrderBy((c) =>
                    SelectInfo(SelectHLC(SelectPerDay(c.Summary))));
                    Data.Products.ShowItems();
                };

                Selector.btn_RSI.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) => c.Rate_RSI_14);
                    await js.GoBack();
                };

                Selector.btn_CRSI.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) => c.Rate_CRSI);
                    await js.GoBack();
                };

                Selector.btn_DEMA_Cross.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) => c.Rate_DEMA_Cross);
                    await js.GoBack();
                };

                //-------------------------------

                Selector.btn_RSI_DEMA.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) =>
                                            c.Rate_RSI_14 +
                                            c.Rate_DEMA_Cross);
                    await js.GoBack();
                };

                Selector.btn_CRSI_DEMA.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) => 
                                            c.Rate_CRSI +
                                            c.Rate_DEMA_Cross);
                    await js.GoBack();
                };

                Selector.btn_RSI_CRSI.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) =>
                                            c.Rate_RSI_14 +
                                            c.Rate_CRSI);
                    await js.GoBack();
                };

                Selector.btn_RSI_CRSI_DEMA.OnClick += async (c1, c2) =>
                {
                    SelectInfo = (c) => c.Sum((c) =>
                                            c.Rate_RSI_14 +
                                            c.Rate_CRSI+
                                            c.Rate_DEMA_Cross);
                    await js.GoBack();
                };

                ShowModal(Selector.main,()=>
                {
                    if(SelectInfo!=null)
                        ShowData();
                });
            }

        }
    }
}