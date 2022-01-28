
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
            Action<Options<ValueType,ViewType>> Maker = null)
            where ViewType : new()
        {
            var ViewItemMaker = ViewItemMaker<ValueType, ViewType>.Default;

            var Options = new Options<ValueType, ViewType>();
            Maker?.Invoke(Options);

            if (Options.FillView != null)
                ViewItemMaker.FillView = Options.FillView;
            if (Options.RegisterEdit != null)
                ViewItemMaker.RegisterEdit = Options.RegisterEdit;
            if (Options.RegisterDelete != null)
                ViewItemMaker.RegisterDelete = Options.RegisterDelete;
            if(Options.GetMain !=null)
                ViewItemMaker.GetMain = Options.GetMain;
            ViewItemMaker<ValueType>.OnMakeView = ViewItemMaker.MakeHtml;
        }

        public class Options<ValueType, ViewType>
            where ViewType : new()
        {

            public Action<(ViewType View, ValueType Value)> FillView;
            public Action<(ViewType View, Action Edit)> RegisterEdit;
            public Action<(ViewType View, Action Delete)> RegisterDelete;
            public Func<ViewType, HTMLElement> GetMain;
        }

        public static void SetHolderView<HolderType>(
            Func<(HolderType Holder, HTMLElement[] Views), HTMLElement> FillHolder = null,
            Func<HolderType,HTMLElement> MakeHolder = null)
        {
            if (FillHolder != null)
                HolderViewItemMaker<HolderType>.FillHolder = FillHolder;
            if (MakeHolder != null)
                HolderViewItemMaker<HolderType>.MakeHolder = MakeHolder;
        }
    }
    internal class ViewItemMaker<ValueType>
    {
        public static Func<
            (ValueType Value,Action Edit,Action Delete), 
            HTMLElement> OnMakeView=(c)=> 
                throw new Exception("Show View Missing in " + typeof(ValueType).FullName);
    }

    internal class HolderViewItemMaker<HolderType>
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