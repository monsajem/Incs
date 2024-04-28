using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("DataTransferItem", typeof(IJSInProcessObjectReference))]
    public sealed class DataTransferItem : DOMObject
    {
        internal DataTransferItem(IJSInProcessObjectReference handle) : base(handle) { }

        //public DataTransferItem() { }
        [Export("kind")]
        public string Kind => GetProperty<string>("kind");
        [Export("type")]
        public string Type => GetProperty<string>("type");
        //[Export("getAsFile")]
        //public File GetAsFile()
        //{
        //  return InvokeMethod<File>("getAsFile");
        //}
        //[Export("getAsString")]
        //public void GetAsString(FunctionStringCallback _callback)
        //{
        //  InvokeMethod<object>("getAsString", _callback);
        //}
        //[Export("webkitGetAsEntry")]
        //public Object WebkitGetAsEntry()
        //{
        //  return InvokeMethod<Object>("webkitGetAsEntry");
        //}
    }

}
