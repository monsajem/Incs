using Monsajem_Incs.Collection.Array.TreeBased;

namespace Monsajem_Incs.Collection
{

    public partial class StreamDictionary<KeyType, ValueType> :
        Dictionary<KeyType, ValueType>
    {
        public new Array<KeyType> Keys { get => (Array<KeyType>)base.Keys; set => base.Keys = value; }
        public new StreamCollection<ValueType> Values { get => (StreamCollection<ValueType>)base.Values; set => base.Values = value; }
        public StreamDictionary(System.IO.Stream Stream, int MinCount = 5000) :
            base(new Array<KeyType>(),
                 new StreamCollection<ValueType>(Stream, MinCount))
        { }
    }
}