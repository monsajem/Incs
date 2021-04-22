using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Monsajem_Incs.DelegateExtentions
{
    public delegate ref t GetRef<t>();

    public static class Actions
    {
        public static void ex<t>(this t dg)
            where t : System.MulticastDelegate
        {
            
        }

        public static Task WaitForHandle(GetRef<Action> Action)
        {
            var a = new object();
            var Locker = new object();
            var Runned = false;
            var Waiter = AsyncTaskMethodBuilder.Create();
            Action MyAction = null;
            MyAction = () => {
                lock(Locker)
                {
                    if(Runned==false)
                    {
                        Action() -= MyAction;
                        Runned = true;
                        Waiter.SetResult();
                    }
                }
            };
            Action() += MyAction;
            return Waiter.Task;
        }

        public static void WaitForHandle(GetRef<Action> Action,Action Handle)
        {
            var Locker = new object();
            var Runned = false;
            Action MyAction = null;
            MyAction = () => {

                lock (Locker)
                {
                    if (Runned == false)
                    {
                        Action() -= MyAction;
                        Runned = true;
                        Handle();
                    }
                }
            };
            Action() += MyAction;
        }

        public static void RunOnNewThreade(Action Action)
        {
            new System.Threading.Thread(() =>
            {
                Action();
            }).Start();
        }
        public static void RunOnNewThreade(IEnumerable<Action> Actions)
        {
            foreach (var Action in Actions)
                new System.Threading.Thread(() =>Action()).Start();
        }
    }
    public static class Extentions
    {
        public static Task WaitForHandle(this GetRef<Action> Action)=>
           Actions.WaitForHandle(Action);

        public static void RunOnNewThreade<t>(this Action Action)=>
            Actions.RunOnNewThreade(Action);

        public static void RunOnNewThreade(this IEnumerable<Action> actions)=>
            Actions.RunOnNewThreade(actions);
    }
}
