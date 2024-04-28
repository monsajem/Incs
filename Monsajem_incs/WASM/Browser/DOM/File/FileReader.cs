using Microsoft.JSInterop;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{
    [Export("FileReader", typeof(IJSInProcessObjectReference))]
    public class FileReader : EventTarget
    {
        internal FileReader(IJSInProcessObjectReference handle) : base(handle) { }

        public FileReader() : this(js.JsNewObject("FileReader")) { }

        [Export("readAsDataURL")]
        public void ReadAsDataURL(Blob selectors)
        {
            _ = InvokeMethod<object>("readAsDataURL", selectors);
        }
        [Export("readAsArrayBuffer")]
        public void ReadAsArrayBuffer(Blob selectors)
        {
            _ = InvokeMethod<object>("readAsArrayBuffer", selectors);
        }
        [Export("readAsText")]
        public void ReadAsText(Blob selectors)
        {
            _ = InvokeMethod<object>("readAsText", selectors);
        }
        [Export("abort")]
        public void Abort()
        {
            _ = InvokeMethod<object>("abort");
        }

        public object[] Result { get => GetProperty<object[]>("result"); }

        public event DOMEventHandler OnLoadStart
        {
            add => AddEventListener("onloadstart", value, false);
            remove => RemoveEventListener("onloadstart", value, false);
        }
        public event DOMEventHandler OnProgress
        {
            add => AddEventListener("onprogress", value, false);
            remove => RemoveEventListener("onprogress", value, false);
        }
        public event DOMEventHandler OnAbort
        {
            add => AddEventListener("onabort", value, false);
            remove => RemoveEventListener("onabort", value, false);
        }
        public event DOMEventHandler OnError
        {
            add => AddEventListener("onerror", value, false);
            remove => RemoveEventListener("onerror", value, false);
        }
        public event DOMEventHandler OnLoad
        {
            add => AddEventListener("onload", value, false);
            remove => RemoveEventListener("onload", value, false);
        }
        public event DOMEventHandler OnLoadEnd
        {
            add => AddEventListener("onloadend", value, false);
            remove => RemoveEventListener("onloadend", value, false);
        }
    }
}