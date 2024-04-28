
using Monsajem_Incs.Convertors;
using Monsajem_Incs.DynamicAssembly;
using Monsajem_Incs.Serialization;
using System;
using System.Linq;
using WebAssembly.Browser.DOM;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Views.Maker.ValueTypes
{
    internal static class EditItemMaker<ValueType, ViewType>
        where ViewType : new()
    {
        public static Action<(ViewType View, ValueType Value)>
            Default_FillViewByValue;

        public static Func<(ViewType View, ValueType OldValue), ValueType>
            Default_MakeValueFromView;

        public static Action<(ViewType View, ValueType Value)>
            FillViewByValue;

        public static Action<(ViewType View, Action Edited)>
            RegisterOnEditedToView;

        public static Func<ViewType, HTMLElement>
            GetMainElementFromView;

        public static Func<(ViewType View, ValueType OldValue), ValueType>
            MakeValueFromView = (c) => c.OldValue;

        static EditItemMaker()
        {
            var FieldsNames = FieldControler.GetFields(typeof(ValueType));
            var ShowNames = FieldControler.GetFields(typeof(ViewType));
            try
            {
                var BtnDoneField = FieldControler.Make(
                    ShowNames.Where((c) => c.Name.ToLower() == "done").First());
                RegisterOnEditedToView = (c) =>
                    BtnDoneField.GetValueAs<HTMLElement>(c.View).OnClick += (q, e) => c.Edited();
            }
            catch
            {
                RegisterOnEditedToView = (c) =>
                {
                    var ViewType = typeof(ViewType);
                    throw new MissingMemberException("Done button not found at " +
                       System.Environment.NewLine +
                       ViewType.FullName.Substring(14));
                };
            }
            try
            {
                var MainElementField = FieldControler.Make(
                    ShowNames.Where((c) => c.Name == "Main").First());
                GetMainElementFromView = (c) => MainElementField.GetValueAs<HTMLElement>(c);
            }
            catch
            {
                GetMainElementFromView = (c) =>
                {
                    var ViewType = typeof(ViewType);
                    throw new MissingMemberException("Main Element not found at " + ViewType.Name +
                       System.Environment.NewLine +
                       ViewType.FullName.Substring(14));
                };
            }
            FieldsNames = FieldsNames.Where((c) =>
                ShowNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                          OrderBy((c) => c.Name).ToArray();
            ShowNames = ShowNames.Where((c) =>
                FieldsNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                            OrderBy((c) => c.Name).ToArray();
            var ValueFields = FieldControler.Make(FieldsNames).
                                Select((c) => (Field: c,
                                             Convertor: ConvertorToString.GetConvertor(c.Info.FieldType))).
                                ToArray();
            var ViewFields = FieldControler.Make(ShowNames);

            Default_FillViewByValue = (c) =>
            {
                for (int i = 0; i < FieldsNames.Length; i++)
                {
                    var ValueField = ValueFields[i];
                    var ViewField = ViewFields[i];
                    if (ValueField.Convertor.IsReadableConvertor &&
                       c.Value != null)
                    {
                        var NodeValue = ValueField.Field.GetValue(c.Value);
                        if (NodeValue != null)
                            ((HTMLElement)ViewField.GetValue(c.View)).Value =
                                ValueField.Convertor.ConvertorToString(NodeValue);
                    }
                }
            };

            Default_MakeValueFromView = (c) =>
            {
                var NewValue = c.OldValue.Serialize().Deserialize(c.OldValue);
                if (c.OldValue == null)
                    NewValue = (ValueType)GetUninitializedObject(typeof(ValueType));
                for (int i = 0; i < FieldsNames.Length; i++)
                {
                    var ValueField = ValueFields[i];
                    var ViewField = ViewFields[i];
                    if (ValueField.Convertor.IsReadableConvertor)
                    {
                        ValueField.Field.SetValue(
                            NewValue,
                            ValueField.Convertor.ConvertorFromString(
                                ((HTMLElement)ViewField.GetValue(c.View)).Value));
                    }
                }
                return NewValue;
            };
        }

        public static HTMLElement GetMainElement(ViewType View) => GetMainElementFromView(View);

        public static ViewType MakeView(
            ValueHolder<ValueType> OldValue,
            Action<(ValueType OldValue, ValueType NewValue)> Edited)
        {
            var View = new ViewType();
            if (OldValue.HaveValue)
            {
                Default_FillViewByValue((View, OldValue));
                FillViewByValue?.Invoke((View, OldValue));
            }
            RegisterOnEditedToView((View, () =>
            {
                var NewValue = Default_MakeValueFromView((View, OldValue));
                NewValue = MakeValueFromView((View, NewValue));
                Edited.Invoke((OldValue, NewValue));
            }
            ));
            return View;
        }
    }
}