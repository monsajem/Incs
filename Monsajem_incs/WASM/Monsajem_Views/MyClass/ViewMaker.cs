
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
using Monsajem_Incs.Database.Base;

namespace Monsajem_Incs.Views
{
    public static class ViewMaker<ValueType, ViewType>
        where ViewType : new()
    {
        public static event Action<(ViewType View, ValueType Value,object Data)> OnMakeView;
        private static Options MyOptions;
        private class Options
        {
            public FieldControler[] Fields;
            public FieldControler[] ViewFields;
            public FieldControler ViewMain;
        }

        static ViewMaker()
        {
            MyOptions = new Options();
            var FieldsNames = FieldControler.GetFields(typeof(ValueType));
            var ShowNames = FieldControler.GetFields(typeof(ViewType));
            try
            {
                MyOptions.ViewMain = FieldControler.Make(
                    ShowNames.Where((c) => c.Name == "Main").First());
            }
            catch 
            {
                var ViewType = typeof(ViewType);
                throw new MissingMemberException("Main Element not found at " +ViewType.Name +
                   System.Environment.NewLine +
                   ViewType.FullName.Substring(14));
            }
            FieldsNames = FieldsNames.Where((c) =>
                ShowNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                          OrderBy((c)=> c.Name).ToArray();
            ShowNames = ShowNames.Where((c) =>
                FieldsNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                            OrderBy((c)=>c.Name).ToArray();
            MyOptions.Fields = FieldControler.Make(FieldsNames);
            MyOptions.ViewFields = FieldControler.Make(ShowNames);
        }

        public static ViewType MakeView(ValueType obj,object Data)
        {
            var View = new ViewType();
            for (int i = 0; i < MyOptions.Fields.Length; i++)
            {
                var NodeValue = MyOptions.Fields[i].GetValue(obj);
                if (NodeValue != null)
                    ((HTMLElement)MyOptions.ViewFields[i].GetValue(View)).TextContent = NodeValue.ToString();
            }
            OnMakeView?.Invoke((View, obj,Data));
            return View;
        }

        public static HTMLElement MakeHtml(
            ValueType obj,
            object Data)
        {
            
            return (HTMLElement) MyOptions.ViewMain.GetValue(MakeView(obj,Data));
        }
        public static HTMLElement MakeHtml(
            ValueType obj,
            Action<ValueType> OnClick,
            object Data)
        {
            var View = MakeHtml(obj,Data);
            View.OnClick+=(c1,c2) =>
            {
                OnClick(obj);
            };
            return (HTMLElement)MyOptions.ViewMain.GetValue(View);
        }

        public static HTMLElement MakeHtml<keyType>(
            ValueType obj,
            Func<ValueType, keyType> GetKey,
            Action<keyType> OnClick,
            object Data)
        {
            var Key = GetKey(obj);
            var View = MakeHtml(obj,Data);
            View.OnClick+=(c1,c2) =>
            {
                OnClick(Key);
            };
            return (HTMLElement)MyOptions.ViewMain.GetValue(View);
        }


        public class ShowView<keyType>
        {

            internal Func<ValueType, keyType> GetKey;
            internal IEnumerable<ValueType> NodeValues;
            public event Action<(keyType key, ViewType View)> SelectedNodeValue;

            public HTMLElement MakeView()
            {
                var Views = new Div_html();
                foreach (var NodeValue in NodeValues)
                {
                    var View = ViewMaker<ValueType, ViewType>.MakeView(NodeValue,null);
                    var Main = (HTMLElement)MyOptions.ViewMain.GetValue(View);
                    Main.OnClick+=(c1,c2)=>GetOnSelect(GetKey(NodeValue), View, SelectedNodeValue);
                    Views.Main.AppendChild(Main);
                }
                return Views.Main;
            }

            private static Action
                GetOnSelect(keyType Key, ViewType View, Action<(keyType key, ViewType View)> OnSelect)
            {
                return () => OnSelect((Key, View));
            }
        }

        public static ShowView<ValueType> MakeView(IEnumerable<ValueType> obj)
        {
            return new ShowView<ValueType>()
            {
                GetKey = (v) => v,
                NodeValues = obj
            };
        }


        public static void MakeDefault()
        {
            ViewMaker<ValueType>.MakeView = MakeHtml;
        }

        public static void MakeDefault(
            Action<(ViewType View, ValueType Value, object Data)> OnMakeView)
        {
            ViewMaker<ValueType, ViewType>.OnMakeView = OnMakeView;
            ViewMaker<ValueType>.MakeView = MakeHtml;
        }
    }
}