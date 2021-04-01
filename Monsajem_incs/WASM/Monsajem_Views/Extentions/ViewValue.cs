
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

namespace Monsajem_Incs.Views.Extentions.Value
{
    public static partial class Ex
    {
        public static HTMLElement MakeView<ValueType>(
            this ValueType value,
            object Data =null)
        {
            if (ViewMaker<ValueType>.MakeView == null)
                throw new NotImplementedException("Make View not declared For " + typeof(ValueType).FullName);
            return ViewMaker<ValueType>.MakeView(value,Data);
        }
        public static HTMLElement MakeView<ValueType, KeyType>(
            this ValueType value,
            Func<ValueType, KeyType> GetKey,
            Action<(KeyType Key, HTMLElement View)> OnMake,
            object Data = null)
        {

            var Key = GetKey(value);
            var View = MakeView(value,Data);
            OnMake?.Invoke((Key, View));
            return View;
        }

        public static HTMLElement MakeView<ValueType>(
            this ValueType value,
            Action<(ValueType Value, HTMLElement View)> OnMake,
            object Data = null)
        {
            return MakeView(value, (c) => c, OnMake, Data);
        }
        
    }
}