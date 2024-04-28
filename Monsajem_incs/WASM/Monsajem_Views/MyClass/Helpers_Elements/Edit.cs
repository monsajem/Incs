using Monsajem_Incs.Resources.Base.Partials.Edit;
using WebAssembly.Browser.DOM;

namespace Monsajem_Incs.Views
{
    public class Edit
    {
        internal HTMLElement Main;

        public HTMLElement Count()
        {
            var Element = new Input_Count_html();
            _ = Main.AppendChild(Element.Main);
            return Element.txt_Count;

        }
    }
}
