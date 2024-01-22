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
        [AutoRun]
        private void Product()
        {
            Data.Products.RegisterView((c) =>
            {
                c.SetEdit<ProductEdit_html>((i) =>
                {
                    i.FillView = (i) =>
                    {

                    };
                    i.FillValue = (i) =>
                    {
                        //var ImageFiles = i.View.ImageFile.Files;
                        //byte[] ImageData = null;
                        //if (ImageFiles.Length > 0)
                        //{
                        //    string Data = js.Eval(@" ( function () {
                        //        var MAX_WIDTH = 500;
                        //        var width = " + i.View.Image.NaturalWidth.ToString() + @";
                        //        var height = " + i.View.Image.NaturalHeight.ToString() + @";
                        //        height =height*( MAX_WIDTH / width);
                        //        width = MAX_WIDTH;                            
                        //        var canvas = document.createElement('canvas');
                        //        canvas.width = width;
                        //        canvas.height = height;
                        //        var ctx = canvas.getContext('2d');
                        //        ctx.drawImage(" + i.View.Image.Name + @",0, 0, width, height);

                        //        return canvas.toDataURL('image/jpeg',0.8);}).call(null); ");

                        //    ImageData = Convert.FromBase64String(Data.Substring(23));
                        //    i.OldValue.Image = ImageData;
                        //};
                        return i.OldValue;
                    };
                });

                c.SetView<ProductView_html>((i) =>
                {
                    i.MakeHolder = (Table) =>
                    {
                        var Div = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                        var H1 = new Monsajem_Incs.Resources.Base.Html.h1_html().Main;

                        var PartTable = Table as Monsajem_Incs.Database.Base.PartOfTable<Product, string>;
                        if (PartTable != null)
                        {
                            H1.TextContent = PartTable.HolderTable.Key.ToString();
                        }
                        Div.AppendChild(H1);
                        return Div;
                    };

                    i.FillView = (i) =>
                    {
                        Func<string> GetImageUri = () =>
                        {

                            var Image = "/ProductImages/" + i.Value.ProductName + App.CachedUri;
                            i.View.Image_Product.SetAttribute("src", Image);
                            i.View.Image_Product.SetAttribute("alt", i.Value.ProductName);
                            return Image;
                        };
                        if (App.IsUser)
                        {
                            i.View.btn_3D.Remove();
                            i.View.ManagerArea.Remove();
                            Action SelectedProductsChanged = () =>
                            {
                                double ProductCount = 0;
                                if (Data.SelectedProducts.IsExist(i.Value.ProductName))
                                {
                                    ProductCount = Data.SelectedProducts[i.Value.ProductName].Value.Count;
                                }
                                if (ProductCount > 0)
                                {
                                    i.View.Btn_DropFromShop.RemoveStyleAttribute("display");
                                    i.View.lbl_ShopedCount.TextContent = ProductCount.ToString();
                                }
                                else
                                {
                                    i.View.Btn_DropFromShop.SetStyleAttribute("display", "none");
                                    i.View.lbl_ShopedCount.TextContent = "";
                                }
                            };
                            i.View.Btn_DropFromShop.OnClick += (c1, c2) =>
                            {
                                Data.SelectedProducts.Delete(i.Value.ProductName);
                                SelectedProductsChanged();
                                ShowDangerMessage("کالا از سبد برداشته شد");
                            };
                            SelectedProductsChanged();
                            i.View.btn_shop.OnClick+=(c1,c2)=>
                                GetProductCount(i.Value.ProductName, (c) => SelectedProductsChanged());
                            i.View.Image_Product.OnClick += (c1, c2) => i.View.btn_shop.Click();
                        }
                        else
                        {
                            i.View.btn_edit_pic.OnClick += (c1, c2) =>
                              {
                                  var PicEditor = new ProductEdit_Pic_html();
                                  PicEditor.Done.OnClick += async (c1, c2) =>
                                  {
                                      var File = PicEditor.ImageFile.Files[0];
                                      var ORG_Data = await File.ReadBytes();

                                      byte[] Low_Data = null;
                                      try
                                      {
                                          Low_Data = PicEditor.Image.GetImageBytes(0.75);
                                      }
                                      catch (Exception ex)
                                      {
                                          while (ex != null)
                                          {
                                              System.Console.WriteLine(ex.Message);
                                              System.Console.WriteLine("\n\n\n");
                                              ex = ex.InnerException;
                                          }
                                          return;
                                      }
                                      var ResText = await App.UploadFile(ORG_Data, "/ProductImages_ORG/" + i.Value.ProductName);
                                      ResText = await App.UploadFile(Low_Data, "/ProductImages/" + i.Value.ProductName);

                                      if (ResText == "Done.")
                                      {
                                          ShowSuccessMessage("درخواست شما ارسال شد");
                                          js.GoBack();
                                          AboutUsPage.Replay();
                                      }
                                  };
                                  ShowModal(PicEditor.Main);
                              };
                        }
                        GetImageUri();
                        i.View.Name.TextContent = i.Value.ProductName;
                        var Price = i.Value.Price;

                        i.View.OldPrice.Remove();
                        i.View.OldPrice.InnerHtml = AddThousandSprator(Price.ToString());

                        i.View.Price.InnerHtml = AddThousandSprator(Price.ToString());
                        i.View.ShortDescribe.TextContent = i.Value.ShortDescribe;
                    };
                    i.GetMain = (i) => i.Main;
                    i.RegisterEdit = (i) => i.View.btn_edit.OnClick += (c1, c2) => i.Edit();
                    i.RegisterDelete = (i) => i.View.btn_delete.OnClick += (c1, c2) => i.Delete();
                });

                c.SetSelector((c) =>
                {
                    var Values = c.Values;
                    if(c.Query!=null)
                    {
                        var keys = c.Query.Split(" ");
                        keys.Where((c) => c.Trim() != "").
                            GroupBy((c)=>c).ToArray();

                        var SValues = Values.Select((c)=>
                        {
                            var i = 0;
                            foreach(var Key in keys)
                            {
                                if (c.ProductName.Contains(Key))
                                    i++;
                            }
                            return (Value:c,Related:i);
                        }).Where((c)=>c.Related>0).ToArray();

                        Values = SValues.OrderByDescending((c) => c.Related).
                                         Select((c) => c.Value).ToArray();

                        if (Values.Count() == 0)
                        {
                            ShowDangerMessage("نتیجه ای یافت نشد");
                            throw new Exception();
                        }
                    }
                    return Values;
                });
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
