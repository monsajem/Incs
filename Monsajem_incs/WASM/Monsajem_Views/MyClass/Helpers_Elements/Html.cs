using Monsajem_Incs.Resources.Base.Html;
using System;
using WebAssembly.Browser.DOM;

namespace Monsajem_Incs.Views
{
    public class Html
    {
        internal HTMLElement Main;

        public HTMLElement Div(Action<HTMLElement> Edit = null)
        {
            var Element = new Div_html().Main;
            Edit?.Invoke(Element);
            _ = Main.AppendChild(Element);
            return Element;
        }

        public HTMLElement Button(Action<HTMLElement> Edit = null)
        {
            var Element = new button_html().Main;
            Edit?.Invoke(Element);
            _ = Main.AppendChild(Element);
            return Element;
        }
    }
}
