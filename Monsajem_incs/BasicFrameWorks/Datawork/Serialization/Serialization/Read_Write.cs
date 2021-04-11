using System;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        private SerializeInfo WriteSerializer(Type Type)
        {
            var Sr = SerializeInfo.GetSerialize(Type);
            VisitedInfoSerialize<object>(Sr.Type, () => (Sr.NameAsByte, null));
            return Sr;
        }

        private SerializeInfo ReadSerializer()
        {
            var Info = VisitedInfoDeserialize(() =>
            {
                return Read();
            });
            return SerializeInfo.GetSerialize(Info);
        }


        private byte[] Write(params string[] str)
        {
            byte[] Results = new byte[0];
            for (int i = 0; i < str.Length; i++)
            {
                var UTF8_Data = UTF8.GetBytes(str[i]);
                var Result = new byte[UTF8_Data.Length + 4];
                System.Array.Copy(BitConverter.GetBytes(UTF8_Data.Length), 0, Result, 0, 4);
                System.Array.Copy(UTF8_Data, 0, Result, 4, UTF8_Data.Length);
                Insert(ref Results, Result);
            }
            return Results;
        }

        private string Read()
        {
            var Len = BitConverter.ToInt32(D_Data, From);
            From += 4;
            var Result = UTF8.GetString(D_Data, From, Len);
            From += Result.Length;
            return Result;
        }
    }

}
