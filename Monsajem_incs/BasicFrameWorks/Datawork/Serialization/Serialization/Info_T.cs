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
        private partial class SerializeInfo<t> : SerializeInfo
        {
            public static Func<object> _Deserializer;
            public static Action<object> _Serializer;

            public SerializeInfo()
            {
                Type = typeof(t);
                TypeHashCode = Type.GetHashCode();

                var TypeName = Type.MidName();
                var Name = UTF8.GetBytes(TypeName);
                NameAsByte = BitConverter.GetBytes(Name.Length);
                Insert(ref NameAsByte, Name);

                CanStoreInVisit =
                    !(Type == typeof(Delegate) |
                      Type.BaseType == typeof(MulticastDelegate) |
                      Type.IsPrimitive);
            }

            private static readonly SerializeInfo<t> SerializerObj = new SerializeInfo<t>();
            public static SerializeInfo<t> GetSerialize()
            {
                var Sr = SerializerObj;
                if (Sr.IsMade == false)
                {
                    Sr.IsMade = true;
                    if (_Serializer == null)
                        Sr.Make();
                    else
                    {
                        Sr.Serializer = _Serializer;
                        Sr.Deserializer = _Deserializer;
                    }
                }
                return Sr;
            }
        }
    }
}