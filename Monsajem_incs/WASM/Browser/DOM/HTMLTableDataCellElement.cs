using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTableDataCellElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTableDataCellElement : HTMLTableCellElement, IHTMLTableDataCellElement
    {
        internal HTMLTableDataCellElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTableDataCellElement () { }
    }
}