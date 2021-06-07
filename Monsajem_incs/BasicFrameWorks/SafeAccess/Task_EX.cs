using System;
using System.Linq;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;
namespace Monsajem_Incs.Async
{
    public static class Task_EX
    {
        public static async Task CheckAll(Task Task1,Task Task2)
        {
            var Checks = new Task[] { Task1, Task2 };
            var Result = await Task.WhenAny(Checks);
            if(Result.Id==Task1.Id)
            {
                await Task1;
                await Task2;
            }
            else
            {
                await Task2;
                await Task1;
            }    
        }
        public static async Task CheckAll(params Task[] Tasks)
        {
            while(Tasks.Length>0)
            {
                var Result = await Task.WhenAny(Tasks);
                await Result;
                Tasks = Tasks.Where((c) => c.Id != Result.Id).ToArray();
            }
        }
        public static async Task<Task> CheckAny(params Task[] Tasks)
        {
            var Result = await Task.WhenAny(Tasks);
            await Result;
            return Result;
        }

        public static async Task StartTryWaitAsQueue(params Task[] Tasks)
        {
            var Len = Tasks.Length;
            for(int i=0;i<Len;i++)
            {
                var Result = Tasks[i];
                Result.Start();
                try
                {
                    await Result;
                }catch{}
            }
        }
        public static async Task StartWaitAsQueue(params Task[] Tasks)
        {
            var Len = Tasks.Length;
            for (int i = 0; i < Len; i++)
            {
                var Result = Tasks[i];
                Result.Start();
                await Result;
            }
        }
        public static async Task StartTryWait(params Task[] Tasks)
        {
            var Len = Tasks.Length;
            for (int i = 0; i < Len; i++)
                Tasks[i].Start();
            await Task.WhenAll(Tasks);
        }
        public static async Task StartWait(params Task[] Tasks)
        {
            var Len = Tasks.Length;
            for (int i = 0; i < Len; i++)
                Tasks[i].Start();
            await CheckAll(Tasks);
        }

        public static async Task StartTryWaitAsQueue(params Func<Task>[] Actions)
        {
            var Len = Actions.Length;
            var Tasks = new Task[Len];
            for (int i = 0; i < Len; i++)
            {
                try
                {
                    await Actions[i]();
                }
                catch { }
            }
        }
        public static async Task StartWaitAsQueue(params Func<Task>[] Actions)
        {
            var Len = Actions.Length;
            for (int i = 0; i < Len; i++)
            {
                var Result = Actions[i];
                await Result();
            }
        }
        public static async Task StartTryWait(params Func<Task>[] Actions)
        {
            var Len = Actions.Length;
            var Tasks = new Task[Len];
            for (int i = 0; i < Len; i++)
                Tasks[i] = Task.Run(Actions[i]);
            await Task.WhenAll(Tasks);
        }
        public static async Task StartWait(params Func<Task>[] Actions)
        {
            var Len = Actions.Length;
            var Tasks = new Task[Len];
            for (int i = 0; i < Len; i++)
                Tasks[i] = Task.Run(Actions[i]);
            await CheckAll(Tasks);
        }

        public static async Task TimeOut(this Task Task,int TimeOut)
        {
            var Timer = Task.Delay(TimeOut);
            var Result = await Task.WhenAny(Task,Timer);
            if (Result.Id == Timer.Id)
                throw new Exception("Task Time Out!");
        }
        public static async Task<t> TimeOut<t>(this Task<t> task, int TimeOut)
        {
            var Timer = Task.Delay(TimeOut);
            var Result = await Task.WhenAny(task, Timer);
            if (Result.Id == Timer.Id)
                throw new Exception("Task Time Out!");
            return task.Result;
        }
    }
}