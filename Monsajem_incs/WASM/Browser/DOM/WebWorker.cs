using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{
    [Export("MessageEvent", typeof(IJSInProcessObjectReference))]
    public class MessageEvent : DOMObject
    {
        public MessageEvent(IJSInProcessObjectReference handle) : base(handle) { }

        [Export("data")]
        public t GetData<t>()=> GetProperty<t>("data");

        [Export("origin")]
        public string origin { get => GetProperty<string>("origin"); }

        [Export("lastEventId")]
        public string lastEventId { get => GetProperty<string>("lastEventId"); }
    }

    [Export("Worker", typeof(IJSInProcessObjectReference))]
    public class WebWorker: DOMObject
    {
        private static WebWorker _CurrentWebWorker;
        public static WebWorker CurrentWebWorker { 
            get
            {
                if (MonsajemDomHelpers.WebProcess.IsInWorker == false)
                    throw new PlatformNotSupportedException("CurrentWebWorker just declared in WebWorker.");
                if(_CurrentWebWorker==null)
                    _CurrentWebWorker = new WebWorker(js.JsGetValue("self"));
                return _CurrentWebWorker;
            }
        }

        public WebWorker(string js):
            this(new Uri("data:text/javascript;base64," + 
                Convert.ToBase64String(Encoding.UTF8.GetBytes(js))))
        {}

        public WebWorker(System.Uri Uri) : 
            this(js.JsNewObject("Worker",Uri.ToString()))
        {}

        public WebWorker(IJSInProcessObjectReference handle) : base(handle) 
        {
            SetProperty("onmessage",(Action<IJSInProcessObjectReference>)onmessage);
        }

        [Export("postMessage")]
        public void PostMessage(object Message)
        {
            InvokeMethod<object>("postMessage", Message);
        }

        private void onmessage(IJSInProcessObjectReference msg)
        {
            OnMessage?.Invoke(new MessageEvent(msg));
        }

        public event Action<MessageEvent> OnMessage;
    }

    public class WebWorkerClient
    {
        private WebWorker worker;
        public WebWorkerClient()
        {
            //worker = new WebWorker("console.log(\"WebWorker Script0\");");
            worker = new WebWorker("self.onmessage=function(e){eval(e.data);}");
            //worker.PostMessage("console.log(\"WebWorker Script1\");");
            worker.OnMessage += OnMessage;
        }

        private class MessageEventHandle
        {
            public Task Handle;
            public MessageEvent Result;
        }
        private Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize.Array<MessageEventHandle>
            MessageQuque = new Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize.Array<MessageEventHandle>(20);
        private void OnMessage(MessageEvent e)
        { 
            if(MessageQuque.Length>0)
            {
                var msg = MessageQuque.Pop();
                msg.Result = e;
                msg.Handle.Start();
            }
        }

        public Task<MessageEvent> GetMessage() => GetMessage(false);
        private async Task<MessageEvent> GetMessage(bool First=false)
        {
            var Handle = new MessageEventHandle(){Handle=new Task(()=>{})};
            if(First)
                MessageQuque.Insert(Handle,0);
            else
                MessageQuque.Insert(Handle);
            await Handle.Handle;
            return Handle.Result;
        }

        public void PostMessage(string SaveAsName,object Message)
        {
            worker.PostMessage($"self.onmessage=function(e){{self.{SaveAsName}=e.data;self.onmessage=function(q){{eval(q.data);}};}}");
            worker.PostMessage(Message);
        }

        public async Task Run(string js)
        {
            var Compeleted = GetMessage(true);
            worker.PostMessage($"eval({MonsajemDomHelpers.js.ToJsValue(js)});postMessage('');");
            await Compeleted;
        }

        public async Task<t> Result<t>(string js)
        {
            var Compeleted = GetMessage(true);
            worker.PostMessage($"postMessage(eval({MonsajemDomHelpers.js.ToJsValue(js)}));");
            return (await Compeleted).GetData<t>();
        }

        public async Task<t> ResultFrom<t>(string URL)
        {
            var Compeleted = GetMessage(true);
            worker.PostMessage(
                @"var xhttp=new XMLHttpRequest();
                  xhttp.onreadystatechange = function() {
                    if (this.readyState == 4 && this.status == 200) {
                        postMessage(eval(xhttp.responseText));
                    }
                  };
                  xhttp.open('GET', "+MonsajemDomHelpers.js.ToJsValue(URL)+@", true);
                  xhttp.send();");
            return (await Compeleted).GetData<t>();
        }

        public async Task RunFrom(string URL)
        {
            var Compeleted = GetMessage(true);
            worker.PostMessage(
                @"var xhttp=new XMLHttpRequest();
                  xhttp.onreadystatechange = function() {
                    if (this.readyState == 4 && this.status == 200) {
                        eval(xhttp.responseText);
                        postMessage('');
                    }
                  };
                  xhttp.open('GET', " + MonsajemDomHelpers.js.ToJsValue(URL) + @", true);
                  xhttp.send();");
            await Compeleted;
        }
    }
}