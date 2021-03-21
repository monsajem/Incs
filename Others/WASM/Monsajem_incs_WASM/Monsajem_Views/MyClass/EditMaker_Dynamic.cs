
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

namespace Monsajem_Incs.Views
{
    public static class EditMaker<ValueType>
    {
        internal static Func<
            ValueType,
            bool,
            Action<ValueType>,
            object, 
            HTMLElement> MakeView=(c1,c2,c3,c4)=> 
                throw new Exception("Edit View Missing in " + typeof(ValueType).FullName);
    }
}