
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
using Monsajem_Incs;

namespace Monsajem_Incs.Views.Maker.ValueTypes
{
    public static class EditItemMaker
    {
        public static ViewType MakeEditView<ViewType, ValueType>(
            this ValueType OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> Edited)
            where ViewType : new() =>
            EditItemMaker<ValueType,ViewType>.MakeView(OldValue, Edited);

        public static HTMLElement MakeEditView<ValueType>(
            this ValueType obj,
            Action<(ValueType OldValue, ValueType NewValue)> Done)
        {
            return EditItemMaker<ValueType>.MakeView((obj,Done));
        }

        public static void MakeDefault<ValueType, ViewType>(
            Action<Options<ValueType, ViewType>> Maker = null)
            where ViewType:new()
        {
            EditItemMaker<ValueType>.MakeView = (c) =>
            {
                 var View = EditItemMaker<ValueType, ViewType>.
                                 MakeView(c.OldValue, c.OnEdited);
                 var HtmlView = EditItemMaker<ValueType, ViewType>.GetMainElement(View);
                 return HtmlView;
            };

            var Options = new Options<ValueType, ViewType>();
            Maker?.Invoke(Options);

            if (Options.FillView != null)
                EditItemMaker<ValueType, ViewType>.FillViewByValue = Options.FillView;
            
            if(Options.GetMain !=null)
                EditItemMaker<ValueType, ViewType>.GetMainElementFromView = Options.GetMain;

            if(Options.FillValue != null)
                EditItemMaker<ValueType, ViewType>.MakeValueFromView = Options.FillValue;

            if (Options.SetEdited != null)
                EditItemMaker<ValueType, ViewType>.RegisterOnEditedToView = Options.SetEdited;
        }

        public class Options<ValueType, ViewType>
        {
            public Action<(ViewType View, ValueType Value)> FillView;
            public Func<ViewType, HTMLElement> GetMain;
            public Func<(ViewType View, ValueType OldValue), ValueType> FillValue ;
            public Action<(ViewType View, Action Edited)> SetEdited;
        }
    }

    internal static class EditItemMaker<ValueType>
    {
        public static Func<
            (ValueHolder<ValueType> OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> OnEdited), 
            HTMLElement> MakeView=(c)=> 
                throw new Exception("Edit View Missing in " + typeof(ValueType).FullName);
    }
}