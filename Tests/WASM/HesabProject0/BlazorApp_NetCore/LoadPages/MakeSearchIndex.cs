using System.Threading.Tasks;
using Monsajem_Incs.Resources.Partials;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using Monsajem_Incs.Views.Extentions;
using System.Linq;
using static Monsajem_Client.App;
using Monsajem_Incs.Serialization;
using System;
using static Monsajem_Incs.UserControler.Publish;
using System.Net.Http;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {

        public static async Task MakeSearchIndex()
        {
            try
            {
                App.IsUser = true;
                WebAssembly.Browser.DOM.Document MainDoc;
                WebAssembly.Browser.DOM.HTMLElement AppPreload_html;
                var MainTitle = "";
                var MainDescription = "";
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var MainPageHtml = await httpClient.GetStringAsync(WASM_Global.Publisher.NavigationManager.BaseUri + "index.html");
                        MainDoc = WebAssembly.Browser.DOM.Document.Parse(MainPageHtml);

                        foreach (var ShouldClear in MainDoc.GetElementsByClassName("ClearOnBot").ToArray())
                        {
                            ShouldClear.Remove();
                        }

                        MainDoc.Body.RemoveAttribute("style");

                        if (MainDoc.Body.InnerHtml == "")
                            throw new Exception("Site html not found!");
                        AppPreload_html = MainDoc.GetElementById("AppPreload");
                        MainTitle = MainDoc.Title;
                        {
                            var description = MainDoc.GetElementsByTagName("meta").ToArray().
                                        Where((c) => c?.GetAttribute("name") == "description").FirstOrDefault();
                            MainDescription = description?.GetAttribute("content");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowDangerMessage("خطای شبکه");
                        throw;
                    }
                }

                var SiteMapInfo = "";
                {
                    var Xml = new System.Xml.XmlDocument();

                    {
                        var Root = Xml.CreateElement("urlset");
                        var Atr = Xml.CreateAttribute("xmlns");
                        Atr.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
                        Root.Attributes.Append(Atr);
                        Xml.AppendChild(Root);

                        Func<string, Task> AddSiteMap = async (Address) =>
                        {
                            var Node = Xml.CreateElement("url");
                            {
                                var Data = Xml.CreateElement("loc");
                                Data.InnerText = Address;
                                Node.AppendChild(Data);
                            }
                            {
                                var Data = Xml.CreateElement("lastmod");
                                Data.InnerText = DateTime.UtcNow.Year + "-" + DateTime.UtcNow.Month + "-" + DateTime.UtcNow.Day;
                                Node.AppendChild(Data);
                            }
                            {
                                var Data = Xml.CreateElement("changefreq");
                                Data.InnerText = "daily";
                                Node.AppendChild(Data);
                            }
                            {
                                var Data = Xml.CreateElement("priority");
                                Data.InnerText = "1";
                                Node.AppendChild(Data);
                            }
                            Root.AppendChild(Node);

                            var OldIsUser = IsUser;

                            //try
                            //{
                            //    IsUser = false;
                            //    var res = await RequestWithLogin(App.ActionUri + @"SubmitBingIndex.php", (c) =>
                            //    {
                            //        c.Add(new StringContent(Address), "UrlAddress");
                            //    });
                            //    Console.WriteLine("Bing Result " + res);
                            //}
                            //finally
                            //{
                            //    IsUser = OldIsUser;
                            //}
                        };

                        var BaseUri = WASM_Global.Publisher.NavigationManager.BaseUri;
                        await AddSiteMap(BaseUri);
                        BaseUri = BaseUri.Substring(0, BaseUri.Length - 1);

                        Func<(string Url,
                              string Description,
                              string Title,
                              WebAssembly.Browser.DOM.HTMLElement Elements),
                              Task>
                            SubmitPage = async (c) =>
                            {
                                MainDoc.Title = c.Title;

                                var description = MainDoc.GetElementsByTagName("meta").ToArray().
                                                    Where((c) => c?.GetAttribute("name") == "description").FirstOrDefault();
                                description?.SetAttribute("content", c.Description);

                                AppPreload_html.InnerHtml = "";
                                AppPreload_html.AppendChild(c.Elements);
                                await AddSiteMap(BaseUri + c.Url);
                                var FileHtml = $"<!DOCTYPE html><html lang=\"fa\">{MainDoc.Head.OuterHtml}{MainDoc.Body.OuterHtml}</html>";
                                if (c.Url.StartsWith("/?"))
                                    c.Url = c.Url.Substring(2);
                                else if (c.Url.StartsWith("/"))
                                    c.Url = c.Url.Substring(1);
                                await UploadFile(System.Text.Encoding.UTF8.GetBytes(FileHtml),
                                   "/crawled/" + c.Url);
                            };


                        foreach (var Group in Data.Groups)
                        {
                            WebAssembly.Browser.DOM.HTMLElement element = default;
                            var Title = MainTitle;
                            var Description = MainDescription;
                            var Url = "";
                            if (Group.Value.Name != "Root")
                            {
                                Title = Group.Value.Name;
                                Description = Group.Value.Describe;
                                if (Description == null || Description == "")
                                    Description = Title;
                            }
                            if (Group.Value.ProductChilds.Length > 0)
                            {
                                Url = Group.Value.ProductChilds.ProvideUri();
                                element = Group.Value.ProductChilds.MakeShowView();
                            }
                            else if (Group.Value.GroupChilds.Length > 0)
                            {
                                Url = Group.Value.GroupChilds.ProvideUri();
                                element = Group.Value.GroupChilds.MakeShowView();
                            }

                            if (Url != "")
                            {
                                await SubmitPage((
                                        Url,
                                        Description,
                                        Title,
                                        element));
                                if (Group.Value.Name == "Root")
                                {
                                    await SubmitPage((
                                            "/index.html",
                                            Description,
                                            Title,
                                            element));
                                }
                            }
                        }
                        IsUser = false;

                        await SubmitPage(("/?AboutUs",
                                          MainDescription,
                                          MainTitle + " درباره ما ",
                                          new Monsajem_Incs.Resources.AboutUs_html().Main));
                    }

                    SiteMapInfo = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Xml.OuterXml;
                }
                await UploadFile(System.Text.Encoding.UTF8.GetBytes(
                    SiteMapInfo), "/sitemap.xml");
            }
            finally
            {
                App.IsUser = false;
            }
        }
    }
}