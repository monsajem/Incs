
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;
using Monsajem_Incs.Resources.Base.Html;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Collection.Array;
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views.Extentions.Value
{
    public static partial class Ex
    {
        public static HTMLElement MakeEditView<ValueType, KeyType>(
            this ValueType obj,
            Func<ValueType,KeyType> GetKey,
            Action<(ValueType NewValue, KeyType OldKey)> Done, 
            object Data = null)
            where KeyType : IComparable<KeyType>
        {
            var OldKey = GetKey(obj);
            return EditMaker<ValueType>.MakeView(obj,true,(c)=> Done((c,OldKey)), Data);
        }
        public static HTMLElement MakeEditView<ValueType>(
            this ValueType obj,
            Action<ValueType> Done,
            object Data = null)
        {
            return EditMaker<ValueType>.MakeView(obj, true, Done, Data);
        }
    }
}