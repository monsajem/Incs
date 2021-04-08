using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.Base
{
    public abstract partial class IArray<ArrayType>
    {

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftEnd(int Count) =>
            shiftEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftBegin(int Count) =>
            shiftBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollEnd(int Count) =>
            shiftRollEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollBegin(int Count) =>
            shiftRollBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraEnd(int Count) =>
            shiftExtraEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraBegin(int Count) =>
            shiftExtraBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftEndFrom(int From, int Count) =>
            shiftEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftBeginFrom(int From, int Count) =>
            shiftBegin(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollEndFrom(int From, int Count) =>
            shiftRollEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollBeginFrom(int From, int Count) =>
            shiftRollBegin(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraEndFrom(int From, int Count) =>
            shiftExtraEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraBeginFrom(int From, int Count) =>
            shiftExtraBegin(From, Length - 1, Count);


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            _shiftEnd(From, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void _shiftEnd(int From, int Count, int ArLen)
        {
            if (Count < 1)
            {
#if DEBUG
                if (Count < 0)
                    throw new ArgumentOutOfRangeException("Count must be equal or greater that zero.");
#endif
                return;
            }
            Copy(this, From, this, From + Count, ArLen - Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            _shiftBegin(From, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void _shiftBegin(int From, int Count, int ArLen)
        {
            if (Count < 1)
            {
#if DEBUG
                if (Count < 0)
                    throw new ArgumentOutOfRangeException("Count must be equal or greater that zero.");
#endif
                return;
            }
            Copy(this, From + Count, this, From, ArLen - Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
            {
                var Roll = new ArrayType[Count];
                Copy(this, From, Roll, 0, Count);
                return Roll;
            }
            return _shiftExtraEnd(From, To, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private ArrayType[] _shiftExtraEnd(int From, int To, int Count, int ArLen)
        {
            if (Count < 1)
            {
#if DEBUG
                if (Count < 0)
                    throw new ArgumentOutOfRangeException("Count must be equal or greater that zero.");
#endif
                return new ArrayType[0];
            }
            var Roll = new ArrayType[Count];
            Copy(this, To + 1 - Count, Roll, 0, Count);
            _shiftEnd(From, Count, ArLen);
            return Roll;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
            {
                var Roll = new ArrayType[Count];
                Copy(this, From, Roll, 0, Count);
                return Roll;
            }
            return _shiftExtraBegin(From, To, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private ArrayType[] _shiftExtraBegin(int From, int To, int Count, int ArLen)
        {
            if (Count < 1)
            {
#if DEBUG
                if (Count < 0)
                    throw new ArgumentOutOfRangeException("Count must be equal or greater that zero.");
#endif
                return new ArrayType[0];
            }
            var Roll = new ArrayType[Count];
            Copy(this, From, Roll, 0, Count);
            _shiftBegin(From, Count, ArLen);
            return Roll;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollEnd(int From, int To, int Count)
        {
            if (Count < 1)
            {
#if DEBUG
                if (Count < 0)
                    throw new ArgumentOutOfRangeException("Count must be equal or greater that zero.");
#endif
                return;
            }
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            var Roll = _shiftExtraEnd(From, To, Count, ArLen);
            Copy(Roll, 0, this, From, Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollBegin(int From, int To, int Count)
        {
            if (Count < 1)
            {
#if DEBUG
                if (Count < 0)
                    throw new ArgumentOutOfRangeException("Count must be equal or greater that zero.");
#endif
                return;
            }
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            var Roll = _shiftExtraBegin(From, To, Count, ArLen);
            To = To + 1 - Count;
            Copy(Roll, 0, this, To, Count);
        }
    }
}