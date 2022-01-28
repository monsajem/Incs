using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Collection.Array.TreeBased;
namespace Monsajem_Incs.DynamicAssembly
{
    public static class DelegatesExtentions
    {
        public static Delegate RemoveAllDelegates(
            this Delegate Source, Delegate Removes)
        {
            var Result = Source;
            foreach (var Remove in Removes.GetInvocationList())
            {
                Result = Delegate.RemoveAll(Result, Remove);
            }
            return Result;
        }
    }

    public class BlocksCounter
    {
        public event Action OnClosedAllBlocks;
        private int _Count;
        public int Count
        {
            get => _Count;
        }

        public BlockContainer UseBlock()
        {
            _Count++;
            return new BlockContainer(this);
        }

        public class BlockContainer : IDisposable
        {
            private bool BlockEnded;
            private BlocksCounter Parent;

            internal BlockContainer(BlocksCounter Parent)
            {
                this.Parent = Parent;
            }

            public void Dispose()
            {
                if (BlockEnded == true)
                    throw new Exception("This block is disposed!");
                BlockEnded = true;
                Parent._Count--;
                if (Parent._Count == 0)
                    Parent.OnClosedAllBlocks?.Invoke();
                Parent = null;
            }
        }
    }

    public class RunOnceInBlock
    {
        public event Action OnClosedAllBlocks;
        private HashSet<string> Used;
        private BlocksCounter Counter;

        public RunOnceInBlock ()
        {
            Used = new HashSet<string>();
            Counter = new BlocksCounter();
            Counter.OnClosedAllBlocks += () => this.Used.Clear();
        }

        public int BlockLengths
        {
            get => Counter.Count;
        }

        public bool Use(string Name)
        {
            if (Used.Contains(Name)==false)
            {
                Used.Add(Name);
                return true;
            }
            else
                return false;
        }

        public void EndUse(string Name)
        {
            Used.Remove(Name);
        }

        public BlocksCounter.BlockContainer UseBlock()
        {
            return Counter.UseBlock();
        }
    }
}
