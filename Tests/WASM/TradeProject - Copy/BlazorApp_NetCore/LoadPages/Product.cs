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
            OrderProducts = (c)=>c;

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
                        i.View.Price.InnerHtml = AddThousandSprator(Price.ToString());

                        {
                            var Info = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                            var Describe = "";
                            Describe += "AVG:" + i.Value.Summary.Rate.RateAvg;
                            Describe += " AVG_D:" + i.Value.Summary.Rate.Rates[10];
                            Describe += " AVG_W:" + i.Value.Summary.Rate.Rates[11];
                            Describe += " AVG_M:" + i.Value.Summary.Rate.Rates[12];
                            Info.TextContent = Describe;
                            i.View.ShortDescribe.AppendChild(Info);
                        }

                        {
                            var Info = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                            var Describe = "";
                            Describe += " Down:" + i.Value.Summary.PercentDown;
                            Describe += " Up:" + i.Value.Summary.PercentUp;
                            Info.TextContent = Describe;
                            i.View.ShortDescribe.AppendChild(Info);
                        }

                        {
                            var Info = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                            var Describe = "";
                            Describe += " D_RSI:" + i.Value.Summary.Rate.Rates[0];
                            Describe += " D_DEMA:" + i.Value.Summary.Rate.Rates[1];
                            Describe += " D_CRSI:" + i.Value.Summary.Rate.Rates[2];
                            Info.TextContent = Describe;
                            i.View.ShortDescribe.AppendChild(Info);
                        }

                        {
                            var Info = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                            var Describe = "";
                            Describe += " W_RSI:" + i.Value.Summary.Rate.Rates[3];
                            Describe += " W_DEMA:" + i.Value.Summary.Rate.Rates[4];
                            Describe += " W_CRSI:" + i.Value.Summary.Rate.Rates[5];
                            Info.TextContent = Describe;
                            i.View.ShortDescribe.AppendChild(Info);
                        }

                        {
                            var Info = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                            var Describe = "";
                            Describe += " M_RSI:" + i.Value.Summary.Rate.Rates[6];
                            Describe += " M_DEMA:" + i.Value.Summary.Rate.Rates[7];
                            Describe += " M_CRSI:" + i.Value.Summary.Rate.Rates[8];
                            Info.TextContent = Describe;
                            i.View.ShortDescribe.AppendChild(Info);
                        }
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
