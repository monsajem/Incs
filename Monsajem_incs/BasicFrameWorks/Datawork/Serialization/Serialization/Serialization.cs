using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;
using Monsajem_Incs.Array.DynamicSize;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        private SerializeInfo InsertSerializer<t>(
            Action<object> Serializer,
            Func<object> Deserializer)
        {
            return InsertSerializer(typeof(t), () => (Serializer, Deserializer));
        }
        private SerializeInfo InsertSerializer(
            Type Type,
            Func<(Action<object> Sr, Func<object> Dr)> Serializer)
        {
            var HashCode = Type.GetHashCode();
            var Position = BinaryInsert(ref SerializersHashCodes, HashCode);
            SerializeInfo Serialize;

            {
                var TypeName = Type.MidName();
                var Name = UTF8.GetBytes(TypeName);
                var NameAsByte = BitConverter.GetBytes(Name.Length);
                Insert(ref NameAsByte, Name);
                Serialize = new SerializeInfo
                {
                    NameAsByte = NameAsByte,
                    Type = Type,
                    TypeHashCode = Type.GetHashCode()
                };
                Insert(ref ExactSerializersByHashCode, Serialize, Position);
                Position = BinaryInsert(ref SerializersNameCodes, TypeName.GetHashCode());
                Insert(ref ExactSerializersByNameCode, Serialize, Position);
            }
            Serialize.CanStoreInVisit =
                !(Type == typeof(Delegate) |
                  Type.BaseType == typeof(MulticastDelegate) |
                  Type.IsPrimitive);
            var Sr = Serializer();

#if DEBUG
            var SR = Sr.Sr;
            var DR = Sr.Dr;
            Sr.Sr = (obj) =>
            {
                var Pos = S_Data.Position;
                Tracer($"Type: {Serialize.Type.ToString()} Pos:({Pos})");
                Check_SR(Type);
                SR(obj);
                UnTracer($"Type: {Serialize.Type.ToString()} Pos:({Pos})");
            };
            Sr.Dr = () =>
            {
                var Pos = From;
                Tracer($"Type: {Serialize.Type.ToString()} Pos:({Pos})");
                Check_DR(Type);
                var Result = DR();
                UnTracer($"Type: {Serialize.Type.ToString()} Pos:({Pos})");
                return Result;
            };
#endif

            Serialize.Serializer = Sr.Sr;
            Serialize.Deserializer = Sr.Dr;
            return Serialize;
        }

        private SerializeInfo FindSerializer(Type Type)
        {
            var HashCode = Type.GetHashCode();
            var ItemsSerializer = System.Array.BinarySearch(SerializersHashCodes, HashCode);
            if (ItemsSerializer < 0)
            {
                MakeSerializer(Type);
                ItemsSerializer = System.Array.BinarySearch(SerializersHashCodes, HashCode);
            }
            return ExactSerializersByHashCode[ItemsSerializer];
        }
        private SerializeInfo FindSerializer(string TypeName)
        {
            var HashCode = TypeName.GetHashCode();
            var ItemsSerializer = System.Array.BinarySearch(SerializersNameCodes, HashCode);
            if (ItemsSerializer < 0)
            {
                var Type = TypeName.GetTypeByName();
                HashCode = Type.GetHashCode();
                ItemsSerializer = System.Array.BinarySearch(SerializersHashCodes, HashCode);
                if (ItemsSerializer < 0)
                    MakeSerializer(Type);
                else
                {
                    var SR = ExactSerializersByHashCode[ItemsSerializer];
                    var Position = BinaryInsert(ref SerializersNameCodes, TypeName.GetHashCode());
                    Insert(ref ExactSerializersByNameCode, SR, Position);
                }
                HashCode = TypeName.GetHashCode();
                ItemsSerializer = System.Array.BinarySearch(SerializersNameCodes, HashCode);
                if (ItemsSerializer < 0)
                {
                    HashCode = Type.GetHashCode();
                    ItemsSerializer = System.Array.BinarySearch(SerializersHashCodes, HashCode);
                    var SR = ExactSerializersByHashCode[ItemsSerializer];
                    var Position = BinaryInsert(ref SerializersNameCodes, TypeName.GetHashCode());
                    Insert(ref ExactSerializersByNameCode, SR, Position);
                    HashCode = TypeName.GetHashCode();
                    ItemsSerializer = System.Array.BinarySearch(SerializersNameCodes, HashCode);
                }
            }
            return ExactSerializersByNameCode[ItemsSerializer];
        }

        public void MakeSerializer(Type Type)
        {
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
                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_Serializable(Type, IsSerializable);
                    return (sr.Serializer, sr.Deserializer);
                });
            }
            else if (Type == typeof(Delegate))
            {
                InsertSerializer<Delegate>(
                (object obj) =>
                {
                    var DType = obj.GetType();
                    var Serializer = FindSerializer(DType);
                    VisitedInfoSerialize(DType.GetHashCode(), () => (Serializer.NameAsByte, null));
                    Serializer.Serializer(obj);
                },
                () =>
                {
                    var Info = VisitedInfoDeserialize(() => Read());

                    return FindSerializer(Info).Deserializer();
                });
            }
            else if (Type.GetInterfaces().Where((c) => c == typeof(Array.Base.IArray)).FirstOrDefault() != null)
            {

                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_Monsajem_Array(Type);
                    return (sr.Serializer, sr.Deserializer);
                });
            }
            else if (Type.IsInterface)
            {
                InsertSerializer(Type, () =>
                {
                    var Sr = FindSerializer(typeof(object));
                    return (Sr.Serializer, Sr.Deserializer);
                });
            }
            else if (Type.IsArray)
            {
                try
                {
                    var size = System.Runtime.InteropServices.Marshal.SizeOf(Type.GetElementType());
                    InsertSerializer(Type, () =>
                    {
                        var sr = MakeSerializer_Array_Struct(Type, size);
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
                catch
                {
                    InsertSerializer(Type, () =>
                    {
                        var sr = MakeSerializer_Array_Class(Type);
                        return (sr.Serializer, sr.Deserializer);
                    });
                }
            }
            else if (Type == typeof(System.Array))
            {
                InsertSerializer(Type, () =>
                {
                    var Sr = FindSerializer(typeof(object));
                    return (Sr.Serializer, Sr.Deserializer);
                });
            }
            else if (Type == typeof(System.Array))
            {
                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_Array_Class(Type);
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
                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_Delegate(Type);
                    return (sr.Serializer, sr.Deserializer);
                });
            }
            else if (Nullable.GetUnderlyingType(Type) != null)
            {
                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_Nullable(Type);
                    return (sr.Serializer, sr.Deserializer);
                });
            }
            else if (Type.IsValueType)
            {
                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_ValueType(Type);
                    return (sr.Serializer, sr.Deserializer);
                });
            }
            else
            {
                InsertSerializer(Type, () =>
                {
                    var sr = MakeSerializer_Else(Type);
                    return (sr.Serializer, sr.Deserializer);
                });
            }
        }

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Serializable(
            Type Type,
            Type InterfaceType)
        {
            var InnerType = InterfaceType.GenericTypeArguments[0];
            var innerSerializer = FindSerializer(InnerType);
            var Getter = InterfaceType.GetMethod("GetData");
            var Setter = InterfaceType.GetMethod("SetData");
            Action<object> Serializer = (object obj) =>
            {
                if (obj == null)
                {
                    S_Data.Write(Byte_0, 0, 1);
                    return;
                }

                S_Data.Write(Byte_1, 0, 1);
                VisitedSerialize(Getter.Invoke(obj, null), innerSerializer);
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
                VisitedDeserialize((c) => Setter.Invoke(Result, new object[] { c }), innerSerializer);
                return Result;
            };

            return (Serializer, Deserializer);
        }

        private (Func<System.Array, int[]> Write, Func<(int[] Ends, System.Array ar)> Read) ArrayGetCreator(Type Type)
        {
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
                    return (Ends, System.Array.CreateInstance(typeof(string), Ends));
                };
            }
            return (Write, Read);
        }

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Array_Class(Type Type)
        {
            var SR = ArrayMakeSerializer_Object(Type);
            var Creator = ArrayGetCreator(Type);
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
            var ItemsSerializer = FindSerializer(Type.GetElementType());

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
                         VisitedSerialize(Getter(ar, Currents), ItemsSerializer);
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
                        VisitedDeserialize((c) => Setter(ar, c, StandAloneCurrent), ItemsSerializer);
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
                h.Free();
                System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, bytes.Length);


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
                h.Free();
                System.Runtime.InteropServices.Marshal.Copy(D_Data, From, ptr, Len);
                From += Len;
            };
            return (Serializer, Deserializer);
        }
        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Array_Struct(Type Type, int size)
        {
            var Sr = ArrayMakeSerializer_Struct(Type, size);
            var Creator = ArrayGetCreator(Type);

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

        [ThreadStatic]
        private static Action AtLast;
        private void VisitedSerialize(
            object obj,
            SerializeInfo serializer)
        {
            if (obj == null)
            {
                S_Data.Write(Byte_Int_N_2, 0, 4);
                return;
            }
            if (serializer.CanStoreInVisit == false)
            {
                S_Data.Write(Byte_Int_N_1, 0, 4);
                serializer.Serializer(obj);
                return;
            }
            var VisitedObj = new ObjectContainer()
            {
                ObjHashCode = obj.GetHashCode(),
                TypeHashCode = serializer.TypeHashCode
            };
            var VisitedPos = Visitor.BinaryInsert(ref VisitedObj);
            if (VisitedPos > -1)
            {
#if DEBUG
                if (VisitedObj.obj.GetType() != obj.GetType())
                    throw new Exception("Type of visited object is wrong" +
                                        "\nMain: " + obj.GetType().ToString() +
                                        "\nVisited: " + VisitedObj.obj.GetType().ToString());
#endif
                S_Data.Write(BitConverter.GetBytes(VisitedObj.FromPos), 0, 4);
                return;
            }
#if DEBUG
            VisitedObj.obj = obj;
#endif
            VisitedObj.FromPos = (int)S_Data.Position;
            S_Data.Write(Byte_Int_N_1, 0, 4);
            serializer.Serializer(obj);
        }
        private void VisitedDeserialize(
            Action<object> Set,
            SerializeInfo deserializer)
        {
            var LastFrom = From;
            var Fr = BitConverter.ToInt32(D_Data, From);
            From += 4;
            if (deserializer.CanStoreInVisit == false)
            {
                if (Fr == -2)
                    return;
                Set(deserializer.Deserializer());
                return;
            }
            ObjectContainer VisitedObj;
            switch (Fr)
            {
                case -1:
                    VisitedObj = new ObjectContainer()
                    {
                        ObjHashCode = LastFrom
                    };
                    Visitor.BinaryInsert(VisitedObj);
                    VisitedObj.obj = deserializer.Deserializer();
                    Set(VisitedObj.obj);
                    return;
                case -2:
                    return;
            }
            VisitedObj = new ObjectContainer()
            {
                ObjHashCode = Fr
            };
            VisitedObj = Visitor.BinarySearch(VisitedObj).Value;
            if (VisitedObj.obj == null)
                AtLast += () => Set(VisitedObj.obj);
            else
                Set(VisitedObj.obj);
        }

        private object VisitedInfoSerialize(
            int HashCode,
            Func<(byte[], object)> GetData)
        {
            var VisitedObj = new ObjectContainer()
            {
                ObjHashCode = HashCode
            };
            int VisitedPos = Visitor_info.BinaryInsert(ref VisitedObj);
            if (VisitedPos > -1)
            {
                S_Data.Write(BitConverter.GetBytes(VisitedObj.FromPos), 0, 4);
                return VisitedObj.obj;
            }
            VisitedObj.FromPos = (int)S_Data.Position;
            var Data = GetData();
            VisitedObj.obj = Data.Item2;
            S_Data.Write(Byte_Int_N_1, 0, 4);
            S_Data.Write(Data.Item1, 0, Data.Item1.Length);
            return VisitedObj.obj;
        }
        private t VisitedInfoDeserialize<t>(
            Func<t> Get)
        {
            var LastFrom = From;
            var Fr = BitConverter.ToInt32(D_Data, From);
            From += 4;
            ObjectContainer VisitedObj;
            if (Fr == -1)
            {
                VisitedObj = new ObjectContainer()
                {
                    ObjHashCode = LastFrom,
                    obj = Get()
                };
                Visitor_info.BinaryInsert(VisitedObj);
                return (t)VisitedObj.obj;
            }
            VisitedObj = new ObjectContainer()
            {
                ObjHashCode = Fr
            };
            return (t)Visitor_info.BinarySearch(VisitedObj).Value.obj;
        }

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Monsajem_Array(Type Type)
        {
            var ItemsSerializer = FindSerializer(System.Array.CreateInstance(
                ((Array.Base.IArray)GetUninitializedObject(Type)).ElementType, 0).GetType());
            var ObjSerializer = FindSerializer(typeof(object));

            Action<object> Serializer = (object obj) =>
            {
                var ar = (Array.Base.IArray)obj;
                ObjSerializer.Serializer(ar.MyOptions);
                var AllArrays = ar.GetAllArrays();
                var Arrays = AllArrays.Ar;
                var ArraysLen = Arrays.Length;
                S_Data.Write(BitConverter.GetBytes(ArraysLen), 0, 4);
                for (int i = 0; i < ArraysLen; i++)
                {
                    var Array = Arrays[i];
                    S_Data.Write(BitConverter.GetBytes(Array.From), 0, 4);
                    S_Data.Write(BitConverter.GetBytes(Array.To), 0, 4);
                    ItemsSerializer.Serializer(Array.Ar);
                }
            };
            Func<object> Deserializer = () =>
            {
                var ar = (Array.Base.IArray)GetUninitializedObject(Type);
                ar.MyOptions = ObjSerializer.Deserializer();
                var ArraysLen = BitConverter.ToInt32(D_Data, From);
                From += 4;
                var AllArrays = (Ar: new (int From, int To, System.Array Ar)[ArraysLen], MaxLen: 0);
                var AllLen = 0;
                for (int i = 0; i < ArraysLen; i++)
                {
                    var ArFrom = BitConverter.ToInt32(D_Data, From);
                    From += 4;
                    var ArLen = BitConverter.ToInt32(D_Data, From);
                    From += 4;
                    var Array = ItemsSerializer.Deserializer();
                    AllLen += ArLen;
                    AllArrays.Ar[i] = (ArFrom, ArLen, (System.Array)Array);
                }
                ar.SetAllArrays(AllArrays);
                ar.SetLen(AllLen);
                return ar;
            };
            return (Serializer, Deserializer);
        }

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Delegate(Type Type)
        {
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
                    LoadedFunc = (LoadedFunc)VisitedInfoSerialize(HashCode,
                    () =>
                    {
                        var FuncPos = System.Array.BinarySearch(LoadedFuncs_Ser,
                        new LoadedFunc()
                        {
                            Hash = HashCode
                        });
                        if (FuncPos < 0)
                        {
                            var TargetType = Delegate.Method.DeclaringType;

                            LoadedFunc = new LoadedFunc()
                            {
                                Hash = HashCode,
                                NameAsByte = Write(
                                    Delegate.Method.Name,
                                    Delegate.Method.ReflectedType.MidName()),
                                SerializerTarget = FindSerializer(TargetType)
                            };
                            BinaryInsert(ref LoadedFuncs_Ser, LoadedFunc);
                        }
                        else
                        {
                            LoadedFunc = LoadedFuncs_Ser[FuncPos];
                        }
                        return (LoadedFunc.NameAsByte, LoadedFunc);
                    });
                    var Target = Delegates[i].Target;
                    VisitedSerialize(Target, LoadedFunc.SerializerTarget);
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
                    LoadedFunc = VisitedInfoDeserialize(() =>
                    {
                        var MethodName = Read();
                        var TypeName = Read();

                        var FuncPos = System.Array.BinarySearch(LoadedFuncs_Des, new LoadedFunc() { Hash = (MethodName + TypeName).GetHashCode() });
                        if (FuncPos < 0)
                        {
                            var ReflectedType = TypeName.GetTypeByName();
                            var Method = ReflectedType.GetMethod(MethodName,
                                BindingFlags.Public |
                                BindingFlags.NonPublic |
                                BindingFlags.CreateInstance |
                                BindingFlags.Instance);

                            var TargetType = Method.DeclaringType;
                            LoadedFunc = new LoadedFunc()
                            {
                                Delegate = Method.CreateDelegate(Type, null),
                                Hash = (MethodName + TypeName).GetHashCode(),
                                SerializerTarget = FindSerializer(TargetType)
                            };
                            BinaryInsert(ref LoadedFuncs_Des, LoadedFunc);
                        }
                        else
                        {
                            LoadedFunc = LoadedFuncs_Des[FuncPos];
                        }
                        return LoadedFunc;
                    });

                    var ThisDelegate = (Delegate)LoadedFunc.Delegate.Clone();
                    Results[i] = ThisDelegate;
                    VisitedDeserialize((c) => Deletage_Target.SetValue(ThisDelegate, c), LoadedFunc.SerializerTarget);
                }

                var Result = Delegate.Combine(Results);
                return Result;
            };

            return (Serializer, Deserializer);
        }

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Nullable(Type Type)
        {
            var InnerType = Nullable.GetUnderlyingType(Type);
            var innerSerializer = FindSerializer(InnerType);

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

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_ValueType(Type Type)
        {
            var FieldsSerializer = MakeFieldsSerializer(Type);

            Action<object> Serializer = FieldsSerializer.Serializer;

            Func<object> Deserializer = FieldsSerializer.Deserializer;

            return (Serializer, Deserializer);
        }

        private (Action<object> Serializer, Func<object> Deserializer) MakeSerializer_Else(Type Type)
        {
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
                        S_Data.Write(Byte_0, 0, 1);
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
            Filds = Filds.Where((c) => FieldCondition(c.Info)).ToArray();

            Filds = Filds.OrderBy((c) => c.Info.Name + c.Info.DeclaringType.FullName,
                                  StringComparer.Ordinal).ToArray();

            var FildSerializer = new (SerializeInfo Sr,
                                      DynamicAssembly.FieldControler Controller)[Filds.Length];

            var FildsLen = Filds.Length;
            for (int i = 0; i < FildsLen; i++)
            {
                var ExactSerializer = FindSerializer(Filds[i].Info.FieldType);
                FildSerializer[i] = (ExactSerializer, Filds[i]);
            }

            Action<object> Serializer = (object obj) =>
            {
#if DEBUG
                S_Data.Write(BitConverter.GetBytes(FildsLen), 0, 4);
                for (int i = 0; i < FildsLen; i++)
                {
                    var Field = FildSerializer[i];
                    var FieldName = Write(Field.Controller.Info.DeclaringType.ToString() + "." + Field.Controller.Info.Name);
                    S_Data.Write(FieldName, 0, FieldName.Length);
                }
#endif
                for (int i = 0; i < FildsLen; i++)
                {
                    var Field = FildSerializer[i];
                    VisitedSerialize(Field.Controller.GetValue(obj), Field.Sr);
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
                    Fields_Types[i] = Read();
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
                    VisitedDeserialize((c) => Field.Controller.SetValue(Owner, c), Field.Sr);
                }
                return Owner;
            };
            return (Serializer, Deserializer);
        }

        private byte[] Write(params string[] str)
        {
            byte[] Results = new byte[0];
            for (int i = 0; i < str.Length; i++)
            {
                var UTF8_Data = UTF8.GetBytes(str[i]);
                var Result = new byte[UTF8_Data.Length + 4];
                System.Array.Copy(BitConverter.GetBytes(UTF8_Data.Length), 0, Result, 0, 4);
                System.Array.Copy(UTF8_Data, 0, Result, 4, UTF8_Data.Length);
                Insert(ref Results, Result);
            }
            return Results;
        }

        private string Read()
        {
            var Len = BitConverter.ToInt32(D_Data, From);
            From += 4;
            var Result = UTF8.GetString(D_Data, From, Len);
            From += Result.Length;
            return Result;
        }

#if DEBUG
        private void Check_SR(Type Type)
        {
            S_Data.Write(BitConverter.GetBytes(S_Data.Length), 0, 8);
            var TypeBytes = Write(Type.MidName());
            S_Data.Write(TypeBytes, 0, TypeBytes.Length);
        }
        private void Check_DR(Type Type)
        {
            var DR_Pos = From;
            var SR_Pos = BitConverter.ToInt64(D_Data, From);
            From += 8;
            if (DR_Pos != SR_Pos ||
                SR_Pos < 0)
                throw new Exception($"Position Isn't Valid. SR:{SR_Pos} , DR:{DR_Pos}");
            var TypeName = Read();
            var SR_Type = TypeName.GetTypeByName();
            if (SR_Type != Type)
                throw new Exception($"Type isn't match\nSR: {SR_Type.MidName()}\nDR: {Type.MidName()}");
        }
#endif
    }

}
