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
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views.Extentions
{
    public static partial class Ex
    {
        public static HTMLElement AppendChild(
            this HTMLElement Main,Action<(Html Html,View View,Edit Edit)> MakeView)
        {
            MakeView((
                new Html() { Main = Main },
                new View() { Main = Main },
                new Edit() { Main = Main }));
            return Main;
        }
        public static HTMLElement ReplaceChilds(
            this HTMLElement Main, Action<(Html Html, View View, Edit Edit)> MakeView)
        {
            MakeView((
                new Html() { Main = Main },
                new View() { Main = Main },
                new Edit() { Main = Main }));
            return Main;
        }
    }
}