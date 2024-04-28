using Monsajem_Incs.UserControler;
using System;
using System.Threading.Tasks;

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