using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("DOMSettableTokenList", typeof(IJSInProcessObjectReference))]
    public sealed class DOMSettableTokenList : DOMTokenList, IDOMSettableTokenList
    {
        internal DOMSettableTokenList(IJSInProcessObjectReference handle) : base(handle) { }

        //public DOMSettableTokenList() { }
        [Export("value")]
        public string NodeValue { get => GetProperty<string>("value"); set => SetProperty<string>("value", value); }
    }
}