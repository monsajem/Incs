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
    public partial class Serialization
    {
        private class LoadedFunc :
            IEquatable<LoadedFunc>
        {
            public LoadedFunc(object Obj)
            {
                this.HashCode = Obj.GetHashCode();
                this.Obj = Obj;
            }
            private object Obj;
            private int HashCode;
            public Delegate Delegate;
            public MethodInfo Method;
            public byte[] NameAsByte;
            public SerializeInfo SerializerTarget;

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public bool Equals(LoadedFunc other)
            {
                return object.Equals(Obj,other.Obj);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public override int GetHashCode()
            {
                return HashCode;
            }
        }

    }
}