using Monsajem_Incs.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Convertors
{
    public static class ConvertorToString
    {
        internal class ExactConvertor :
            IEquatable<ExactConvertor>
        {
            public bool IsReadableConvertor;
            public int HashCode;
            public Type Type;
            public Func<string, object> ConvertorFromString;
            public Func<object, string> ConvertorToString;
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool Equals(ExactConvertor other)
            {
                return Type == other.Type;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public override int GetHashCode() => HashCode;
        }

        internal static HashSet<ExactConvertor> ExactConvertors = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static ExactConvertor GetConvertor(Type Type)
        {
        Again:
            var Key = new ExactConvertor()
            { HashCode = Type.GetHashCode(), Type = Type };
            if (ExactConvertors.TryGetValue(Key, out var Result) == false)
            {
                _ = typeof(ConvertorToString<>).MakeGenericType(Type).
                    GetMethod("Safe").Invoke(null, null);
                goto Again;
            }
            else
                return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static (Func<string, object> ConvertorFromString,
                       Func<object, string> ConvertorToString,
                       bool IsReadableConvertor)
            GetStringConvertors(this Type Type)
        {
            var Result = GetConvertor(Type);
            return (Result.ConvertorFromString, Result.ConvertorToString, Result.IsReadableConvertor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string ConvertToString(this object Value, Type Type) =>
            GetConvertor(Type).ConvertorToString(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string ConvertToString(this object Value) =>
            GetConvertor(Value.GetType()).ConvertorToString(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object ConvertFromString<t>(this string Value, Type Type) =>
            GetConvertor(Type).ConvertorFromString(Value);


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string ConvertToString<t>(this t Value) => ConvertorToString<t>.ToString(Value);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static t ConvertFromString<t>(this string Value) => ConvertorToString<t>.FromString(Value);
    }

    internal static class ConvertorToString<t>
    {
        public static void Safe() { }
        static ConvertorToString()
        {
            lock (ConvertorToString.ExactConvertors)
            {
                _ = ConvertorToString.ExactConvertors.Add(
                    new ConvertorToString.ExactConvertor()
                    {
                        ConvertorFromString = Obj_ConvertorFromString,
                        ConvertorToString = Obj_ConvertorToString,
                        HashCode = typeof(t).GetHashCode(),
                        Type = typeof(t),
                        IsReadableConvertor = IsReadable()
                    }); ;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static object Obj_ConvertorFromString(string Value) => FromString(Value);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static string Obj_ConvertorToString(object Value) => ToString((t)Value);


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static t FromString(string Value)
        {
            var t = typeof(t);
            if (t == typeof(string))
                return Unsafe.As<string, t>(ref Value);
            else if (t == typeof(Int16))
            {
                var Result = Int16.Parse(Value);
                return Unsafe.As<Int16, t>(ref Result);
            }
            else if (t == typeof(UInt16))
            {
                var Result = UInt16.Parse(Value);
                return Unsafe.As<UInt16, t>(ref Result);
            }
            else if (t == typeof(Int32))
            {
                var Result = Int32.Parse(Value);
                return Unsafe.As<Int32, t>(ref Result);
            }
            else if (t == typeof(UInt32))
            {
                var Result = UInt32.Parse(Value);
                return Unsafe.As<UInt32, t>(ref Result);
            }
            else if (t == typeof(Int64))
            {
                var Result = Int64.Parse(Value);
                return Unsafe.As<Int64, t>(ref Result);
            }
            else if (t == typeof(UInt64))
            {
                var Result = UInt64.Parse(Value);
                return Unsafe.As<UInt64, t>(ref Result);
            }
            else if (t == typeof(long))
            {
                var Result = long.Parse(Value);
                return Unsafe.As<long, t>(ref Result);
            }
            else if (t == typeof(float))
            {
                var Result = float.Parse(Value);
                return Unsafe.As<float, t>(ref Result);
            }
            else if (t == typeof(decimal))
            {
                var Result = decimal.Parse(Value);
                return Unsafe.As<decimal, t>(ref Result);
            }
            else
                return System.Convert.FromBase64String(Value).Deserialize<t>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string ToString(t Value)
        {
            var t = typeof(t);
            if (t == typeof(string))
                return Unsafe.As<t, string>(ref Value);
            else if (t == typeof(Int16))
                return Value.ToString();
            else if (t == typeof(Int32))
                return Value.ToString();
            else if (t == typeof(Int64))
                return Value.ToString();
            else if (t == typeof(UInt16))
                return Value.ToString();
            else if (t == typeof(UInt32))
                return Value.ToString();
            else if (t == typeof(UInt64))
                return Value.ToString();
            else if (t == typeof(long))
                return Value.ToString();
            else return t == typeof(float)
                ? Value.ToString()
                : t == typeof(decimal) ? Value.ToString() : System.Convert.ToBase64String(Value.Serialize<t>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool IsReadable()
        {
            var t = typeof(t);
            if (t == typeof(string))
                return true;
            else if (t == typeof(Int16))
                return true;
            else if (t == typeof(Int32))
                return true;
            else if (t == typeof(Int64))
                return true;
            else if (t == typeof(UInt16))
                return true;
            else if (t == typeof(UInt32))
                return true;
            else if (t == typeof(UInt64))
                return true;
            else if (t == typeof(long))
                return true;
            else return t == typeof(float) ? true : t == typeof(decimal);
        }
    }
}
