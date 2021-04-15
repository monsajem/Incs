﻿using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
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
    public static class SerializationExtentions
    {
        public static readonly Serialization Serializere = new Serialization(
            (c) => c.GetCustomAttributes(typeof(NonSerializedAttribute)).Count() == 0);

        public static byte[] Serialize<t>(this t obj, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Serialize(obj,TrustToType,TrustToMethod);
        }

        public static t Deserialize<t>(this byte[] Data, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Deserialize<t>(Data,TrustToType,TrustToMethod);
        }

        public static t Deserialize<t>(this byte[] Data, ref int From, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Deserialize<t>(Data, ref From,TrustToType,TrustToMethod);
        }

        public static t Deserialize<t>(this byte[] Data, t SampleType, 
            Action<Type> TrustToType = null,
            Action<MethodInfo> TrustToMethod = null)
        {
            return Serializere.Deserialize<t>(Data,TrustToType,TrustToMethod);
        }
    }
}