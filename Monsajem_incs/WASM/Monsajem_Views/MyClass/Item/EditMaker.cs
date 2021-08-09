
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
using System.Runtime.Serialization;
using Monsajem_Incs.Serialization;
using Monsajem_incs;

namespace Monsajem_Incs.Views.Maker
{
    internal static class EditItemMaker<ValueType, ViewType>
        where ViewType : new()
    {
        private static Action<(ViewType View, ValueType Value, object ExtraData)> FillViewByValue;
        private static Action<ViewType,Action> RegisterOnEditedToView;
        private static Func<(ViewType View, ValueType OldValue, object ExtraData),ValueType> MakeValueFromView;
        private static MyOptions Option;
        private class MyOptions
        {
            public FieldControler[] Fields;
            public FieldControler[] ViewFields;
            public FieldControler ViewBtnDone;
            public FieldControler ViewMain;
            public Func<string, object>[] ConvertFromStr;
        }

        static EditItemMaker()
        {
            Option = new MyOptions();
            var FieldsNames = FieldControler.GetFields(typeof(ValueType));
            var ShowNames = FieldControler.GetFields(typeof(ViewType));
            try
            {
                Option.ViewBtnDone = FieldControler.Make(
                    ShowNames.Where((c) => c.Name == "Done").First());
            }
            catch
            {
                var ViewType = typeof(ViewType);
                throw new MissingMemberException("Done button not found at " +
                   System.Environment.NewLine +
                   ViewType.FullName.Substring(14));
            }
            try
            {
                Option.ViewMain = FieldControler.Make(
                    ShowNames.Where((c) => c.Name == "Main").First());
            }
            catch
            {
                var ViewType = typeof(ViewType);
                throw new MissingMemberException("Main Element not found at " + ViewType.Name +
                   System.Environment.NewLine +
                   ViewType.FullName.Substring(14));
            }
            FieldsNames = FieldsNames.Where((c) =>
                ShowNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                          OrderBy((c) => c.Name).ToArray();
            ShowNames = ShowNames.Where((c) =>
                FieldsNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                            OrderBy((c) => c.Name).ToArray();
            Option.Fields = FieldControler.Make(FieldsNames);
            Option.ViewFields = FieldControler.Make(ShowNames);
            Option.ConvertFromStr = new Func<string, object>[Option.Fields.Length];

            for (int i = 0; i < Option.Fields.Length; i++)
            {
                try
                {
                    Option.ConvertFromStr[i] = ConvertStringToNodeValue.GetConvertor(Option.Fields[i].Info.FieldType);
                }
                catch
                {
                    DeleteByPosition(ref Option.ConvertFromStr, i);
                    DeleteByPosition(ref Option.Fields, i);
                    DeleteByPosition(ref Option.ViewFields, i);
                    i--;
                }
            }
        }

        public static ViewType MakeView<KeyType>(
            ValueHolder<ValueType> OldValue,
            Func<ValueType,KeyType> GetKey,
            Action<(KeyType OldKey, ValueType NewValue)> Edited,
            object Data)
        {
            var OldKey = default(KeyType);
            var View = new ViewType();
            if (OldValue.HaveValue)
            {
                OldKey = GetKey(OldValue);
                for (int i = 0; i < Option.Fields.Length; i++)
                {
                    var NodeValue = Option.Fields[i].GetValue(OldValue);
                    if (NodeValue != null)
                        ((HTMLElement)Option.ViewFields[i].GetValue(View)).Value = NodeValue.ToString();
                }
                FillViewByValue?.Invoke((View, OldValue.Value, Data));
            }
            ((HTMLElement)Option.ViewBtnDone.GetValue(View)).OnClick += (c1, c2) =>
            {
                var NewValue = (ValueType)FormatterServices.GetUninitializedObject(typeof(ValueType));
                for (int i = 0; i < Option.Fields.Length; i++)
                {
                    string Val = ((HTMLInputElement)Option.ViewFields[i].GetValue(View)).Value;
                    try
                    {
                        Option.Fields[i].SetValue(OldValue, Option.ConvertFromStr[i](Val));
                    }
                    catch { }
                }
                NewValue = OnEdited.Invoke((View,OldValue, NewValue, Data));
                Edited.Invoke((OldKey, OldValue));
            };
            return View;
        }
    }
}