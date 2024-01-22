using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Resources.Product;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using System;
using WebAssembly.Browser.MonsajemDomHelpers;
using static Monsajem_Client.App;
using System.Threading.Tasks;
using static Monsajem_Incs.UserControler.Publish;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {

        public static Func<IEnumerable<Product>, IEnumerable<Product>>
            OrderProducts = (c) => c;

        [AutoRun]
        private void Product()
        {
            Data.Products.RegisterView((c) =>
            {
                c.SetView<ProductView_html>((i) =>
                {
                    i.FillView = (i) =>
                    {
                        i.View.Name.TextContent = i.Value.ProductName;
                        var Price = i.Value.Price;


                        static void AppendData(
                            WebAssembly.Browser.DOM.HTMLElement Holder,
                            string InfoString)
                        {
                            var Info = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                            Info.TextContent = InfoString;
                            Holder.AppendChild(Info);
                        }

                        static void AppendInfo(
                            WebAssembly.Browser.DOM.HTMLElement Holder,
                            Calculate_wall.Calculator.UniverseSummary Info)
                        {
                            static void AppendInfo(
                            WebAssembly.Browser.DOM.HTMLElement Holder,
                            Calculate_wall.Calculator.UniverseSummary.Info_HLC Info,
                            string InfoName)
                            {
                                AppendData(Holder, InfoName);
                                static void AppendInfo(
                                    WebAssembly.Browser.DOM.HTMLElement Holder,
                                    Calculate_wall.Calculator.UniverseSummary.Info Info,
                                    string InfoName)
                                {
                                    AppendData(Holder, InfoName);
                                    var Describe = "";
                                    Describe += " DEMA_50:" + Info.DEMA_50;
                                    Describe += " DEMA_100:" + Info.DEMA_100;
                                    Describe += " DEMA_150:" + Info.DEMA_150;
                                    Describe += " DEMA_200:" + Info.DEMA_200;
                                    Describe += " DEMA_250:" + Info.DEMA_250;
                                    Describe += " DEMA_300:" + Info.DEMA_300;
                                    AppendData(Holder, Describe);
                                }
                                AppendInfo(Holder, Info.High, "High");
                                AppendInfo(Holder, Info.Low, "Low");
                                AppendInfo(Holder, Info.Close, "Close");
                            }

                            {
                                var Describe = " Down:" + Info.PercentDown;
                                Describe += " Up:" + Info.PercentUp;
                                AppendData(Holder, Describe);
                            }

                            AppendInfo(Holder, Info.Info_1, "1 Day");
                            AppendInfo(Holder, Info.Info_7, "7 Day");
                            AppendInfo(Holder, Info.Info_15, "15 Day");
                            AppendInfo(Holder, Info.Info_30, "30 Day");
                        }

                        AppendInfo(i.View.ShortDescribe, i.Value.Summary);
                    };
                    i.GetMain = (i) => i.Main;
                });
                c.SetSelector((c) => OrderProducts(c.Values));
            });
        }

        private static void GetProductCount(
            string ProductName,
            Action<int> changed)
        {
            var Value = Data.Products[ProductName].Value;
            var Menu = new GetCount_html();
            Menu.Price.InnerHtml = AddThousandSprator(Value.Price);
            if (Data.SelectedProducts.IsExist(ProductName))
                Menu.Count.TextContent = Data.SelectedProducts[ProductName].Value.Count.ToString();
            Action TextChanged = () =>
            {
                var Count = 0;
                try
                {
                    Count = int.Parse(Menu.Count.InnerText);
                }
                catch { }
                var SumPrice = Count * Value.Price;
                Menu.SumPrice.InnerHtml = AddThousandSprator(SumPrice);
            };
            Menu.Count.OnClick += (c1, c2) => TextChanged();
            TextChanged();
            var Image = "/ProductImages/" + Value.ProductName + App.CachedUri;
            Menu.Img.SetAttribute("src", Image);
            Menu.Img.SetAttribute("alt", Value.ProductName);
            ShowModal(Menu.Main);
            Menu.btn_Done.OnClick += async (c1, c2) =>
            {
                string TxtCount = Menu.Count.TextContent;
                if (TxtCount == "")
                    TxtCount = "0";
                var Count = double.Parse(TxtCount);
                if (Count == 0)
                {
                    ShowDangerMessage("تعداد نباید کوچکتر از یک باشد");
                    return;
                }
                Data.SelectedProducts.UpdateOrInsert(Value.ProductName,
                      new SelectedProduct()
                      {
                          Count = Count,
                          ProductName = Value.ProductName
                      });
                SelectedProductsChanged();
                changed((int)Count);
                var ShopImage = new ShopImage_html();
                js.Document.Body.AppendChild(ShopImage.ShopImage);
                js.GoBack();
                await Task.Delay(3000);
                ShopImage.ShopImage.Remove();
            };
        }

        private static string AddThousandSprator(int Value)
        {
            return AddThousandSprator(Value.ToString());
        }
        private static string AddThousandSprator(string Value)
        {
            var Result = "";
            for (int i = Value.Length - 1; i > 2; i -= 3)
            {
                Result = "<b style='color:chocolate;'>'</b style='color:black;'><b>" + Value.Substring(i - 2, 3) + "</b>" + Result;
            }
            var LastPos = (Value.Length) % 3;
            if (LastPos == 0)
                LastPos = 3;
            Result = "<div dir='ltr' style='flex-wrap:nowrap'><b>" + Value.Substring(0, LastPos) + "</b>" + Result + "</div>";
            return Result;
        }
    }
}
