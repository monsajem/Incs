
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

namespace Monsajem_Incs.Views
{
   
    public class ViewMaker<ValueType>
    {
        internal static Func<ValueType,object, HTMLElement> MakeView;

        public class Options
        {
            public string ValueContainerClass;
            public string LabelContainerClass;
            public string FieldViewContainerClass;
            public string[] Labels;
            public FieldControler[] Fields;
            public Func<object, HTMLElement> MakeView;
            public Func<object, Action<object>, HTMLElement> MakeEdit;

            public Options()
            {
                Fields = FieldControler.Make(typeof(ValueType));
                Labels = new string[Fields.Length];
                for (int i = 0; i < Fields.Length; i++)
                {
                    Labels[i] = Fields[i].Info.Name;
                }
            }

            public void Label<FieldType>(
                Expression<Func<ValueType, FieldType>> WichField,
                string Label)
            {
                var FieldName = ((MemberExpression)WichField.Body).Member.Name;
                int i = 0;
                for (; i < Fields.Length; i++)
                {
                    if (FieldName == Fields[i].Info.Name)
                    {
                        Labels[i] = Label;
                    }
                }
            }

            internal void Ready()
            {
                MakeView = (object obj) =>
                {
                    var View = new Div_html();
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        var Value = new Div_html();
                        Value.Main.TextContent = Fields[i].GetValue(obj).ToString();

                        var FieldShow = new Div_html();

                        var Label = new Div_html();
                        Label.Main.TextContent = Labels[i];

                        FieldShow.Main.AppendChild(Label.Main);
                        FieldShow.Main.AppendChild(Value.Main);

                        if (FieldViewContainerClass != null)
                            FieldShow.Main.Attribute["class"]=FieldViewContainerClass;
                        if (LabelContainerClass != null)
                            Label.Main.Attribute["class"]=LabelContainerClass;
                        if (ValueContainerClass != null)
                            Value.Main.Attribute["class"]=ValueContainerClass;

                        View.Main.AppendChild(FieldShow.Main);
                    }
                    return View.Main;
                };

                MakeEdit = (object obj, Action<object> Done) =>
                {
                    var View = new Div_html();
                    var Edits = new HTMLElement[Fields.Length];
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        var Edit = new input_Text_html();
                        Edit.Main.TextContent = Fields[i].GetValue(obj).ToString();
                        View.Main.AppendChild(Edit.Main);
                        Edits[i] = Edit.Main;
                    }

                    var BtnDone = new button_html();
                    BtnDone.Main.OnClick+=(c1,c2) =>
                    {
                        for (int i = 0; i < Fields.Length; i++)
                            {
                                Fields[i].SetValue(obj, (string)Edits[i].NodeValue);
                            }
                            Done(obj);
                    };
                    View.Main.AppendChild(BtnDone.Main);

                    return View.Main;
                };
            }
        }

        public Options MyOptions;



        public ViewMaker(Action<Options> GetOptions = null)
        {
            MyOptions = new Options();
            MyOptions.Ready();
            GetOptions?.Invoke(MyOptions);
        }
    }

}