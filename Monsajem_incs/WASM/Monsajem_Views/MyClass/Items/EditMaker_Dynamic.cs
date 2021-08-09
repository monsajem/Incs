
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
    public static class ShowItemsMaker
    {
        public static string DefaultInsertText;
        public static string DefaultEditText;
        public static string DefaultDeleteText;
        public static Func<((Func<object, Task> Action, string Text) InsertAction,
                            (Func<object,Task> Action,string Text) EditAction,
                            (Func<object, Task> Action, string Text) DeleteAction,
                            IEnumerable<(HTMLElement Veiw, object Value)> Values), HTMLElement> DefaultMakeView = (c) =>
                  throw new Exception("Default show items missing in " + typeof(ValueType).FullName);
        
        public static HTMLElement MakeView(
                              IEnumerable<(HTMLElement Veiw,object Value)> Values,
                              (Func<object, Task> Action, string Text) InsertAction=default,
                              (Func<object, Task> Action, string Text) EditAction=default,
                              (Func<object, Task> Action, string Text) DeleteAction=default)
        {
            if (InsertAction != default && InsertAction.Text == null)
                InsertAction.Text = DefaultInsertText;

            if (EditAction != default && EditAction.Text == null)
                EditAction.Text = DefaultEditText;

            if (DeleteAction != default && DeleteAction.Text == null)
                DeleteAction.Text = DefaultDeleteText;

            return DefaultMakeView((InsertAction, EditAction, DeleteAction,Values));
        }

        public static HTMLElement MakeView(
                      IEnumerable<(HTMLElement Veiw, object Value)> Values,
                      Func<object, Task> InsertAction = default,
                      Func<object, Task> EditAction = default,
                      Func<object, Task> DeleteAction = default)
        {

            (Func<object, Task> Action, string Text) _InsertAction=default;
            (Func<object, Task> Action, string Text) _EditAction=default;
            (Func<object, Task> Action, string Text) _DeleteAction=default;

            if (InsertAction != default)
            {
                _InsertAction.Text = DefaultInsertText;
                _InsertAction.Action = InsertAction;
            }

            if (EditAction != default)
            {
                _EditAction.Text = DefaultEditText;
                _EditAction.Action = EditAction;
            }

            if (DeleteAction != default)
            {
                _DeleteAction.Text = DefaultDeleteText;
                _DeleteAction.Action = DeleteAction;
            }

            return MakeView(Values,_InsertAction, _EditAction, _DeleteAction);
        }
    }
    public static class ShowItemsMaker<ValueType>
    {
        public static string DefaultInsertText;
        public static string DefaultEditText;
        public static string DefaultDeleteText;

        public static Func<(Func<ValueType, Task> InsertAction,
                            Func<ValueType, Task> EditAction,
                            Func<ValueType, Task> DeleteAction,
                            IEnumerable<ValueType> Values), HTMLElement> DefaultMakeView = (c) =>
        {
            (Func<object, Task> Action, string Text) _InsertAction = default;
            (Func<object, Task> Action, string Text) _EditAction = default;
            (Func<object, Task> Action, string Text) _DeleteAction = default;

            if (c.InsertAction != default)
            {
                _InsertAction.Text = DefaultInsertText;
                _InsertAction.Action = (q) => c.InsertAction((ValueType)q);
            }

            if (c.EditAction != default)
            {
                _EditAction.Text = DefaultEditText;
                _EditAction.Action = (q) => c.EditAction((ValueType)q);
            }

            if (c.DeleteAction != default)
            {
                _DeleteAction.Text = DefaultDeleteText;
                _DeleteAction.Action = (q) => c.DeleteAction((ValueType)q);
            }

            try
            {
                return ShowItemsMaker.MakeView(c.Values.Select((c)=>(c.make,_InsertAction, _EditAction, _DeleteAction);
            }
            catch
            {
                throw new Exception("Edit View Missing in " + typeof(ValueType).FullName);
            }
        };

        public static HTMLElement MakeView(
                      IEnumerable<ValueType> Values,
                      Func<ValueType, Task> InsertAction = default,
                      Func<ValueType, Task> EditAction = default,
                      Func<ValueType, Task> DeleteAction = default)
        {
            return DefaultMakeView((InsertAction, EditAction, DeleteAction,Values));
        }
    }
}