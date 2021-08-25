
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

namespace Monsajem_Incs.Views.Maker.ValueTypes
{
    public static class ViewItemMaker
    {
        public static ViewType MakeView<ValueType, ViewType>(
            this ValueType obj,
            Action OnEdit=null,
            Action OnDelete=null)
            where ViewType : new()
            => ViewItemMaker<ValueType, ViewType>.MakeView(obj,OnEdit,OnDelete);

        public static HTMLElement MakeView<ValueType>(
            this ValueType obj,
            Action OnEdit = null,
            Action OnDelete = null) =>
            ViewItemMaker<ValueType>.OnMakeView((obj,OnEdit,OnDelete));

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
        
        public static void SetView<ValueType, ViewType>(
            Action<(ViewType View, ValueType Value)> FillView=null,
            Action<(ViewType View, Action Edit)> RegisterEdit=null,
            Action<(ViewType View, Action Delete)> RegisterDelete = null)
            where ViewType : new()
        {
            if(FillView!=null)
                ViewItemMaker<ValueType, ViewType>.FillView = FillView;
            if (RegisterEdit != null)
                ViewItemMaker<ValueType, ViewType>.RegisterEdit = RegisterEdit;
            if (RegisterDelete != null)
                ViewItemMaker<ValueType, ViewType>.RegisterDelete = RegisterDelete;
            ViewItemMaker<ValueType>.OnMakeView = ViewItemMaker<ValueType, ViewType>.MakeHtml;
        }
    }
    internal class ViewItemMaker<ValueType>
    {
        public static Func<
            (ValueType Value,Action Edit,Action Delete), 
            HTMLElement> OnMakeView;
    }

}