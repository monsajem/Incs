
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
using Monsajem_Incs.Resources.Html;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.ArrayExtentions;
using System.Runtime.Serialization;

namespace Monsajem_Incs.Views
{
    public static class Menu
    {

        public static void Show(HTMLElement Element, Action OnBack = null)
        {
            var Modal = new Resources.Partials.Modal_html();
            Modal.body.AppendChild(Element);
            Document.document.Body.AppendChild(Modal.myModal);
            var Win = new Window();
            Win.OnPopState += (c1, c2) =>
            {
                Modal.Btn_Close.Click();
            };
            Win.History.PushState("","","");
            Modal.Btn_Close.OnClick+=(c1,c2) =>
            {
                OnBack?.Invoke();
                Modal.myModal.Remove();
            };
        }
    }
}