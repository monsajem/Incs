﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.Base
{
    public abstract partial class IArray<ArrayType> :
        IArray,IEnumerable<ArrayType>
    {

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected static void Copy(
            IArray<ArrayType> sourceArray,
            int sourceIndex,
            IArray<ArrayType> destinationArray,
            int destinationIndex,
            int length)
        {
            sourceArray.CopyTo(sourceIndex, destinationArray, destinationIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected static void Copy(
            IArray<ArrayType> sourceArray,
            int sourceIndex,
            ArrayType[] destinationArray,
            int destinationIndex,
            int length)
        {
            sourceArray.CopyTo(sourceIndex, destinationArray, destinationIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected static void Copy(
            ArrayType[] sourceArray,
            int sourceIndex,
            IArray<ArrayType> destinationArray,
            int destinationIndex,
            int length)
        {
            destinationArray.CopyFrom(sourceIndex, sourceArray, destinationIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void CopyTo(int sourceIndex, ArrayType[] destination, int destinationIndex, int Length)
        {
            for (int i = 0; i < Length; i++)
                destination[i + destinationIndex] = this[i + sourceIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void CopyTo(int sourceIndex, IArray<ArrayType> destination, int destinationIndex, int Length)
        {
            var Ar = new ArrayType[Length];
            CopyTo(sourceIndex, Ar, 0, Length);
            CopyFrom(0, Ar, destinationIndex, Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void CopyFrom(int sourceIndex, ArrayType[] sourceArray, int destinationIndex, int Length)
        {
            for (int i = 0; i < Length; i++)
                this[i + destinationIndex] = sourceArray[i + sourceIndex];
        }
    }
}