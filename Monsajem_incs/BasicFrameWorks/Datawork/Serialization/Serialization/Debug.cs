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

#if DEBUG
            private static void Check_SR()
            {
                var Type = typeof(t);
                S_Data.Write(BitConverter.GetBytes(S_Data.Length), 0, 8);
                var TypeBytes = Serializere.Write(Type.MidName());
                S_Data.Write(TypeBytes, 0, TypeBytes.Length);
            }
            private static void Check_DR()
            {
                var Type = typeof(t);
                var DR_Pos = From;
                var SR_Pos = BitConverter.ToInt64(D_Data, From);
                From += 8;
                if (DR_Pos != SR_Pos ||
                    SR_Pos < 0)
                    throw new Exception($"Position Isn't Valid. SR:{SR_Pos} , DR:{DR_Pos}");
                var TypeName = Serializere.Read();
                var SR_Type = TypeName.GetTypeByName();
                if (SR_Type != Type)
                    throw new Exception($"Type isn't match\nSR: {SR_Type.MidName()}\nDR: {Type.MidName()}");
            }

            private static void Tracer(string On)
            {
                Traced += "\n >> " + On;
            }
            private static void UnTracer(string On)
            {
                Traced = Traced.Substring(0, Traced.Length - (On.Length + "\n >> ".Length));
            }

#endif

        }
    }
}