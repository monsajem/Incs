using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monsajem_Incs.Async
{
    public class Locked : IDisposable
    {
        internal Locked()
        { }
        internal Action Unlock;

        public static Locked operator +(Locked a, Locked b)
        {
            return new Locked() { Unlock = a.Unlock + b.Unlock };
        }

        public static Locked operator &(Locked a, Locked b)
        {
            return new Locked() { Unlock = a.Unlock + b.Unlock };
        }

        public void Dispose()
        {
            Unlock();
            System.GC.SuppressFinalize(this);
        }
    }

    public class Locker<ResourceType>
    {
        public event Action OnChanged;
        public bool IgnoreChangeEvents;
        private Collection.Array.ArrayBased.DynamicSize.Array<Task>
            ChangedQueue = new(50);

        public Locker()
        {
            OnChanged = () =>
            {
                if (ChangedQueue.Length > 0)
                    ChangedQueue.Pop().Start();
            };
        }

        private ResourceType _Value;
        public ResourceType Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                if (IgnoreChangeEvents == false)
                    OnChanged?.Invoke();
            }
        }
        public ResourceType LockedValue
        {
            get
            {
                using (Lock())
                    return _Value;
            }
            set
            {
                using (Lock())
                    _Value = value;
                if (IgnoreChangeEvents == false)
                    OnChanged?.Invoke();
            }
        }

        public Task WaitForChange()
        {
            return DelegateActions.WaitForHandle(() => ref OnChanged);
        }

        public Task WaitForChangeQuque() => WaitForChangeQuque(() => { });
        public Task WaitForChangeQuque(Action action)
        {
            var task = new Task(() =>
            {
                IgnoreChangeEvents = true;
                action();
                IgnoreChangeEvents = false;
            });
            ChangedQueue.Insert(task, 0);
            return task;
        }

        public Locked Lock()
        {
            Monitor.Enter(this);
            return new Locked()
            {
                Unlock = () => Monitor.Exit(this)
            };
        }

        public void Changed() => OnChanged?.Invoke();

        public void Action(Action AC)
        {
            lock (this)
            {
                AC();
            }
        }
    }
}