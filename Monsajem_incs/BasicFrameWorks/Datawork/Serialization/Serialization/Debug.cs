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
            private static void Check_SR(SerializeData Data)
            {
                var Type = typeof(t);
                Data.Data.Write(BitConverter.GetBytes(Data.Data.Length), 0, 8);
                var TypeBytes = Serializere.Write(Type.MidName());
                Data.Data.Write(TypeBytes, 0, TypeBytes.Length);
            }
            private static void Check_DR(DeserializeData Data)
            {
                var Type = typeof(t);
                var DR_Pos = Data.From;
                var SR_Pos = BitConverter.ToInt64(Data.Data, Data.From);
                Data.From += 8;
                if (DR_Pos != SR_Pos ||
                    SR_Pos < 0)
                    throw new Exception($"Position Isn't Valid. SR:{SR_Pos} , DR:{DR_Pos}");
                var TypeName = Serializere.Read(Data);
                var SR_Type = TypeName.GetTypeByName();
                if (SR_Type != Type)
                    throw new Exception($"Type isn't match\nSR: {SR_Type.MidName()}\nDR: {Type.MidName()}");
            }

            private static void Tracer(Data Data,string On)
            {
                Data.Traced += "\n >> " + On;
            }
            private static void UnTracer(Data Data,string On)
            {
                var Traced = Data.Traced;
                Traced = Traced.Substring(0, Traced.Length - (On.Length + "\n >> ".Length));
                Data.Traced = Traced;
            }

#endif

        }
    }
}