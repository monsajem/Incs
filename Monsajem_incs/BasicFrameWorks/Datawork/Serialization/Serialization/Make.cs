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
                    (object obj) =>
                    {
                        var DType = obj.GetType();
                        var Serializer = GetSerialize(DType);
                        Serializere.VisitedInfoSerialize<object>(DType, () => (Serializer.NameAsByte, null));
                        Serializer.Serializer(obj);
                    },
                    () =>
                    {
                        var Info = Serializere.VisitedInfoDeserialize(() => Serializere.Read());

                        return GetSerialize(Info).Deserializer();
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
                        return ((obj) =>
                        {
                            var ar = (System.Array)obj;
                        }, () =>
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

            private (Action<object> Serializer, Func<object> Deserializer)
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
                Action<object> Serializer = (object obj) =>
                {
                    if (obj == null)
                    {
                        S_Data.WriteByte(0);
                        return;
                    }

                    S_Data.WriteByte(1);
                    Serializere.VisitedSerialize(Getter.Invoke(obj, null), innerSerializer);
                };

                Func<object> Deserializer = () =>
                {
                    var ThisFrom = From;
                    if (D_Data[From] == 0)
                    {
                        From += 1;
                        return null;
                    }
                    From += 1;

                    object Result = GetUninitializedObject(Type);
                    Serializere.VisitedDeserialize((c) => Setter.Invoke(Result, new object[] { c }), innerSerializer);
                    return Result;
                };

                return (Serializer, Deserializer);
            }

            private (Func<System.Array, int[]> Write, Func<(int[] Ends, System.Array ar)> Read)
                ArrayGetCreator()
            {
                var Type = typeof(t);
                Func<System.Array, int[]> Write;
                Func<(int[] Ends, System.Array ar)> Read;
                if (Type != typeof(System.Array))
                {
                    var Creator = DynamicAssembly.TypeController.CreateArray(Type);
                    var Rank = Type.GetArrayRank();
                    Write = (ar) =>
                    {
                        var Ends = new int[Rank];
                        for (int i = 0; i < Rank; i++)
                        {
                            Ends[i] = ar.GetUpperBound(i) + 1;
                            S_Data.Write(BitConverter.GetBytes(Ends[i]), 0, 4);
                        }
                        return Ends;
                    };
                    Read = () =>
                    {
                        var Ends = new int[Rank];
                        for (int i = 0; i < Rank; i++)
                        {
                            Ends[i] = BitConverter.ToInt32(D_Data, From);
                            From += 4;
                        }
                        var ArrayLen = Ends[0];
                        for (int i = 1; i < Rank; i++)
                            ArrayLen = ArrayLen * Ends[i];
                        if (ArrayLen > D_Data.Length - From)
                            throw new ArgumentException("Array length is more than bytes length!");
                        return (Ends, (System.Array)Creator(Ends));
                    };
                }
                else
                {
                    Write = (ar) =>
                    {
                        var Rank = ar.Rank;
                        S_Data.Write(BitConverter.GetBytes(Rank), 0, 4);
                        var Ends = new int[Rank];
                        for (int i = 0; i < Rank; i++)
                        {
                            Ends[i] = ar.GetUpperBound(i) + 1;
                            S_Data.Write(BitConverter.GetBytes(Ends[i]), 0, 4);
                        }
                        return Ends;
                    };
                    Read = () =>
                    {
                        var Rank = BitConverter.ToInt32(D_Data, From);
                        From += 4;
                        var Ends = new int[Rank];
                        for (int i = 0; i < Rank; i++)
                        {
                            Ends[i] = BitConverter.ToInt32(D_Data, From);
                            From += 4;
                        }
                        var ArrayLen = Ends[0];
                        for (int i = 1; i < Rank; i++)
                            ArrayLen = ArrayLen * Ends[i];
                        if (ArrayLen > D_Data.Length - From)
                            throw new ArgumentException("Array length is more than bytes length!");
                        return (Ends, System.Array.CreateInstance(typeof(string), Ends));
                    };
                }
                return (Write, Read);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_Array_Class()
            {
                var Type = typeof(t);
                var SR = ArrayMakeSerializer_Object(Type);
                var Creator = ArrayGetCreator();
                Action<object> Serializer = (object obj) =>
                {
                    var ar = (System.Array)obj;
                    SR.Serializer((ar, Creator.Write(ar)));
                };
                Func<object> Deserializer = () =>
                {
                    var info = Creator.Read();
                    SR.Deserializer((info.ar, info.Ends));
                    return info.ar;
                };
                return (Serializer, Deserializer);
            }

            private (Action<(System.Array ar, int[] Ends)> Serializer,
                     Action<(System.Array ar, int[] Ends)> Deserializer)
                ArrayMakeSerializer_Object(Type Type)
            {
                var Setter = DynamicAssembly.TypeController.SetArray(Type);
                var Getter = DynamicAssembly.TypeController.GetArray(Type);
                var ElementType = Type.GetElementType();
                var ItemsSerializer = GetSerialize(Type.GetElementType());

                Action<(System.Array ar, int[] Ends)> Serializer = (obj) =>
                {
                    var ar = obj.ar;
                    var Ends = obj.Ends;
                    var Rank = Ends.Length;
                    var Currents = new int[Rank];
                    while (Currents[Currents.Length - 1] < Ends[Ends.Length - 1])
                    {
                        for (Currents[0] = 0; Currents[0] < Ends[0]; Currents[0]++)
                        {
                            Serializere.VisitedSerialize(Getter(ar, Currents), ItemsSerializer);
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
                Action<(System.Array ar, int[] Ends)> Deserializer = (obj) =>
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
                            Serializere.VisitedDeserialize((c) => Setter(ar, c, StandAloneCurrent), ItemsSerializer);
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
            private unsafe (Action<System.Array> Serializer,
             Action<(System.Array ar, int[] Ends)> Deserializer)
                ArrayMakeSerializer_Struct(Type Type, int size)
            {

                if (Type.GetElementType() == typeof(bool))
                    size = 1;

                Action<System.Array> Serializer = (ar) =>
                {
                    byte[] bytes = new byte[ar.Length * size];

                    System.Runtime.InteropServices.GCHandle h =
                        System.Runtime.InteropServices.GCHandle.Alloc(ar,
                            System.Runtime.InteropServices.GCHandleType.Pinned);

                    var ptr = h.AddrOfPinnedObject();
                    System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, bytes.Length);
                    h.Free();

                    S_Data.Write(bytes, 0, bytes.Length);
                };
                Action<(System.Array ar, int[] Ends)> Deserializer = (obj) =>
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
                    System.Runtime.InteropServices.Marshal.Copy(D_Data, From, ptr, Len);
                    h.Free();
                    From += Len;
                };
                return (Serializer, Deserializer);
            }
            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_Array_Struct(int size)
            {
                var Type = typeof(t);
                var Sr = ArrayMakeSerializer_Struct(Type, size);
                var Creator = ArrayGetCreator();

                Action<object> Serializer = (object obj) =>
                {
                    var ar = (System.Array)obj;
                    Creator.Write(ar);
                    Sr.Serializer(ar);
                };
                Func<object> Deserializer = () =>
                {
                    var info = Creator.Read();
                    Sr.Deserializer((info.ar, info.Ends));
                    return info.ar;
                };
                return (Serializer, Deserializer);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_Monsajem_Array()
            {
                var Type = typeof(t);
                var ItemsSerializer = GetSerialize(System.Array.CreateInstance(
                    ((Collection.Array.Base.IArray)GetUninitializedObject(Type)).ElementType, 0).GetType());
                var ObjSerializer = SerializeInfo<object>.GetSerialize();
                Action<object> Serializer = (object obj) =>
                {
                    var ar = (Collection.Array.Base.IArray)obj;
                    ObjSerializer.Serializer(ar.Comparer);
                    ObjSerializer.Serializer(ar.MyOptions);
                    ItemsSerializer.Serializer(ar.ToArray());
                };
                Func<object> Deserializer = () =>
                {
                    var ar = (Collection.Array.Base.IArray)GetUninitializedObject(Type);
                    ar.Comparer = ObjSerializer.Deserializer();
                    ar.MyOptions = ObjSerializer.Deserializer();
                    ar.Insert((Array)ItemsSerializer.Deserializer());
                    return ar;
                };
                return (Serializer, Deserializer);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_Delegate()
            {
                var Type = typeof(t);
                Action<object> Serializer = (object obj) =>
                {
                    var MD = (MulticastDelegate)obj;
                    var Delegates = MD.GetInvocationList();
                    S_Data.Write(BitConverter.GetBytes(Delegates.Length), 0, 4);
                    for (int i = 0; i < Delegates.Length; i++)
                    {
                        LoadedFunc LoadedFunc;
                        var Delegate = Delegates[i];
                        var HashCode = Delegate.Method.GetHashCode();
                        LoadedFunc = Serializere.VisitedInfoSerialize(Delegate.Method,
                        () =>
                        {

                            var Key = new LoadedFunc(Delegate.Method);
                            if (Serializere.LoadedFuncs_Ser.TryGetValue(Key,out LoadedFunc)==false)
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
                        Serializere.VisitedSerialize(Target, LoadedFunc.SerializerTarget);
                    }
                };

                Func<object> Deserializer = () =>
                {
                    var Count = BitConverter.ToInt32(D_Data, From);
                    From += 4;
                    var Results = new Delegate[Count];
                    for (int i = 0; i < Count; i++)
                    {
                        LoadedFunc LoadedFunc;
                        LoadedFunc = Serializere.VisitedInfoDeserialize(() =>
                        {
                            var MethodName = Serializere.Read();
                            var TypeName = Serializere.Read();

                            var Key =new LoadedFunc(MethodName + TypeName);
                            if (Serializere.LoadedFuncs_Des.TryGetValue(Key,out LoadedFunc)==false)
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
                        Serializere.VisitedDeserialize((c) => Deletage_Target.SetValue(ThisDelegate, c), LoadedFunc.SerializerTarget);
                    }

                    var Result = Delegate.Combine(Results);
                    return Result;
                };

                return (Serializer, Deserializer);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_Nullable()
            {
                var Type = typeof(t);
                var InnerType = Nullable.GetUnderlyingType(Type);
                var innerSerializer = GetSerialize(InnerType);

                Action<object> Serializer = (object obj) =>
                {
                    innerSerializer.Serializer(obj);
                };

                var CreateInstance = Type.GetConstructor(new Type[] { InnerType });

                Func<object> Deserializer = () =>
                {
                    object Result = innerSerializer.Deserializer();
                    return CreateInstance.Invoke(new object[] { Result });
                };

                return (Serializer, Deserializer);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_ValueType()
            {
                var Type = typeof(t);
                var FieldsSerializer = MakeFieldsSerializer(Type);

                Action<object> Serializer = FieldsSerializer.Serializer;

                Func<object> Deserializer = FieldsSerializer.Deserializer;

                return (Serializer, Deserializer);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
                MakeSerializer_Else()
            {
                var Type = typeof(t);
                var FieldsSerializer = MakeFieldsSerializer(Type);

                Action<object> Serializer = FieldsSerializer.Serializer;

                if (Type.GetInterfaces().Where((c) => c == typeof(IPreSerialize)).Count() > 0)
                {
                    var BaseSerializer = Serializer;
                    Serializer = (object obj) =>
                    {
                        ((IPreSerialize)obj).PreSerialize();
                        BaseSerializer(obj);
                    };
                }

                if (Type.GetInterfaces().Where((c) => c == typeof(IWhenCanSerialize)).Count() > 0)
                {
                    var BaseSerializer = Serializer;
                    Serializer = (object obj) =>
                    {
                        if (((IWhenCanSerialize)obj).CanSerialize == false)
                        {
                            S_Data.WriteByte(0);
                        }
                        else
                        {
                            BaseSerializer(obj);
                        }
                    };
                }

                Func<object> Deserializer = FieldsSerializer.Deserializer;

                if (Type.GetInterfaces().Where((c) => c == typeof(IAfterDeserialize)).Count() > 0)
                {
                    var BaseDeserializer = Deserializer;
                    Deserializer = () =>
                    {
                        var Result = BaseDeserializer();
                        ((IAfterDeserialize)Result).AfterDeserialize();
                        return Result;
                    };
                }

                return (Serializer, Deserializer);
            }

            private (Action<object> Serializer, Func<object> Deserializer)
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

                Action<object> Serializer = (object obj) =>
                {
#if DEBUG
                    S_Data.Write(BitConverter.GetBytes(FildsLen), 0, 4);
                    for (int i = 0; i < FildsLen; i++)
                    {
                        var Field = FildSerializer[i];
                        var FieldName = Serializere.Write(Field.Controller.Info.DeclaringType.ToString() + "." + Field.Controller.Info.Name);
                        S_Data.Write(FieldName, 0, FieldName.Length);
                    }
#endif
                    for (int i = 0; i < FildsLen; i++)
                    {
                        var Field = FildSerializer[i];
                        Serializere.VisitedSerialize(Field.Controller.GetValue(obj), Field.Sr);
                    }
                };

                Func<object> Deserializer = () =>
                {
                    object Owner = GetUninitializedObject(Type);
#if DEBUG
                    var F_len = BitConverter.ToInt32(D_Data, From);
                    From += 4;
                    string[] Fields_Types = new string[F_len];
                    for (int i = 0; i < Fields_Types.Length; i++)
                        Fields_Types[i] = Serializere.Read();
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
                        Serializere.VisitedDeserialize((c) => Field.Controller.SetValue(Owner, c), Field.Sr);
                    }
                    return Owner;
                };
                return (Serializer, Deserializer);
            }
        }
    }
}