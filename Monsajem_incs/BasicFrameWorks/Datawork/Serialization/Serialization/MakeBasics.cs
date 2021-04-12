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
        public static Serialization Serializere;
        public Serialization()
        {
            Serializere = this;
            SerializeInfo<object>.InsertSerializer(
            (object obj) => { },
            () => new object(), false);

            SerializeInfo<bool>.InsertSerializer(
            (object obj) =>
            {
                if ((bool)obj == true)
                    S_Data.WriteByte(1);
                else
                    S_Data.WriteByte(0);
            },
            () =>
            {
                int Position = From; From += 1;
                var Result = (D_Data)[Position];
                return Result > 0;
            }, true);

            SerializeInfo<char>.InsertSerializer(
            (object Obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((char)Obj), 0, 2);
            },
            () =>
            {
                int Position = From; From += 2;
                return BitConverter.ToChar(D_Data, Position);
            }, true);

            SerializeInfo<byte>.InsertSerializer(
            (object Obj) =>
            {
                S_Data.WriteByte((byte)Obj);
            },
            () =>
            {
                int Position = From; From += 1;
                return D_Data[Position];
            }, true);

            SerializeInfo<sbyte>.InsertSerializer(
            (object Obj) =>
            {
                S_Data.WriteByte((byte)((sbyte)Obj));
            },
            () =>
            {
                int Position = From; From += 1;
                return (sbyte)D_Data[Position];
            }, true);

            SerializeInfo<short>.InsertSerializer(
            (object obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((short)obj), 0, 2);
            },
            () =>
            {
                int Position = From; From += 2;
                return BitConverter.ToInt16(D_Data, Position);
            }, true);

            SerializeInfo<ushort>.InsertSerializer(
            (object Obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((ushort)Obj), 0, 2);
            },
            () =>
            {                 /// as UInt16
                int Position = From; From += 2;
                return BitConverter.ToUInt16(D_Data, Position);
            }, true);

            SerializeInfo<int>.InsertSerializer(
            (object obj) =>
            {                 /// as Int32
                S_Data.Write(BitConverter.GetBytes((int)obj), 0, 4);
            },
            () =>
            {                 /// as Int32
                int Position = From; From += 4;
                return BitConverter.ToInt32(D_Data, Position);
            }, true);

            SerializeInfo<uint>.InsertSerializer(
            (object obj) =>
            {                 /// as UInt32
                S_Data.Write(BitConverter.GetBytes((uint)obj), 0, 4);
            },
            () =>
            {                 /// as UInt32
                int Position = From; From += 4;
                return BitConverter.ToUInt32(D_Data, Position);
            }, true);

            SerializeInfo<long>.InsertSerializer(
            (object obj) =>
            {                 /// as Int64
                S_Data.Write(BitConverter.GetBytes((long)obj), 0, 8);
            },
            () =>
            {                 /// as Int64
                int Position = From; From += 8;
                return BitConverter.ToInt64(D_Data, Position);
            }, true);

            SerializeInfo<ulong>.InsertSerializer(
            (object obj) =>
            {
                S_Data.Write(BitConverter.GetBytes((ulong)obj), 0, 8);
            },
            () =>
            {
                int Position = From; From += 8;
                return BitConverter.ToUInt64(D_Data, Position);
            }, true);

            SerializeInfo<float>.InsertSerializer(
            (object obj) =>
            {    /// as float
                S_Data.Write(BitConverter.GetBytes((float)obj), 0, 4);
            },
            () =>
            {    /// as float
                int Position = From; From += 4;
                return BitConverter.ToSingle(D_Data, Position);
            }, true);

            SerializeInfo<double>.InsertSerializer(
            (object obj) =>
            {                 /// as double
                S_Data.Write(BitConverter.GetBytes((double)obj), 0, 8);
            },
            () =>
            {                 /// as double
                int Position = From; From += 8;
                return BitConverter.ToDouble(D_Data, Position);
            }, true);

            SerializeInfo<DateTime>.InsertSerializer(
            (object obj) =>
            {
                S_Data.Write(BitConverter.GetBytes(((DateTime)obj).Ticks), 0, 8);
            },
            () =>
            {
                int Position = From; From += 8;
                return DateTime.FromBinary(BitConverter.ToInt64(D_Data, Position));
            }, true);

            SerializeInfo<string>.InsertSerializer(
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
            }, true);

            SerializeInfo<IntPtr>.InsertSerializer(
            (object obj) =>
            {                 /// as IntPtr
                S_Data.Write(BitConverter.GetBytes(((IntPtr)obj).ToInt64()), 0, 8);
            },
            () =>
            {                 /// as IntPtr
                int Position = From; From += 8;
                return new IntPtr(BitConverter.ToInt64(D_Data, Position));
            }, true);

            SerializeInfo<UIntPtr>.InsertSerializer(
            (object obj) =>
            {                 /// as UIntPtr
                S_Data.Write(BitConverter.GetBytes(((UIntPtr)obj).ToUInt64()), 0, 8);
            },
            () =>
            {                 /// as UIntPtr
                int Position = From; From += 8;
                return new UIntPtr(BitConverter.ToUInt64(D_Data, Position));
            }, true);

            SerializeInfo<decimal>.InsertSerializer(
            (object obj) =>
            {                 /// as Decimal
                S_Data.Write(BitConverter.GetBytes(Decimal.ToDouble((decimal)obj)), 0, 8);
            },
            () =>
            {                 /// as Decimal
                int Position = From; From += 8;
                return System.Convert.ToDecimal(BitConverter.ToDouble(D_Data, Position));
            }, true);

            SerializeInfo<Type>.InsertSerializer(
                   (object obj) =>
                   {
                       var Name = Write(((Type)obj).MidName());
                       S_Data.Write(Name, 0, Name.Length);
                   },
                   () =>
                   {
                       return Read().GetTypeByName();
                   }, true);

            {
                var SR = SerializeInfo<object>.GetSerialize();
                SerializeInfo<System.Runtime.InteropServices.GCHandle>.InsertSerializer(
                    (object obj) =>
                    {
                        var GC = (System.Runtime.InteropServices.GCHandle)obj;
                        SR.Serializer(GC.Target);
                    },
                    () =>
                    {
                        return System.Runtime.InteropServices.GCHandle.Alloc(SR.Deserializer());
                    }, true);
            }

            SerializeInfo<IEqualityComparer<string>>.InsertSerializer(
                (object obj) =>
                {
                    if (obj == null)
                        S_Data.WriteByte(0);
                    else
                        S_Data.WriteByte(1);
                },
                () =>
                {
                    if (D_Data[From++] == 0)
                        return null;
                    else
                        return EqualityComparer<string>.Default;
                }, true);
        }
    }
}