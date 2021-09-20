using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Collection.Array;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Views.Maker.ValueTypes;
using static Monsajem_Client.SafeRun;
using static Monsajem_Client.Network;
using Monsajem_Incs.Views.Maker.Database;

namespace Monsajem_Incs.Views.Shower.Database
{
    public static partial class Shower
    {
        public static void ShowItems<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            new ShowPage().Show(Table);
        }
        public static void ShowItems<ValueType, KeyType>(
            this PartOfTable<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            new ShowPage().Show(Table);
        }

        public static void ShowInsert<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            new insertPage().Show(Table);
        }
        public static void ShowInsert<ValueType, KeyType>(
            this PartOfTable<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            new insertPage().Show(Table);
        }

        public static void ShowUpdate<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table,KeyType Key)
            where KeyType : IComparable<KeyType>
        {
            if (Table is PartOfTable<ValueType, KeyType>)
                Table = (Table as PartOfTable<ValueType, KeyType>).Parent;
              new UpdatePage().Show(Table,Key);
        }
    }
}