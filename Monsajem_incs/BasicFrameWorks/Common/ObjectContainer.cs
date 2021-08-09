using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_incs
{
    public struct ValueHolder<t>
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
            get =>_Value;
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { _Value = value; _HaveValue = true; } 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator ValueHolder<t> (t Value)
        {
            return new ValueHolder<t>() { Value = Value };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator t(ValueHolder<t> ValueHolder)
        {
            if (ValueHolder.HaveValue)
                return ValueHolder.Value;
            else
                return default(t);
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
            if (Value == null && obj == null)
                return true;
            return Value.Equals(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override string ToString() => Value.ToString();
        
    }
}