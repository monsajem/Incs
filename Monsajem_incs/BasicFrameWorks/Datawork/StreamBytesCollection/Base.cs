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
        Array.Base.IArray<byte[], StreamCollection>,
        ISerializable<StreamCollection.MyData>
    { 
        [Serialization.NonSerialized]
        [CopyOrginalObject]
        public System.IO.Stream Stream;
        
        public StreamCollection(int MinCount = 5000)
        {
            Info.minCount = MinCount;
            Info.MinLen = -1 * Info.minCount;
            Info.MaxLen = Info.minCount;
            Info.Keys = new Array<Data>();
            Info.GapsByFrom = new Array<DataByForm>();
            Info.GapsByLen = new Array<DataByLen>();
            Info.GapsByTo = new Array<DataByTo>();
        }

        public override object MyOptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void DeleteLen(int Count)
        {
            Info.StreamLen = Info.StreamLen - Count;
            if (Info.StreamLen < Info.MinLen)
            {
                Info.MaxLen = Info.StreamLen + Info.minCount;
                Info.MinLen = Info.StreamLen - Info.minCount;
                Stream.SetLength(Info.MaxLen);
            }
        }
        private void AddLen(long Count)
        {
            Info.StreamLen = Info.StreamLen + Count;
            if (Info.StreamLen > Info.MaxLen)
            {
                Info.MaxLen = Info.StreamLen + Info.minCount;
                Info.MinLen = Info.StreamLen - Info.minCount;
                Stream.SetLength(Info.MaxLen);
            }
        }

        MyData ISerializable<MyData>.GetData()
        {
            return Info;
        }

        void ISerializable<MyData>.SetData(MyData Data)
        {
            Info = Data;
            this.Length = Info.Keys.Length;
        }
    }
}
