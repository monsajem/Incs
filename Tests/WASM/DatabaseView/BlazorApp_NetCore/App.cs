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
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;

using static Monsajem_Client.Network;

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
            Monsajem_Incs.Views.Page.SubmitPage(MainElement);

            Window.window.LocalStorage.Clear();
            Data = new ClientDataBase(Window.window.LocalStorage);

            {
                Data.Groups.RegisterEdit().SetDefault<GroupEdit_html>(
                    FillView: (c) =>
                    {
                        if(c.Value!=null)
                            c.View.Name.Value = c.Value.Name;
                    },
                    FillValue: (c) =>
                     {
                         return new ProductGroup()
                         {
                             Name = c.View.Name.Value
                         };
                     },
                    SetEdited: (c) => c.View.Done.OnClick += (c1, c2) => c.Edited(),
                    GetMain: (c) => c.Main);
                Data.Groups.RegisterView().SetDefault<GroupView_html>(
                    FillView: (c) =>
                    {
                        c.View.Name.Value = c.Value.Name;
                    },
                    GetMain: (c) => c.Main,
                    RegisterEdit:(c)=>
                    {
                        c.View.Edit.OnClick += (c1, c2) => c.Edit();
                    },
                    RegisterDelete:(c)=>
                    {
                        c.View.Delete.OnClick += (c1, c2) => c.Delete();
                    });
            }

            await Data.Groups.SyncUpdate();

            Data.Groups.ShowItems();

            return;

            MainElement.ReplaceChilds(await Data.Groups.MakeShowView(
                async (c)=>
                {
                    MainElement.ReplaceChilds(Data.Groups.MakeEditView(c.Key));
                    Console.WriteLine("Edit" + c.ToString());
                },
                async (c)=>
                {
                    await c.TableInfo.Delete(c.Key);
                    Console.WriteLine("Delete" + c.ToString());
                }));
        }
    }
}