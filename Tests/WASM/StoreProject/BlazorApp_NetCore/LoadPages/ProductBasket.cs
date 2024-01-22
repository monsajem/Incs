using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Resources.Product;
using Monsajem_Incs.Views.Maker.Database;
using static Monsajem_Client.App;
using WebAssembly.Browser.MonsajemDomHelpers;
using Monsajem_Incs.Resources.Delivery;
using Monsajem_Incs.Resources.Partials;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System;
using Monsajem_Incs.Serialization;
using static Monsajem_Incs.UserControler.Publish;
using System.Threading.Tasks;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        [AutoRun]
        private void ProductBasket()
        {
            Data.SelectedProducts.RegisterView((c) =>
            {
                c.SetView<ProductInBasket_html>((i) =>
                {
                    i.MakeHolder = (c) =>
                    {
                        var DeliverView = new Delivery_frame_html();
                        {
                            var DeliveryOptionsView = new Success_html();
                            DeliveryOptionsView.txt_Success.TextContent = "پرداخت در محل تحویل";
                            DeliverView.Body.AppendChild(DeliveryOptionsView.main);
                            DeliveryOptionsView = new Success_html();
                            DeliveryOptionsView.txt_Success.TextContent = "جنس فروخته شده تا 24 ساعت پس گرفته میشود(در صورت عدم خرابی)";
                            DeliverView.Body.AppendChild(DeliveryOptionsView.main);
                        }
                        var DeliverRequestView = new StartDelivery_html();
                        DeliverView.Card.AppendChild(DeliverRequestView.main);

                        var Person = Data.SelectedProducts;
                        DeliverRequestView.ShopCount.TextContent = Data.SelectedProducts.Length.ToString();
                        DeliverRequestView.ShopCountAll.TextContent =
                            Data.SelectedProducts.Sum((c) => c.Value.Count).ToString();
                        DeliverRequestView.SumPrice.InnerHtml =
                            AddThousandSprator(Data.SelectedProducts.Sum((c) =>
                                                        Data.Products[c.Value.ProductName].Value.Price * c.Value.Count).ToString());


                        var Btn = MakeButton("درخواست ارسال کالا",
                            async () =>
                            {
                                if(Data.SelectedProducts.Length<1)
                                {
                                    ShowDangerMessage("شما هنوز کالایی انتخاب نکرده اید");
                                    return;
                                }
                                if (IsUser)
                                {
                                    var Res = await RequestWithLogin(ActionUri + @"SubmitMyOrders.php", (c) =>
                                    {
                                        var SelectedProducts = Convert.ToBase64String(
                                                             Data.SelectedProducts.Select((c) => c.Value).
                                                                     Where((c) => c.Count > 0).ToArray().Serialize());
                                        c.Add(new StringContent(SelectedProducts), "Data");
                                    });
                                    if (Res == "Done.")
                                    {
                                        ShowSuccessMessage("درخواست شما ارسال شد");
                                    }
                                }
                                else
                                {
                                    var Res = await RequestWithLogin(ActionUri + @"DropOrders.php", (c) =>
                                    {
                                        c.Add(new StringContent(ShowUsersPage.CurrentUser), "username_Submit");
                                    });
                                    if (Res == "Done.")
                                    {
                                        ShowSuccessMessage("کالاها برداشته شد");
                                    }
                                }
                            }, "./Files/ShoppedProduct.png");

                        DeliverRequestView.main.AppendChild(Btn);

                        DeliverRequestView.main.AppendChild("<br/>");
                        DeliverRequestView.main.AppendChild("<br/>");

                        var Div = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                        Div.AppendChild(DeliverView.Main);
                        return Div;
                    };

                    i.FillView = (i) =>
                    {
                        var Product = Data.Products[i.Value.ProductName].Value;
                        i.View.Name.TextContent = Product.ProductName;
                        i.View.Price.InnerHtml = AddThousandSprator(Product.Price);
                        i.View.Count.TextContent = i.Value.Count.ToString();
                        var Price = i.Value.Count * Product.Price;
                        i.View.SumPrice.InnerHtml = AddThousandSprator(Price.ToString());
                        i.View.Image.SetAttribute("src", "/ProductImages/" + Product.ProductName + App.CachedUri);
                        i.View.Image.SetAttribute("alt", i.Value.ProductName);
                        var ProductName = i.Value.ProductName;

                        i.View.DropFromShop.OnClick += (c1, c2) =>
                        {
                            Data.SelectedProducts.Delete(i.Value);
                            App.SelectedProductsChanged();
                            Monsajem_Incs.Views.Page.Replay();
                        };

                        i.View.btn_ChangeCount.OnClick += (c1, c2) =>
                          {
                              GetProductCount(i.Value.ProductName,
                                  (c) => Monsajem_Incs.Views.Page.Replay());
                          };

                    };
                });
            });
        }
    }
}
