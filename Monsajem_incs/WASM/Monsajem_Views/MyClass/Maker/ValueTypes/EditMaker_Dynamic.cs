
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
using Monsajem_incs;

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
            Action<(ViewType View, ValueType Value)> FillView=null,
            Func<ViewType, HTMLElement> GetMain = null,
            Func<(ViewType View,ValueType OldValue),ValueType> FillValue=null,
            Action<(ViewType View,Action Edited)> SetEdited = null)
            where ViewType:new()
        {
            EditItemMaker<ValueType>.MakeView = (c) =>
            {
                 var View = EditItemMaker<ValueType, ViewType>.
                                 MakeView(c.OldValue, c.OnEdited);
                 var HtmlView = EditItemMaker<ValueType, ViewType>.GetMainElement(View);
                 return HtmlView;
            };

            if(FillView!=null)
                EditItemMaker<ValueType, ViewType>.FillViewByValue = FillView;
            
            if(GetMain!=null)
                EditItemMaker<ValueType, ViewType>.GetMainElementFromView = GetMain;

            if(FillValue != null)
                EditItemMaker<ValueType, ViewType>.MakeValueFromView = FillValue;

            if (SetEdited != null)
                EditItemMaker<ValueType, ViewType>.RegisterOnEditedToView = SetEdited;
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