﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;
using Monsajem_Incs.Resources.Base.Partials.Edit;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Collection.Array;
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views
{
    public class Edit
    {
        internal HTMLElement Main;

        public HTMLElement Count()
        {
            var Element = new Input_Count_html();
            Main.AppendChild(Element.Main);
            return Element.txt_Count;

        }
    }
}
