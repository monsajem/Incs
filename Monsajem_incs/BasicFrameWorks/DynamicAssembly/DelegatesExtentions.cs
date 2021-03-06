﻿using System;
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

    public struct RunOnceInStack
    {
        private SortedSet<string> RunIfNotRuning_Name;
        public bool Use(string Name)
        {
            if (RunIfNotRuning_Name == null)
                RunIfNotRuning_Name = new SortedSet<string>();
            if (RunIfNotRuning_Name.Contains(Name)==false)
            {
                RunIfNotRuning_Name.Add(Name);
                return true;
            }
            else
                return false;
        }

        public void EndUse(string Name)
        {
            RunIfNotRuning_Name.Remove(Name);
        }

        private bool RunIfAnyNotRuning_flag;
        public bool AnyUse()
        {
            if (RunIfNotRuning_Name == null)
                RunIfNotRuning_Name =new SortedSet<string>();
            if (RunIfNotRuning_Name.Count == 0 &&
                RunIfAnyNotRuning_flag == false)
            {
                RunIfAnyNotRuning_flag = true;
                return true;
            }
            else
                return false;
        }

        public void EndAnyUse()
        {
            RunIfAnyNotRuning_flag = true;
        }
    }

    public class RunOnceInBlock:IDisposable 
    {
        public event Action OnEndBlocks;
        private SortedSet<string> Used;
        private int Blocks;
        public int BlockLengths
        {
            get => Blocks;
        }
        public bool Use(string Name)
        {
            if (Used == null)
                Used = new SortedSet<string>();
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

        public RunOnceInBlock Block()
        {
            Blocks++;
            return this;
        }

        public void EndBlock()
        {
            Blocks--;
            if (Blocks == 0)
            {
                Used = new SortedSet<string>();
                OnEndBlocks?.Invoke();
            }
        }

        public void Dispose()
        {
            Blocks--;
            if (Blocks == 0)
            {
                Used = new SortedSet<string>();
                OnEndBlocks?.Invoke();
            };
        }
    }
}
