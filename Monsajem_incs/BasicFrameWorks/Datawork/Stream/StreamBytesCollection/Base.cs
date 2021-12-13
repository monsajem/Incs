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

    public partial class StreamCollection:
        Array.Base.IArray<byte[], StreamCollection>
    { 
        [Serialization.NonSerialized]
        [CopyOrginalObject]
        private Monsajem_Incs.DataWork.StreamController Stream;
        
        public StreamCollection(System.IO.Stream Stream,int MinCount = 5000)
        {
            this.Stream = new DataWork.StreamController(Stream,4,MinCount);
            {
                Keys = new Array<Data>();
                GapsByFrom = new Array<DataByForm>();
                GapsByLen = new Array<DataByLen>();
                GapsByTo = new Array<DataByTo>();

                var NextPos = BitConverter.ToInt32(this.Stream.GetHeader());

                NextPos--;

                if(NextPos!=-1)
                {
                    while (NextPos!=-1)
                    {
                        this.Stream.Seek(NextPos, System.IO.SeekOrigin.Begin);
                        var Header =this.Stream.Read(HeadSize).Deserialize<DataHeader>();
                        Keys.Insert(new Data()
                        {
                            From = NextPos,
                            Len = Header.DataLen,
                            To = NextPos + Header.DataLen
                        });
                        NextPos = Header.NextData;
                    }

                    Length = Keys.Length;

                    var NewDatas = new Array<Data>();
                    NewDatas.BinaryInsert(Keys.ToArray());
                    var CurrentPos = 0;
                    for (int i = 0; i < NewDatas.Length; i++)
                    {
                        var NewData = NewDatas[i];
                        if (CurrentPos != NewData.From)
                        {
                            InsertGap(new Data()
                            {
                                From = CurrentPos,
                                Len = (NewData.From - CurrentPos),
                                To = NewData.From - 1
                            });
                        }
                        CurrentPos = NewData.To + 1;
                    }
                }
            }

            Keys.ChangedNextSequence = (c) =>
            {
                if(c.Before==null)
                {
                    if(Keys.Length>0)
                        this.Stream.SetHeader(BitConverter.GetBytes(Keys[0].From + 1));
                    else
                        this.Stream.SetHeader(BitConverter.GetBytes(0));
                }
                else
                {
                    var Data = c.Before.Value;
                    var Header = new DataHeader();
                    Header.DataLen = Data.Len;
                    if (c.Next == null)
                        Header.NextData = -1;
                    else
                        Header.NextData = c.Next.Value.From;
                    var Stream = this.Stream;
                    Stream.Seek(Data.From, System.IO.SeekOrigin.Begin);
                    Stream.Write(Header.Serialize(), 0, HeadSize);
                }
            };
        }

        public override object MyOptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
