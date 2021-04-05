using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database;
using Monsajem_Incs.Collection.Array.TreeBased;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Collection.Array.Base;

namespace Monsajem_Incs.Collection
{

    public partial class StreamDictionary<KeyType,ValueType>:
        Dictionary<KeyType,ValueType>
    {
        public new Array<KeyType> Keys { get => (Array<KeyType>)base.Keys; set => base.Keys = value; }
        public new StreamCollection<ValueType> Values { get => (StreamCollection<ValueType>)base.Values; set => base.Values = value; }
        public StreamDictionary(int MinCount = 5000):
            base(new Array<KeyType>(),
                 new StreamCollection<ValueType>(MinCount))
        {}
    }
}