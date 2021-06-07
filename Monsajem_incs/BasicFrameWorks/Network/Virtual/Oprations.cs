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
using Monsajem_Incs.Async;

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

        public async Task<t> SendData<t>(t Data)
        {
#if DEBUG
            await SendParity(OprationType.SendData, OprationType.GetData);
#endif
            await Socket.Send(Data);
            return Data;
        }

        public async Task<t> GetData<t>()
        {
#if DEBUG
            await GetParity(OprationType.GetData, OprationType.SendData);
#endif
            return await Socket.Recive<t>();
        }


        public void Dispose()
        { }

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

        public Task RemoteServices(params Func<object, Task<object>>[] Services)
        {
            throw new NotImplementedException();
        }

        public Task RunServices(Func<Func<(int Position, object Data), Task<object>>, Task> Service)
        {
            throw new NotImplementedException();
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

        public new void Stop()
        {
            throw new NotImplementedException();
        }
    }
}