using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly.Browser.MonsajemDomHelpers;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Resources.Base.Partials;
using WebAssembly.Browser.DOM;
using static Monsajem_Incs.Collection.Array.Extentions;
using Monsajem_Incs.WasmClient;

namespace Monsajem_Incs.UserControler
{
    public static class Publish
    {
        private static HTMLElement Loader = js.Document.GetElementById("Loading");
        private static HTMLLabelElement Loader_Message =
            js.Document.GetElementById<HTMLLabelElement>("LoaderMessage");
        public static Func<Task>[] States = new Func<Task>[0];

        public static void PushState(Action Action) =>
            PushState(async () => Action());
        
        private static void OnPopState()
        {
            if (States.Length > 1)
            {
                try
                {
                    Pop(ref States);
                    SafeRun.Safe(()=>States[States.Length - 1]());
                }
                finally { }

            }
            else
                js.PushState(OnPopState);
        }

        public static async Task PushState(Func<Task> Action)
        {
            try
            {
                await SafeRun.Safe(()=> Action.Invoke());
                js.PushState(OnPopState);
                Insert(ref States, Action);
            }
            finally { }
        }

        public static async Task Back()
        {
            await Task.Delay(1);
            js.GoBack();
            await Task.Delay(1);
        }

        public static async Task Replay()
        {
            await States[States.Length - 1]();
        }

        private static void ShowMessage(
            string Message,
            string Type,
            int Delay,
            bool AllowDissmiss)
        {
            js.JsEval(@"$.notify(" +
                js.ToJsValue(Message) + @", {
                allow_dismiss: " + AllowDissmiss.ToString().ToLower() + @",
                delay:" + Delay + @",
                type: '" + Type + "'});");
        }

        public static void ShowSuccessMessage(string Message) =>
            ShowMessage(Message, "success", 3000, false);

        public static void ShowDangerMessage(string Message) =>
            ShowMessage(Message, "danger", 3000, false);

        public static void ShowAction(string Message)
        {
            Loader.SetStyleAttribute("height", "100vh");
            Loader.SetStyleAttribute("width", "100vw");
            Loader_Message.TextContent = Message;
        }
        public static void HideAction()
        {
            Loader.SetStyleAttribute("height", "0vh");
            Loader.SetStyleAttribute("width", "0vw");
        }

        public static void Show(this HTMLElement Element)
        {
            Element.SetStyleAttribute("display", "block");
            Element.SetStyleAttribute("transition", "0.5s");
            Element.SetStyleAttribute("opacity", "100");
        }

        private static void Hide(this HTMLElement Element)
        {
            Element.SetStyleAttribute("transition", "0.5s");
            Element.SetStyleAttribute("opacity", "0");
            Element.SetStyleAttribute("display", "none");
        }

        private static async Task HideAfter(this HTMLElement Element, int Time)
        {
            await Task.Delay(Time);
            Element.Hide();
        }

        public static string AddThousandSprator(string Value)
        {
            var Result = "";
            for (int i = Value.Length - 1; i > 2; i -= 3)
            {
                Result = "<b style='color:chocolate;'>,</b style='color:black;'><b>" + Value.Substring(i - 2, 3) + "</b>" + Result;
            }
            var LastPos = (Value.Length) % 3;
            if (LastPos == 0)
                LastPos = 3;
            Result = "<div style='flex-wrap:nowrap'><b>" + Value.Substring(0, LastPos) + "</b>" + Result + "</div>";
            return Result;
        }

        public static void ShowModal(HTMLElement Element, Action OnBack = null)
        {
            var Modal = new Modal_html();
            Modal.body.AppendChild(Element);
            js.Document.Body.AppendChild(Modal.myModal);
            Modal.myModal.SetStyleAttribute("display","block");
            Modal.Btn_Close.OnClick+=(c1,c2)=>
            {
                js.GoBack();
            };
            js.PushState(() =>
            { 
                OnBack?.Invoke();
                Modal.myModal.Remove();
            });
        }
    }
}