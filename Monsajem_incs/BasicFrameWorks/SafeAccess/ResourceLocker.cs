using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Monsajem_Incs.Async
{
    public class AsyncLocker<ResourceType> : IDisposable
    {
        public ResourceType Value;
        public event Action OnChanged;
        private event Action OnCommand;

        private Collection.Array.ArrayBased.DynamicSize.Array<Task>
            ChangedQueue = new(50);

        private Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>
            ReadQueue = new(50);
        private Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>
            WriteQueue = new(50);

        private async void Start()
        {
            while (true)
            {
                Task Wait;
                lock (this)
                {
                    if (ReadQueue.Length > 0)
                    {
                        var Tasks = ReadQueue.ToArray();
                        ReadQueue.Clear();
                        Wait = Async.Task_EX.StartTryWait(Tasks);
                    }
                    else if (WriteQueue.Length > 0)
                    {
                        var Tasks = WriteQueue.ToArray();
                        WriteQueue.Clear();
                        Wait = Async.Task_EX.StartTryWaitAsQueue(Tasks);
                    }
                    else
                    {
                        if (Disposed)
                            return;
                        Wait = DelegateActions.WaitForHandle(() => ref OnCommand);
                    }
                }

                await Wait;
            }
        }

        public AsyncLocker()

        {
            OnChanged = () =>
            {
                lock (this)
                {
                    if (ChangedQueue.Length > 0)
                        ChangedQueue.Pop().Start();
                }
            };
            Start();
        }

        public async Task LockRead(Func<Task> Action)
        {
            var Done = AsyncTaskMethodBuilder.Create();
            async Task Waiter()
            {
                try
                {
                    await Action();
                }
                finally
                {
                    Done.SetResult();
                }
            }
            lock (this)
            {
                ReadQueue.Insert(Waiter);
                OnCommand?.Invoke();
            }
            await Done.Task;
        }

        public async Task LockWrite(Func<Task> Action)
        {
            var Done = AsyncTaskMethodBuilder.Create();
            async Task Waiter()
            {
                try
                {
                    await Action();
                }
                finally
                {
                    Done.SetResult();
                }
            }
            lock (this)
            {
                WriteQueue.Insert(Waiter);
                OnCommand?.Invoke();
            }
            await Done.Task;
        }

        public Task WaitForChange()
        {
            return DelegateActions.WaitForHandle(() => ref OnChanged);
        }

        public async Task WaitForChangeQuque()
        {
            var Waiter = new Task(() => { });
            lock (this)
                ChangedQueue.Insert(Waiter, 0);
            await Waiter;
        }

        public void Changed() => OnChanged?.Invoke();

        public void Action(Action AC)
        {
            lock (this)
            {
                AC();
            }
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