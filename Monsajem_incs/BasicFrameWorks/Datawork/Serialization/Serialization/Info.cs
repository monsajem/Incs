﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {

        private abstract class SerializeInfo :
            IEquatable<SerializeInfo>
        {
            public Type Type;
            public int TypeHashCode;
            public Func<DeserializeData, object> Deserializer;
            public Action<SerializeData, object> Serializer;
            public byte[] NameAsByte;
            public bool CanStoreInVisit;
            public int ConstantSize;
            protected bool IsMade;
            protected bool IsMading;

            public abstract void Make();

            public abstract void ArraySerializer(SerializeData Data, System.Array ar);
            public abstract void ArrayDeserializer(DeserializeData Data, System.Array ar);


            private class ExactSerializer
            {
                public int HashCode;
                public SerializeInfo Serializer;
                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public override int GetHashCode()
                {
                    return HashCode;
                }
            }

            private class ExactSerializerByType :
                ExactSerializer, IEquatable<ExactSerializerByType>
            {
                public Type Type;
                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public bool Equals(ExactSerializerByType other)
                {
                    return Type.IsEquivalentTo(other.Type);
                }
            }

            private class ExactSerializerByTypeName :
                ExactSerializer, IEquatable<ExactSerializerByTypeName>
            {
                public string TypeName;
                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public bool Equals(ExactSerializerByTypeName other)
                {
                    return TypeName == other.TypeName;
                }
            }

            private static HashSet<ExactSerializerByType>
                SerializersByHashCode = [];
            private static HashSet<ExactSerializerByTypeName>
                SerializersByNameCode = [];

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public static SerializeInfo GetSerialize(Type Type)
            {
            Again:
                SerializeInfo SR;
                var Key = new ExactSerializerByType()
                { HashCode = Type.GetHashCode(), Type = Type };
                if (SerializersByHashCode.TryGetValue(Key, out var Result) == false)
                {
                    lock (SerializersByHashCode)
                    {
                        if (SerializersByHashCode.TryGetValue(Key, out Result))
                            goto Again;
                        SR = (SerializeInfo)
                            typeof(SerializeInfo<>).MakeGenericType(Type).GetMethod("GetSerialize").
                        Invoke(null, null);
                        Key.Serializer = SR;
                        if (SerializersByHashCode.Contains(Key) == false)
                            SerializersByHashCode.Add(Key);
                    }
                }
                else
                    SR = Result.Serializer;
#if DEBUG
                if (SR.Type != Type)
                    throw new Exception("invalid Serializers Found!");
#endif
                return SR;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public static SerializeInfo GetSerialize(string TypeName)
            {
            Again:
                SerializeInfo SR;
                var HashCode = TypeName.GetHashCode();
                var Key = new ExactSerializerByTypeName()
                { HashCode = HashCode, TypeName = TypeName };
                if (SerializersByNameCode.TryGetValue(Key, out var Result) == false)
                {
                    lock (SerializersByNameCode)
                    {
                        if (SerializersByNameCode.TryGetValue(Key, out Result))
                            goto Again;
                        SR = (SerializeInfo)
                            typeof(SerializeInfo<>).MakeGenericType(Assembly.Assembly.GetType(TypeName)).GetMethod("GetSerialize").
                        Invoke(null, null);
                        Key.Serializer = SR;
                        if (SerializersByNameCode.Contains(Key) == false)
                            SerializersByNameCode.Add(Key);
                    }
                }
                else
                    SR = Result.Serializer;
#if DEBUG
                if (SR.Type != Assembly.Assembly.GetType(TypeName))
                    throw new Exception("invalid Serializers Found!");
#endif
                return SR;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public bool Equals(SerializeInfo other)
            {
                return Type.IsEquivalentTo(other.Type);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode()
            {
                return Type.GetHashCode();
            }
        }
    }
}