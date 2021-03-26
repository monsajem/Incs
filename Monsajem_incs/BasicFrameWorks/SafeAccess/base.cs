using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
namespace Monsajem_Incs.SafeAccess
{
    public class SafeAccess<KeyType>
        where KeyType:IComparable<KeyType>
    {
        private ReaderWriterLockSlim ThisSafe = new ReaderWriterLockSlim();

        private SortedDictionary<KeyType, ReaderWriterLockSlim> Keys = 
            new SortedDictionary<KeyType, ReaderWriterLockSlim>();

        public void Read(KeyType Key, Action Action)
        {
            var RwLock= FindRW(Key);
            RwLock.EnterReadLock();
            Action();
            RwLock.ExitReadLock();
            CheckForRelase(RwLock,Key);
        }

        public void Write(KeyType Key, Action Action)
        {
            var RwLock = FindRW(Key);
            RwLock.EnterWriteLock();
            Action();
            RwLock.ExitWriteLock();
            CheckForRelase(RwLock, Key);
        }

        private void CheckForRelase(ReaderWriterLockSlim RwLock, KeyType Key)
        {
            if (RwLock.CurrentReadCount == 0 &
                RwLock.WaitingReadCount == 0 &
                RwLock.WaitingWriteCount == 0)
            {
                ThisSafe.EnterWriteLock();
                if (Keys.ContainsKey(Key))
                    Keys.Remove(Key);
                ThisSafe.ExitWriteLock();
            }
        }

        private ReaderWriterLockSlim FindRW(KeyType Key)
        {
            ThisSafe.EnterWriteLock();

            ReaderWriterLockSlim RwLock;
            Keys.TryGetValue(Key, out RwLock);
            if (RwLock==null)
            {
                RwLock = new ReaderWriterLockSlim();
                Keys.Add(Key, RwLock);
            }
            ThisSafe.ExitWriteLock();
            return RwLock;
        }
    }
}
