using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {

        public static unsafe byte[] ReadBytesOfArray_T<t>((Array ar_Obj, int ElementSize) Info)
            where t : unmanaged
        {
            var ar = Unsafe.As<t[]>(Info.ar_Obj);
            byte[] bytes = new byte[ar.Length * Info.ElementSize];

            fixed (t* ptr = ar)
            {
                Marshal.Copy((IntPtr)ptr, bytes, 0, bytes.Length);
            }

            return bytes;
        }
        static Func<(Array ar_Obj, int ElementSize), byte[]>
            ReadBytesOfArray(Type ElementType)
        {
            var method = typeof(Serialization).GetMethod("ReadBytesOfArray_T");

            return method == null
                ? throw new Exception("ReadBytesOfArray_T method not found!")
                : method.MakeGenericMethod(ElementType).
                CreateDelegate<Func<(Array ar_Obj, int ElementSize), byte[]>>();
        }

        public static unsafe void WriteBytesOfArray_T<t>(
            (Array ar_Obj, byte[] BytesToWrite, int From, int Len, int ElementSize) Info)
            where t : unmanaged
        {
            var ar = Unsafe.As<t[]>(Info.ar_Obj);
            byte[] bytes = new byte[ar.Length * Info.ElementSize];

            fixed (t* ptr = ar)
            {
                Marshal.Copy(Info.BytesToWrite, Info.From, (IntPtr)ptr, Info.Len);
            }
        }
        static Action<(Array ar_Obj, byte[] BytesToWrite, int From, int Len, int ElementSize)>
            WriteBytesOfArray(Type ElementType)
        {
            var method = typeof(Serialization).GetMethod("WriteBytesOfArray_T");

            return method == null
                ? throw new Exception("WriteBytesOfArray_T method not found!")
                : method.MakeGenericMethod(ElementType).
                CreateDelegate<Action<(Array ar_Obj, byte[] BytesToWrite, int From, int Len, int ElementSize)>>();
            ;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static byte[] StructToBytes<t>(t value, int Size)
        {
            byte[] bytes = new byte[Size];
            System.Runtime.CompilerServices.Unsafe.As<byte, t>(ref bytes[0]) = value;
            return bytes;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static t BytesToStruct<t>(byte[] value, int startIndex)
        {
            return System.Runtime.CompilerServices.Unsafe.ReadUnaligned<t>(ref value[startIndex]);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private SerializeInfo WriteSerializer(SerializeData Data, Type Type)
        {
            var Sr = SerializeInfo.GetSerialize(Type);
            _ = VisitedInfoSerialize<object>(Data, Sr.Type, () => (Sr.NameAsByte, null));
            return Sr;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private SerializeInfo ReadSerializer(DeserializeData Data)
        {
            var Info = VisitedInfoDeserialize(Data, () =>
            {
                return Read(Data);
            });
            return SerializeInfo.GetSerialize(Info);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private string Read(DeserializeData Data)
        {
            var Len = BitConverter.ToInt32(Data.Data, Data.From);
            Data.From += 4;
            var Result = UTF8.GetString(Data.Data, Data.From, Len);
            Data.From += Result.Length;
            return Result;
        }
    }

}
