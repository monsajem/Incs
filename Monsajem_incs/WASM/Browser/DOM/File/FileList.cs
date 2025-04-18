using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{
    /// <summary>
    /// An object of this type is returned by the files property of the HTML input element; this lets you access the list of files selected with the &lt;input type="file"&gt; element. It's also used for a list of files dropped into web content when using the drag and drop API; see the DataTransfer object for details on this usage.
    /// </summary>

    [Export("FileList", typeof(IJSInProcessObjectReference))]
    public class FileList : DOMObject
    {
        internal FileList(IJSInProcessObjectReference jsObject) : base(jsObject) { }

        public File this[int index] { get => GetItem(index); }

        [Export("item", typeof(IJSInProcessObjectReference))]
        public File GetItem(int index)
        {
            return InvokeMethod<File>("item", index);
        }

        [Export("length")]
        public int Length { get => GetProperty<int>("length"); }
    }
}