
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

namespace Monsajem_Incs.Views.Maker
{
    public static class EditItemMaker
    {
        public static ViewType MakeEditView<ViewType, ValueType>(
            this ValueType OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> Edited,
            object ExtraData = null)
            where ViewType : new() =>
            EditItemMaker<ValueType,ViewType>.MakeView(OldValue,(c)=>c, Edited, ExtraData);

        public static ViewType MakeEditView<ViewType, ValueType>(
            this ValueType OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> Edited,
            object ExtraData = null)
            where ViewType : new() =>
            EditItemMaker<ValueType, ViewType>.MakeView(OldValue, (c) => c, Edited, ExtraData);

        public static ViewType MakeView<ViewType, ValueType ,KeyType>(
            ValueType obj,
            Func<ValueType, KeyType> GetKey,
            Action<ValueType> Done,
            object Data = null)
        {
            return MakeView(obj, GetKey, Done, Data);
        }

        public static ViewType MakeView<KeyType>(
            Action<ValueType> Done,
            Func<ValueType, KeyType> GetKey,
            object Data = null)
        {
            return MakeView(default, GetKey, Done, Data);
        }

        public static HTMLElement MakeHtml(
            ValueType obj,
            Action<(ValueType OldValue, ValueType NewValue)> Done,
            object Data = null)
        {
            return (HTMLElement)Option.ViewMain.GetValue(MakeView(obj, HaveObj, Done, Data));
        }
        public static HTMLElement MakeHtml(
            Action<(ValueType OldValue, ValueType NewValue)> Done,
            object Data = null)
        {
            return MakeHtml(default, false, Done, Data);
        }

        public static void MakeDefault()
        {
            EditItemMaker<ValueType>.MakeView = MakeHtml;
        }
        public static void MakeDefault(
            Action<(ViewType View, ValueType Value, object Data)> OnMakeView,
            Action<(ViewType View, ValueType Value, object Data)> OnEdited)
        {
            EditItemMaker<ValueType, ViewType>.FillViewByValue = OnMakeView;
            EditItemMaker<ValueType, ViewType>.OnEdited = OnEdited;
            EditItemMaker<ValueType>.MakeView = MakeHtml;
        }

        public static void SetEditView()
        {
            EditItemMaker<ValueType>.MakeView = MakeHtml;
        }
    }

    internal static class EditItemMaker<ValueType>
    {
        public static Func<
            (ValueHolder<ValueType> OldValue,
            Action<(ValueHolder<ValueType> OldValue, ValueType NewValue)> OnEdited,
            object ExtraData), 
            HTMLElement> MakeView=(c)=> 
                throw new Exception("Edit View Missing in " + typeof(ValueType).FullName);
    }
}