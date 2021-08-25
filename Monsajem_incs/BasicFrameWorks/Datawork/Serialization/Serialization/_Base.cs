using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        private static Type ISerializableType = typeof(ISerializable<object>).GetGenericTypeDefinition();
        private static byte[] Byte_Int_N_1 = BitConverter.GetBytes(-1);
        private static byte[] Byte_Int_N_2 = BitConverter.GetBytes(-2);
        internal static FieldInfo Deletage_Target = ((Func<FieldInfo>)(() =>
        {
            return DynamicAssembly.FieldControler.GetFields(typeof(Delegate)).
                        Where((c) => c.Name.ToLower().Contains("target")).FirstOrDefault();
        }))();

        public Serialization(Func<FieldInfo, bool> FieldCondition) :
            this()
        {
            this._FieldCondition = FieldCondition;
        }

        private Func<FieldInfo, bool> _FieldCondition;
        private bool FieldCondition(FieldInfo Field)
        {
            if (_FieldCondition?.Invoke(Field) == false)
                return false;
            if (Field.DeclaringType.IsGenericType)
                if (Field.DeclaringType.GetGenericTypeDefinition() ==
                    typeof(Dictionary<string, string>).GetGenericTypeDefinition() &&
                    Field.Name == "_syncRoot")
                    return false;
            return true;
        }

        private HashSet<LoadedFunc> LoadedFuncs_Des = new HashSet<LoadedFunc>();
        private HashSet<LoadedFunc> LoadedFuncs_Ser = new HashSet<LoadedFunc>();


        internal class Data
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            public Data(bool NeedVisitors, 
                        Action<Type> TrustToType,
                        Action<MethodInfo> TrustToMethod)
            {
                if(NeedVisitors==true)
                {
                    Visitor = new HashSet<ObjectContainer>();
                    Visitor_info = new HashSet<ObjectContainer>();
                }
                this.TrustToType = TrustToType;
                this.TrustToMethod = TrustToMethod;
            }
            public HashSet<ObjectContainer> Visitor;
            public HashSet<ObjectContainer> Visitor_info;
            public Action<Type> TrustToType;
            public Action<MethodInfo> TrustToMethod;
            public Action AtLast;
#if DEBUG
            public string Traced;
#endif
        }
        internal class DeserializeData : Data
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            public DeserializeData(bool NeedVisitors,
                                   Action<Type> TrustToType,
                                   Action<MethodInfo> TrustToMethod,
                                   byte[] Data) : 
                              base(NeedVisitors,
                                   TrustToType,
                                   TrustToMethod)
            {
                this.Data = Data;
            }
            public byte[] Data;

#if DEBUG
            private int _From;
            public int From
            {
                get
                {
                    return _From;
                }
                set
                {
                    _From = value;
                }
            }
#else
        public int From;
#endif
        }

        internal class SerializeData : Data
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            public SerializeData(bool NeedVisitors,
                                 Action<Type> TrustToType,
                                 Action<MethodInfo> TrustToMethod) :
                            base(NeedVisitors,
                                 TrustToType,
                                 TrustToMethod){}
            public MemoryStream Data = new MemoryStream();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public int SizeOf<t>()=> SerializeInfo<t>.GetSerialize().ConstantSize;

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public byte[] Serialize<t>(t obj,
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
#if DEBUG
            var Result = _Serialize(obj, TrustToType,TrustToMethod);
            var DS = Deserialize<t>(Result, TrustToType);
            return Result;
#else
            return _Serialize(obj,TrustToType, TrustToMethod);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        private byte[] _Serialize<t>(t obj, 
            Action<Type> TrustToType,
            Action<MethodInfo> TrustToMethod)
        {
            byte[] Result;
            var SR = SerializeInfo<t>.GetSerialize();
            SerializeData SR_Data;
            if (SR.ConstantSize == -1)
                SR_Data = new SerializeData(true,TrustToType,TrustToMethod);
            else
                SR_Data = new SerializeData(false, TrustToType, TrustToMethod);
            try
            {
#if DEBUG
                if (Deletage_Target == null)
                {
                    var Fields = DynamicAssembly.FieldControler.GetFields(typeof(Delegate));
                    var Fields_str = "";
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        Fields_str += "\n" + Fields[i].Name;
                    }
                    throw new Exception("Cant Access to Deletage Target field at serializer, Fields >>" + Fields_str);
                }
#endif
                VisitedSerialize(SR_Data, obj, SR);
                Result = SR_Data.Data.ToArray();
            }
#if DEBUG
            catch (Exception ex)
            {
                var Traced = SR_Data.Traced;
                if (Traced != null)
                    Traced = "On " + Traced;
                SR_Data.Traced = null;
                throw new Exception($"Serialize Of Type >> {obj.GetType().FullName} Is Failed " + Traced, ex);
            }
#endif
            finally { }
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public t Deserialize<t>(byte[] Data,
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            var Type = typeof(t);
            var From = 0;
            return Deserialize<t>(Data, ref From, TrustToType,TrustToMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public t Deserialize<t>(byte[] Data, ref int From, 
            Action<Type> TrustToType,
            Action<MethodInfo> TrustToMethod)
        {
            t Result = default;
            var SR = SerializeInfo<t>.GetSerialize();
            DeserializeData DR_Data;
            if (SR.ConstantSize == -1)
                DR_Data = new DeserializeData(true, TrustToType, TrustToMethod,Data);
            else
                DR_Data = new DeserializeData(false, TrustToType, TrustToMethod,Data);
            try
            {
#if DEBUG
                if (Deletage_Target == null)
                {
                    var Fields = DynamicAssembly.FieldControler.GetFields(typeof(Delegate));
                    var Fields_str = "";
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        Fields_str += "\n" + Fields[i].Name;
                    }
                    throw new Exception("Cant Access to Deletage Target field at serializer, Fields >>" + Fields_str);
                }
#endif
                VisitedDeserialize(DR_Data,
                    (c) => Result = (t)c,SR);
                DR_Data.AtLast?.Invoke();
            }
#if DEBUG
            catch (Exception ex)
            {
                var Traced = DR_Data.Traced;
                if (Traced != null)
                    Traced = "On " + Traced;
                DR_Data.Traced = null;
                throw new Exception($"Deserialize From Point {DR_Data.From} Of Type >> {typeof(t).FullName} Is Failed {Traced}\nDatas As B64:\n" + System.Convert.ToBase64String(Data), ex);
            }
#endif
            finally { }
            return Result;
        }
    }
}