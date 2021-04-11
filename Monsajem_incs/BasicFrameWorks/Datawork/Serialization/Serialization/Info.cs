using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        private abstract class SerializeInfo:
            IEquatable<SerializeInfo>
        {
            public Type Type;
            public int TypeHashCode;
            public Func<object> Deserializer;
            public Action<object> Serializer;
            public byte[] NameAsByte;
            public bool CanStoreInVisit;
            protected bool IsMade;
            private static Type BaseType = typeof(SerializeInfo<object>).GetGenericTypeDefinition();

            public abstract void Make();


            private class ExactSerializer
            {
                public int HashCode;
                public SerializeInfo Serializer;
                public override int GetHashCode()
                {
                    return HashCode;
                }
            }

            private class ExactSerializerByType :
                ExactSerializer,IEquatable<ExactSerializerByType>
            {
                public Type Type;
                public bool Equals(ExactSerializerByType other)
                {
                    return Type.IsEquivalentTo(other.Type);
                }
            }

            private class ExactSerializerByTypeName :
                ExactSerializer,IEquatable<ExactSerializerByTypeName>
            {
                public string TypeName;
                public bool Equals(ExactSerializerByTypeName other)
                {
                    return TypeName == other.TypeName;
                }
            }

            private static HashSet<ExactSerializerByType>
                SerializersByHashCode = new HashSet<ExactSerializerByType>();
            private static HashSet<ExactSerializerByTypeName>
                SerializersByNameCode = new HashSet<ExactSerializerByTypeName>();

            public static SerializeInfo GetSerialize(Type Type)
            {
                SerializeInfo SR;
                var Key = new ExactSerializerByType()
                                { HashCode = Type.GetHashCode(), Type = Type };
                if (SerializersByHashCode.TryGetValue(Key,out var Result)==false)
                {
                    SR = (SerializeInfo)
                            BaseType.MakeGenericType(Type).GetMethod("GetSerialize").
                        Invoke(null, null);
                    Key.Serializer = SR;
                    if (SerializersByHashCode.Contains(Key) ==false)
                        SerializersByHashCode.Add(Key);
                }
                else
                    SR = Result.Serializer;
#if DEBUG
                if (SR.Type != Type)
                    throw new Exception("invalid Serializers Found!");
#endif
                return SR;
            }

            public static SerializeInfo GetSerialize(string TypeName)
            {
                SerializeInfo SR;
                var HashCode = TypeName.GetHashCode();
                var Key = new ExactSerializerByTypeName() 
                                { HashCode = HashCode ,TypeName =TypeName};
                if (SerializersByNameCode.TryGetValue(Key,out var Result) == false)
                {
                    SR = (SerializeInfo)
                            BaseType.MakeGenericType(TypeName.GetTypeByName()).GetMethod("GetSerialize").
                        Invoke(null, null);
                    Key.Serializer = SR;
                    if (SerializersByNameCode.Contains(Key) == false)
                        SerializersByNameCode.Add(Key);
                }
                else
                    SR = Result.Serializer;
#if DEBUG
                if (SR.Type != TypeName.GetTypeByName())
                    throw new Exception("invalid Serializers Found!");
#endif
                return SR;
            }

            public bool Equals(SerializeInfo other)
            {
                return Type.IsEquivalentTo(other.Type);
            }

            public override int GetHashCode()
            {
                return Type.GetHashCode();
            }
        }
    }
}