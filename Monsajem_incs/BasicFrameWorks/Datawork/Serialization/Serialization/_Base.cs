using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        [ThreadStaticAttribute]
        private static Action<Type> Trust;

        private HashSet<LoadedFunc> LoadedFuncs_Des = new HashSet<LoadedFunc>();
        private HashSet<LoadedFunc> LoadedFuncs_Ser = new HashSet<LoadedFunc>();

        [ThreadStaticAttribute]
        private static byte[] D_Data;

#if DEBUG
        [ThreadStaticAttribute]
        private static int _From;
        private static int From 
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
        [ThreadStaticAttribute]
        private static int From;
#endif

        [ThreadStaticAttribute]
        private static MemoryStream S_Data;
        [ThreadStaticAttribute]
        private static HashSet<ObjectContainer> Visitor;
        [ThreadStaticAttribute]
        private static HashSet<ObjectContainer> Visitor_info;

        public byte[] Serialize<t>(t obj, Action<Type> TrustToType = null)
        {
#if DEBUG
            var Result = _Serialize(obj,TrustToType);
            var DS = Deserialize<t>(Result,TrustToType);
            return Result;
#else
            return _Serialize(obj,TrustToType);
#endif
        }

        private byte[] _Serialize<t>(t obj,Action<Type> TrustToType)
        {
            lock (this)
            {
                byte[] Result;

                if (Serialization.S_Data == null)
                {
                    Serialization.S_Data = new MemoryStream();
                    Serialization.Visitor = new HashSet<ObjectContainer>();
                    Serialization.Visitor_info = new HashSet<ObjectContainer>();
                }
                var SR = SerializeInfo<t>.GetSerialize();
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
                    Trust = TrustToType;
                    VisitedSerialize(obj, SR);
                    Result = S_Data.ToArray();
                }
#if DEBUG
                //catch (Exception ex)
                //{
                //    var Traced = Serialization.Traced;
                //    if (Traced != null)
                //        Traced = "On " + Traced;
                //    Serialization.Traced = null;
                //    throw new Exception($"Serialize Of Type >> {obj.GetType().FullName} Is Failed " + Traced, ex);
                //}
#endif
                finally
                {
                    Trust = null;
                    Serialization.S_Data.SetLength(0);
                    Serialization.Visitor.Clear();
                    Serialization.Visitor_info.Clear();
                }
                return Result;
            }
        }

#if DEBUG
        [ThreadStaticAttribute]
        private static string Traced;
#endif

        public t Deserialize<t>(byte[] Data, Action<Type> TrustToType=null)
        {
            var Type = typeof(t);
            var From = 0;
            return Deserialize<t>(Data, ref From,TrustToType);
        }

        public t Deserialize<t>(byte[] Data, ref int From,Action<Type> TrustToType)
        {
            lock (this)
            {
                t Result = default;
                if (Serialization.Visitor_info == null)
                {
                    Serialization.Visitor = new HashSet<ObjectContainer>();
                    Serialization.Visitor_info = new HashSet<ObjectContainer>();
                }
                Serialization.D_Data = Data;
                Serialization.From = From;
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
                    Trust = TrustToType;
                    VisitedDeserialize((c) => Result = (t)c, SerializeInfo<t>.GetSerialize());
                    AtLast?.Invoke();
                }
#if DEBUG
                catch (Exception ex)
                {
                    var Traced = Serialization.Traced;
                    if (Traced != null)
                        Traced = "On " + Traced;
                    Serialization.Traced = null;
                    throw new Exception($"Deserialize From Point {Serialization.From} Of Type >> {typeof(t).FullName} Is Failed {Traced}\nDatas As B64:\n" + System.Convert.ToBase64String(Data), ex);
                }
#endif
                finally
                {
                    Trust = null;
                    Serialization.Visitor.Clear();
                    Serialization.Visitor_info.Clear();
                    Serialization.D_Data = null;
                    AtLast = null;
                }
                return Result;
            }
        }
    }
}