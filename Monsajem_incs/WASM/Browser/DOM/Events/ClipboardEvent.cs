using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM.Events
{

    [Export("ClipboardEvent", typeof(IJSInProcessObjectReference))]
    public sealed class ClipboardEvent : Event
    {
        internal ClipboardEvent(IJSInProcessObjectReference handle) : base(handle) { }

        //public ClipboardEvent (string type, ClipboardEventInit eventInitDict) { }
        DataTransfer clipboardData;

        [Export("clipboardData")]
        public DataTransfer ClipboardData
        {
            get
            {
                clipboardData ??= GetProperty<DataTransfer>("clipboardData");
                return clipboardData;
            }
            set => SetProperty<DataTransfer>("clipboardData", value);
        }


        protected override void Dispose(bool disposing)
        {
            // the event object handle is already unregistered within the event handling function
            // no need to do this again.
            if (disposing)
            {
                if (clipboardData != null)
                {
                    clipboardData.Dispose();
                }
            }

        }
    }
}