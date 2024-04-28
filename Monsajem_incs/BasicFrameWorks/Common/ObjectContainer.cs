using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs
{
    public class ValueHolder<t>
    {
        private bool _HaveValue;
        public bool HaveValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _HaveValue;
        }

        private t _Value;
        public t Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _Value;
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { _Value = value; _HaveValue = true; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator ValueHolder<t>(t Value)
        {
            return new ValueHolder<t>() { Value = Value };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator t(ValueHolder<t> ValueHolder) => ValueHolder.ConvertToT();

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private t ConvertToT()
        {
            return Value;
            return HaveValue ? Value : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ValueHolder<t> operator <<(ValueHolder<t> Value, int Count) =>
            ((dynamic)(t)Value) << Count;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ValueHolder<t> operator >>(ValueHolder<t> Value, int Count) =>
            ((dynamic)(t)Value) >> Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator +(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) + b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator -(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) - b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator *(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) * b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator /(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) / b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator %(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) % b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator ^(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) ^ b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator &(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) & b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator |(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) & b;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator +(dynamic a, ValueHolder<t> b) =>
            a + ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator -(dynamic a, ValueHolder<t> b) =>
            a - ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator *(dynamic a, ValueHolder<t> b) =>
            a * ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator /(dynamic a, ValueHolder<t> b) =>
            a / ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator %(dynamic a, ValueHolder<t> b) =>
            a % ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator ^(dynamic a, ValueHolder<t> b) =>
            a ^ ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator &(dynamic a, ValueHolder<t> b) =>
            a & ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator |(dynamic a, ValueHolder<t> b) =>
            a & ((dynamic)(t)b);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) == b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) != b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <=(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) <= b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >=(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) >= b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) < b;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >(ValueHolder<t> a, dynamic b) =>
            ((dynamic)(t)a) > b;


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(dynamic a, ValueHolder<t> b) =>
            a == ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(dynamic a, ValueHolder<t> b) =>
            a != ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <=(dynamic a, ValueHolder<t> b) =>
            a <= ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >=(dynamic a, ValueHolder<t> b) =>
            a >= ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <(dynamic a, ValueHolder<t> b) =>
            a < ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >(dynamic a, ValueHolder<t> b) =>
            a > ((dynamic)(t)b);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator +(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) + ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator -(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) - ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator *(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) * ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator /(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) / ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator %(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) % ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator ^(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) ^ ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator &(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) & ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static dynamic operator |(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) | ((dynamic)(t)b);


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) == ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) != ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <=(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) <= ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >=(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) >= ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) < ((dynamic)(t)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >(ValueHolder<t> a, ValueHolder<t> b) =>
            ((dynamic)(t)a) > ((dynamic)(t)b);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override bool Equals(object obj)
        {
            return Value == null && obj == null ? true : Value.Equals(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override string ToString() => Value.ToString();

        public static CallSite<Func<CallSite, input1, input2, object>>
            MakeAction<input1, input2>(
            ExpressionType OpType,
            CSharpArgumentInfoFlags Op1Flag = CSharpArgumentInfoFlags.None,
            CSharpArgumentInfoFlags Op2Flag = CSharpArgumentInfoFlags.None)
        {
            Type typeFromHandle = typeof(ValueHolder<t>);
            CSharpArgumentInfo[] array = new CSharpArgumentInfo[2];

            array[0] = Op1Flag == CSharpArgumentInfoFlags.None && typeof(input1) != typeof(object)
                ? CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                : CSharpArgumentInfo.Create(Op1Flag, null);

            array[1] = Op2Flag == CSharpArgumentInfoFlags.None && typeof(input2) != typeof(object)
                ? CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                : CSharpArgumentInfo.Create(Op2Flag, null);

            return CallSite<Func<CallSite, input1, input2, object>>
                        .Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(
                                                CSharpBinderFlags.None,
                                                OpType,
                                                typeFromHandle,
                                                array));
        }

    }
}