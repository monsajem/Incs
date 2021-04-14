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
        private partial class SerializeInfo<t> : SerializeInfo
        {
            public override void Make()
            {
                var Type = typeof(t);

                var IsSerializable = Type.GetInterfaces().Where(
                    (c) =>
                    {
                        if (c.IsGenericType)
                            if (c.GetGenericTypeDefinition() == ISerializableType)
                                return true;
                        return false;
                    }).FirstOrDefault();
                if (IsSerializable != null)
                {
                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_Serializable();
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
                else if (Type == typeof(Delegate))
                {
                    InsertSerializer(
                    (Data,obj) =>
                    {
                        var DType = obj.GetType();
                        var Serializer = GetSerialize(DType);
                        Serializere.VisitedInfoSerialize<object>(Data,DType, () => (Serializer.NameAsByte, null));
                        Serializer.Serializer(Data,obj);
                    },
                    (Data) =>
                    {
                        var Info = Serializere.VisitedInfoDeserialize(Data,() => Serializere.Read(Data));

                        return GetSerialize(Info).Deserializer(Data);
                    });
                }
                else if (Type.GetInterfaces().Where((c) => c == typeof(Collection.Array.Base.IArray)).FirstOrDefault() != null)
                {

                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_Monsajem_Array();
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
                else if (Type.IsInterface)
                {
                    InsertSerializer(() =>
                    {
                        var Sr = SerializeInfo<object>.GetSerialize();
                        return (Sr.Serializer, Sr.Deserializer);
                    });
                }
                else if (Type.IsArray)
                {
                    try
                    {
                        var size = System.Runtime.InteropServices.Marshal.SizeOf(Type.GetElementType());
                        InsertSerializer(() =>
                        {
                            var sr = MakeSerializer_Array_Struct(size);
                            return (sr.Serializer, sr.Deserializer);
                        });
                    }
                    catch
                    {
                        InsertSerializer(() =>
                        {
                            var sr = MakeSerializer_Array_Class();
                            return (sr.Serializer, sr.Deserializer);
                        });
                    }
                }
                else if (Type == typeof(System.Array))
                {
                    InsertSerializer(() =>
                    {
                        var Sr = SerializeInfo<object>.GetSerialize();
                        return (Sr.Serializer, Sr.Deserializer);
                    });
                }
                else if (Type == typeof(System.Array))
                {
                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_Array_Class();
                        return ((Data,obj) =>
                        {
                            var ar = (System.Array)obj;
                        }, (Data) =>
                        {
                            return null;
                        }
                        );
                    });
                }
                else if (Type.BaseType == typeof(MulticastDelegate))
                {
                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_Delegate();
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
                else if (Nullable.GetUnderlyingType(Type) != null)
                {
                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_Nullable();
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
                else if (Type.IsValueType)
                {
                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_ValueType();
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
                else
                {
                    InsertSerializer(() =>
                    {
                        var sr = MakeSerializer_Else();
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
            }

            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_Serializable()
            {
                var Type = typeof(t);
                var InterfaceType = Type.GetInterfaces().
                    Where((c) => c.IsGenericType).
                    Where((c) => c.GetGenericTypeDefinition() == ISerializableType).
                    FirstOrDefault();

                var InnerType = InterfaceType.GenericTypeArguments[0];
                var innerSerializer = GetSerialize(InnerType);
                var Getter = InterfaceType.GetMethod("GetData");
                var Setter = InterfaceType.GetMethod("SetData");
                Action<SerializeData,object> Serializer = (Data,obj) =>
                {
                    if (obj == null)
                    {
                        Data.S_Data.WriteByte(0);
                        return;
                    }

                    Data.S_Data.WriteByte(1);
                    Serializere.VisitedSerialize(Data,Getter.Invoke(obj, null), innerSerializer);
                };

                Func<DeserializeData,object> Deserializer = (Data) =>
                {
                    var ThisFrom = Data.From;
                    if (Data.D_Data[Data.From] == 0)
                    {
                        Data.From += 1;
                        return null;
                    }
                    Data.From += 1;

                    object Result = GetUninitializedObject(Type);
                    Serializere.VisitedDeserialize(Data,(c) => Setter.Invoke(Result, new object[] { c }), innerSerializer);
                    return Result;
                };

                return (Serializer, Deserializer);
            }

            private (Func<SerializeData,System.Array, int[]> Write, 
                     Func<DeserializeData,(int[] Ends, System.Array ar)> Read)
                ArrayGetCreator()
            {
                var Type = typeof(t);
                Func<SerializeData,System.Array, int[]> Write;
                Func<DeserializeData,(int[] Ends, System.Array ar)> Read;

                var Creator = DynamicAssembly.TypeController.CreateArray(Type);
                var Rank = Type.GetArrayRank();
                Write = (Data,ar) =>
                {
                    var Ends = new int[Rank];
                    for (int i = 0; i < Rank; i++)
                    {
                        Ends[i] = ar.GetUpperBound(i) + 1;
                        Data.S_Data.Write(BitConverter.GetBytes(Ends[i]), 0, 4);
                    }
                    return Ends;
                };
                Read = (Data) =>
                {
                    var Ends = new int[Rank];
                    for (int i = 0; i < Rank; i++)
                    {
                        Ends[i] = BitConverter.ToInt32(Data.D_Data, Data.From);
                        Data.From += 4;
                    }
                    var ArrayLen = Ends[0];
                    for (int i = 1; i < Rank; i++)
                        ArrayLen = ArrayLen * Ends[i];
                    if (ArrayLen > Data.D_Data.Length - Data.From)
                        throw new ArgumentException("Array length is more than bytes length!");
                    return (Ends, (System.Array)Creator(Ends));
                };

                return (Write, Read);
            }

            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_Array_Class()
            {
                var Type = typeof(t);
                var SR = ArrayMakeSerializer_Object(Type);
                var Creator = ArrayGetCreator();
                Action<SerializeData,object> Serializer = (Data,obj) =>
                {
                    var ar = (System.Array)obj;
                    SR.Serializer(Data,(ar, Creator.Write(Data,ar)));
                };
                Func<DeserializeData,object> Deserializer = (Data) =>
                {
                    var info = Creator.Read(Data);
                    SR.Deserializer(Data,(info.ar, info.Ends));
                    return info.ar;
                };
                return (Serializer, Deserializer);
            }

            private (Action<SerializeData,(System.Array ar, int[] Ends)> Serializer,
                     Action<DeserializeData,(System.Array ar, int[] Ends)> Deserializer)
                ArrayMakeSerializer_Object(Type Type)
            {
                var Setter = DynamicAssembly.TypeController.SetArray(Type);
                var Getter = DynamicAssembly.TypeController.GetArray(Type);
                var ElementType = Type.GetElementType();
                var ItemsSerializer = GetSerialize(Type.GetElementType());

                Action<SerializeData,(System.Array ar, int[] Ends)> Serializer = (Data,obj) =>
                {
                    var ar = obj.ar;
                    var Ends = obj.Ends;
                    var Rank = Ends.Length;
                    var Currents = new int[Rank];
                    while (Currents[Currents.Length - 1] < Ends[Ends.Length - 1])
                    {
                        for (Currents[0] = 0; Currents[0] < Ends[0]; Currents[0]++)
                        {
                            Serializere.VisitedSerialize(Data,Getter(ar, Currents), ItemsSerializer);
                        }
                        for (int i = 1; i < Rank; i++)
                        {
                            if (Currents[i] < Ends[i])
                            {
                                Currents[i]++;
                                Currents[i - 1] = 0;
                            }
                        }
                    }
                };
                Action<DeserializeData,(System.Array ar, int[] Ends)> Deserializer = (Data,obj) =>
                {
                    var ar = obj.ar;
                    var Ends = obj.Ends;
                    var Rank = Ends.Length;
                    var Currents = new int[Rank];

                    while (Currents[Currents.Length - 1] < Ends[Ends.Length - 1])
                    {
                        for (Currents[0] = 0; Currents[0] < Ends[0]; Currents[0]++)
                        {
                            var StandAloneCurrent = new int[Rank];
                            for (int i = 0; i < Rank; i++)
                                StandAloneCurrent[i] = Currents[i];
                            Serializere.VisitedDeserialize(Data,(c) => Setter(ar, c, StandAloneCurrent), ItemsSerializer);
                        }
                        for (int i = 1; i < Rank; i++)
                        {
                            if (Currents[i] < Ends[i])
                            {
                                Currents[i]++;
                                Currents[i - 1] = 0;
                            }
                        }
                    }
                };
                return (Serializer, Deserializer);
            }
            private unsafe (Action<SerializeData,System.Array> Serializer,
             Action<DeserializeData,(System.Array ar, int[] Ends)> Deserializer)
                ArrayMakeSerializer_Struct(Type Type, int size)
            {

                if (Type.GetElementType() == typeof(bool))
                    size = 1;

                Action<SerializeData,System.Array> Serializer = (Data,ar) =>
                {
                    byte[] bytes = new byte[ar.Length * size];

                    System.Runtime.InteropServices.GCHandle h =
                        System.Runtime.InteropServices.GCHandle.Alloc(ar,
                            System.Runtime.InteropServices.GCHandleType.Pinned);

                    var ptr = h.AddrOfPinnedObject();
                    System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, bytes.Length);
                    h.Free();

                    Data.S_Data.Write(bytes, 0, bytes.Length);
                };
                Action<DeserializeData,(System.Array ar, int[] Ends)> 
                    Deserializer = (Data,obj) =>
                {
                    var ar = obj.ar;
                    var Ends = obj.Ends;
                    var Rank = Ends.Length;
                    var Len = 0;
                    Len = Ends[0];
                    for (int i = 1; i < Rank; i++)
                    {
                        Len = Len * Ends[i];
                    }
                    Len = Len * size;

                    System.Runtime.InteropServices.GCHandle h =
                        System.Runtime.InteropServices.GCHandle.Alloc(ar,
                            System.Runtime.InteropServices.GCHandleType.Pinned);

                    var ptr = h.AddrOfPinnedObject();
                    System.Runtime.InteropServices.Marshal.Copy(Data.D_Data, Data.From, ptr, Len);
                    h.Free();
                    Data.From += Len;
                };
                return (Serializer, Deserializer);
            }
            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_Array_Struct(int size)
            {
                var Type = typeof(t);
                var Sr = ArrayMakeSerializer_Struct(Type, size);
                var Creator = ArrayGetCreator();

                Action<SerializeData,object> Serializer = (Data,obj) =>
                {
                    var ar = (System.Array)obj;
                    Creator.Write(Data,ar);
                    Sr.Serializer(Data,ar);
                };
                Func<DeserializeData,object> Deserializer = (Data) =>
                {
                    var info = Creator.Read(Data);
                    Sr.Deserializer(Data,(info.ar, info.Ends));
                    return info.ar;
                };
                return (Serializer, Deserializer);
            }

            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_Monsajem_Array()
            {
                var Type = typeof(t);
                var ItemsSerializer = GetSerialize(System.Array.CreateInstance(
                    ((Collection.Array.Base.IArray)GetUninitializedObject(Type)).ElementType, 0).GetType());
                var ObjSerializer = SerializeInfo<object>.GetSerialize();
                Action<SerializeData,object> Serializer = (Data,obj) =>
                {
                    var ar = (Collection.Array.Base.IArray)obj;
                    ObjSerializer.Serializer(Data,ar.Comparer);
                    ObjSerializer.Serializer(Data,ar.MyOptions);
                    ItemsSerializer.Serializer(Data,ar.ToArray());
                };
                Func<DeserializeData,object> Deserializer = (Data) =>
                {
                    var ar = (Collection.Array.Base.IArray)GetUninitializedObject(Type);
                    ar.Comparer = ObjSerializer.Deserializer(Data);
                    ar.MyOptions = ObjSerializer.Deserializer(Data);
                    ar.Insert((Array)ItemsSerializer.Deserializer(Data));
                    return ar;
                };
                return (Serializer, Deserializer);
            }

            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_Delegate()
            {
                var Type = typeof(t);
                Action<SerializeData,object> Serializer = (Data,obj) =>
                {
                    var MD = (MulticastDelegate)obj;
                    var Delegates = MD.GetInvocationList();
                    Data.S_Data.Write(BitConverter.GetBytes(Delegates.Length), 0, 4);
                    for (int i = 0; i < Delegates.Length; i++)
                    {
                        LoadedFunc LoadedFunc;
                        var Delegate = Delegates[i];
                        var HashCode = Delegate.Method.GetHashCode();
                        LoadedFunc = Serializere.VisitedInfoSerialize(Data,Delegate.Method,
                        () =>
                        {

                            var Key = new LoadedFunc(Delegate.Method);
                            if (Serializere.LoadedFuncs_Ser.TryGetValue(Key, out LoadedFunc) == false)
                            {
                                var TargetType = Delegate.Method.DeclaringType;

                                LoadedFunc = Key;
                                LoadedFunc.NameAsByte =
                                    Serializere.Write(
                                        Delegate.Method.Name,
                                        Delegate.Method.ReflectedType.MidName());
                                LoadedFunc.SerializerTarget = GetSerialize(TargetType);
                                Serializere.LoadedFuncs_Ser.Add(LoadedFunc);
                            }
                            return (LoadedFunc.NameAsByte, LoadedFunc);
                        });
                        var Target = Delegates[i].Target;
                        Serializere.VisitedSerialize(Data,Target, LoadedFunc.SerializerTarget);
                    }
                };

                Func<DeserializeData,object> Deserializer = (Data) =>
                {
                    var Count = BitConverter.ToInt32(Data.D_Data, Data.From);
                    Data.From += 4;
                    var Results = new Delegate[Count];
                    for (int i = 0; i < Count; i++)
                    {
                        LoadedFunc LoadedFunc;
                        LoadedFunc = Serializere.VisitedInfoDeserialize(Data,
                        () =>
                        {
                            var MethodName = Serializere.Read(Data);
                            var TypeName = Serializere.Read(Data);

                            var Key = new LoadedFunc(MethodName + TypeName);
                            if (Serializere.LoadedFuncs_Des.TryGetValue(Key, out LoadedFunc) == false)
                            {
                                var ReflectedType = TypeName.GetTypeByName();
                                var Method = ReflectedType.GetMethod(MethodName,
                                    BindingFlags.Public |
                                    BindingFlags.NonPublic |
                                    BindingFlags.CreateInstance |
                                    BindingFlags.Instance);

                                var TargetType = Method.DeclaringType;
                                LoadedFunc = Key;
                                LoadedFunc.Delegate = Method.CreateDelegate(Type, null);
                                LoadedFunc.SerializerTarget = GetSerialize(TargetType);
                                Serializere.LoadedFuncs_Des.Add(LoadedFunc);
                            }
                            return LoadedFunc;
                        });

                        var ThisDelegate = (Delegate)LoadedFunc.Delegate.Clone();
                        Results[i] = ThisDelegate;
                        Serializere.VisitedDeserialize(Data,(c) => Deletage_Target.SetValue(ThisDelegate, c), LoadedFunc.SerializerTarget);
                    }

                    var Result = Delegate.Combine(Results);
                    return Result;
                };

                return (Serializer, Deserializer);
            }

            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_Nullable()
            {
                var Type = typeof(t);
                var InnerType = Nullable.GetUnderlyingType(Type);
                var innerSerializer = GetSerialize(InnerType);

                Action<SerializeData,object> Serializer = (Data,obj) =>
                {
                    innerSerializer.Serializer(Data,obj);
                };

                var CreateInstance = Type.GetConstructor(new Type[] { InnerType });

                Func<DeserializeData,object> Deserializer = (Data) =>
                {
                    object Result = innerSerializer.Deserializer(Data);
                    return CreateInstance.Invoke(new object[] { Result });
                };

                return (Serializer, Deserializer);
            }

            private (Action<SerializeData,object> Serializer, Func<DeserializeData,object> Deserializer)
                MakeSerializer_ValueType()
            {
                var Type = typeof(t);
                var FieldsSerializer = MakeFieldsSerializer(Type);

                Action<SerializeData,object> Serializer = FieldsSerializer.Serializer;

                Func<DeserializeData,object> Deserializer = FieldsSerializer.Deserializer;

                return (Serializer, Deserializer);
            }

            private (Action<SerializeData, object> Serializer, Func<DeserializeData, object> Deserializer)
                MakeSerializer_Else()
            {
                var Type = typeof(t);
                var FieldsSerializer = MakeFieldsSerializer(Type);

                Action<SerializeData, object> Serializer = FieldsSerializer.Serializer;

                if (Type.GetInterfaces().Where((c) => c == typeof(IPreSerialize)).Count() > 0)
                {
                    var BaseSerializer = Serializer;
                    Serializer = (Data,obj) =>
                    {
                        ((IPreSerialize)obj).PreSerialize();
                        BaseSerializer(Data,obj);
                    };
                }

                if (Type.GetInterfaces().Where((c) => c == typeof(IWhenCanSerialize)).Count() > 0)
                {
                    var BaseSerializer = Serializer;
                    Serializer = (Data,obj) =>
                    {
                        if (((IWhenCanSerialize)obj).CanSerialize == false)
                        {
                            Data.S_Data.WriteByte(0);
                        }
                        else
                        {
                            BaseSerializer(Data,obj);
                        }
                    };
                }

                Func<DeserializeData, object> Deserializer = FieldsSerializer.Deserializer;

                if (Type.GetInterfaces().Where((c) => c == typeof(IAfterDeserialize)).Count() > 0)
                {
                    var BaseDeserializer = Deserializer;
                    Deserializer = (Data) =>
                    {
                        var Result = BaseDeserializer(Data);
                        ((IAfterDeserialize)Result).AfterDeserialize();
                        return Result;
                    };
                }

                return (Serializer, Deserializer);
            }

            private (Action<SerializeData, object> Serializer, Func<DeserializeData, object> Deserializer)
                MakeFieldsSerializer(
                Type Type)
            {
                var Filds = new DynamicAssembly.TypeFields(Type).Fields;
                Filds = Filds.Where((c) => Serializere.FieldCondition(c.Info)).ToArray();

                Filds = Filds.OrderBy((c) => c.Info.Name + c.Info.DeclaringType.FullName,
                                      StringComparer.Ordinal).ToArray();

                var FildSerializer = new (SerializeInfo Sr,
                                          DynamicAssembly.FieldControler Controller)[Filds.Length];

                var FildsLen = Filds.Length;
                for (int i = 0; i < FildsLen; i++)
                {
                    var ExactSerializer = GetSerialize(Filds[i].Info.FieldType);
                    FildSerializer[i] = (ExactSerializer, Filds[i]);
                }

                Action<SerializeData, object> Serializer = (Data,obj) =>
                {
#if DEBUG
                    Data.S_Data.Write(BitConverter.GetBytes(FildsLen), 0, 4);
                    for (int i = 0; i < FildsLen; i++)
                    {
                        var Field = FildSerializer[i];
                        var FieldName = Serializere.Write(Field.Controller.Info.DeclaringType.ToString() + "." + Field.Controller.Info.Name);
                        Data.S_Data.Write(FieldName, 0, FieldName.Length);
                    }
#endif
                    for (int i = 0; i < FildsLen; i++)
                    {
                        var Field = FildSerializer[i];
                        Serializere.VisitedSerialize(Data,Field.Controller.GetValue(obj), Field.Sr);
                    }
                };

                Func<DeserializeData, object> Deserializer = (Data) =>
                {
                    object Owner = GetUninitializedObject(Type);
#if DEBUG
                    var F_len = BitConverter.ToInt32(Data.D_Data, Data.From);
                    Data.From += 4;
                    string[] Fields_Types = new string[F_len];
                    for (int i = 0; i < Fields_Types.Length; i++)
                        Fields_Types[i] = Serializere.Read(Data);
                    for (int i = 0; i < Fields_Types.Length; i++)
                    {
                        var Field = FildSerializer[i];
                        var FieldName = Fields_Types[i];
                        if (FieldName != Field.Controller.Info.DeclaringType.ToString() + "." + Field.Controller.Info.Name)
                        {
                            var EX_Str = "Wrong place of fields ";
                            for (int j = 0; j < Fields_Types.Length; j++)
                            {
                                FieldName = Fields_Types[j];
                                EX_Str += "\n SR:" + FieldName;
                            }
                            EX_Str += "\n";
                            for (int j = 0; j < FildSerializer.Length; j++)
                            {
                                Field = FildSerializer[j];
                                EX_Str += "\n DR:" + Field.Controller.Info.DeclaringType.ToString() + "." + Field.Controller.Info.Name;
                            }
                            throw new Exception(EX_Str);
                        }
                    }
#endif
                    for (int i = 0; i < FildsLen; i++)
                    {
                        var Field = FildSerializer[i];
                        Serializere.VisitedDeserialize(Data,(c) => Field.Controller.SetValue(Owner, c), Field.Sr);
                    }
                    return Owner;
                };
                return (Serializer, Deserializer);
            }
        }
    }
}