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

    public interface IWhenCanSerialize
    {
        bool CanSerialize { get; }
    }

    public interface IPreSerialize
    {
        void PreSerialize();
    }

    public interface IAfterDeserialize
    {
        void AfterDeserialize();
    }

    public interface ISerializable<DataType>
    {
        DataType GetData();
        void SetData(DataType Data);
    }

    public class NonSerializedAttribute : Attribute
    { }

    public static class SerializationExtentions
    {
        public static readonly Serialization Serializere = new Serialization(
            (c) => c.GetCustomAttributes(typeof(NonSerializedAttribute)).Count() == 0);

        public static readonly Serialization HoleSerializere = new Serialization();

        public static byte[] Serialize<t>(this t obj)
        {
            return Serializere.Serialize(obj);
        }

        public static t Deserialize<t>(this byte[] Data)
        {
            return Serializere.Deserialize<t>(Data);
        }

        public static t Deserialize<t>(this byte[] Data, ref int From)
        {
            return Serializere.Deserialize<t>(Data, ref From);
        }

        public static t Deserialize<t>(this byte[] Data, t SampleType)
        {
            return Serializere.Deserialize<t>(Data);
        }

        public static object Deserialize(this byte[] Data, Type Type)
        {
            return Serializere.Deserialize(Data, Type);
        }

        public static byte[] HoleSerialize<t>(this t obj)
        {
            return HoleSerializere.Serialize(obj);
        }

        public static t HoleDeserialize<t>(this byte[] Data)
        {
            return HoleSerializere.Deserialize<t>(Data);
        }

        public static object HoleDeserialize(this byte[] Data, Type Type)
        {
            return HoleSerializere.Deserialize(Data, Type);
        }
    }

    public partial class Serialization
    {
        private class SerializeInfo
        {
            public Type Type;
            public int TypeHashCode;
            public Func<object> Deserializer;
            public Action<object> Serializer;
            public byte[] NameAsByte;
            public bool CanStoreInVisit;
        }

        private class LoadedFunc :
            IComparable<LoadedFunc>
        {
            public int Hash;
            public Delegate Delegate;
            public byte[] NameAsByte;
            public SerializeInfo SerializerTarget;
            public int CompareTo(LoadedFunc other)
            {
                return this.Hash - other.Hash;
            }
        }
        private static Type ISerializableType = typeof(ISerializable<object>).GetGenericTypeDefinition();
        private static byte[] Byte_0 = new byte[] { 0 };
        private static byte[] Byte_1 = new byte[] { 1 };
        private static byte[] Byte_PosN_1 = BitConverter.GetBytes(-1);
        private static byte[] Byte_PosN_2 = BitConverter.GetBytes(-2);
        public static FieldInfo Deletage_Target =
            typeof(Delegate).GetField("_target",
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy);

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

        public Serialization()
        {
            InsertSerializer<object>(
            (object obj) =>
            {
                var Type = obj.GetType();
                if (Type == typeof(object))
                {
                    S_Data.Write(Byte_0, 0, 1);
                    return;
                }
                var Serializer = FindSerializer(Type);

                S_Data.Write(Byte_1, 0, 1);
                VisitedInfoSerialize(Type.GetHashCode(), () => (Serializer.NameAsByte, null));
                Serializer.Serializer(obj);
            },
            () =>
            {
                if (D_Data[From] == 0)
                {
                    From += 1;
                    return new object();
                }
                From += 1;
                var Info = VisitedInfoDeserialize(() =>
                {
                    return Read();
                });

                return FindSerializer(Info).Deserializer();
            });

            InsertSerializer<bool>(
            (object obj) =>
            {
                if ((bool)obj == true)
                    S_Data.Write(Byte_1, 0, 1);
                else
                    S_Data.Write(Byte_0, 0, 1);
            },
            () =>
            {
                int Position = From; From += 1;
                var Result = (D_Data)[Position];
                return Result > 0;
            });

            InsertSerializer<char>(
            (object Obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((char)Obj), 0, 2);
            },
            () =>
            {
                int Position = From; From += 2;
                return BitConverter.ToChar(D_Data, Position);
            });

            InsertSerializer<byte>(
            (object Obj) =>
            {
                S_Data.Write(new byte[] { (byte)Obj }, 0, 1);
            },
            () =>
            {
                int Position = From; From += 1;
                return D_Data[Position];
            });

            InsertSerializer<sbyte>(
            (object Obj) =>
            {
                S_Data.Write(new byte[] { (byte)((sbyte)Obj) }, 0, 1);
            },
            () =>
            {
                int Position = From; From += 1;
                return (sbyte)D_Data[Position];
            });

            InsertSerializer<short>(
            (object obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((short)obj), 0, 2);
            },
            () =>
            {
                int Position = From; From += 2;
                return BitConverter.ToInt16(D_Data, Position);
            });

            InsertSerializer<ushort>(
            (object Obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((ushort)Obj), 0, 2);
            },
            () =>
            {                 /// as UInt16
                int Position = From; From += 2;
                return BitConverter.ToUInt16(D_Data, Position);
            });

            InsertSerializer<int>(
            (object obj) =>
            {                 /// as Int32
                S_Data.Write(BitConverter.GetBytes((int)obj), 0, 4);
            },
            () =>
            {                 /// as Int32
                int Position = From; From += 4;
                return BitConverter.ToInt32(D_Data, Position);
            });

            InsertSerializer<uint>(
            (object obj) =>
            {                 /// as UInt32
                S_Data.Write(BitConverter.GetBytes((uint)obj), 0, 4);
            },
            () =>
            {                 /// as UInt32
                int Position = From; From += 4;
                return BitConverter.ToUInt32(D_Data, Position);
            });

            InsertSerializer<long>(
            (object obj) =>
            {                 /// as Int64
                S_Data.Write(BitConverter.GetBytes((long)obj), 0, 8);
            },
            () =>
            {                 /// as Int64
                int Position = From; From += 8;
                return BitConverter.ToInt64(D_Data, Position);
            });

            InsertSerializer<ulong>(
            (object obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((ulong)obj), 0, 8);
            },
            () =>
            {
                int Position = From; From += 8;
                return BitConverter.ToUInt64(D_Data, Position);
            });

            InsertSerializer<float>(
            (object obj) =>
            {    /// as float
                S_Data.Write(BitConverter.GetBytes((float)obj), 0, 4);
            },
            () =>
            {    /// as float
                int Position = From; From += 4;
                return BitConverter.ToSingle(D_Data, Position);
            });

            InsertSerializer<double>(
            (object obj) =>
            {                 /// as double
                S_Data.Write(BitConverter.GetBytes((double)obj), 0, 8);
            },
            () =>
            {                 /// as double
                int Position = From; From += 8;
                return BitConverter.ToDouble(D_Data, Position);
            });

            InsertSerializer<DateTime>(
            (object obj) =>
            {
                S_Data.Write(BitConverter.GetBytes(((DateTime)obj).Ticks), 0, 8);
            },
            () =>
            {
                int Position = From; From += 8;
                return DateTime.FromBinary(BitConverter.ToInt64(D_Data, Position));
            });

            InsertSerializer<string>(
            (object obj) =>
            {
                if (obj == null)
                {
                    S_Data.Write(BitConverter.GetBytes(-1), 0, 4);
                    return;
                }
                var Str = UTF8.GetBytes((string)obj);
                var Len = BitConverter.GetBytes(Str.Length);
                S_Data.Write(Len, 0, 4);
                S_Data.Write(Str, 0, Str.Length);
            },
            () =>
            {
                var StrSize = BitConverter.ToInt32(D_Data, From);
                From += 4;
                if (StrSize == -1)
                    return null;
                var Position = From;
                From += StrSize;
                return UTF8.GetString(D_Data, Position, StrSize);
            });

            InsertSerializer<IntPtr>(
            (object obj) =>
            {                 /// as IntPtr
                S_Data.Write(BitConverter.GetBytes(((IntPtr)obj).ToInt64()), 0, 8);
            },
            () =>
            {                 /// as IntPtr
                int Position = From; From += 8;
                return new IntPtr(BitConverter.ToInt64(D_Data, Position));
            });

            InsertSerializer<UIntPtr>(
            (object obj) =>
            {                 /// as UIntPtr
                S_Data.Write(BitConverter.GetBytes(((UIntPtr)obj).ToUInt64()), 0, 8);
            },
            () =>
            {                 /// as UIntPtr
                int Position = From; From += 8;
                return new UIntPtr(BitConverter.ToUInt64(D_Data, Position));
            });

            InsertSerializer<decimal>(
            (object obj) =>
            {                 /// as Decimal
                S_Data.Write(BitConverter.GetBytes(Decimal.ToDouble((decimal)obj)), 0, 8);
            },
            () =>
            {                 /// as Decimal
                int Position = From; From += 8;
                return System.Convert.ToDecimal(BitConverter.ToDouble(D_Data, Position));
            });

            InsertSerializer<Type>(
                   (object obj) =>
                   {
                       var Name = Write(((Type)obj).MidName());
                       S_Data.Write(Name, 0, Name.Length);
                   },
                   () =>
                   {
                       return Read().GetTypeByName();
                   });

            {
                var SR = FindSerializer(typeof(object));
                InsertSerializer<System.Runtime.InteropServices.GCHandle>(
                    (object obj) =>
                    {
                        var GC = (System.Runtime.InteropServices.GCHandle)obj;
                        SR.Serializer(GC.Target);
                    },
                    () =>
                    {
                        return System.Runtime.InteropServices.GCHandle.Alloc(SR.Deserializer());
                    });
            }

            InsertSerializer<IEqualityComparer<string>>(
                (object obj) =>
                {
                    if (obj == null)
                        S_Data.Write(Byte_0, 0, 1);
                    else
                        S_Data.Write(Byte_1, 0, 1);
                },
                () =>
                {
                    if (D_Data[From++] == 0)
                        return null;
                    else
                        return EqualityComparer<string>.Default;
                });
        }

        private void Tracer(string On)
        {
            Traced += "\n >> " + On;
        }
        private void UnTracer(string On)
        {
            Traced = Traced.Substring(0, Traced.Length - (On.Length + "\n >> ".Length));
        }

        [ThreadStaticAttribute]
        private static string Traced;

        private int[] SerializersHashCodes = new int[0];
        private int[] SerializersNameCodes = new int[0];

        private SerializeInfo[] ExactSerializersByHashCode = new SerializeInfo[0];
        private SerializeInfo[] ExactSerializersByNameCode = new SerializeInfo[0];

        private LoadedFunc[] LoadedFuncs_Des = new LoadedFunc[0];
        private LoadedFunc[] LoadedFuncs_Ser = new LoadedFunc[0];

        [ThreadStaticAttribute]
        private static byte[] D_Data;
        [ThreadStaticAttribute]
        private static int From;

        [ThreadStaticAttribute]
        private static MemoryStream S_Data;
        [ThreadStaticAttribute]
        private static SortedArray<ObjectContainer> Visitor;
        [ThreadStaticAttribute]
        private static SortedArray<ObjectContainer> Visitor_info;

        public byte[] Serialize<t>(t obj)
        {
#if DEBUG
            var Result = _Serialize(obj);
            var DS = Deserialize<t>(Result);
            DS.GetType();
            return Result;
#else
            return _Serialize(obj);
#endif
        }

        private byte[] _Serialize<t>(t obj)
        {
            lock (this)
            {
                byte[] Result;
                var Type = typeof(t);

                if (Serialization.S_Data == null)
                {
                    Serialization.S_Data = new MemoryStream();
                    Serialization.Visitor = new SortedArray<ObjectContainer>(20);
                    Serialization.Visitor_info = new SortedArray<ObjectContainer>(20);
                }
                var SR = FindSerializer(Type);
                try
                {
                    VisitedSerialize(obj, SR);
                    Result = S_Data.ToArray();
                }
                catch (Exception ex)
                {
                    var Traced = Serialization.Traced;
                    if (Traced != null)
                        Traced = "On " + Traced;
                    throw new Exception($"Serialize Of Type >> {obj.GetType().FullName} Is Failed " + Traced, ex);
                }
                finally
                {
                    Serialization.Traced = null;
                    Serialization.S_Data.SetLength(0);
                    Serialization.Visitor.Clear();
                    Serialization.Visitor_info.Clear();
                }
                return Result;
            }
        }

        public t Deserialize<t>(byte[] Data)
        {
            var Type = typeof(t);
            var From = 0;
            return (t)Deserialize(Data, Type, ref From);
        }

        public t Deserialize<t>(byte[] Data, ref int From)
        {
            var Type = typeof(t);
            return (t)Deserialize(Data, Type, ref From);
        }

        public object Deserialize(byte[] Data, Type Type)
        {
            From = 0;
            return Deserialize(Data, Type, ref From);
        }

        public object Deserialize(byte[] Data, Type Type, ref int From)
        {
            lock (this)
            {
                object Result = null;
                if (Serialization.Visitor_info == null)
                {
                    Serialization.Visitor = new SortedArray<ObjectContainer>(20);
                    Serialization.Visitor_info = new SortedArray<ObjectContainer>(20);
                }
                Serialization.D_Data = Data;
                Serialization.From = From;
                try
                {
                    VisitedDeserialize((c) => Result = c, FindSerializer(Type));
                    AtLast?.Invoke();
                }
                catch (Exception ex)
                {
                    var Traced = Serialization.Traced;
                    if (Traced != null)
                        Traced = "On " + Traced;
                    throw new Exception($"Deserialize From Point {From} Of Type >> {Type.FullName} Is Failed {Traced}\nDatas As B64:\n" + System.Convert.ToBase64String(Data), ex);
                }
                finally
                {
                    Serialization.Traced = null;
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
