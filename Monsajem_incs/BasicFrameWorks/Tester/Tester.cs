using Monsajem_Incs.Serialization;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Monsajem_Incs.TimeingTester
{
    public static class Timing
    {
        public static TimeSpan run(Action Somthing)
        {
            static void InnerAction() { }
            Stopwatch HaveLate = new();
            HaveLate.Start();
            InnerAction();
            HaveLate.Stop();


            Stopwatch stopWatch = new();
            stopWatch.Start();
            Somthing();
            stopWatch.Stop();

            return stopWatch.Elapsed - HaveLate.Elapsed;
        }

        public static async Task<TimeSpan> run(Func<Task> Somthing)
        {
            static async Task InnerAction() { }
            Stopwatch HaveLate = new();
            HaveLate.Start();
            await InnerAction();
            HaveLate.Stop();


            Stopwatch stopWatch = new();
            stopWatch.Start();
            await Somthing();
            stopWatch.Stop();

            return stopWatch.Elapsed - HaveLate.Elapsed;
        }
    }

    public static class StreamLoger
    {
        public static System.IO.Stream Stream;
        private static int CurrentPos;
        private static int _StreamLen;
        private static int StreamLen
        {
            get => _StreamLen;
            set
            {
                _StreamLen = value;
                if (_StreamLen < MinLen)
                {
                    MaxLen = _StreamLen + minCount;
                    MinLen = _StreamLen - minCount;
                    Stream.SetLength(MaxLen);
                }
            }
        }
        private static int MaxLen = 1000;
        private static int MinLen = -1000;
        private static int minCount = 1000;
        private static int ActionsCount;
        public static void run(Action Action)
        {
            lock (Stream)
            {
                bool IsDone = false;
                var ActionByte = Action.Serialize();
                void Save()
                {
                    if (ActionsCount == 0)
                        StreamLen += 4;
                    ActionsCount += 1;
                    _ = Stream.Seek(0, System.IO.SeekOrigin.Begin);
                    Stream.Write(BitConverter.GetBytes(ActionsCount), 0, 4);

                    _ = Stream.Seek(StreamLen, System.IO.SeekOrigin.Begin);
                    StreamLen += ActionByte.Length + 5;
                    if (IsDone)
                        Stream.Write(new byte[] { 1 }, 0, 1);
                    else
                        Stream.Write(new byte[] { 0 }, 0, 1);
                    Stream.Write(BitConverter.GetBytes(ActionByte.Length), 0, 4);
                    Stream.Write(ActionByte, 0, ActionByte.Length);
                    Stream.Flush();
                }
                try
                {
                    Action();
                    IsDone = true;
                    Save();
                }
                finally
                {
                    if (IsDone == false)
                        Save();
                }
            }
        }

        private static byte[] Read(int Count)
        {
            var data = new byte[Count];
            var Pos = 0;
            while (Count > 0)
            {
                var C = Stream.Read(data, Pos, Count);
                if (C == 0)
                    throw new Exception("Stream Error");
                Count -= C;
                Pos += C;
            }
            return data;
        }
        public static void DebugStream(Action<(Action Action, bool IsSafe)> Action)
        {
            _ = Stream.Seek(0, System.IO.SeekOrigin.Begin);
            var data = Read(4);
            ActionsCount = BitConverter.ToInt32(data, 0);
            for (int i = 0; i < ActionsCount; i++)
            {
                data = Read(1);
                bool IsSafe = data[0] == 1;
                data = Read(4);
                var Len = BitConverter.ToInt32(data, 0);
                data = Read(Len);
                var Ac = data.Deserialize<Action>();
                Action((Ac, IsSafe));
            }
        }
    }
}