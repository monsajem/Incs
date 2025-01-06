using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Monsajem_Incs.Collection.Array.Base;
using Monsajem_Incs.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public static class JsInfo
        {
            private static IJSUnmarshalledObjectReference JsInfo_obj;


            static JsInfo()
            {
                CreateJsInfo();
            }

            private static void CreateJsInfo()
            {
                if (JsInfo_obj != null)
                {
                    JsInfo_obj = JsRuntime.Invoke<IJSUnmarshalledObjectReference>("eval", "JsInfoValues");
                    return;
                }
                JsInfo_obj = JsRuntime.Invoke<IJSUnmarshalledObjectReference>("eval", @"({
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
        return (obj!== null && obj[name] !== undefined && obj[name] !== null);
  },
  HaveValue_name: function (name) {
        return (self[name] !== undefined && self[name] !== null);
  },
  GetTypeName_obj: function (obj) {
        return obj.constructor.name;
  },
  GetTypeName_name: function (name) {
        return self[name].constructor.name;
  },
  CreateArInSelf: function (ParamLen) {
        self['SubmitedArObj']= new Array(ParamLen);
        self['SubmitedArObj_Pos']= 0;
  },
  PushArToSelf: function (obj) {
        var Pos = self['SubmitedArObj_Pos'];
        self['SubmitedArObj'][Pos]=obj;
        self['SubmitedArObj_Pos'] = Pos+1;
console.log('PushArToSelf in js');
console.log('Pos '+ Pos);
console.log(obj);
  },
  SubmitToSelf: function (name,obj) {
        self[name]=obj;
  },
  InvokeFuncAr: function (obj,name,SaveTo) {
       var Params = self['SubmitedArObj'];
       var Result = obj[name](...Params);
       self[SaveTo]=Result;
  },
})");
                JsInfo_obj.InvokeVoid("SubmitToSelf", "JsInfoValues", JsInfo_obj);

            }

            public static bool CanUnmarshal(object value)
            {
                if (value == null)
                    return true;
                if (value.GetType() == typeof(string))//||
                                           //Type == typeof(Int16) ||
                                           // Type == typeof(Int32) ||
                                           // Type == typeof(Int64) ||
                                           // Type == typeof(UInt16) ||
                                           // Type == typeof(UInt32) ||
                                           // Type == typeof(UInt64) ||
                                           // Type == typeof(Single) ||
                                           // Type == typeof(Double) ||
                                           // Type == typeof(Decimal)||
                                           // Type == typeof(bool))
                    return true;
                else
                    return false;
            }

            static TimeSpan TotalTime = TimeSpan.Zero;
            static int TotalTimeCount = 0;
            private static void TryingAction(Action ac, string MethodName)
            {
                var Time = Monsajem_Incs.TimeingTester.Timing.run(ac, MethodName);
                TotalTime = TotalTime + Time;
                TotalTimeCount++;
                if (TotalTimeCount >= 250)
                {
                    var AVG = TotalTime / TotalTimeCount;
                    Console.WriteLine("////////////////////////////////////////////////////////////////////////////////");
                    Console.WriteLine("\r\nTotal Time : " + TotalTime);
                    Console.WriteLine("AVG Total Time : " + AVG);
                    Console.WriteLine("Total Count : " + TotalTimeCount);
                    Console.WriteLine(Monsajem_Incs.TimeingTester.Timing.GetInfos());
                }
            }

            private static t TryingAction<t>(Func<t> ac, string MethodName)
            {
                var Result = default(t);
                TryingAction(() =>
                {
                    Result = ac();
                }, MethodName);
                return Result;
            }
            public static void SubmitToSelf(object[] args)
            {
                TryingAction(() =>
                {
                    var ArgsLen = 0;
                    if (args != null && args.Length > 0)
                        ArgsLen = args.Length;

                    JsInfo_obj.InvokeVoid("CreateArInSelf", ArgsLen);
                    for (int a = 0; a < ArgsLen; a++)
                    {
                        var Arg = args[a];
                        if(Arg!=null)
                        {
                            Type argType = args[a].GetType();
                            if (argType.IsSubclassOf(typeof(DOMObject)) || argType == typeof(DOMObject))
                            {
                                var jsObj = (DOMObject)args[a];
                                if (jsObj.ManagedJSObject == null)
                                    throw new Exception("managed Object is lost at " + jsObj.GetType());
                                args[a] = jsObj.ManagedJSObject;
                            }
                        }
                        if(CanUnmarshal(Arg))
                            JsInfo_obj.InvokeUnmarshalJs("PushArToSelf",null, args[a]);
                        else
                            JsInfo_obj.InvokeVoid("PushArToSelf", args[a]);
                    }

                }, "SubmitToSelf");
            }
            public static void SubmitToSelf(string Name, object obj)
            {
                TryingAction(() =>
                {
                    JsInfo_obj.InvokeVoid("SubmitToSelf", Name, obj);
                }, "SubmitToSelf");
            }

            public static void InvokeFuncViaArray(object OwnerObj, string FuncName, string SaveResultToGlobal)
            {
                TryingAction(() =>
                {
                    JsInfo_obj.InvokeVoid("InvokeFuncAr", OwnerObj, FuncName, SaveResultToGlobal);
                }, "InvokeFuncAr");
            }

            public static T InvokeJs<T>(IJSInProcessObjectReference obj,string identifier, object[] args)
            {
                return TryingAction(() =>
                {
                    var ArgsLen = 0;
                    if (args != null && args.Length > 0)
                        ArgsLen = args.Length;
                    for (int a = 0; a < ArgsLen; a++)
                    {
                        var Arg = args[a];
                        if (Arg != null)
                        {
                            Type argType = args[a].GetType();
                            if (argType.IsSubclassOf(typeof(DOMObject)) || argType == typeof(DOMObject))
                            {
                                var jsObj = (DOMObject)args[a];
                                if (jsObj.ManagedJSObject == null)
                                    throw new Exception("managed Object is lost at " + jsObj.GetType());
                                args[a] = jsObj.ManagedJSObject;
                            }
                        }
                    }
                    var type = typeof(T);
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
                }, "InvokeJs<" + typeof(T).FullName);
            }

            public static T JsGetValue<T>(string Name)
            {
                return TryingAction(() =>
                {
                    if (JsInfo_obj.Invoke<bool>("HaveValue_obj", Name))
                    {
                        var type = typeof(T);
                        if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
                        {
                            var Result_JsObj = JsInfo_obj.Invoke<IJSInProcessObjectReference>("GetValue_name", Name);
                            if (Result_JsObj == null)
                                return default;
                            var Result = (DOMObject)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
                            Result.ManagedJSObject = Result_JsObj;
                            return (T)(object)Result;
                        }
                        else
                        {
                            return JsInfo_obj.Invoke<T>("GetValue_name", Name);
                        }
                    }
                    else
                        return default;
                }, "JsGetValue(1)<" + typeof(T).FullName);
            }
            public static IJSInProcessObjectReference JsGetValue(string Name)
            {
                return TryingAction(() =>
                {
                    return JsGetValue<IJSInProcessObjectReference>(Name);
                }, "JsGetValue");
            }
            public static void JsSetValue(string Name, object Value)
            {
                TryingAction(() =>
                {
                    if (Value!=null)
                    {
                        var type = Value.GetType();
                        if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
                        {
                            var JsObj = ((DOMObject)Value).ManagedJSObject;
                            JsInfo_obj.InvokeVoid("SetValue_name", Name, JsObj);
                        }
                        else
                        {
                            JsInfo_obj.InvokeVoid("SetValue_name", Name, Value);
                        }
                    }
                    else
                        JsInfo_obj.InvokeVoid("SetValue_name", Name, Value);
                }, "JsSetValue");
            }
            public static bool JsHaveValue(string Name)
            {
                return TryingAction(() =>
                {
                    return JsInfo_obj.Invoke<bool>("HaveValue_name", Name);
                }, "JsHaveValue");
            }

            public static T JsGetValue<T>(IJSInProcessObjectReference obj, string Name)
            {
                return TryingAction(() =>
                {
                    if (JsInfo_obj.Invoke<bool>("HaveValue_obj",obj, Name))
                    {
                        var type = typeof(T);
                        if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
                        {
                            var Result_JsObj = JsInfo_obj.Invoke<IJSInProcessObjectReference>("GetValue_obj", obj, Name);
                            if (Result_JsObj == null)
                                return default;
                            var Result = (DOMObject)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
                            Result.ManagedJSObject = Result_JsObj;
                            return (T)(object)Result;
                        }
                        else
                        {
                            var Result = JsInfo_obj.Invoke<T>("GetValue_obj", obj, Name);
                            if (typeof(T) == typeof(string))
                            {
                                var str = ((string)(object)Result);
                                Console.WriteLine("String Len" + str.Length);
                                if (str.Length > 100)
                                    Console.WriteLine(Environment.StackTrace);
                            }
                            return Result;
                        }
                    }
                    else
                        return default;
                }, " JsGetValue<" + typeof(T).FullName);
            }
            public static IJSInProcessObjectReference JsGetValue(IJSInProcessObjectReference obj, string Name)
            {
                return TryingAction(() =>
                {
                    return JsGetValue<IJSInProcessObjectReference>(obj, Name);
                }, " JsGetValue");
            }
            public static IJSInProcessObjectReference JsGetStaticValue(string Name)
            {
                return TryingAction(() =>
                {
                    return new JsObjectOfType(Name);
                }, "JsGetStaticValue");
            }
            public static void JsSetValue(IJSInProcessObjectReference obj, string Name, object Value)
            {
                TryingAction(() =>
                {
                    if (obj == null)
                        throw new Exception("js object is null");
                    if (Value !=null)
                    {
                        var type = Value.GetType();
                        if (typeof(Delegate).IsAssignableFrom(type))
                        {
                            JsSetEvent(obj, Name, (Delegate)Value);
                        }
                        else
                        {
                            JsInfo_obj.InvokeVoid("SetValue_obj", obj, Name, Value);
                        }
                    }
                    else
                    {
                        JsInfo_obj.InvokeVoid("SetValue_obj", obj, Name, Value);
                    }
                }, "JsSetValue");
            }
            public static bool JsHaveValue(IJSInProcessObjectReference obj, string Name)
            {
                return TryingAction(() =>
                {
                    return JsInfo_obj.Invoke<bool>("HaveValue_obj", obj, Name);
                }, "JsHaveValue");

            }
            public static void JsSetEvent(IJSInProcessObjectReference obj, string Name, Delegate Value)
            {
                TryingAction(() =>
                {
                    var Binder = new InvokableDelegate() { DG = Value, Js = JsRuntime };
                    Binder.Bind(obj, Name);
                }, "JsSetEvent");
            }

            public static IJSInProcessObjectReference JsConvert(object obj)
            {
                return TryingAction(() =>
                {
                    return JsRuntime.Invoke<IJSInProcessObjectReference>("eval", obj);
                }, "JsConvert");
            }

            public static IJSInProcessObjectReference JsNewObject(string TypeName, params object[] Params)
            {
                return TryingAction(() =>
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
                    var JsInfo_obj = JsRuntime.Invoke<IJSUnmarshalledObjectReference>("eval", @"({
  CreateNewObj: function (" + ParamsInput + @") {
        return new window[name](" + ParamsFunc + @");
  }
})");
                    return JsInfo_obj.Invoke<IJSInProcessObjectReference>("CreateNewObj", TypeName, Params);
                }, "JsNewObject");
            }
        }
    }
}