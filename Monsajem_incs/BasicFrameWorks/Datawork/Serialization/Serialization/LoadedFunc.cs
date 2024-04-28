using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        private class LoadedFunc :
            IEquatable<LoadedFunc>
        {
            public LoadedFunc(object Obj)
            {
                HashCode = Obj.GetHashCode();
                this.Obj = Obj;
            }
            private object Obj;
            private int HashCode;
            public Delegate Delegate;
            public MethodInfo Method;
            public byte[] NameAsByte;
            public SerializeInfo SerializerTarget;

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public bool Equals(LoadedFunc other)
            {
                return object.Equals(Obj, other.Obj);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode()
            {
                return HashCode;
            }
        }

    }
}