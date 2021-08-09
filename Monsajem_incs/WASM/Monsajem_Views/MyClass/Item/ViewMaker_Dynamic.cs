
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

namespace Monsajem_Incs.Views.Maker
{
    public static class ViewItemMaker
    {
        public static ViewType MakeView<ValueType, ViewType>(this ValueType obj,object ExtraData=null)
            where ViewType : new()
            => ViewItemMaker<ValueType, ViewType>.MakeView(obj,ExtraData);

        public static HTMLElement MakeView<ValueType>(ValueType obj, object ExtraData = null) =>
            ViewItemMaker<ValueType>.OnMakeView((obj, ExtraData));

        public static HTMLElement MakeView<ValueType>(
            this ValueType obj,
            Action<ValueType> OnClick,
            object ExtraData = null)
        {
            var View = MakeView(obj);
            View.OnClick += (c1, c2) =>
            {
                OnClick(obj);
            };
            return View;
        }

        public static HTMLElement MakeView<ValueType,keyType>(
            this ValueType obj,
            Func<ValueType, keyType> GetKey,
            Action<keyType> OnClick,
            object ExtraData = null)
        {
            var Key = GetKey(obj);
            var View = MakeView(obj, ExtraData);
            View.OnClick += (c1, c2) =>
            {
                OnClick(Key);
            };
            return View;
        }

        public static void SetView<ValueType, ViewType>()
            where ViewType:new()
            =>ViewItemMaker<ValueType>.OnMakeView = ViewItemMaker<ValueType,ViewType>.MakeHtml;
        
        public static void SetView<ValueType, ViewType>(
            Action<(ViewType View, ValueType Value, object ExtraData)> OnMakeView)
            where ViewType : new()
        {
            ViewItemMaker<ValueType, ViewType>.OnMakeView = OnMakeView;
            ViewItemMaker<ValueType>.OnMakeView = ViewItemMaker<ValueType, ViewType>.MakeHtml;
        }
    }
    internal class ViewItemMaker<ValueType>
    {
        public static Func<(ValueType Value,object ExtraData), HTMLElement> OnMakeView;
    }

}