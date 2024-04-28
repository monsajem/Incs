using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLOptionsCollection", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLOptionsCollection : HTMLCollectionOf<HTMLOptionElement>
    {
        internal HTMLOptionsCollection(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLOptionsCollection() { }
        //[Export("length")]
        //public double Length { get => GetProperty<double>("length"); set => SetProperty<double>("length", value); }
        [Export("selectedIndex")]
        public double SelectedIndex { get => GetProperty<double>("selectedIndex"); set => SetProperty<double>("selectedIndex", value); }
        [Export("add")]
        public void Add(object element, object before)
        {
            _ = InvokeMethod<object>("add", element, before);
        }
        [Export("remove")]
        public void Remove(double index)
        {
            _ = InvokeMethod<object>("remove", index);
        }
    }
}