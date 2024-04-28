using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monsajem_Incs.Async
{
    public delegate ref t GetRef<t>();

    public static class DelegateActions
    {
        public static void ex<t>(this t dg)
            where t : System.MulticastDelegate
        {

        }

        public static Task WaitForHandle(GetRef<Action> Action)
        {
            var Runned = false;
            var Waiter = new Task(() => { });
            void MyAction()
            {
                lock (Waiter)
                {
                    if (Runned == false)
                    {
                        Action() -= MyAction;
                        Runned = true;
                        Waiter.Start();
                    }
                }
            }

            Action() += MyAction;
            return Waiter;
        }

        public static void WaitForHandle(GetRef<Action> Action, Action Handle)
        {
            var Locker = new object();
            var Runned = false;
            void MyAction()
            {

                lock (Locker)
                {
                    if (Runned == false)
                    {
                        Action() -= MyAction;
                        Runned = true;
                        Handle();
                    }
                }
            }

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
                new System.Threading.Thread(() => Action()).Start();
        }
    }
    public static class Extentions
    {
        public static Task WaitForHandle(this GetRef<Action> Action) =>
           DelegateActions.WaitForHandle(Action);

        public static void RunOnNewThreade<t>(this Action Action) =>
            DelegateActions.RunOnNewThreade(Action);

        public static void RunOnNewThreade(this IEnumerable<Action> actions) =>
            DelegateActions.RunOnNewThreade(actions);
    }
}
