using System;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Resources.Database;
using WebAssembly.Browser.MonsajemDomHelpers;
using WebAssembly.Browser.DOM;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.TimeingTester;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop;
using static Monsajem_Incs.Views.Maker.ViewItemMaker;
using static Monsajem_Incs.Views.Maker.EditItemMaker;

namespace Monsajem_Client
{
	public class App
    {
        public static Uri Client = new Uri("http://localhost:49696/ServerInfo/net5.0/DB");
        internal static DataBase Data;

        internal static BasePage_html BasePage_html;
        internal static HTMLElement MainElement { get => BasePage_html.P_Page; }

        public static async Task Main_Run()
        {
            Network.Service_IpAddress = "127.0.0.1";
            Network.Service_Port = 8845;
            js.IJSUnmarshalledRuntime = (IJSUnmarshalledRuntime)WASM_Global.Publisher.jSRuntime;
            js.Document.Body.AppendChild(BasePage_html.HtmlText);
            BasePage_html = new BasePage_html(true);
            UserControler.Page.SubmitPage(MainElement);

            {
                SetView<ProductGroup, GroupView_html>((i) =>
                {
                    i.View.Name.TextContent = i.Value.Name;
                });
                EditItemMaker<ProductGroup, GroupEdit_html>.MakeDefault(
                OnMakeView: (i) =>
                {
                    i.View.Name.Value = i.Value.Name;
                },
                OnEdited:(i)=>
                {
                    i.Value.Name = i.View.Name.Value;
                });
            }

            Window.window.LocalStorage.Clear();
            Data = new ClientDataBase(Window.window.LocalStorage);

            new UserControler.Partial.ShowPage().Show(Data.Groups);
        }
    }
}