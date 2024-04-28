
using System;
using WebAssembly.Browser.DOM;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace Monsajem_Incs.Views
{
    public static class Menu
    {

        public static void Show(HTMLElement Element, Action OnBack = null)
        {
            var Modal = new Resources.Base.Partials.Modal_html();
            _ = Modal.body.AppendChild(Element);
            _ = js.Document.Body.AppendChild(Modal.myModal);
            var Win = new Window();
            Win.OnPopState += (c1, c2) =>
            {
                Modal.Btn_Close.Click();
            };
            Win.History.PushState("", "", "");
            Modal.Btn_Close.OnClick += (c1, c2) =>
            {
                OnBack?.Invoke();
                Modal.myModal.Remove();
            };
        }
    }
}