using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database;
using Monsajem_Incs.Collection.Array.TreeBased;

namespace Monsajem_Incs.Collection
{

    internal struct DataHeader
    {
        public int NextData;
        public int DataLen;
    }

    internal struct Data : IComparable<Data>
    {
        public int From;
        public int To;
        public int Len;
        public int CompareTo(Data other)
        {
            return From - other.From;
        }
    }
    internal struct DataByForm : IComparable<DataByForm>
    {
        public Data Data;
        public int CompareTo(DataByForm other)
        {
            return Data.From - other.Data.From;
        }
    }
    internal struct DataByTo : IComparable<DataByTo>
    {
        public Data Data;
        public int CompareTo(DataByTo other)
        {
            return Data.To - other.Data.To;
        }
    }
    internal struct DataByLen : IComparable<DataByLen>
    {
        public Data Data;
        public int CompareTo(DataByLen other)
        {
            var c = other.Data.Len - Data.Len;
            if (c == 0)
                c = Data.From - other.Data.From;
            return c;
        }
    }

    public partial class StreamCollection :
        Array.Base.IArray<byte[], StreamCollection>
    {
        [Serialization.NonSerialized]
        internal Array<Data> Keys;
        [Serialization.NonSerialized]
        internal Array<DataByForm> GapsByFrom;
        [Serialization.NonSerialized]
        internal Array<DataByLen> GapsByLen;
        [Serialization.NonSerialized]
        internal Array<DataByTo> GapsByTo;

        private int HeadSize = new DataHeader().SizeOf();


        internal Data GetInfo(int Pos)
        {
            return Keys[Pos];
        }
        internal Data PopInfo(int Pos)
        {
            var Result = Keys[Pos];
            Keys.DeleteByPosition(Pos);
            return Result;
        }
        internal void DeleteInfo(int Pos)
        {
            Keys.DeleteByPosition(Pos);
        }
        internal bool PopGapMinLen(int Len, out Data Gap)
        {
            Gap = default;
            if (GapsByLen.Length == 0)
                return false;
            var Finded = GapsByLen.FirstOrDefault();
            if (Finded.Data.Len < Len)
                return false;
            Gap = Finded.Data;
            DeleteGap(Gap);
            return true;
        }

        internal bool PopNextGap(ref Data Data)
        {
            var Result = GapsByFrom.BinarySearch(new DataByForm()
            {
                Data = new Data() { From = Data.To + 1 }
            });
            if (Result.Index > -1)
            {
                Data = Result.Value.Data;
                DeleteGap(Data);
                return true;
            }
            return false;
        }
        internal bool PopBeforeGap(ref Data Data)
        {
            var Result = GapsByTo.BinarySearch(new DataByTo()
            {
                Data = new Data() { To = Data.From - 1 }
            });
            if (Result.Index > -1)
            {
                Data = Result.Value.Data;
                DeleteGap(Data);
                return true;
            }
            return false;
        }

        internal void InsertGap(Data Gap)
        {
            GapsByFrom.BinaryInsert(new DataByForm() { Data = Gap });
            GapsByLen.BinaryInsert(new DataByLen() { Data = Gap });
            GapsByTo.BinaryInsert(new DataByTo() { Data = Gap });
        }
        internal void DeleteGap(Data Gap)
        {
#if DEBUG
            if (
#endif
            GapsByFrom.BinaryDelete(new DataByForm() { Data = Gap })
#if DEBUG
                   .Index < 0) throw new Exception()
#endif
                    ;
#if DEBUG
            if (
#endif
            GapsByLen.BinaryDelete(new DataByLen() { Data = Gap })
#if DEBUG
                  .Index < 0) throw new Exception()
#endif
                        ;
#if DEBUG
            if (
#endif
            GapsByTo.BinaryDelete(new DataByTo() { Data = Gap })
#if DEBUG
                  .Index < 0) throw new Exception()
#endif
                        ;
        }
#if DEBUG
        private void Debug(StreamCollection Parent)
        {

            {
                // Debug Keys and gaps

                if (Parent.Length != this.Keys.Length)
                    throw new Exception("miss match Len");
                if (GapsByFrom.Where((c) =>
                     GapsByLen.Contains(new DataByLen() { Data = c.Data }) == false).
                    Count() > 0)
                    throw new Exception();
                var Datas = new List<(string Role, Data Data)>(
                    GapsByFrom.Select((c) => ("isGap", c.Data)));
                foreach (var Key in this.Keys)
                    Datas.Add(("key", Key));
                var Keys = Datas.OrderBy((c) => c.Data.From).ToArray();
                for (int i = 0; i < Keys.Length; i++)
                {
                    Debug(i, Keys);
                }
            }

            {
                // Debug Stream Keys
                var NextPos = BitConverter.ToInt32(Stream.GetHeader());

                NextPos--;

                if (Keys.Length == 0 && NextPos != -1)
                    throw new Exception();

                if (Keys.Length > 0 && NextPos == -1)
                    throw new Exception();

                for (int i = 0; i < Keys.Length; i++)
                {
                    if (Keys[i].From != NextPos)
                        throw new Exception();

                    Stream.Seek(NextPos, System.IO.SeekOrigin.Begin);
                    var Header = Stream.Read(HeadSize).Deserialize<DataHeader>();

                    if (Keys[i].Len != Header.DataLen)
                        throw new Exception();

                    NextPos = Header.NextData;
                }

                if (NextPos != -1)
                    throw new Exception();
            }
        }
        private void Debug(
            int i,
            (string Role, Data Data)[] Keys)
        {
            var Key = Keys[i];
            if (Key.Data.To >= Stream.Length)
                throw new Exception();
            if (Key.Data.Len != (Key.Data.To - Key.Data.From) + 1)
                throw new Exception();
            if (Keys.Length > i + 1)
            {
                var NextKey = Keys[i + 1];
                if (Key.Data.To + 1 != NextKey.Data.From)
                    throw new Exception("Conflict Key place and NextKey Place!");
                if (Key.Role == "isGap" & NextKey.Role == "isGap")
                    throw new Exception("Gap is in wrong place!");
            }
            else
            {
                if (Key.Data.To != Stream.Length - 1)
                    throw new Exception();
            }
        }
#endif

    }
}