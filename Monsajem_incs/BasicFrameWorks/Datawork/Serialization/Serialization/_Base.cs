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
            public HashSet<ObjectContainer> Visitor = new HashSet<ObjectContainer>();
            public HashSet<ObjectContainer> Visitor_info = new HashSet<ObjectContainer>();
            public Action<Type> TrustToType;
            public Action<MethodInfo> TrustToMethod;
            public Action AtLast;
#if DEBUG
            public string Traced;
#endif
        }
        internal class DeserializeData : Data
        {
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
            public MemoryStream Data = new MemoryStream();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int SizeOf<t>()=> SerializeInfo<t>.GetSerialize().ConstantSize;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private byte[] _Serialize<t>(t obj, 
            Action<Type> TrustToType,
            Action<MethodInfo> TrustToMethod)
        {
            byte[] Result;
            var SR = SerializeInfo<t>.GetSerialize();
            var SR_Data = new SerializeData()
            {
                TrustToType = TrustToType,
                TrustToMethod =TrustToMethod
            };
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public t Deserialize<t>(byte[] Data,
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            var Type = typeof(t);
            var From = 0;
            return Deserialize<t>(Data, ref From, TrustToType,TrustToMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public t Deserialize<t>(byte[] Data, ref int From, 
            Action<Type> TrustToType,
            Action<MethodInfo> TrustToMethod)
        {
            t Result = default;
            var DR_Data = new DeserializeData() { 
                Data = Data, From = From, 
                TrustToType = TrustToType,
                TrustToMethod = TrustToMethod };
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
                    (c) => Result = (t)c, SerializeInfo<t>.GetSerialize());
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