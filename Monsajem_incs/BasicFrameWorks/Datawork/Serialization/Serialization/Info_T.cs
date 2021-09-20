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
        private partial class SerializeInfo<t> : SerializeInfo
        {
            public static Func<DeserializeData,object> Default_Deserializer;
            public static Action<SerializeData,object> Default_Serializer;

            public SerializeInfo()
            {
                Type = typeof(t);
                TypeHashCode = Type.GetHashCode();

                var TypeName = Type.MidName();
                var Name = UTF8.GetBytes(TypeName);
                NameAsByte = BitConverter.GetBytes(Name.Length);
                Insert(ref NameAsByte, Name);

                if (Type.IsValueType)
                {
                    try
                    {
                        ConstantSize = System.Runtime.InteropServices.Marshal.SizeOf(Type);
                        ConstantSize = System.Runtime.CompilerServices.Unsafe.SizeOf<t>();
                    }
                    catch(Exception ex)
                    {
                        ex.ToString();
                        ConstantSize = -1;
                    }
                }
                else
                    ConstantSize = -1;

                if(ConstantSize>-1)
                {
                    Default_Serializer = (Data, Obj) =>
                    {
                        Data.Data.Write(StructToBytes((t)Obj, ConstantSize));
                    };
                    Default_Deserializer = (Data) =>
                    {
                        var From = Data.From;
                        var Result = BytesToStruct<t>(Data.Data, From);
                        Data.From = From + ConstantSize;
                        return Result;
                    };
                }


                CanStoreInVisit =
                    !(Type == typeof(Delegate) |
                      Type.BaseType == typeof(MulticastDelegate) |
                      Type.IsPrimitive);
            }

            private static readonly SerializeInfo<t> SerializerObj = new SerializeInfo<t>();

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public static SerializeInfo<t> GetSerialize()
            {
                var Sr = SerializerObj;
                if (Sr.IsMade == false)
                {
                    lock(SerializerObj)
                    {
                        if(Sr.IsMading==false)
                        {
                            Sr.IsMading = true;
                            if (Default_Serializer == null)
                                Sr.Make();
                            else
                            {
                                Sr.Serializer = Default_Serializer;
                                Sr.Deserializer = Default_Deserializer;
                            }
                            Sr.IsMade = true;
                        }
                    }
                }
                return Sr;
            }
        }
    }
}