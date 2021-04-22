using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using Monsajem_Incs.DelegateExtentions;

namespace Monsajem_Incs.SafeAccess
{
    public class AsyncLocker<ResourceType>:IDisposable
    {
        public ResourceType Value;
        public event Action OnChanged;
        private event Action OnCommand;

        private Collection.Array.ArrayBased.DynamicSize.Array<Task>
            ChangedQueue = new Collection.Array.ArrayBased.DynamicSize.Array<Task>(50);

        private Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>
            ReadQueue = new Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>(50);
        private Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>
            WriteQueue = new Collection.Array.ArrayBased.DynamicSize.Array<Func<Task>>(50);
       
        private async void Start()
        {
            while(true)
            {
                Task Wait;
                lock (this)
                {
                    if (ReadQueue.Length > 0)
                    {
                        var Tasks = ReadQueue.ToArray();
                        ReadQueue.Clear();
                        Wait = Threading.Task_EX.StartTryWait(Tasks);
                    }
                    else if (WriteQueue.Length > 0)
                    {
                        var Tasks = WriteQueue.ToArray();
                        WriteQueue.Clear();
                        Wait = Threading.Task_EX.StartTryWaitAsQueue(Tasks);
                    }
                    else
                    {
                        if (Disposed)
                            return;
                        Wait = Actions.WaitForHandle(() => ref OnCommand);
                    }
                }

                await Wait;
            }
        }

        public AsyncLocker()

        {
            OnChanged = ()=>
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
            Func<Task> Waiter = async () =>
            {
                try
                {
                    await Action();
                }
                finally
                {
                    Done.SetResult();
                }
            };
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
            Func<Task> Waiter = async () =>
            {
                try
                {
                    await Action();
                }
                finally
                {
                    Done.SetResult();
                }
            };
            lock (this)
            {
                WriteQueue.Insert(Waiter);
                OnCommand?.Invoke();
            }
            await Done.Task;
        }

        public Task WaitForChange()
        {
            return Actions.WaitForHandle(() => ref OnChanged);
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
            lock(this)
            {
                AC();
            }
        }

        private bool Disposed;
        public void Dispose()
        {
            lock(this)
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