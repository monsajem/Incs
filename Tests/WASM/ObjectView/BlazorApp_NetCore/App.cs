﻿using System;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Resources.Database;
using WebAssembly.Browser.MonsajemDomHelpers;
using WebAssembly.Browser.DOM;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.TimeingTester;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop;
using Monsajem_Incs.Views.Maker.ValueTypes;
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
            js.IJSUnmarshalledRuntime = (IJSUnmarshalledRuntime)WASM_Global.Publisher.jSRuntime;
            js.Document.Body.AppendChild(BasePage_html.HtmlText);
            BasePage_html = new BasePage_html(true);
            Monsajem_Incs.Views.Page.SubmitPage(MainElement);

             ViewItemMaker.SetView<ProductGroup,GroupEdit_html>(
                    (c) =>
                    {
                        c.FillView = (c) =>
                        {
                            if (c.Value != null)
                                c.View.Name.Value = c.Value.Name;
                        };
                        c.GetMain = (c) => c.Main;
                    });

            MainElement.ReplaceChilds(new ProductGroup() { Name = "Root" }.MakeView());
        }
    }
}