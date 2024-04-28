using System;
using WebAssembly.Browser.DOM;

namespace Monsajem_Incs.Views.Extentions
{
    public static partial class Ex
    {
        public static HTMLElement AppendChild(
            this HTMLElement Main, Action<(Html Html, View View, Edit Edit)> MakeView)
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