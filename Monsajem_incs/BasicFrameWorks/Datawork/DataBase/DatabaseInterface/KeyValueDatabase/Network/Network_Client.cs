using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Collection.Array.TreeBased;
using System.Linq.Expressions;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        private class IRemoteUpdateReciver<ValueType, KeyType>:
            IRemoteUpdateSender<ValueType,KeyType>
            where KeyType : IComparable<KeyType>
        {
            public Collection.Array.TreeBased.Array<KeyType> ShouldDelete = 
                new Collection.Array.TreeBased.Array<KeyType>();
            public int StartPos;
            public int ServerItemsCount;
            public int EndPos;

            public IRemoteUpdateReciver(
                IAsyncOprations Client,
                Table<ValueType, KeyType> Table,
                UpdateAble<KeyType>[] UpdateCodes,
                Func<KeyType, Task<ValueType>> GetItem,
                bool IsPartOfTable,
                int ServerItemsCount) :
                base(Client,Table, UpdateCodes,GetItem,IsPartOfTable)
            {
                this.ServerItemsCount = ServerItemsCount;
                EndPos = Math.Min(Table.UpdateAble.UpdateCodes.Length, ServerItemsCount) - 1;

                UpdateFromPosToUpCode = async (Pos, ClientUpCode) =>
                {
                    await UpdateNextItems();
                };
                UpdateFromPosToEnd = async (Pos) =>
                {
                    await UpdateNextItems();
                };

#if DEBUG_UpdateAble
                Debuger = async () =>
                {
                    {
                        var ClientUpdates = Table.UpdateAble.UpdateCodes;
                        var ServerUpdates = await Client.GetData(ClientUpdates);//1
                        if (ServerUpdates.Length != ClientUpdates.Length)
                            throw new FormatException("Updates are wrong. Count is Wrong.");
                        for (int i = 0; i < ServerUpdates.Length; i++)
                        {
                            var ClientUpdate = ClientUpdates[i];
                            var ServerUpdate = ServerUpdates[i];
                            if (ClientUpdate.Key.CompareTo(ServerUpdate.Key)!=0)
                                throw new FormatException("Updates are wrong. key is wrong.");
                            if (ClientUpdate.UpdateCode!=ServerUpdate.UpdateCode)
                                throw new FormatException("Updates are wrong. Update Code is wrong.");
                        }
                    }

                    {
                        var ClientDatas = Table.KeysInfo.Keys.ToArray();
                        var ServerDatas = await Client.GetData(ClientDatas);//2
                        if (ServerDatas.Length != ClientDatas.Length)
                            throw new FormatException("Updates are wrong. Count is Wrong.");
                        for (int i = 0; i < ServerDatas.Length; i++)
                        {
                            var ClientData = ClientDatas[i];
                            var ServerData = ServerDatas[i];
                            if (ClientData.CompareTo(ServerData) != 0)
                                throw new FormatException("Updates are wrong. key is wrong.");    
                        }
                    }
                };
#endif
            }

            private void Added(KeyType Key,ulong UpdateCode,ulong ParentUpdateCode=0)
            {
                Table.UpdateAble.Changed(Key,Key, UpdateCode);
                if (IsPartOfTable)
                    ParentTable.UpdateAble.Changed(Key, Key, UpdateCode);
                if (ShouldDelete.BinarySearch(Key).Index > -1)
                    ShouldDelete.BinaryDelete(Key);
            }

            private void Removed(KeyType Key)
            {
                Table.UpdateAble.DeleteDontUpdate(Key);
                ShouldDelete.BinaryInsert(Key);
            }

            private async Task UpdateNextItems()
            {
                var Len = await Client.GetData<int>();
                var ServerUpCodes = new ulong[Len];
                ulong[] ServerUpCodes_Parent=null;
                if (IsPartOfTable)
                    ServerUpCodes_Parent = new ulong[Len];
                for (int i = 0; i < Len; i++)
                {
                    var UpCode = await Client.GetData<ulong>();
                    if (IsPartOfTable)
                    {
                        var ParentUpCode = await Client.GetData<ulong>();
                        if (ParentTable.UpdateAble.IsExist(ParentUpCode))
                        {
                            var ParentUpDate = ParentTable.UpdateAble[ParentUpCode];
                            if(PartTable.IsExist(ParentUpDate.Key)==false)
                                PartTable.Accept(ParentUpDate.Key);
                            Added(ParentUpDate.Key, UpCode);
                            await Client.SendData(false);
                            StartPos++;
                            Len--;
                            i--;
                        }
                        else
                        {
                            await Client.SendData(true);
                            ServerUpCodes[i] = UpCode;
                            ServerUpCodes_Parent[i] = ParentUpCode;
                        }
                    }
                    else
                    {
                        await Client.SendData(true);
                        ServerUpCodes[i] = UpCode;
                    }
                }
                for (int i = 0; i < Len; i++)
                {
                    var Value = await Client.GetData<ValueType>();
                    var Key = Table.GetKey(Value);
                    var Update = new UpdateAble<KeyType>()
                    { Key = Key, UpdateCode = ServerUpCodes[i] };
                    Table.UpdateOrInsert(Key, (c) =>
                    {
                         Table.MoveRelations(c, Value);
                         return Value;
                    });
                    if(IsPartOfTable)
                        Added(Key, ServerUpCodes[i],ServerUpCodes_Parent[i]);
                    else
                        Added(Key, ServerUpCodes[i]);
                    StartPos++;
                }
            }
            
            public async Task FindLastTrue()
            {
                var StartPos = this.StartPos;
                var EndPos = this.EndPos;
                var EndPosParameter = EndPos;
                var Update = Table.UpdateAble;
                if (StartPos == EndPos && 
                    Update.UpdateCodes[StartPos].UpdateCode !=
                        await this.GetUpdateCodeAtPos(StartPos))
                {
                    this.StartPos--;
                    return;
                }
                while (StartPos < EndPos &&
                       EndPos <= EndPosParameter)
                {
                    if (Update.UpdateCodes[EndPos].UpdateCode !=
                        await this.GetUpdateCodeAtPos(EndPos))
                        EndPos = (EndPos + StartPos) / 2;
                    else
                    {
                        var OldEnd = EndPos;
                        EndPos = (EndPos * 2) - StartPos;
                        StartPos = OldEnd;
                    }
                }
                if (EndPos > EndPosParameter)
                    this.StartPos = EndPosParameter;
                else
                    this.StartPos = EndPos;
            }

            public void DeleteExtraItems(ulong UpdateCode)
            {
                UpdateAble<KeyType> MyUpCode = null;
                while (Table.UpdateAble.UpdateCodes.Length > StartPos)
                {
                    MyUpCode = Table.UpdateAble.UpdateCodes[StartPos];
                    if (MyUpCode.UpdateCode < UpdateCode)
                    {
                        Table.UpdateAble.DeleteDontUpdate(MyUpCode.Key);
                        ShouldDelete.Insert(MyUpCode.Key);
                    }
                    else
                        return;
                }
            }

            public void DeleteItemsFrom(int Pos)
            {
                for(; Pos < Table.UpdateAble.UpdateCodes.Length; Pos++)
                {
                    var MyUpCode = Table.UpdateAble.UpdateCodes[Pos];
                    Table.UpdateAble.DeleteDontUpdate(MyUpCode.Key);
                    ShouldDelete.Insert(MyUpCode.Key);
                }
            }

            public async Task MakeUpdate(ulong ServerLastUpCode)
            {
                if (Table.UpdateAble.UpdateCode > ServerLastUpCode)
                {
                    Table.UpdateAble.Clear();
                    if (IsPartOfTable)
                    {
                        ParentTable.UpdateAble.Clear();
                    }
                }
                while (StartPos <= EndPos)
                {
                    await FindLastTrue();
                    StartPos++;
                    if (StartPos >= ServerItemsCount)
                        break;
                    else
                    {
                        var NextUpCode = await GetUpdateCodeAtPos(StartPos);
                        DeleteExtraItems(NextUpCode);
                        if (StartPos >= Table.UpdateAble.UpdateCodes.Length)
                            break;
                        var MyUpCode = Table.UpdateAble.UpdateCodes[StartPos];
                        if (MyUpCode.UpdateCode > NextUpCode)
                            await UpdateFromPosToUpCode(StartPos, MyUpCode.UpdateCode);
                    }
                }
                if (StartPos < ServerItemsCount)
                    await UpdateFromPosToEnd(StartPos);
                else
                    DeleteItemsFrom(ServerItemsCount);

                if(IsPartOfTable)
                    foreach(var Delete in ShouldDelete)
                    {
                        if (await IsExistAtParent(Delete))
                            PartTable.Ignore(Delete);
                        else
                            PartTable.Delete(Delete);
                    }
                else
                    foreach (var Delete in ShouldDelete)
                        Table.Delete(Delete);

                Table.UpdateAble.UpdateCode.Save(ServerLastUpCode);

                if (IsPartOfTable)
                    PartTable.SaveToParent();
#if DEBUG_UpdateAble
                await Debuger();
#endif
            }
        }

        private static async Task<bool> I_GetUpdate<ValueType, KeyType>(
            this IAsyncOprations Client,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate,
            Action<KeyType> Deleted,
            bool IsPartOfTable)
            where KeyType : IComparable<KeyType>
        {
            if (Table._UpdateAble == null)
                Table._UpdateAble = new UpdateAbles<KeyType>(
                                            Table.BasicActions.UpdateCode,
                                            Table.BasicActions.Items.Select((c)=>(Table.GetKey(c.Value),c.UpdateCode)));
            var Result = false;

            await Client.SendData(Table.UpdateAble.UpdateCode.Value);//1
            var LastUpdateCode = await Client.GetData<ulong>();//2
            if (Table.UpdateAble.UpdateCode.Value != LastUpdateCode)
            {
                var ServerItemsCount = await Client.GetData<int>();//3
                var Remote = new IRemoteUpdateReciver<ValueType, KeyType>(
                                    Client, Table, null, null, IsPartOfTable,ServerItemsCount);
                await Client.Remote(Remote,
                async (Remote) =>await Remote.MakeUpdate(LastUpdateCode));
            }
            return Result;
        }
    }
}