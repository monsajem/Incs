
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
using Monsajem_Incs.Resources.Html;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.ArrayExtentions;
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views.Extentions.Table
{
    public static partial class Ex
    {
        public static HTMLElement MakeEditView<ValueType,KeyType>(
            this Table<ValueType, KeyType>.ValueInfo obj,
            Action<(ValueType NewValue, KeyType OldKey)> Done, 
            object Data = null)
            where KeyType:IComparable<KeyType>
        {
            var OldValue = obj.Value;
            var OldKey = obj.Parent.GetKey(OldValue);
            return EditMaker<ValueType>.MakeView(OldValue, true,(c)=> Done((c,OldKey)), Data);
        }
        public static HTMLElement MakeInsertView<ValueType,KeyType>(
            this Table<ValueType,KeyType> Table,
            Action<ValueType> Done, 
            object Data = null)
            where KeyType:IComparable<KeyType>
        {
            return EditMaker<ValueType>.MakeView(default,false,Done, Data);
        }
    }
}