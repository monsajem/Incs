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
using Monsajem_Incs.Resources.Partials;

namespace Monsajem_Client
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
                    UserControler.Publish.HideAction();
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
                        UserControler.Publish.ShowDangerMessage(Message);
                    else if (ex.StackTrace.IndexOf("Monsajem_Incs.Database") > -1 &&
                             ex.StackTrace.IndexOf("Monsajem_Incs.Database.Base.Extentions.<GetUpdate>") < 0)
                    {
                        DataBaseInfo.ClearDatabase();
                        Console.WriteLine("Database Error!");
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