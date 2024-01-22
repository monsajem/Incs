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
using Monsajem_Incs.Convertors;

namespace Monsajem_Incs.Views.Maker.ValueTypes
{
    public abstract class ViewItemMaker<ValueType>
    {
        private static ViewItemMaker<ValueType> _Default;
        public static ViewItemMaker<ValueType> Default
        {
            get
            {
                if(_Default==null)
                    throw new Exception("Default of Show View Missing in " + typeof(ValueType).FullName);
                return _Default;
            }
            set => _Default = value;
        }

        public abstract HTMLElement MakeHtmlView(ValueType Value, Action Edit=null, Action Delete=null);
    }

    public class ViewItemMaker<ValueType, ViewType> : 
        ViewItemMaker<ValueType>
        where ViewType : new()
    {
        public static new ViewItemMaker<ValueType, ViewType> Default
        {
            get => ViewItemMaker<ValueType>.Default as ViewItemMaker<ValueType, ViewType> ;
            set => ViewItemMaker<ValueType>.Default = value;
        }

        public Action<(ViewType View, ValueType Value)> Default_FillView;
        public Action<(ViewType View, ValueType Value)> FillView;
        public Action<(ViewType View, Action Edit)> RegisterEdit;
        public Action<(ViewType View, Action Delete)> RegisterDelete;
        public Func<ViewType, HTMLElement> GetMain;

        public ViewItemMaker()
        {
            var FieldsNames = FieldControler.GetFields(typeof(ValueType));
            var ShowNames = FieldControler.GetFields(typeof(ViewType));
            try
            {
                var MainElementField = FieldControler.Make(
                    ShowNames.Where((c) => c.Name.ToLower() == "main").First());
                GetMain = (c) => (HTMLElement)MainElementField.GetValue(c);
            }
            catch
            {
                var ViewType = typeof(ViewType);
                GetMain = (c) =>
                    throw new MissingMemberException("Main Element not found at " + ViewType.Name +
                    System.Environment.NewLine +
                    ViewType.FullName.Substring(14));
            }
            try
            {
                var btn_Edit_Field = FieldControler.Make(
                    ShowNames.Where((c) => c.Name.ToLower() == "edit").First());
                RegisterEdit = (c) => ((HTMLElement)btn_Edit_Field.GetValue(c.View)).
                                        OnClick += (c1, c2) => c.Edit();
            }
            catch
            {
                //var ViewType = typeof(ViewType);
                //RegisterEdit = (c) =>
                //    throw new MissingMemberException("Edit Element not found at " + ViewType.Name +
                //    System.Environment.NewLine +
                //    ViewType.FullName.Substring(14));

                RegisterEdit = (c) => { };
            }
            try
            {
                var btn_Delete_Field = FieldControler.Make(
                    ShowNames.Where((c) => c.Name.ToLower() == "delete").First());
                RegisterDelete = (c) => ((HTMLElement)btn_Delete_Field.GetValue(c.View)).
                                        OnClick += (c1, c2) => c.Delete();
            }
            catch
            {
                //var ViewType = typeof(ViewType);
                //RegisterDelete = (c) =>
                //    throw new MissingMemberException("Delete Element not found at " + ViewType.Name +
                //    System.Environment.NewLine +
                //    ViewType.FullName.Substring(14));

                RegisterDelete = (c) => { };
            }

            FieldsNames = FieldsNames.Where((c) =>
                ShowNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                          OrderBy((c) => c.Name).ToArray();
            ShowNames = ShowNames.Where((c) =>
                FieldsNames.Where((q) => q.Name == c.Name).FirstOrDefault() != null).
                            OrderBy((c) => c.Name).ToArray();
            var ValueFields = FieldControler.Make(FieldsNames);
            var ViewFields = FieldControler.Make(ShowNames);

            Default_FillView +=(c)=>{};

            for (int i = 0; i < ValueFields.Length; i++)
            {
                var ValueField = ValueFields[i];
                var ViewField = ViewFields[i];
                var StringConvertor = ConvertorToString.GetConvertor(ValueField.Info.FieldType);
                if (StringConvertor.IsReadableConvertor)
                    Default_FillView += (c) =>
                    {
                        var NodeValue = ValueField.GetValue(c.Value);
                        if (NodeValue != null)
                            ((HTMLElement)ViewField.GetValue(c.View)).TextContent =
                                StringConvertor.ConvertorToString(ValueField.GetValue(c.Value));
                    };
            }

        }

        public ViewType MakeView(
            ValueType obj,
            Action Edit,
            Action Delete)
        {
            var View = new ViewType();
            if (Edit != null)
                RegisterEdit((View, Edit));
            if (Delete != null)
                RegisterDelete((View, Delete));
            Default_FillView((View, obj));
            FillView((View, obj));
            return View;
        }

        public override HTMLElement MakeHtmlView(ValueType Value, Action Edit=null, Action Delete=null)
        {
            return GetMain(MakeView(Value, Edit, Delete));
        }

        public void SetAsDefault() => Default = this;
    }
}