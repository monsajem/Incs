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
    public static class SerializationExtentions
    {
        public static readonly Serialization Serializere = new Serialization(
            (c) => c.GetCustomAttributes(typeof(NonSerializedAttribute)).Count() == 0);

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public static byte[] Serialize<t>(this t obj, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Serialize(obj,TrustToType,TrustToMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<t>(this t obj)=>Serializere.SizeOf<t>();
        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<t>()=>Serializere.SizeOf<t>();

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public static t Deserialize<t>(this byte[] Data, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Deserialize<t>(Data,TrustToType,TrustToMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public static t Deserialize<t>(this byte[] Data, ref int From, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Deserialize<t>(Data, ref From,TrustToType,TrustToMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public static t Deserialize<t>(this byte[] Data, t SampleType, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Deserialize<t>(Data,TrustToType,TrustToMethod);
        }
    }
}