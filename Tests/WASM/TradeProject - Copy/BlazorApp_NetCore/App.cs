using Microsoft.JSInterop;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using System;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using WebAssembly.Browser.MonsajemDomHelpers;
using Monsajem_Incs.Serialization;
using System.Linq;
using static Monsajem_Incs.UserControler.Publish;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.IO.Compression;

namespace Monsajem_Client
{
    public class App
    {
        public static string ActionUri
        {
            get
            {
                if (IsUser == false)
                    return @"php/AdminActions/";
                else
                    return @"php/UserActions/";
            }
        }
        public static bool IsUser
        {
            get
            {
                if (UserName == null ||
                    Window.window.LocalStorage.Contains("UserType") == false)
                    IsUser = true;
                var Res = Window.window.LocalStorage.GetItem("UserType");
                if (Res == "User")
                    return true;
                else if (Res == "Admin")
                    return false;
                else
                {
                    IsUser = true;
                    return true;
                }
            }
            set
            {
                if (value == true)
                    Window.window.LocalStorage.SetItem("UserType", "User");
                else
                    Window.window.LocalStorage.SetItem("UserType", "Admin");
            }
        }
        private static string _CachedUri;
        public static string CachedUri
        {
            get
            {
                if (IsUser)
                    return _CachedUri;
                else
                    return "?" + DateTime.UtcNow.ToShortDateString() +
                                 DateTime.UtcNow.ToLongTimeString();
            }
        }

        public static string UserName
        {
            get
            {
                if (Window.window.LocalStorage.Contains("UserName"))
                    return Window.window.LocalStorage.GetItem("UserName");
                else
                    return null;
            }
            set
            {
                if (value == null)
                    Window.window.LocalStorage.RemoveItem("UserName");
                else
                    Window.window.LocalStorage.SetItem("UserName", value);
            }
        }
        public static string Password
        {
            get
            {
                if (Window.window.LocalStorage.Contains("Password"))
                    return Window.window.LocalStorage.GetItem("Password");
                else
                    return null;
            }
            set
            {
                if (value == null)
                    Window.window.LocalStorage.RemoveItem("Password");
                else
                    Window.window.LocalStorage.SetItem("Password", value);
            }
        }

        public static bool IsLocal
        {
            get
            {
                return WASM_Global.Publisher.NavigationManager.BaseUri.
                     StartsWith("http://localhost");
            }
        }

        internal static Func<Task> TasksAfterUploadDB;

        internal static DataBase Data;

        internal static BasePage_html BasePage_html;
        internal static HTMLElement MainElement { get => BasePage_html.P_Page; }

        internal static Monsajem_Incs.Database.Register.MemoryRegister<object> Register =
            new Monsajem_Incs.Database.Register.MemoryRegister<object>();
        private static bool BasketLoaded;

        public static async Task Main_Run()
        {
            js.IJSUnmarshalledRuntime = (IJSUnmarshalledRuntime)WASM_Global.Publisher.jSRuntime;
            js.Document.Title = "trader finder";
            js.Document.Body.AppendChild(BasePage_html.HtmlText);
            BasePage_html = new BasePage_html(true);

            Monsajem_Incs.Assembly.Assembly.AddAssembly(typeof(App).Assembly);
            
            HideAction();
        
            Data = new DataBase(Register);

            try
            {
                var Products = System.Convert.FromBase64String(
                                    Window.window.LocalStorage.GetItem("Products")).
                                    Deserialize<SelectedProduct[]>();
                foreach (var Product in Products)
                    try
                    {
                        if (Data.Products.IsExist(Product.ProductName))
                            Data.SelectedProducts.Insert(Product);
                    }
                    catch { }
            }
            catch
            { }
            BasketLoaded = true;

            OnLoadApp.GetReady();

            if ((UserName == null || IsUser == true)&&IsLocal==false)
                new OnLoadApp.LoginPage().Show();
        }

        public static void SelectedProductsChanged()
        {
            if (BasketLoaded == false)
                return;
            var SelectedProducts = Data.SelectedProducts.Select((c) => c.Value).
                                        Where((c) => c.Count > 0).ToArray();
            if (SelectedProducts.Length > 0)
            {
                BasePage_html.lbl_ShopedCount.TextContent = SelectedProducts.Length.ToString();
                BasePage_html.img_basket.SetAttribute("class", "mn-btn-basket");
            }
            else
            {
                BasePage_html.lbl_ShopedCount.TextContent = "";
                BasePage_html.img_basket.SetAttribute("class", "w3-grayscale-max");
            }

            Window.window.LocalStorage.SetItem("Products",
                System.Convert.ToBase64String(SelectedProducts.Serialize()));
        }

        public static async Task<byte[]> Request(
            string Address,
            Action<MultipartFormDataContent> Data = null)
        {
            if (Address.StartsWith("/"))
                Address = Address.Substring(1);
            Address = WASM_Global.Publisher.NavigationManager.BaseUri + Address;
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Address);
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(""), "Q");
                Data?.Invoke(content);
                request.Content = content;
                HttpResponseMessage response = null;
                byte[] ResBytes = null;
                Exception Error = null;
                try
                {
                    response = await httpClient.SendAsync(request);
                    ResBytes = await response.Content.ReadAsByteArrayAsync();
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
                if (Error != null)
                {
                    ShowDangerMessage("خطای شبکه");
                    throw Error;
                }
                return ResBytes;
            }
        }

        public static async Task<string> RequestAsString(
            string Address,
            Action<MultipartFormDataContent> Data = null)=>
            System.Text.Encoding.UTF8.GetString(await Request(Address, Data));

        public static async Task<byte[]> RequestLogin(
            string Address,
            Action<MultipartFormDataContent> Data = null,
            bool? IsUser = null)
        {
            if (UserName == null)
            {
                ShowDangerMessage("لطفا ابتدا وارد شوید");
                new OnLoadApp.LoginPage().Show();
                throw new Exception("User Not Found.");
            }

            var ResBytes = await Request(Address, (content) =>
            {
                content.Add(new StringContent(App.UserName), "UserName");
                content.Add(new StringContent(App.Password), "Password");
                Data?.Invoke(content);
            });

            var ResText = System.Text.Encoding.UTF8.GetString(ResBytes);

            if (ResText.StartsWith("LogedIn."))
            {
                return ResBytes.AsSpan(8).ToArray();
            }
            else if (ResText == "User Not Found.")
            {
                App.UserName = null;
                App.Password = null;
                ShowDangerMessage("شماره تلفن یا پسورد شما اشتباه است");
                new OnLoadApp.LoginPage().Show();
                throw new Exception("User Not Found.");
            }
            else
            {
                ShowDangerMessage("خطای ناسناخته");
                throw new Exception("unknown error");
            }
        }

        public static async Task<string> RequestLoginAsString(
            string Address,
            Action<MultipartFormDataContent> Data = null,
            bool? IsUser = null) =>
            System.Text.Encoding.UTF8.GetString(await RequestLogin(Address, Data, IsUser));

        public static async Task<XmlDocument> RequestXml(
            string Address,
            Action<MultipartFormDataContent> Data = null)
        {
            var doc = new XmlDocument();
            doc.LoadXml(await RequestLoginAsString(Address, Data));
            return doc;
        }

        public static async Task<string> UploadFile(byte[] Data, string FileAddress)
        {
            if (FileAddress.StartsWith("/"))
                FileAddress = FileAddress.Substring(1);
            var OldIsUser = IsUser;
            try
            {
                IsUser = false;
                return await RequestLoginAsString(App.ActionUri + @"SubmitFile.php", (c) =>
                {
                    c.Add(new StringContent(FileAddress), "address");
                    c.Add(new StreamContent(new MemoryStream(Data)), "fileToUpload", "FileName");
                });
            }
            finally
            {
                IsUser = OldIsUser;
            }
        }

        public static async Task<string> RenameFile(
            string OldFileAddress,
            string NewFileAddress)
        {
            if (OldFileAddress.StartsWith("/"))
                OldFileAddress = OldFileAddress.Substring(1);
            if (OldFileAddress.StartsWith("/"))
                OldFileAddress = OldFileAddress.Substring(1);
            var OldIsUser = IsUser;
            try
            {
                IsUser = false;
                return await RequestLoginAsString(App.ActionUri + @"RenameFile.php", (c) =>
                {
                    c.Add(new StringContent(OldFileAddress), "OldName");
                    c.Add(new StringContent(NewFileAddress), "NewName");
                });
            }
            finally
            {
                IsUser = OldIsUser;
            }
        }

        public static async Task<string> DeleteFile(string FileAddress)
        {
            if (FileAddress.StartsWith("/"))
                FileAddress = FileAddress.Substring(1);
            var OldIsUser = IsUser;
            try
            {
                IsUser = false;
                return await RequestLoginAsString(App.ActionUri + @"RenameFile.php", (c) =>
                {
                    c.Add(new StringContent(FileAddress), "FileAddress");
                });
            }
            finally
            {
                IsUser = OldIsUser;
            }
        }

        public static async Task<byte[]> DownloadData(string Url)
        {
            var OldIsUser = IsUser;
            try
            {
                IsUser = false;
                return await RequestLogin(App.ActionUri + @"DownloadFromOther.php", (c) =>
                {
                    c.Add(new StringContent(Url), "UrlAddress");
                });
            }
            finally
            {
                IsUser = OldIsUser;
            }
        }
    }
}