using Microsoft.JSInterop;
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
  SubmitToSelf: function (name,obj) {
        self[name]=obj;
//console.log('SubmitToSelf');
//console.log(name);
//console.log(obj);
  },
  SubmitStringToSelf: function (name,obj) {
        self[new TextDecoder().decode(name)]=new TextDecoder().decode(obj);
  },
  InvokeFunc: function (obj,name,ParamPreName,ParamLen,SaveTo) {
    
    var res = new Array(ParamLen);
//console.log('InvokeFunc');
//console.log(name);
    for(i=0;i<ParamLen;i++)
    {
     res[i] = self[ParamPreName+i];
//console.log(ParamPreName+i); 
//console.log(res[i]);
    }
//console.log('b');
       self[SaveTo]=obj[name](...res);
//console.log('c');
  },
})");
                JsInfo_obj.InvokeVoid("SubmitToSelf", "JsInfoValues", JsInfo_obj);

            }

            //static TimeSpan TotalTime = TimeSpan.Zero;
            //static int TotalTimeCount = 0;
            private static void TryingAction(Action ac,string MethodName)
            {
                //CreateJsInfo();
                var Time = Monsajem_Incs.TimeingTester.Timing.run(ac,MethodName);
                //TotalTime = TotalTime + Time;
                //TotalTimeCount++;
                //if(TotalTimeCount>=540)
                //{
                //    var AVG = TotalTime / TotalTimeCount;
                //    Console.WriteLine("\r\nTotal Time : " + TotalTime);
                //    Console.WriteLine("AVG Total Time : " + AVG);
                //    Console.WriteLine("Total Count : " + TotalTimeCount);
                //    Console.WriteLine(Monsajem_Incs.TimeingTester.Timing.GetInfos());
                //}
            }

            private static t TryingAction<t>(Func<t> ac,string MethodName)
            {
                var Result = default(t);
                TryingAction(() =>
                {
                    Result = ac();
                },MethodName);
                return Result;
            }

            public static void SubmitStringToSelf(string Name, string obj)
            {
                TryingAction(() =>
                { 
                    JsInfo_obj.InvokeVoid("SubmitToSelf", Name, obj);
                    //JsInfo_obj.InvokeVoid("SubmitStringToSelf", System.Text.Encoding.UTF8.GetBytes(Name), System.Text.Encoding.UTF8.GetBytes(obj));
                }, "SubmitStringToSelf");
            }
            public static void SubmitToSelf(string Name, object obj)
            {
                TryingAction(() =>
                {
                    JsInfo_obj.InvokeVoid("SubmitToSelf", Name, obj);
                },"SubmitToSelf");
            }

            public static void InvokeFunc(object OwnerObj, string FuncName, string ParamsPreName, int ParamsLen, string SaveResultToGlobal)
            {
                TryingAction(() =>
                {
                    JsInfo_obj.InvokeVoid("InvokeFunc", OwnerObj, FuncName, ParamsPreName, ParamsLen, SaveResultToGlobal);
                }, "InvokeFunc");
            }

            public static t JsGetValue<t>(string Name)
            {
                return TryingAction(() =>
                {
                    if (JsInfo_obj.Invoke<bool>("HaveValue_name", Name))
                    {
                        var Type = typeof(t);
                        if(Type == typeof(string))//||
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
                            return JsInfo_obj.InvokeUnmarshalled<string,t>("GetValue_name", Name);
                        else
                            return JsInfo_obj.Invoke<t>("GetValue_name", Name);
                    }
                    else
                        return default;
                }, "JsGetValue(1)<"+typeof(t).FullName);
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
                    //if (Value.GetType() == typeof(string))
                    //    JsInfo.InvokeUnmarshalled<string,string,object>("SetValue_name", Name,(string) Value);
                    //else

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

            public static t JsGetValue<t>(IJSInProcessObjectReference obj, string Name)
            {
                return TryingAction(() =>
                {
                    if (JsInfo_obj.Invoke<bool>("HaveValue_obj", obj, Name))
                    {
                        var Type = typeof(t);
                        if (Type == typeof(string) )//||
                           //Type == typeof(Int16) ||
                           // Type == typeof(Int32) ||
                           // Type == typeof(Int64) ||
                           // Type == typeof(UInt16) ||
                           // Type == typeof(UInt32) ||
                           // Type == typeof(UInt64) ||
                           // Type == typeof(Single) ||
                           // Type == typeof(Double) ||
                           // Type == typeof(Decimal) ||
                           // Type == typeof(bool))
                            return JsInfo_obj.InvokeUnmarshalled<string, t>("GetValue_obj", Name);
                        else
                            return JsInfo_obj.Invoke<t>("GetValue_obj", obj, Name);
                    }
                    else
                        return default;
                }, " JsGetValue<"+typeof(t).FullName);
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
                    if (Value != null &&
                        typeof(Delegate).IsAssignableFrom(Value.GetType()))
                    {
                        JsSetEvent(obj, Name, (Delegate)Value);
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
                },"JsHaveValue");

            }
            public static void JsSetEvent(IJSInProcessObjectReference obj, string Name, Delegate Value)
            {
                TryingAction(() =>
                {
                    var Binder = new InvokableDelegate() { DG = Value, Js = JsRuntime };
                    Binder.Bind(obj, Name);
                },"JsSetEvent");
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