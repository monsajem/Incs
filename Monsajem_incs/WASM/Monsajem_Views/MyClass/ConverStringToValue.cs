
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

namespace Monsajem_Incs.Views
{
    internal static class ConvertStringToNodeValue
    {
        public static Func<string,object> GetConvertor(Type Type)
        {
           if (Type == typeof(string))
                return (str) => str;
           var Method = Type.GetMethods().Where((c) => c.Name == "Parse" & c.GetParameters().Length == 1).FirstOrDefault();
           if (Method == null)
                throw new InvalidCastException("Cannot convert " + Type.FullName + " to string.");
           return (string str)=> Method.Invoke(null,new object[] {str });
        }

    }
}