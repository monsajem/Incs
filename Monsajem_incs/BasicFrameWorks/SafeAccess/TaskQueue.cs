using System;
using System.Threading.Tasks;

namespace Monsajem_Incs.Async
{
    public class AsyncTaskQueue : IDisposable
    {
        private event Action OnCommand;

        private Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>
            TaskQueue = new(50);

        public AsyncTaskQueue()
        {
            Start();
        }

        private async void Start()
        {
            while (true)
            {
                Task WaitForNewItems = null;
                Func<Task>[] Tasks = null;
                lock (this)
                {
                    if (TaskQueue.Length > 0)
                    {
                        Tasks = TaskQueue.ToArray();
                        TaskQueue.Clear();
                    }
                    else
                    {
                        if (Disposed)
                            return;
                        WaitForNewItems = DelegateActions.WaitForHandle(() => ref OnCommand);
                    }
                }
                if (WaitForNewItems == null)
                    await Async.Task_EX.StartTryWaitAsQueue(Tasks);
                else
                    await WaitForNewItems;
            }
        }

        public Task AddToQueue(Func<Task> Action) =>
            AddToQueue<object>(async () =>
            {
                await Action();
                return null;
            });


        public Task<t> AddToQueue<t>(Func<Task<t>> Action)
        {
            t Result = default;
            Exception TaskEx = null;
            var Done = new Task<t>(() =>
            {
                return TaskEx != null ? throw TaskEx : Result;
            });
            async Task Waiter()
            {
                try
                {
                    Result = await Action();
                    Done.Start();
                }
                catch (Exception ex)
                {
                    TaskEx = ex;
                    Done.Start();
                }
            }
            lock (this)
            {
                TaskQueue.Insert(Waiter);
                OnCommand?.Invoke();
            }
            return Done;
        }

        private bool Disposed;
        public void Dispose()
        {
            lock (this)
            {
                if (Disposed == false)
                {
                    Disposed = true;
                    OnCommand?.Invoke();
                }
            }
        }
    }
}