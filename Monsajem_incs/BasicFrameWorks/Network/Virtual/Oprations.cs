using System;
using System.Linq;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using static Monsajem_Incs.Collection.Array.Extentions;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.DynamicAssembly;
using System.Collections.Generic;
using Monsajem_Incs.Net.Base;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Monsajem_Incs.SafeAccess;

namespace Monsajem_Incs.Net.Virtual
{

    public class Socket
    {
        public readonly Socket OtherSide;
        private AsyncLocker<Array<object>> Data =
            new AsyncLocker<Array<object>>() { Value = new Array<object>(10) };

        public Socket()
        {
            this.OtherSide = new Socket(this);
        }
        private Socket(Socket OtherSide)
        {
            this.OtherSide = OtherSide;
        }

        public async Task Send(object Data)
        {
            await OtherSide.Data.LockWrite(async () =>
            {
                OtherSide.Data.Value.Insert(Data, 0);
                OtherSide.Data.Changed();
            });
        }
        public Task Send<t>(t Data) => Send((object)Data);

        public async Task<object> Recive()
        {
            while(true)
            {
                object Result = null;
                Task Wait = null;
                await Data.LockWrite(async () =>
                {
                    if (Data.Value.Length > 0)
                    {
                        Result = Data.Value.Pop();
                    }
                    else
                        Wait = Data.WaitForChangeQuque();
                });
                if (Wait==null)
                    return Result;
                await Wait;
                Wait = null;
            }
        }
        public async Task<t> Recive<t>() => (t)await Recive();
    }

    public class AsyncOprations :
        IDisposable, IAsyncOprations
    {
        private Socket Socket;
        public AsyncOprations(Socket Socket)
        {
            this.Socket = Socket;
        }

        public async Task<bool> SendCondition(bool Condition)
        {
#if DEBUG
            await SendParity(OprationType.SendCondition, OprationType.GetCondition);
#endif
            await Socket.Send(Condition);
            return Condition;
        }

        public async Task<bool> GetCondition()
        {
#if DEBUG
            await GetParity(OprationType.GetCondition, OprationType.SendCondition);
#endif
            return await Socket.Recive<bool>();
        }

        public async Task SendData<t>(t Data)
        {
#if DEBUG
            await SendParity(OprationType.SendData, OprationType.GetData);
#endif
            await Socket.Send(Data);
        }

        public async Task<t> GetData<t>()
        {
#if DEBUG
            await GetParity(OprationType.GetData, OprationType.SendData);
#endif
            return await Socket.Recive<t>();
        }

        public async Task<t> GetData<t>(t SampleType)
        {
            return await GetData<t>();
        }

        public async Task SendArray<t>(IEnumerable<t> Datas, Action<t> DataSended = null)
        {
            await Socket.Send(Datas.Count());
            foreach (var data in Datas)
            {
                await Socket.Send(data);
                DataSended?.Invoke(data);
            }
        }

        public async Task GetArray<t>(Action<t> DataCome)
        {
            for (int i = await Socket.Recive<int>(); i > 0; i--)
                DataCome(await Socket.Recive<t>());
        }

        public async Task<t[]> GetArray<t>()
        {
            var Datas = new t[await Socket.Recive<int>()];
            for (int i = 0; i < Datas.Length; i++)
                Datas[i] = await Socket.Recive<t>();
            return Datas;
        }

        //0
        public async Task RunOnOtherSide(Action<ISyncOprations> Action)
        {
            await SendData<byte>(0);
            await SendData(Action);
        }
        //1
        public async Task RunOnOtherSide<Data>(Action<ISyncOprations, Data> Action)
        {
            await SendData<byte>(1);
            await SendData(Action);
        }
        //2
        public async Task RunOnOtherSide(Action Action)
        {
            await SendData<byte>(2);
            await SendData(Action);
        }
        //3
        public async Task RunOnOtherSide<Data>(Action<Data> Action)
        {
            await SendData<byte>(3);
            await SendData(Action);
        }
        //4
        public async Task<Result> RunOnOtherSide<Result>(Func<Result> Func)
        {
            await SendData<byte>(4);
            await SendData<Delegate>(Func);
            return (Result)await GetData<object>();
        }
        //5
        public async Task<Result> RunOnOtherSide<Result, Data>(Func<Data, Result> Func)
        {
            await SendData<byte>(5);
            await SendData<Delegate>(Func);
            return (Result)await GetData<object>();
        }
        //6
        public async Task RunOnOtherSide(Func<IAsyncOprations, Task> Action)
        {
            await SendData<byte>(6);
            await SendData<object>(Action);
        }
        //7
        public async Task RunOnOtherSide<Data>(Func<IAsyncOprations, Data, Task> Action)
        {
            await SendData<byte>(7);
            await SendData(Action);
        }
        //8
        public async Task RunOnOtherSide(Func<Task> Action)
        {
            await SendData<byte>(8);
            await SendData<object>(Action);
        }
        //9
        public async Task RunOnOtherSide<Data>(Func<Data, Task> Action)
        {
            await SendData<byte>(9);
            await SendData(Action);
        }
        //10
        public async Task<Result> RunOnOtherSide<Result>(Func<Task<Result>> Func)
        {
            await SendData<byte>(10);
            await SendData<Delegate>(Func);
            return await GetData<Result>();
        }
        //11
        public async Task<Result> RunOnOtherSide<Result, Data>(Func<Data, Task<Result>> Func)
        {
            await SendData<byte>(11);
            await SendData<Delegate>(Func);
            return await GetData<Result>();
        }
        public Task RunRecivedAction(
            Action<Delegate> Permition = null)
        {
            return RunRecivedAction<string>(Permition, null);
        }
        public async Task RunRecivedAction<DataType>(
            Action<Delegate> Permition, DataType Data)
        {
            var Type = await GetData<byte>();
            switch (Type)
            {
                case 0:
                    {
                        var dg = await GetData<Action<ISyncOprations>>();
                        Permition?.Invoke(dg);
                        dg.Invoke((ISyncOprations)this); break;
                    }
                case 1:
                    {
                        var dg = await GetData<Action<ISyncOprations, DataType>>();
                        Permition?.Invoke(dg);
                        dg.Invoke((ISyncOprations)this, Data); break;
                    }
                case 2:
                    {
                        var dg = await GetData<Action>();
                        Permition?.Invoke(dg);
                        dg.Invoke(); break;
                    }
                case 3:
                    {
                        var dg = await GetData<Action<DataType>>();
                        Permition?.Invoke(dg);
                        dg.Invoke(Data); break;
                    }
                case 4:
                    {
                        var dg = await GetData<Delegate>();
                        Permition?.Invoke(dg);
                        await SendData(dg.DynamicInvoke()); break;
                    }
                case 5:
                    {
                        var dg = await GetData<Delegate>();
                        Permition?.Invoke(dg);
                        await SendData(dg.DynamicInvoke(Data)); break;
                    }
                case 6:
                    {
                        var dg = await GetData<Func<IAsyncOprations, Task>>();
                        Permition?.Invoke(dg);
                        await dg.Invoke(this); break;
                    }
                case 7:
                    {
                        var dg = await GetData<Func<IAsyncOprations, DataType, Task>>();
                        Permition?.Invoke(dg);
                        await dg.Invoke((ISyncOprations)this, Data); break;
                    }
                case 8:
                    {
                        var dg = await GetData<Func<Task>>();
                        Permition?.Invoke(dg);
                        await dg.Invoke(); break;
                    }
                case 9:
                    {
                        var dg = await GetData<Func<DataType, Task>>();
                        Permition?.Invoke(dg);
                        await dg.Invoke(Data); break;
                    }
                case 10:
                    {
                        var dg = await GetData<Delegate>();
                        Permition?.Invoke(dg);
                        var Result = (Task)dg.DynamicInvoke();
                        await Result;
                        await SendData(Result.GetType().GetProperty("Result").GetValue(Result)); break;
                    }
                case 11:
                    {
                        var dg = await GetData<Delegate>();
                        Permition?.Invoke(dg);
                        var Result = (Task)dg.DynamicInvoke(Data);
                        await Result;
                        await SendData(Result.GetType().GetProperty("Result").GetValue(Result)); break;
                    }
                    throw new Exception();
            }
        }

        public async Task Sync(Action Action)
        {
            try
            {
                Action();
                await SendCondition(false);
            }
            catch (Exception ex)
            {
                await SendCondition(true);
#if DEBUG
                await SendData(ex.Message);
                await SendData(ex.StackTrace);
#else
                await SendData(ex.Message);
#endif
            }
        }

        public async Task Sync()
        {
            if (await GetCondition())
            {
#if DEBUG
                Exception ex = new Exception("Other side net sync Error >> " +
                    await GetData<string>() + "\n At >> " + await GetData<string>());
#else
                Exception ex =new Exception("Other side net sync Error >> "+
                    await GetData<string>());
#endif
                throw ex;
            }
        }


        public void Dispose()
        { }

        public Task Remote<t>(t obj)
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }

#if DEBUG

        private async Task SendParity(OprationType ThisType, OprationType ThatType)
        {

            await Socket.Send(ThisType);

            var Status = await Socket.Recive<OprationType>();
            if (Status != ThatType)
            {
                var EX = "Wrong opration, This side is '" + ThisType.ToString() + "'" +
                                      " And other side must be '" + ThatType.ToString() + "'" +
                                      " But that is '" + Status.ToString() + "'";
                throw new InvalidOperationException(EX);
            }
        }

        private async Task GetParity(
                      OprationType ThisType,
                      OprationType ThatType)
        {
            var Status = await Socket.Recive<OprationType>();
            await Socket.Send(ThisType);
            if (Status != ThatType)
            {
                var EX = "Wrong opration, This side is '" + ThisType.ToString() + "'" +
                        " And other side must be '" + ThatType.ToString() + "'" +
                        " But that is '" + Status.ToString() + "'";
                throw new InvalidOperationException(EX);
            }
        }
#endif

    }

    public class SyncOprations :
        AsyncOprations,
        ISyncOprations
    {
        public SyncOprations(Socket Socket) :
            base(Socket)
        { }

        public new bool SendCondition(bool Condition) =>
            base.SendCondition(Condition).GetAwaiter().GetResult();

        public new bool GetCondition() =>
            base.GetCondition().GetAwaiter().GetResult();

        public new void SendData<t>(t Data) =>
            base.SendData(Data).Wait();

        public new t GetData<t>() =>
            base.GetData<t>().GetAwaiter().GetResult();

        public new t GetData<t>(t SampleType) =>
            base.GetData(SampleType).GetAwaiter().GetResult();

        public new void SendArray<t>(IEnumerable<t> Datas, Action<t> DataSended = null) =>
            base.SendArray(Datas, DataSended).Wait();

        public new void GetArray<t>(Action<t> DataCome) =>
            base.GetArray(DataCome).Wait();

        public new t[] GetArray<t>() =>
            base.GetArray<t>().GetAwaiter().GetResult();

        public new void Sync(Action Action) =>
            base.Sync(Action).Wait();

        public new void Sync() =>
            base.Sync().Wait();

        
        public new void RunRecivedAction<DataType>(Action<Delegate> Permition, DataType Data) =>
            base.RunRecivedAction(Permition, Data).GetAwaiter().GetResult();
        public void RunRecivedAction() =>
            base.RunRecivedAction().GetAwaiter().GetResult();

        public new void RunOnOtherSide(Func<IAsyncOprations, Task> Action)
        {
            throw new NotImplementedException();
        }

        public new void RunOnOtherSide<Data>(Func<IAsyncOprations, Data, Task> Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide(Func<Task> Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Func<Data, Task> Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result>(Func<Task<Result>> Func) =>
            base.RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result, Data>(Func<Data, Task<Result>> Func) =>
            base.RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new void RunOnOtherSide(Action<ISyncOprations> Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Action<ISyncOprations, Data> Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide(Action Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Action<Data> Action) =>
            base.RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result>(Func<Result> Func) =>
            base.RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result, Data>(Func<Data, Result> Func) =>
            base.RunOnOtherSide(Func).GetAwaiter().GetResult();

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}