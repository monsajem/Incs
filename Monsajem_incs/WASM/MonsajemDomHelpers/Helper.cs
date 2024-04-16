using System;
using System.Collections.Generic;
using System.Text;
using static Monsajem_Incs.Collection.Array.Extentions;
using WebAssembly.Browser.DOM;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using Microsoft.JSInterop;
using Microsoft.JSInterop;
using System.IO;
using Monsajem_Incs.Serialization;
using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.MonsajemDomHelpers
{
    public static class js
    {
        public static Document Document;
        public static JSInProcessRuntime JsRuntime;
        private static Action OnPopState;
        private static Action[] MyHistorySates;
        private static IJSInProcessObjectReference JsInfo;

        private static void CreateJsInfo()
        {
            JsInfo = JsRuntime.Invoke<IJSInProcessObjectReference>("eval", @"({
  GetValue_obj: function (obj,name) {
        return obj[name];
  },
  GetValue_name: function (name) {
        return self[name];
  },
  SetValue_obj: function (obj,name,val) {
        obj[name]=val;
  },
  SetValue_name: function (name,val) {
        self[name]=val;
  },
  HaveValue_obj: function (obj,name) {
        return obj[name] !== undefined;
  },
  HaveValue_name: function (name) {
        return self[name] !== undefined;
  },
  GetTypeName_obj: function (obj) {
        return obj.constructor.name;
  },
  GetTypeName_name: function (name) {
        return self[name].constructor.name;
  },
})");
        }
        public static void Start(JSInProcessRuntime JSR)
        {
            JsRuntime = JSR;
            var Doc = JsRuntime.Invoke<IJSInProcessObjectReference>("eval", "document");
            var Win = JsRuntime.Invoke<IJSInProcessObjectReference>("eval", "window");
            CreateJsInfo();
            Document = new Document();
        }

        public static t JsGetValue<t>(this string Name)
        {
            CreateJsInfo();
            return JsInfo.Invoke<t>("GetValue_name", Name);
        }
        public static IJSInProcessObjectReference JsGetValue(this string Name)
        {
            CreateJsInfo();
            return JsGetValue<IJSInProcessObjectReference>(Name);
        }
        public static void JsSetValue(this string Name, object Value)
        {
            CreateJsInfo();
            JsInfo.InvokeVoid("SetValue_name", Name, Value);
        }
        public static bool JsHaveValue(this string Name)
        {
            CreateJsInfo();
            return JsInfo.Invoke<bool>("HaveValue_name", Name);
        }
        public static t JsGetValue<t>(this IJSInProcessObjectReference obj, string Name)
        {
            CreateJsInfo();
            return JsInfo.Invoke<t>("GetValue_obj", obj, Name);
        }
        public static IJSInProcessObjectReference JsGetValue(this IJSInProcessObjectReference obj, string Name)
        {
            CreateJsInfo();
            return JsGetValue<IJSInProcessObjectReference>(obj, Name);
        }
        public static void JsSetValue(this IJSInProcessObjectReference obj, string Name, object Value)
        {
            CreateJsInfo();
            JsInfo.InvokeVoid("SetValue_obj", obj, Name, Value);
        }
        public static bool JsHaveValue(this IJSInProcessObjectReference obj, string Name)
        {
            CreateJsInfo();
            return JsInfo.Invoke<bool>("HaveValue_obj", obj, Name);
        }

        public static IJSInProcessObjectReference JsConvert(this object obj)
        {
            CreateJsInfo();
            return JsRuntime.Invoke<IJSInProcessObjectReference>("eval", obj);
        }

        public static IJSInProcessObjectReference JsNewObject(this string TypeName, params object[] Params)
        {
            var ParamsInput = "name";
            var ParamsFunc = "";
            if (Params != null && Params.Length > 0)
            {
                for (int i = 0; i < Params.Length; i++)
                    ParamsFunc += "P" + i.ToString() + ",";
                ParamsFunc = ParamsFunc.Substring(0, ParamsFunc.Length - 1);
                ParamsInput = ParamsInput + "," + ParamsFunc;
            }
            JsInfo = JsRuntime.Invoke<IJSInProcessObjectReference>("eval", @"({
  CreateNewObj: function ("+ ParamsInput + @") {
        return new window[name]("+ ParamsFunc + @");
  }
})");
           return JsInfo.Invoke<IJSInProcessObjectReference>("CreateNewObj",TypeName, Params);
        }

        private static void MakeState()
        {
            if (MyHistorySates==null)
            {
                MyHistorySates=new Action[0];
                Window.window.OnPopState += (c1, c2) => OnPopState();
            }
            OnPopState = () =>
            {
                if(MyHistorySates.Length>0)
                    Pop(ref MyHistorySates)();
            };
        }
        public static void DropState()
        {
            Pop(ref MyHistorySates)();
        }

        public static void PushState(Action Onpop)
        {
            MakeState();
            Insert(ref MyHistorySates,Onpop);
            Window.window.History.PushState("", "",Window.window.Location.Href);
        }

        public static void LockBack()
        {
            Action Lock =null;
            Lock = () =>
            {
                js.JsEval("alert('1');");
                Window.window.History.Go(1);
                PushState(Lock);
                js.JsEval("alert('1');");
            };
            PushState(Lock);
        }

        public static async Task GoBack()
        {
            await Task.Delay(1);
            Window.window.History.Back();
            await Task.Delay(1);
        }

        public static void UnLockBack()
        {
            Pop(ref MyHistorySates);
        }

        public static void Redirect(string Path)
        {
            Window.window.Location.Pathname = Path;
        }


        public static string ToJsValue(bool Value)
        {
            if (Value == true)
                return "true";
            return "false";
        }
        public static string ToJsValue(int Value)
        {
            return Value.ToString();
        }
        public static string ToJsValue(string Value)
        {
            if (Value == null)
                return"''";
            else
                return $"decodeURIComponent(escape(atob('{Convert.ToBase64String(Encoding.UTF8.GetBytes(Value))}')))";
        }

        public static string JsEval(this string js)
        {
            try
            {
                return JsRuntime.Invoke<string>(js);
            }
            catch(Exception ex)
            {
                throw new Exception("Eval >> " + js, ex);
            }
        }

        public static void JsEvalGlobal(this string js)
        {
            JsRuntime.InvokeVoid($"var s=document.createElement('script');s.innerHTML={ToJsValue(js)};document.body.appendChild(s);");
        }

        public static async Task<byte[]> ReadBytes(this Blob File)
        {
            using (var WebClient = new HttpClient())
                return await WebClient.GetByteArrayAsync(URL.CreateObjectUrl(File));
        }

        public static string ToDataUrl(this byte[] Data)
        {
            return URL.CreateDataUrl(Data);
        }

        public static T JsGetGlobalObject<T>(this string Name)
        {
            return JsRuntime.Invoke<T>("eval", Name);
        }
        public static object JsGetGlobalObject(this string Name) => JsGetGlobalObject<IJSInProcessObjectReference>(Name);

        public static string ToObjectUrlUnmarshalled(this byte[] Data)
        {
            //return Window.window.Url.CreateObjectUrl(new Blob(Data));
            js.JsEvalGlobal(
                @"
                var MNObjectUrl='';
                function MNToObjectUrl(ar){
                    ar = Blazor.platform.toUint8Array(ar);
                    MNObjectUrl = URL.createObjectURL(new Blob([ar],{type:'application/octet-stream'}));
                }");
            JsRuntime.InvokeVoid("MNToObjectUrl", Data);
            return "MNObjectUrl".JsGetGlobalObject<string>();
        }

        public static string ToObjectUrl(this byte[] Data)
        {
            return URL.CreateObjectUrl(new Blob(Data));
        }

        public static async Task<string> ToDataUrl(this Blob File)
        {
            return (await File.ReadBytes()).ToDataUrl();
        }

        public static string ToObjectUrl(this Blob File)
        {
            return URL.CreateObjectUrl(File);
        }

        public static byte[] GetImageBytes(this HTMLImageElement img,double quality=1)
        {
            if (quality > 1)
                throw new ArgumentOutOfRangeException("quality should be equal or less than 1.");
            var imgID = img.Id;
            img.Id = "imgMN";
            string URL = JsEval(
                @"(function(){
                  var MAX_WIDTH = 500;
                  var width = " + img.NaturalWidth + @";
                  var height = " + img.NaturalHeight + @";
                  height =height*( MAX_WIDTH / width);
                  width = MAX_WIDTH;                            
                  var canvas = document.createElement('canvas');
                  canvas.width = width;
                  canvas.height = height;
                  var ctx = canvas.getContext('2d');
                  ctx.drawImage(document.getElementById('" + img.Id + @"'),0,0, width, height);
                  return canvas.toDataURL('image/jpeg',"+quality.ToString("#.############", System.Globalization.CultureInfo.InvariantCulture) +");}).call(null);");
            img.Id = imgID;
            return System.Convert.FromBase64String(URL.Substring(23));
        }

        public static async Task<byte[]> GetImageBytesFast(this HTMLImageElement img, double quality=1)
        {
            if (quality > 1)
                throw new ArgumentOutOfRangeException("quality should be equal or less than 1.");
            var imgID = img.Id;
            img.Id = "imgMN";
            string URL = JsEval(
                @"(function(){
                  var MAX_WIDTH = 500;
                  var width = " + img.NaturalWidth + @";
                  var height = " + img.NaturalHeight + @";
                  height =height*( MAX_WIDTH / width);
                  width = MAX_WIDTH;                            
                  var canvas = document.createElement('canvas');
                  canvas.width = width;
                  canvas.height = height;
                  var ctx = canvas.getContext('2d');
                  ctx.drawImage(document.getElementById('" + img.Id + @"'),0,0,width,height);
                  return URL.createObjectURL(canvas.toBlob('image/jpeg'," + quality.ToString("#.############") + "));}).call(null);");
            img.Id = imgID;
            using (var WebClient = new HttpClient())
                return await WebClient.GetByteArrayAsync(URL);
        }

        public static async Task<byte[]> LoadBytesFromURL(string URL)
        {
            using (var WebClient = new HttpClient())
                return await WebClient.GetByteArrayAsync(URL);
        }

        public static Task<byte[]> LoadBytesFromBaseURL(string URL) =>
            LoadBytesFromURL(WASM_Global.Publisher.NavigationManager.ToAbsoluteUri("/") + URL);

        public static async Task<string> LoadStringFromURL(string URL)
        {
            using (var WebClient = new HttpClient())
                return await WebClient.GetStringAsync(URL);
        }

        public static Task<string> LoadStringFromBaseURL(string URL) =>
            LoadStringFromURL(WASM_Global.Publisher.NavigationManager.ToAbsoluteUri("/") + URL);
    }

    public class WebProcess
    {
        private readonly static string WebWorkerJs = ((Func<string>)(() => {
            var assembly = typeof(WebWorker).Assembly;
            var resourceName = "Monsajem_incs.WASM.MonsajemDomHelpers.WebWorker.js";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }))();
        private WebWorkerClient Worker;
        public readonly Task IsReady;
        internal static bool IsInWorker;
        private static void ThisIsInWorker()
        {
            IsInWorker = true;
            var AssemblyFilenames = js.JsGetValue("MN").JsGetValue<string[]>("AssemblyFilenames");
            foreach (var AssemblyFilename in AssemblyFilenames)
                Monsajem_Incs.Assembly.Assembly.TryLoadAssembely(AssemblyFilename);
            if (js.JsRuntime == null)
                Console.WriteLine("No Runtime");
            js.JsEval("postMessage('');");
        }
        
        public WebProcess()
        {
            if (IsInWorker)
                throw new BadImageFormatException("new process can't declare in WebWorker!");
            IsReady = GetReady();
        }
        private async Task GetReady()
        {
            Worker = new WebWorkerClient();
            var AssembellyNames = "[";
            foreach (var Asm in Monsajem_Incs.Assembly.Assembly.AllAppAssemblies)
                AssembellyNames += $"'{Asm.GetName().Name}.dll',";
            if (Monsajem_Incs.Assembly.Assembly.AllAppAssemblies.Length > 0)
                AssembellyNames = AssembellyNames.Substring(0, AssembellyNames.Length - 1);
            AssembellyNames += "]";
            var Location = Window.window.Location;
            Console.WriteLine("x1");
            await Worker.Run($"console.log(\"WebWorker Script2\");self.MN={{}};self.MN.AssemblyFilenames={AssembellyNames};self.MN.baseUrl = '{Location.Protocol}//{Location.Hostname}:{Location.Port}/_framework/';");
            Console.WriteLine("x2");
            var Ready = Worker.GetMessage();
            await Worker.Run(WebWorkerJs);
            Console.WriteLine("x3");
            await Ready;
            Console.WriteLine("x4");
        }

        public async Task Run(Action Action)
        {
            await Run(async () =>
            {
                Action();
                return null;
            });
        }
        public async Task<object> Run(Func<object> Func)
        {
            return await Run(async () =>
            {
                return Func();
            });
        }
        public async Task Run(Func<Task> Action)
        {
            await Run(async () =>
            {
                await Action();
                return null;
            });
        }
        public async Task<object> Run(Func<Task<object>> Func)
        {
            await IsReady;
            Worker.PostMessage("MNData",Func.Serialize());
            var Message = Worker.GetMessage();
            await Worker.Run($"self.MN.RunFunctionTaskResult()");
            var Result = (await Message).GetData<byte[]>();
            if (Result.Length == 0)
                throw new Exception("Error On Proccess");
            return Result.Deserialize<object>();
        }
        private static async void RunFunctionTaskResult()
        {
            try
            {
                var result = await js.JsGetGlobalObject<byte[]>("MNData").
                Deserialize<Func<Task<object>>>()();
                WebWorker.CurrentWebWorker.PostMessage(result.Serialize());
            }
            catch
            {
                WebWorker.CurrentWebWorker.PostMessage(new byte[0]);
            }
        }
    }
}