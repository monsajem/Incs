namespace Monsajem_Incs.Database.Base
{
    public partial class Table<ValueType, KeyType>
    {
        public class KeyInfo
        {
            public Monsajem_Incs.Collection.Array.Base.IArray<KeyType> Keys;
        }
    }
}
