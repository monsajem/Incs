using System;
using Monsajem_Incs.Net.Base.Socket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Net.Base.Service
{
    public interface ISyncOprations:
        IAsyncOprations
    {
        public new void SendData<t>(t Data) =>
                (this as IAsyncOprations).SendData(Data).Wait();
        public new t GetData<t>() =>
            (this as IAsyncOprations).GetData<t>().GetAwaiter().GetResult();

        public new t GetData<t>(t SampleType) =>
            (this as IAsyncOprations).GetData(SampleType).GetAwaiter().GetResult();

        public new void SendArray<t>(IEnumerable<t> Datas, Action<t> DataSended = null) =>
            (this as IAsyncOprations).SendArray(Datas, DataSended).Wait();

        public new void GetArray<t>(Action<t> DataCome) =>
            (this as IAsyncOprations).GetArray(DataCome).Wait();

        public new t[] GetArray<t>() =>
            (this as IAsyncOprations).GetArray<t>().GetAwaiter().GetResult();

        public new void Sync(Action Action) =>
            (this as IAsyncOprations).Sync(Action).Wait();

        public new void Sync() =>
            (this as IAsyncOprations).Sync().Wait();

        public new void RunRecivedAction<DataType>(Action<Delegate> Permition, DataType Data) =>
            (this as IAsyncOprations).RunRecivedAction(Permition, Data).GetAwaiter().GetResult();
        public void RunRecivedAction() =>
            (this as IAsyncOprations).RunRecivedAction().GetAwaiter().GetResult();

        public new void RunOnOtherSide(Func<IAsyncOprations, Task> Action)=>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Func<IAsyncOprations, Data, Task> Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide(Func<Task> Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Func<Data, Task> Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result>(Func<Task<Result>> Func) =>
            (this as IAsyncOprations).RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result, Data>(Func<Data, Task<Result>> Func) =>
            (this as IAsyncOprations).RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new void RunOnOtherSide(Action<ISyncOprations> Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Action<ISyncOprations, Data> Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide(Action Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new void RunOnOtherSide<Data>(Action<Data> Action) =>
            (this as IAsyncOprations).RunOnOtherSide(Action).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result>(Func<Result> Func) =>
            (this as IAsyncOprations).RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new Result RunOnOtherSide<Result, Data>(Func<Data, Result> Func) =>
            (this as IAsyncOprations).RunOnOtherSide(Func).GetAwaiter().GetResult();

        public new void RemoteServices(params Func<object, Task<object>>[] Services)=>
             (this as IAsyncOprations).RemoteServices(Services).GetAwaiter().GetResult();

        public new void RunServices(Func<Func<(int Position, object Data), Task<object>>, Task> Service) =>
             (this as IAsyncOprations).RunServices(Service).GetAwaiter().GetResult();

        new void Stop();
    }
    public class SyncOprations<AddressType> :
        AsyncOprations<AddressType>,
        ISyncOprations
    {
        public SyncOprations(
            ClientSocket<AddressType> Client, bool IsServer):
            base(Client, IsServer){}        
        public new void Stop() =>
            base.Stop().Wait();
    }
}