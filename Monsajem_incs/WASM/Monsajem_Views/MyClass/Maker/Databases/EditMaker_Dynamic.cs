
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
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Views.Maker.ValueTypes;

namespace Monsajem_Incs.Views.Maker.Database
{
    public static class EditItemMaker
    {
        public static ViewType MakeEditView<ViewType, ValueType>(
            this ValueType OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> Edited,
            object ExtraData = null)
            where ViewType : new() =>
            EditItemMaker<ValueType,ViewType>.MakeView(OldValue, Edited, ExtraData);

        public static HTMLElement MakeEditView<ValueType>(
            this ValueType obj,
            Action<(ValueType OldValue, ValueType NewValue)> Done,
            object ExtraData = null)
        {
            return EditItemMaker<ValueType>.MakeView((obj,Done, ExtraData));
        }

        public static void GetRegisterEdit<ValueType, KeyType>(this Table<ValueType, KeyType> Table)
            where KeyType :IComparable<KeyType>
            => new RegisterEdit<ValueType, KeyType>() { Table = Table };

        public class RegisterEdit<ValueType, KeyType>
            where KeyType:IComparable<KeyType>
        {
            internal Table<ValueType, KeyType> Table;

            public static void SetDefault<ViewType>(
                Action<(ViewType View,ValueType Value)> FillView = null,
                Func<ViewType, HTMLElement> GetMain = null,
                Func<(ViewType View,ValueType OldValue), ValueType> FillValue = null,
                Action<(ViewType View, Action Edited)> SetEdited = null)
                where ViewType : new()
            {
                ValueTypes.EditItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>
                    .Default_FillViewByValue =
                    (c) =>
                    {
                        ValueTypes.EditItemMaker<ValueType, ViewType>.Default_FillViewByValue
                            ((c.View, c.Value.Value));
                    };

                var _FillView = default(Action<(ViewType View, (Table<ValueType, KeyType> Table, ValueType Value) Value)>);
                if (FillView != null)
                    _FillView = (c) => FillView((c.View, c.Value.Value));


                ValueTypes.EditItemMaker<(Table<ValueType, KeyType> Table, ValueType Value), ViewType>
                    .Default_MakeValueFromView =
                    (c) =>
                    {
                        var result = ValueTypes.EditItemMaker<ValueType, ViewType>.Default_MakeValueFromView
                            ((c.View,c.OldValue.Value));
                        return (c.OldValue.Table, result);
                    };

                var _FillValue = default(Func<(ViewType View, (Table<ValueType, KeyType> Table, KeyType Key) OldValue), (Table<ValueType, KeyType> Table, KeyType Key)>);
                if (FillValue != null)
                    _FillValue = (c) => FillValue((c.View, c.Value.Table, c.Value.Key));


                ValueTypes.EditItemMaker.MakeDefault
                    <(Table<ValueType, KeyType> Table, ValueType Key), ViewType>
                        (_FillView, GetMain, _FillValue, SetEdited);
            }
        }
    }

    internal static class EditItemMaker<ValueType>
    {
        public static Func<
            (ValueHolder<ValueType> OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> OnEdited,
            object ExtraData), 
            HTMLElement> MakeView=(c)=> 
                throw new Exception("Edit View Missing in " + typeof(ValueType).FullName);
    }
}