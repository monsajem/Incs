
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
            => ViewItemMaker<ValueType, ViewType>.Default.MakeView(obj,OnEdit,OnDelete);

        public static HTMLElement MakeView<ValueType>(
            this ValueType obj,
            Action OnEdit = null,
            Action OnDelete = null) =>
            ViewItemMaker<ValueType>.OnMakeView((obj,OnEdit,OnDelete));
        
        public static void SetView<ValueType, ViewType>(
            Action<(ViewType View, ValueType Value)> FillView=null,
            Action<(ViewType View, Action Edit)> RegisterEdit=null,
            Action<(ViewType View, Action Delete)> RegisterDelete = null,
            Func<ViewType, HTMLElement> GetMain = null)
            where ViewType : new()
        {
            var ViewMaker = ViewItemMaker<ValueType, ViewType>.Default;
            if (FillView!=null)
                ViewMaker.FillView = FillView;
            if (RegisterEdit != null)
                ViewMaker.RegisterEdit = RegisterEdit;
            if (RegisterDelete != null)
                ViewMaker.RegisterDelete = RegisterDelete;
            if(GetMain!=null)
                ViewMaker.GetMain = GetMain;
            ViewItemMaker<ValueType>.OnMakeView = ViewMaker.MakeHtml;
        }

        public static void SetHolderView<HolderType>(
            Func<(HolderType Holder, HTMLElement[] Views), HTMLElement> FillHolder = null,
            Func<HolderType,HTMLElement> MakeHolder = null)
        {
            if (FillHolder != null)
                HolderViewMaker<HolderType>.FillHolder = FillHolder;
            if (MakeHolder != null)
                HolderViewMaker<HolderType>.MakeHolder = MakeHolder;
        }
    }
    internal class ViewItemMaker<ValueType>
    {
        public static Func<
            (ValueType Value,Action Edit,Action Delete), 
            HTMLElement> OnMakeView=(c)=> 
                throw new Exception("Show View Missing in " + typeof(ValueType).FullName);
    }

    internal class HolderViewMaker<HolderType>
    {
        public static Func<HolderType,HTMLElement> MakeHolder = 
            (c) => new Monsajem_Incs.Resources.Base.Html.Div_html().Main;

        public static Func<(HolderType Holder, HTMLElement[] Views), HTMLElement> 
            FillHolder = (Info) =>
            {
                var Holder = MakeHolder(Info.Holder);

                foreach (var View in Info.Views)
                    Holder.AppendChild(View);

                return Holder;
            };
    }
}