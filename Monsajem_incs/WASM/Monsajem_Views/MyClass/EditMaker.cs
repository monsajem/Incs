
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

namespace Monsajem_Incs.Views
{
    public static class EditMaker<ValueType,ViewType>
     where ViewType : new()
    {
        public static event Action<(ViewType View, ValueType Value, object Data)> OnMakeView;
        public static event Action<(ViewType View, ValueType Value, object Data)> OnEdited;
        private static MyOptions Option;
        private class MyOptions
        {
            public FieldControler[] Fields;
            public FieldControler[] ViewFields;
            public FieldControler ViewBtnDone;
            public FieldControler ViewMain;
            public Func<string, object>[] ConvertFromStr;
        }

        static EditMaker()
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
                          OrderBy((c)=> c.Name).ToArray();
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

        private static ViewType MakeView(
            ValueType obj,
            bool HaveObj, 
            Action<ValueType> Done,
            object Data)
        {
            var View = new ViewType();
            if(HaveObj)
            {
                for (int i = 0; i < Option.Fields.Length; i++)
                {
                    var NodeValue = Option.Fields[i].GetValue(obj);
                    if (NodeValue != null)
                        ((HTMLElement)Option.ViewFields[i].GetValue(View)).Value = NodeValue.ToString();
                }
            }
            ((HTMLElement)Option.ViewBtnDone.GetValue(View)).OnClick+=(c1,c2) =>
            {
                var NewNodeValue =(ValueType) FormatterServices.GetUninitializedObject(typeof(ValueType));
                    for (int i = 0; i < Option.Fields.Length; i++)
                    {
                        string Val = ((HTMLInputElement)Option.ViewFields[i].GetValue(View)).Value;
                        try
                        {
                            Option.Fields[i].SetValue(NewNodeValue, Option.ConvertFromStr[i](Val));
                        }
                        catch { }
                    }
                    OnEdited?.Invoke((View, NewNodeValue, Data));
                    Done.Invoke((ValueType)NewNodeValue);
            };
            if(HaveObj)
                OnMakeView?.Invoke((View, obj, Data));
            return View;
        }

        public static ViewType MakeView<KeyType>(
            ValueType obj,
            Func<ValueType, KeyType> GetKey,
            Action<ValueType> Done, 
            object Data = null)
        {
            return MakeView(obj, GetKey, Done, Data);
        }

        public static ViewType MakeView<KeyType>(
            Action<ValueType> Done,
            Func<ValueType,KeyType> GetKey, 
            object Data = null)
        {
            return MakeView(default, GetKey, Done, Data);
        }

        public static HTMLElement MakeHtml(
            ValueType obj,
            bool HaveObj,
            Action<ValueType> Done, 
            object Data = null)
        {
            return (HTMLElement)Option.ViewMain.GetValue(MakeView(obj,HaveObj, Done, Data));
        }
        public static HTMLElement MakeHtml(Action<ValueType> Done, object Data = null)
        {
            return MakeHtml(default,false, Done, Data);
        }

        public static void MakeDefault()
        {
            EditMaker<ValueType>.MakeView = MakeHtml;
        }
        public static void MakeDefault(
            Action<(ViewType View, ValueType Value, object Data)> OnMakeView,
            Action<(ViewType View, ValueType Value, object Data)> OnEdited)
        {
            EditMaker<ValueType, ViewType>.OnMakeView = OnMakeView;
            EditMaker<ValueType, ViewType>.OnEdited = OnEdited;
            EditMaker<ValueType>.MakeView = MakeHtml;
        }
    }
}