using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Monsajem_Incs.Collection.Array.Base;
using Monsajem_Incs.Resources.Partials;
using WebAssembly.Browser.DOM;

namespace WASM_Global
{
    public static class Publisher
    {
        public static IJSRuntime jSRuntime;
        public static IJSInProcessRuntime jSInProcessRuntime;
        public static NavigationManager NavigationManager;
        private static Storage LocalStorage;

        public static void ShowModal(this HTMLElement Element)
        {
            var Modal = new Modal_html();
            Modal.body.AppendChild(Element);
            Document.document.Body.AppendChild(Modal.myModal);
            Modal.Btn_Close.OnClick+=(c1,c2) =>
            {
                Modal.myModal.Remove();
            };
        }
    }
}