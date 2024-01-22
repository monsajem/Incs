using System;
using static MonsajemData.DataBase;
using MonsajemData;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.KeyValue;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Net.Web;
using Monsajem_Incs.Net.Base.Service;
using WebAssembly.Browser.DOM;
using System.Reflection;
using System.Threading.Tasks;
using static WASM_Global.Publisher;
using Monsajem_Incs.Resources.Base.Partials;
using Monsajem_Incs.UserControler;

namespace Monsajem_Incs.WasmClient
{
    public class SafeRun
    {
        public static Action ShowCrashOnDataBase;
        public static Action<string> OnException;

        public static async Task Safe(Func<Task> Action)
        {
            try
            {
                await Action();
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                try
                {
                    Publish.HideAction();
                    {
                        var ex2 = ex;
                        ex = ex.InnerException;
                        while (ex != null)
                        {
                            Message = "   " + ex.Message;
                            ex = ex.InnerException;
                        }
                        ex = ex2;
                    }
                    if (ex is ThisException)
                        Publish.ShowDangerMessage(Message);
                    else if (ex.StackTrace.IndexOf("Monsajem_Incs.Database") > -1 &&
                             ex.StackTrace.IndexOf("Monsajem_Incs.Database.Base.Extentions.<GetUpdate>") < 0)
                    {
                        Console.WriteLine("Database Error! need to cleare!");
                        ShowCrashOnDataBase?.Invoke();
                        //await Task.Delay(2000);
                        //NavigationManager.NavigateTo(NavigationManager.Uri, true);
                    }
                    else
                        OnException(Message);
                }
                catch { }
                
                Console.WriteLine(Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}