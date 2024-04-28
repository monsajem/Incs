using Monsajem_Incs.Database.Base;
using System;

namespace Monsajem_Incs.Views.Shower.Database
{
    public static partial class Shower
    {
        public static string ProvideUri<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            var Uri = new ShowPage().ProvideFullUri(Table);
            return Uri;
        }

        public static void ShowItems<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table,
            string Query = null)
            where KeyType : IComparable<KeyType>
        {
            new ShowPage().Show(Table, Query);
        }

        public static void ShowInsert<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            new insertPage().Show(Table);
        }
        public static void ShowInsertForCurrentURL()
        {
            new insertPage().Show();
        }

        public static void ShowUpdate<ValueType, KeyType>(
            this Table<ValueType, KeyType> Table, KeyType Key)
            where KeyType : IComparable<KeyType>
        {
            if (Table is PartOfTable<ValueType, KeyType>)
                Table = (Table as PartOfTable<ValueType, KeyType>).Parent;
            new UpdatePage().Show(Table, Key);
        }
    }
}