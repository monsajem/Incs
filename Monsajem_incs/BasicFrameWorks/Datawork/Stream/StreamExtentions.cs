using System;
using System.IO;

namespace Monsajem_Incs.Data.StreamExtentions
{
    public static class StreamExtentions
    {
        public static void Fill(this Stream Stream, byte[] Data, int From, int Length)
        {
            while (Length > 0)
            {
                var Readed = Stream.Read(Data, From, Length);
                if (Readed == 0)
                    throw new Exception("End of stream!");
                From += Readed;
                Length -= Readed;
            }
        }
        public static void Fill(this Stream Stream, byte[] Data) =>
            Stream.Fill(Data, 0, Data.Length);

    }
}
