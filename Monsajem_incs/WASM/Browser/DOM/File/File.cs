using System.IO;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{   
    [Export("File", typeof(IJSInProcessObjectReference))]
    public class File : Blob
    {
        internal File(IJSInProcessObjectReference jsObject) :base(jsObject) {}
        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }
    }
}