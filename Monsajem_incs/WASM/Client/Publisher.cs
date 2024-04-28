using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Monsajem_Incs.Resources.Base.Partials;
using WebAssembly.Browser.DOM;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WASM_Global
{
    public static class Publisher
    {
        public static JSInProcessRuntime jSRuntime;
        public static NavigationManager NavigationManager;
        private static Storage LocalStorage;

        public static void ShowModal(this HTMLElement Element)
        {
            var Modal = new Modal_html();
            _ = Modal.body.AppendChild(Element);
            _ = js.Document.Body.AppendChild(Modal.myModal);
            Modal.Btn_Close.OnClick += (c1, c2) =>
            {
                Modal.myModal.Remove();
            };
        }
    }
}