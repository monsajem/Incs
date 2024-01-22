using Monsajem_Incs.Resources.Group;
using Monsajem_Incs.Resources.Product;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using static Monsajem_Incs.UserControler.Publish;
using System;
using WebAssembly.Browser.MonsajemDomHelpers;
using static Monsajem_Client.App;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        private void Group()
        {
            Data.Groups.RegisterView((c) =>
            { 
                c.SetEdit<GroupEdit_html>((i) =>
                {
                     i.FillView = (i) =>
                     {
                         if (i.Value != null)
                         {
                             i.View.Image.SetAttribute("src", "/GroupImages/" + i.Value.Name + App.CachedUri);
                             Action ShowSubGroups=()=>
                             {
                                 var SubGroupsText = "";
                                 foreach (var SubGroup in i.Value.GroupChilds)
                                     SubGroupsText += " , " + SubGroup.Value.Name;
                                 i.View.GroupChildsText.InnerText = SubGroupsText;
                             };
                             ShowSubGroups();

                             Action ShowSubProducts = () =>
                             {
                                 var SubProductsText = "";
                                 foreach (var SubProduct in i.Value.ProductChilds)
                                     SubProductsText += " , " + SubProduct.Value.ProductName;
                                 i.View.ProductsChildsText.InnerText = SubProductsText;
                             };
                             ShowSubProducts();

                             i.View.btn_AddSubGroups.OnClick+=(c1,c2)=>
                             {
                                 i.Value.GroupChilds.Accept(i.View.txt_SubGroup.Value);
                                 i.View.txt_SubGroup.Value = "";
                                 ShowSubGroups();
                             };
                             i.View.btn_RemoveSubGroups.OnClick += (c1, c2) =>
                             {
                                 i.Value.GroupChilds.Ignore(i.View.txt_SubGroup.Value);
                                 i.View.txt_SubGroup.Value = "";
                                 ShowSubGroups();
                             };
                             i.View.btn_AddSubProducts.OnClick += (c1, c2) =>
                             {
                                 i.Value.ProductChilds.Accept(i.View.txt_SubProduct.Value);
                                 i.View.txt_SubProduct.Value = "";
                                 ShowSubProducts();
                             };
                             i.View.btn_RemoveSubProducts.OnClick += (c1, c2) =>
                             {
                                 i.Value.ProductChilds.Ignore(i.View.txt_SubProduct.Value);
                                 i.View.txt_SubProduct.Value = "";
                                 ShowSubProducts();
                             };
                         }
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

                c.SetView<GroupView_html>((i) =>
                {
                    i.MakeHolder = (Table) =>
                    {
                        var Div = new Monsajem_Incs.Resources.Base.Html.Div_html().Main;
                        var H1 = new Monsajem_Incs.Resources.Base.Html.h1_html().Main;

                        var PartTable = Table as Monsajem_Incs.Database.Base.PartOfTable<ProductGroup, string>;
                        if (PartTable != null)
                        {
                            var GroupName = PartTable.HolderTable.Key.ToString();
                            if (GroupName == "Root")
                                GroupName = "فروشگاه صادقی";
                            H1.TextContent = GroupName;
                        }
                        Div.AppendChild(H1);
                        return Div;
                    };
                    i.FillView = (i) =>
                    {
                        i.View.Image.SetAttribute("src", "/GroupImages/" + i.Value.Name + App.CachedUri);
                        i.View.Image.SetAttribute("alt", i.Value.Name);
                        if (App.IsUser)
                        {
                            i.View.ManagerArea.Remove();
                            if (i.Value.GroupChilds.Length > 0)
                                i.View.Link.Href = i.Value.GroupChilds.ProvideUri();
                            else if (i.Value.ProductChilds.Length > 0)
                                i.View.Link.Href = i.Value.ProductChilds.ProvideUri();
                        }
                        else
                        {
                            i.View.Link.Remove();
                            i.View.btn_showgroups.OnClick += (c1, c2) => i.Value.GroupChilds.ShowItems();
                            i.View.btn_showproducts.OnClick += (c1, c2) => i.Value.ProductChilds.ShowItems();
                            i.View.btn_edit_pic.OnClick+=(c1,c2)=>
                            {
                                var PicEditor = new ProductEdit_Pic_html();
                                PicEditor.Done.OnClick += async (c1, c2) =>
                                {
                                    var File = PicEditor.ImageFile.Files[0];
                                    var ORG_Data = await File.ReadBytes();
                                    var Low_Data = PicEditor.Image.GetImageBytes(0.75);
                                    var ResText = await App.UploadFile(ORG_Data, "/GroupImages_ORG/" + i.Value.Name);
                                    ResText = await App.UploadFile(Low_Data, "/GroupImages/" + i.Value.Name);

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
                    };
                    i.GetMain = (i) => i.Main;
                    i.RegisterEdit = (i) =>
                    {
                        if (App.IsUser == false)
                            i.View.btn_edit.OnClick += (c1, c2) => i.Edit();
                    };
                    i.RegisterDelete = (i) =>
                    {
                        if (App.IsUser == false)
                            i.View.btn_delete.OnClick += (c1, c2) => i.Delete();
                    };
                });
            });
        }
    }
}