using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;
using Monsajem_Incs.Resources.Base.Html;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Collection.Array;
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views
{
    public class Html
    {
        internal HTMLElement Main;
        
        public HTMLElement Div(Action<HTMLElement> Edit=null)
        {
            var Element = new Div_html().Main;
            Edit?.Invoke(Element);
            Main.AppendChild(Element);
            return Element;
        }

        public HTMLElement Button(Action<HTMLElement> Edit = null)
        {
            var Element = new button_html().Main;
            Edit?.Invoke(Element);
            Main.AppendChild(Element);
            return Element;
        }
    }
}
