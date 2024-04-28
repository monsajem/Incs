using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Convertors
{
    public static class ConvertorFromTo
    {
        internal struct ExactConvertor :
            IEquatable<ExactConvertor>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public ExactConvertor(Type FromType, Type ToType)
            {
                this.FromType = FromType;
                this.ToType = ToType;
                HashCode = FromType.GetHashCode() + ToType.GetHashCode();
                Convertor = null;
            }

            public int HashCode;
            public Type FromType;
            public Type ToType;
            public Func<object, object> Convertor;
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool Equals(ExactConvertor other)
            {
                return FromType == other.FromType &&
                       ToType == other.ToType;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public override int GetHashCode() => HashCode;
        }

        internal static HashSet<ExactConvertor> ExactConvertors = [];

        internal static SortedDictionary<Type, MethodInfo> GenericConvertor =
            new(
                    Comparer<Type>.Create((c1, c2) => c1.FullName.CompareTo(c2.FullName)));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static ExactConvertor _GetConvertor(Type FromType, Type ToType)
        {

        Again:
            var Key = new ExactConvertor(FromType, ToType);
            if (ExactConvertors.TryGetValue(Key, out var Result) == false)
            {
                var ConvertorType = typeof(Convertor<,>).MakeGenericType(FromType, ToType);
                _ = ConvertorType.GetMethod("Safe").Invoke(null, null);
                goto Again;
            }
            else
                return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Func<object, object> GetConvertor(Type FromType, Type ToType)
        {
            var Result = _GetConvertor(FromType, ToType);
            return Result.Convertor;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ToType Convert<FromType, ToType>(this FromType Value) => Convertor<FromType, ToType>._Convertor(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object Convert(this object Value, Type FromType, Type ToType) => GetConvertor(FromType, ToType)(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ToType Convert<ToType>(this object Value) => ConvertorTo<ToType>.Convert(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object Convert(this object Value, Type ToType) => Convert(Value, Value.GetType(), ToType);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void RegisterConvertor<FromType, ToType>(Func<FromType, ToType> Convertor) =>
            Convertor<FromType, ToType>._Convertor = Convertor;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void RegisterConvertor(Type FromGenericType, MethodInfo GenericMethod)
        {
            if (FromGenericType == null)
                throw new Exception($"{nameof(FromGenericType)} is null.");
            if (GenericMethod == null)
                throw new Exception($"{nameof(GenericMethod)} is null.");
            if (FromGenericType.IsGenericTypeDefinition == false && FromGenericType.IsArray == false)
                throw new Exception($"{nameof(FromGenericType)} must be a generic type definition or array of object.");
            if (FromGenericType.IsArray == true && FromGenericType.GetElementType() != typeof(object))
                throw new Exception($"{nameof(FromGenericType)} must be a generic type definition or array of object.");

            GenericConvertor.Add(FromGenericType, GenericMethod);
        }
    }


    internal struct Convertor<FromType, ToType>
    {
        public static void Safe()
        {
            var FromType = typeof(FromType);
            if (FromType.IsGenericType)
            {
                var GenericArguments = FromType.GetGenericArguments();
                FromType = FromType.GetGenericTypeDefinition();
                if (ConvertorFromTo.GenericConvertor.ContainsKey(FromType))
                {
                    var Convertor = ConvertorFromTo.GenericConvertor[FromType];
                    _Convertor = Convertor.MakeGenericMethod(GenericArguments).
                                           CreateDelegate<Func<FromType, ToType>>();
                }
            }
            else if (FromType.IsArray)
            {
                var GenericArgument = FromType.GetElementType();
                FromType = Array.CreateInstance(typeof(object), new int[FromType.GetArrayRank()]).GetType();
                if (ConvertorFromTo.GenericConvertor.ContainsKey(FromType))
                {
                    var Convertor = ConvertorFromTo.GenericConvertor[FromType];
                    _Convertor = Convertor.MakeGenericMethod(GenericArgument).
                                           CreateDelegate<Func<FromType, ToType>>();
                }
            }
            lock (ConvertorFromTo.ExactConvertors)
            {
                _ = ConvertorFromTo.ExactConvertors.Add(
                    new ConvertorFromTo.ExactConvertor(typeof(FromType), typeof(ToType))
                    {
                        Convertor = Obj_Convertor
                    });
            }
            lock (ConvertorTo<ToType>.ExactConvertors)
            {
                _ = ConvertorTo<ToType>.ExactConvertors.Add(
                    new ConvertorTo<ToType>.ExactConvertor(typeof(FromType))
                    {
                        Convertor = To_Convertor
                    });
            }
        }

        public static Func<FromType, ToType> _Convertor = (c) =>
              throw new InvalidCastException(
                  $"Convertor not found for from type '{typeof(FromType)}' to type '{typeof(ToType)}'");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static object Obj_Convertor(object Value) => _Convertor((FromType)Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static ToType To_Convertor(object Value) => _Convertor((FromType)Value);
    }

    internal struct ConvertorTo<ToType>
    {
        internal struct ExactConvertor :
            IEquatable<ExactConvertor>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public ExactConvertor(Type FromType)
            {
                this.FromType = FromType;
                HashCode = FromType.GetHashCode();
                Convertor = null;
            }

            public int HashCode;
            public Type FromType;
            public Func<object, ToType> Convertor;
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool Equals(ExactConvertor other)
            {
                return FromType == other.FromType;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public override int GetHashCode() => FromType.GetHashCode();
        }

        internal static HashSet<ExactConvertor> ExactConvertors = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static ExactConvertor _GetConvertor(Type FromType)
        {
        Again:
            var Key = new ExactConvertor(FromType);
            if (ExactConvertors.TryGetValue(Key, out var Result) == false)
            {
                var ConvertorType = typeof(Convertor<,>).MakeGenericType(FromType, typeof(ToType));
                _ = ConvertorType.GetMethod("Safe").Invoke(null, null);
                goto Again;
            }
            else
                return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ToType Convert(object Value, Type FromType) => _GetConvertor(FromType).Convertor(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ToType Convert(object Value) => Convert(Value, Value.GetType());
    }
}
