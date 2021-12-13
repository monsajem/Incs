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

    public partial class StreamCollection<ValueType>:
        Array.Base.IArray<ValueType, StreamCollection<ValueType>>,
        ISerializable<StreamCollection>
    { 
        public StreamCollection Collection;
        
        public StreamCollection(System.IO.Stream Stream, int MinCount = 5000)
        {
            Collection = new StreamCollection(Stream,MinCount);
        }

        public override ValueType this[int Pos] { 
            get =>Collection[Pos].Deserialize<ValueType>(); 
            set =>Collection[Pos]=value.Serialize(); }

        public override object MyOptions { get =>null; set { } }

        public override void DeleteByPosition(int Position) =>
            Collection.DeleteByPosition(Position);

        public override void Insert(ValueType Value, int Position) =>
            Collection.Insert(Value.Serialize(), Position);

        public StreamCollection GetData()
        {
            return Collection; ;
        }

        public void SetData(StreamCollection Data)
        {
            this.Collection = Data;
        }

        public override int Length { get => Collection.Length; protected set { } }

        protected override StreamCollection<ValueType> MakeSameNew()
        {
            throw new NotImplementedException();
        }
    }
}