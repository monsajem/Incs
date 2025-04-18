﻿using Microsoft.JSInterop;
using Monsajem_Incs.Collection.Array.Base;
using Monsajem_Incs.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAssembly.Browser.DOM;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace WebAssembly.Browser.MonsajemDomHelpers
{
    public static partial class js
    {
        public static Document Document;
        public static JSInProcessRuntime JsRuntime;
        public static IJSUnmarshalledRuntime JsRuntime_unmarshal;
        private static Action OnPopState;
        private static Action[] MyHistorySates;

        public static void Start(JSInProcessRuntime JSR)
        {
            JsRuntime = JSR;
            _ = JsRuntime.Invoke<IJSInProcessObjectReference>("eval", "document");
            _ = JsRuntime.Invoke<IJSInProcessObjectReference>("eval", "window");
            Document = new Document();
        }

        private static T InvokeJs_Total<T>(this IJSInProcessObjectReference obj,bool IsGlobal, string identifier, params object[] args)
        {
            if (IsGlobal == false && obj == null)
                throw new Exception("js object is null");
            var type = typeof(T);
            var UseUnmarshal= true;
            if (args != null && args.Length > 0)
            {
                for (int a = 0; a < args.Length; a++)
                {
                    Type argType = args[a].GetType();
                    if (argType.IsSubclassOf(typeof(DOMObject)) || argType == typeof(DOMObject))
                    {
                        var jsObj = (DOMObject)args[a];
                        if (jsObj.ManagedJSObject == null)
                            throw new Exception("managed Object is lost at " + jsObj.GetType());
                        args[a] =jsObj .ManagedJSObject;
                    }
                    if (argType == typeof(string))
                    {
                        UseUnmarshal=true;
                    }
                }
            }
            if(UseUnmarshal)
            {
                for (int a = 0; a < args.Length; a++)
                {
                    if (args[a] == null)
                    {
                        JsInfo.SubmitToSelf("p" + a, null);
                    }
                    else
                    {
                        var argType = args[a].GetType();
                        if (argType == typeof(string))
                        {
                            JsInfo.SubmitStringToSelf("p" + a, (string)args[a]);
                        }
                        else
                        {
                            JsInfo.SubmitToSelf("p" + a, args[a]);
                        }
                    }
                }
                JsInfo.InvokeFunc(obj, identifier, "p", args.Length, "res");
                return JsGetValue<T>("res");
            }
            else
            {
                if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
                {
                    var Result_JsObj = obj.Invoke<IJSInProcessObjectReference>(identifier, args);
                    if (Result_JsObj == null)
                        return default;
                    var Result = (DOMObject)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
                    Result.ManagedJSObject = Result_JsObj;
                    return (T)(object)Result;
                }
                else
                {
                    return obj.Invoke<T>(identifier, args);
                }
            }
        }

        private static t JsGetValue_Total<t>(this IJSInProcessObjectReference obj,bool IsGlobal, string Name)
        {
            if(IsGlobal==false&&obj == null)
                throw new Exception("js object is null");
            if (obj == null)
            {
                if (JsInfo.HaveValue_name(Name) == false)
                    return default;
            }
            else
            {
                if(JsInfo.HaveValue_obj(obj,Name)==false)
                    return default;
            }
            var type = typeof(t);
            if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
            {
                IJSInProcessObjectReference Result_JsObj;
                if (obj != null)
                    Result_JsObj = JsInfo.GetValue_obj<IJSInProcessObjectReference>(obj, Name);
                else
                    Result_JsObj = JsInfo.GetValue_name<IJSInProcessObjectReference>(Name);
                if (Result_JsObj == null)
                    return default;
                var Result = (DOMObject)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
                Result.ManagedJSObject = Result_JsObj;
                return (t)(object)Result;
            }
            else if (type == typeof(string))
            {
                if (obj != null)
                    return (t)(object)JsInfo.GetStringValue_obj(obj, Name);
                else
                    return (t)(object)JsInfo.GetStringValue_name(Name);
            }
            else
            {
                if (obj != null)
                    return JsInfo.GetValue_obj<t>(obj, Name);
                else
                    return JsInfo.GetValue_name<t>(Name);
            }
        }
        private static void JsSetValue_Total(this IJSInProcessObjectReference obj,bool IsGlobal, string Name, object Value)
        {
            if (IsGlobal == false && obj == null)
                throw new Exception("js object is null");
            if (Value != null)
            {
                var ValueType = Value.GetType();
                if (typeof(Delegate).IsAssignableFrom(ValueType))
                {
                    JsInfo.JsSetEvent(obj, Name, (Delegate)Value);
                }
                else if (ValueType.IsSubclassOf(typeof(DOMObject)) || ValueType == typeof(DOMObject))
                {
                    Value = ((DOMObject)Value).ManagedJSObject;
                    if (obj != null)
                        JsInfo.SetValue_obj(obj, Name, Value);
                    else
                        JsInfo.SetValue_name(Name, Value);
                }
                else if (ValueType == typeof(string))
                {
                    if (obj != null)
                        JsInfo.SetStringValue_obj(obj, Name,(string)Value);
                    else
                        JsInfo.SetStringValue_name(Name,(string) Value);
                }
                else
                {
                    if (obj != null)
                        JsInfo.SetValue_obj(obj, Name,Value);
                    else
                        JsInfo.SetValue_name(Name, Value);
                }
            }
            else
            {
                if(obj!=null)
                    JsInfo.SetValue_obj(obj, Name, null);
                else
                    JsInfo.SetValue_name(Name, null);
            }
        }


        public static T InvokeJs<T>(this IJSInProcessObjectReference obj, string identifier, params object?[]? args)=>
            InvokeJs_Total<T>(obj,false,identifier,args);
        
        public static object InvokeJs(this IJSInProcessObjectReference obj, Type type, string identifier, params object?[]? args)
        {
            var bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;
            return typeof(js).GetMethods(bindingFlags).Where((c)=>c.Name== "InvokeJs_Total" && c.IsGenericMethod).First().MakeGenericMethod(type).Invoke(null,new object[] { obj, false,identifier, args });
        }

        public static t JsGetValue<t>(this IJSInProcessObjectReference obj,string Name) => JsGetValue_Total<t>(obj, false, Name);
        public static t JsGetValue<t>(this string Name)=>JsGetValue_Total<t>(null,true, Name);
        public static IJSInProcessObjectReference JsGetValue(this string Name) => JsGetValue_Total<IJSInProcessObjectReference>(null,true,Name);
        public static void JsSetValue(this string Name, object Value) => JsSetValue_Total(null, true, Name, Value);
        public static bool JsHaveValue(this string Name)=>JsInfo.HaveValue_name(Name);

        public static void JsSetValue(this IJSInProcessObjectReference obj, string Name, object Value) =>
            JsSetValue_Total(obj, false, Name, Value);
        public static IJSInProcessObjectReference JsGetValue(this IJSInProcessObjectReference obj, string Name) =>
            JsGetValue_Total<IJSInProcessObjectReference>(obj, false, Name);
        public static IJSInProcessObjectReference JsGetStaticValue(this string Name)=>JsInfo.JsGetStaticValue(Name);
        public static void JsSetEvent(this IJSInProcessObjectReference obj, string Name, Delegate Value)=> JsInfo.JsSetEvent(obj, Name, Value);

        public static IJSInProcessObjectReference JsConvert(this object obj)=>JsInfo.JsConvert(obj);

        public static IJSInProcessObjectReference JsNewObject(this string TypeName, params object[] Params)=>JsInfo.JsNewObject(TypeName, Params);

        private static void MakeState()
        {
            if (MyHistorySates == null)
            {
                MyHistorySates = new Action[0];
                Window.window.OnPopState += (c1, c2) => OnPopState();
            }
            OnPopState = () =>
            {
                if (MyHistorySates.Length > 0)
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
            Insert(ref MyHistorySates, Onpop);
            Window.window.History.PushState("", "", Window.window.Location.Href);
        }

        public static void LockBack()
        {
            static void Lock()
            {
                Window.window.History.Go(1);
                PushState(Lock);
            }

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
            _ = Pop(ref MyHistorySates);
        }

        public static void Redirect(string Path)
        {
            Window.window.Location.Pathname = Path;
        }


        public static string ToJsValue(bool Value)
        {
            return Value == true ? "true" : "false";
        }
        public static string ToJsValue(int Value)
        {
            return Value.ToString();
        }
        public static string ToJsValue(string Value)
        {
            return Value == null ? "''" : $"decodeURIComponent(escape(atob('{Convert.ToBase64String(Encoding.UTF8.GetBytes(Value))}')))";
        }

        public static t JsEval<t>(this string js)
        {
            try
            {
                return JsRuntime.Invoke<t>("eval",js);
            }
            catch (Exception ex)
            {
                throw new Exception("Error at Eval >> " + js+"\n\r"+ex.Message, ex);
            }
        }
        public static void JsEval(this string js)
        {
            try
            {
                JsRuntime.InvokeVoid("eval", js);
            }
            catch (Exception ex)
            {
                throw new Exception("Error at Eval >> " + js + "\n\r" + ex.Message, ex);
            }
        }

        public static void JsEvalGlobal(this string js)
        {
            JsRuntime.InvokeVoid("eval",$"var s=document.createElement('script');s.innerHTML={ToJsValue(js)};document.body.appendChild(s);");
        }

        public static async Task<byte[]> ReadBytes(this Blob File)
        {
            using var WebClient = new HttpClient();
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

        public static byte[] GetImageBytes(this HTMLImageElement img, double quality = 1)
        {
            if (quality > 1)
                throw new ArgumentOutOfRangeException("quality should be equal or less than 1.");
            var imgID = img.Id;
            img.Id = "imgMN";
            string URL = JsEval<string>(
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
                  return canvas.toDataURL('image/jpeg'," + quality.ToString("#.############", System.Globalization.CultureInfo.InvariantCulture) + ");}).call(null);");
            img.Id = imgID;
            return System.Convert.FromBase64String(URL.Substring(23));
        }

        public static async Task<byte[]> GetImageBytesFast(this HTMLImageElement img, double quality = 1)
        {
            if (quality > 1)
                throw new ArgumentOutOfRangeException("quality should be equal or less than 1.");
            var imgID = img.Id;
            img.Id = "imgMN";
            string URL = JsEval<string>(
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
            using var WebClient = new HttpClient();
            return await WebClient.GetByteArrayAsync(URL);
        }

        public static async Task<byte[]> LoadBytesFromURL(string URL)
        {
            using var WebClient = new HttpClient();
            return await WebClient.GetByteArrayAsync(URL);
        }

        public static Task<byte[]> LoadBytesFromBaseURL(string URL) =>
            LoadBytesFromURL(WASM_Global.Publisher.NavigationManager.ToAbsoluteUri("/") + URL);

        public static async Task<string> LoadStringFromURL(string URL)
        {
            using var WebClient = new HttpClient();
            return await WebClient.GetStringAsync(URL);
        }

        public static Task<string> LoadStringFromBaseURL(string URL) =>
            LoadStringFromURL(WASM_Global.Publisher.NavigationManager.ToAbsoluteUri("/") + URL);

        private class JsObjectOfType : IJSInProcessObjectReference
        {
            private string TypeAddress;
            public JsObjectOfType(string TypeAddress)
            {
                this.TypeAddress = TypeAddress;
            }
            public void Dispose()
            {
            }

            public async ValueTask DisposeAsync()
            {
            }

            public TValue Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object[] args)
            {
                return js.JsRuntime.Invoke<TValue>(TypeAddress + "." + identifier, args);
            }

            public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object[] args)
            {
                return js.JsRuntime.InvokeAsync<TValue>(TypeAddress + "." + identifier, args);
            }

            public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object[] args)
            {
                return js.JsRuntime.InvokeAsync<TValue>(TypeAddress + "." + identifier,cancellationToken, args);
            }
        }
    }

    public class WebProcess
    {
        private readonly static string WebWorkerJs = ((Func<string>)(() =>
        {
            var assembly = typeof(WebWorker).Assembly;
            var resourceName = "Monsajem_incs.WASM.MonsajemDomHelpers.WebWorker.js";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
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
            await Worker.Run($"self.MN={{}};self.MN.AssemblyFilenames={AssembellyNames};self.MN.baseUrl = '{Location.Protocol}//{Location.Hostname}:{Location.Port}/_framework/';");
            Console.WriteLine("x2");
            var Ready = Worker.GetMessage();
            await Worker.Run(WebWorkerJs);
            Console.WriteLine("x3");
            _ = await Ready;
            Console.WriteLine("x4");
        }

        public async Task Run(Action Action)
        {
            _ = await Run(async () =>
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
            _ = await Run(async () =>
            {
                await Action();
                return null;
            });
        }
        public async Task<object> Run(Func<Task<object>> Func)
        {
            await IsReady;
            Worker.PostMessage("MNData", Func.Serialize());
            var Message = Worker.GetMessage();
            await Worker.Run($"self.MN.RunFunctionTaskResult()");
            var Result = (await Message).GetData<byte[]>();
            return Result.Length == 0 ? throw new Exception("Error On Proccess") : Result.Deserialize<object>();
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