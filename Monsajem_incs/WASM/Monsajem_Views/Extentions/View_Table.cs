
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;
using Monsajem_Incs.Resources.Html;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Collection.Array;
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views.Extentions.Table
{
    public static partial class Ex
    {
        public static HTMLElement MakeView<ValueType, KeyType>(
            this Table<ValueType,KeyType>.ValueInfo value,
            Action<(KeyType Key, HTMLElement View)> OnMake = null,
            object Data = null)
            where KeyType:IComparable<KeyType>
        {
            return Value.Ex.MakeView(value.Value,value.Parent.GetKey, OnMake, Data);
        }

        public static HTMLElement MakeView<ValueType, KeyType>(
            this IEnumerable<Table<ValueType, KeyType>.ValueInfo> values,
            Action<(KeyType Key, HTMLElement View)> OnMake = null,
            object Data = null)
            where KeyType : IComparable<KeyType>
        {
            var View = new Div_html();
            foreach (var Value in values)
            {
                View.Main.AppendChild(MakeView(Value, OnMake, Data));
            }
            
            return View.Main;
        }
    }
}