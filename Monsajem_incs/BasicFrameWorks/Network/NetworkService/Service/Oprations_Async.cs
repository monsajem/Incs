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

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class Remotable : Attribute
    { }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class Syncable : Attribute
    { }
    public interface IAsyncOprations
    {
        Task<t> SendData<t>(t Data);
        Task<t> GetData<t>();
        public async Task<t> GetData<t>(t SampleType) => await GetData<t>();
        public async Task SendArray<t>(IEnumerable<t> Datas, Action<t> DataSended = null)
        {
            await SendData(Datas.Count());
            foreach (var data in Datas)
            {
                await SendData(data);
                DataSended?.Invoke(data);
            }
        }
        public async Task GetArray<t>(Action<t> DataCome)
        {
            for (int i = await GetData<int>(); i > 0; i--)
                DataCome(await GetData<t>());
        }
        public async Task<t[]> GetArray<t>()
        {
            var Datas = new t[await GetData<int>()];
            for (int i = 0; i < Datas.Length; i++)
                Datas[i] = await GetData<t>();
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

        private static
            (FieldInfo[] RemoteFields,
             FieldInfo[] SyncFields) GetRemoteFields<t>()
        {
            static FieldInfo[] GetFields<AttributeType>()
            {
                var Fields = DynamicAssembly.TypeController.GetAllFields(typeof(t), null, (c) =>
                c.GetCustomAttributes(typeof(AttributeType)).Count() > 0);
                foreach (var Field in Fields)
                    if (Field.FieldType.BaseType != typeof(MulticastDelegate))
                        throw new Exception(Field.Name + " Should be strong type delegate.");
                return Fields;
            }

            var RemoteFields = GetFields<Remotable>();
            var SyncFields = DynamicAssembly.TypeController.GetAllFields(typeof(t), null, (c) =>
                c.GetCustomAttributes(typeof(Syncable)).Count() > 0);
            foreach (var SyncField in SyncFields)
            {
                var FieldType = SyncField.FieldType;
                if (FieldType.BaseType != typeof(MulticastDelegate))
                    throw new Exception(SyncField.Name + " Should be strong type delegate.");
                var Method = FieldType.GetMethod("Invoke");
                if (Method.ReturnType != typeof(void) &&
                   Method.ReturnType != typeof(Task))
                    throw new Exception(SyncField.Name + " Should haven't result, you can use SyncableServerResult or SyncableClientResult.");
            }

            return (RemoteFields, SyncFields);
        }

        public Task Remote<t>(t obj, Func<t, Task> Talk) =>
            Remote<t, object>(obj, async (c) => { await Talk(c); return null; });
        public async Task<r> Remote<t, r>(t obj, Func<t, Task<r>> Talk)
        {
            var Fields = GetRemoteFields<t>();
            var Service = new Service<byte>(this);
            var SendQueue = new Async.AsyncTaskQueue();
            var ReciveQueue = new Async.AsyncTaskQueue();

            Func<byte, object[], bool, Task<object>> Request = null;
            byte Address = 0;
            Func<FieldInfo, Delegate> MakeDelegate = (Field) =>
             {
                 var LocAddress = Address++;
                 var LocRequest = Request;
                 var DG = DynamicAssembly.TypeController.CreateDelegateWrapper(Field.FieldType,
                          (inputs) =>
                          {
                              LocRequest(LocAddress, inputs, false).Wait();
                          },
                          (inputs) =>
                          {
                              return LocRequest(LocAddress, inputs, true).GetAwaiter().GetResult();
                          },
                          async (inputs) =>
                          {
                              await LocRequest(LocAddress, inputs, false);
                          },
                          async (inputs) =>
                          {
                              return await LocRequest(LocAddress, inputs, true);
                          });
                 return DG;
             };

            {// Remote
                Request =
                async (byte Address,
                       object[] inputs,
                       bool HaveResult) =>
                {
                    Task SendTask;
                    Task<object> ReciveTask;
                    lock (Service)
                    {
                        SendTask = SendQueue.AddToQueue(async () =>
                        {
                            await Service.Request(Address);
                            await SendData(inputs);
                        });
                        ReciveTask = ReciveQueue.AddToQueue(async () =>
                        {
                            await Sync();
                            if (HaveResult)
                                return await GetData<object>();
                            else
                                return null;
                        });
                    }
                    await SendTask;
                    return await ReciveTask;
                }; ;
                for (int i = 0; i < Fields.RemoteFields.Length; i++)
                {
                    var Field = Fields.RemoteFields[i];
                    Field.SetValue(obj, MakeDelegate(Field));
                }
            }

            {// Sync
                Request =
                async (byte Address,
                   object[] inputs,
                   bool HaveResult) =>
                {
                    await Service.Request(Address);
                    await SendData(inputs);
                    return default;
                };
                for (int i = 0; i < Fields.SyncFields.Length; i++)
                {
                    var Field = Fields.SyncFields[i];
                    var DG = MakeDelegate(Field);
                    var OldDG = (Delegate)Field.GetValue(obj);
                    DG = Delegate.Combine(DG, OldDG);
                    Field.SetValue(obj, DG);
                }
            }

            var Result = await Talk(obj);
            await Service.EndService(255);
            return Result;
        }
        public async Task Remote<t>(t obj)
        {
            var TaskQueue = new Async.AsyncTaskQueue();

            var Fields = GetRemoteFields<t>();

            Func<Func<object[], Task<object>>, bool, Task> Handle=null;
            byte Address=0;
            var Service = new Service<byte>(this);

            Action<FieldInfo> AddService = (Field) =>
            {
                var LocAddress = Address++;
                var LocHandle = Handle;
                var Method = Field.FieldType.GetMethod("Invoke");
                if (Method.ReturnType != typeof(void))
                {
                    if (Method.ReturnType == typeof(Task))
                        Service.AddService(LocAddress, async () =>
                        {
                            await LocHandle(async (Params) =>
                            {
                                await (Task)((Delegate)Field.GetValue(obj)).DynamicInvoke(Params);
                                return null;
                            }, false);
                        });
                    else if (Method.ReturnType.IsAssignableTo(typeof(Task<string>).BaseType))
                        Service.AddService(LocAddress, async () =>
                        {
                            await LocHandle(async (Params) =>
                            {
                                var Rs = (Task)((Delegate)Field.GetValue(obj)).DynamicInvoke(Params);
                                await Rs;
                                return ((dynamic)Rs).Result;
                            }, true);
                        });
                    else
                        Service.AddService(LocAddress, async () =>
                        {
                            await LocHandle(async (Params) =>
                            {
                                return ((Delegate)Field.GetValue(obj)).DynamicInvoke(Params);
                            }, true);
                        });
                }
                else
                {
                    Service.AddService(LocAddress, async () =>
                    {
                        await LocHandle(async (Params) =>
                        {
                            ((Delegate)Field.GetValue(obj)).DynamicInvoke(Params);
                            return null;
                        }, false);
                    });
                }
            };

            {// Remote
                Handle = async (ac, HaveResult) =>
                {
                    var Params = await GetData<object[]>();
                    object Result = null;
                    Exception Ex = null;

                    var Q = TaskQueue.AddToQueue(async () =>
                    {
                        try
                        {
                            Result = await ac(Params);
                        }
                        catch (Exception ex)
                        {
                            Ex = ex;
                        }
                        await Sync(Ex);
                        if (HaveResult)
                            await SendData(Result);
                    });
                };
                for (int i = 0; i < Fields.RemoteFields.Length; i++)
                    AddService(Fields.RemoteFields[i]);
            }

            {// Sync
                Handle = async (ac, HaveResult) =>
                {
                    var Params = await GetData<object[]>();
                    await ac(Params);
                };
                for (int i = 0; i < Fields.SyncFields.Length; i++)
                    AddService(Fields.SyncFields[i]);
            }

            await Service.Response(255);
        }

        public Task Sync(Action Action)
        {
            Exception AcEx = null;
            try
            {
                Action();
            }
            catch (Exception ex)
            {
                AcEx = ex;
            }
            return Sync(AcEx);
        }

        public async Task Sync(Exception ex)
        {
            if (ex == null)
                await SendData(false);
            else
            {
                await SendData(true);
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
            if (await GetData<bool>())
            {
#if DEBUG
                Exception ex = new Exception("Other side net sync Error >> " +
                    await GetData<string>() + "\n At >> " + await GetData<string>());
#else
                Exception ex = new Exception("Other side net sync Error >> " +
                    await GetData<string>());
#endif
                throw ex;
            }
        }

        Task Stop();
    }
    public class AsyncOprations<AddressType> :
        IDisposable, IAsyncOprations
    {
        internal ClientSocket<AddressType> Client;
        public event Action<AddressType> Misstake;
#if DEBUG
        private Func<OprationType, OprationType, Task> Parity;
        private int Sequnce = 0;
#endif
        private bool IsServer;
        public AsyncOprations(
            ClientSocket<AddressType> Client, bool IsServer)
        {
            this.Client = Client;
            this.IsServer = IsServer;
#if DEBUG
            if (IsServer)
                Parity = ServerParity;
            else
                Parity = ClientParity;
#endif
        }

        private t Deserialize<t>(byte[] arrBytes)
        {
            if (IsServer)
            {
                bool IsSafe = false;
                try
                {
                    var Result = arrBytes.Deserialize<t>();
                    IsSafe = true;
                    return Result;
                }
                finally
                {
                    if (IsSafe == false)
                    {
                        Misstake?.Invoke(Client.Address);
                    }
                }
            }
            else
            {
                return arrBytes.Deserialize<t>();
            }
        }

        public async Task<t> SendData<t>(t Data)
        {
#if DEBUG
            await Parity(OprationType.SendData, OprationType.GetData);
#endif
            var DataSize = Data.SizeOf();
            if (DataSize < 0)
                await Client.SendPacket(Data.Serialize());
            else
                await Client.Send(Data.Serialize());
            return Data;
        }

        public async Task<t> GetData<t>()
        {
#if DEBUG
            await Parity(OprationType.GetData, OprationType.SendData);
#endif
            t Data = default;
            var DataSize = Data.SizeOf();
            if (DataSize < 0)
                Data = Deserialize<t>(await Client.RecivePacket());
            else
                Data = Deserialize<t>(await Client.Recive(DataSize));
            return Data;
        }

        public async Task Stop()
        {
#if DEBUG
            Client.AddDebugInfo("End.");
#endif
            await Client.Disconncet();
        }

        public void Dispose()
        { }

#if DEBUG

        private async Task ClientParity(OprationType ThisType, OprationType ThatType)
        {
#if TRACE_NET
            Console.WriteLine($"Net:{Client.Address} {ThisType.ToString()}   {ThatType.ToString()}");
            Console.WriteLine($"Net:{Client.Address} Sequence:{Sequnce}");
#endif

            await Client.Send(new byte[] { (byte)ThisType });
            await Client.Send(BitConverter.GetBytes(Sequnce));

            OprationType Status = (OprationType)(await Client.Recive(1))[0];
            int CurrentSequnce = BitConverter.ToInt32(await Client.Recive(4), 0);

            if (Status == OprationType.Exeption)
                throw (await Client.RecivePacket()).Deserialize<Exception>();
            if (Sequnce != CurrentSequnce)
            {
                var Address = Client.Address.ToString();
                var ex = $"Wrong Sequnce On {Address}, This side is {Sequnce}" +
                                       $" but other side is {CurrentSequnce}";
                Client.AddDebugInfo(ex);
                await Client.Disconncet();
                throw new InvalidOperationException(ex);
            }

            Sequnce++;

            if (Status != ThatType)
            {
                var Address = Client.Address.ToString();
                var ex = "Wrong opration On " + Address + ", This side is '" + ThisType.ToString() + "'" +
                                      " And other side must be '" + ThatType.ToString() + "'" +
                                      " But that is '" + Status.ToString() + "'";
                Client.AddDebugInfo(ex);
                await Client.Disconncet();
                throw new InvalidOperationException(ex);
            }
        }

        private async Task ServerParity(
                      OprationType ThisType,
                      OprationType ThatType)
        {
#if TRACE_NET
            Console.WriteLine($"Net:{Client.Address} {ThisType.ToString()}   {ThatType.ToString()}");
            Console.WriteLine($"Net:{Client.Address} Sequence:{Sequnce}");
#endif
            OprationType Status = (OprationType)(await Client.Recive(1))[0];
            int CurrentSequnce = BitConverter.ToInt32(await Client.Recive(4), 0);

            await Client.Send(new byte[] { (byte)ThisType });
            await Client.Send(BitConverter.GetBytes(Sequnce));

            if (Sequnce != CurrentSequnce)
            {
                var Address = Client.Address.ToString();
                var ex = $"Wrong Sequnce On {Address}, This side is {Sequnce}" +
                                       $" but other side is {CurrentSequnce}";
                Client.AddDebugInfo(ex);
                await Client.Disconncet();
                throw new InvalidOperationException(ex);
            }

            Sequnce++;

            if (Status != ThatType)
            {
                var Address = Client.Address.ToString();
                Misstake?.Invoke(Client.Address);
                var ex =
                      "Wrong opration On " + Address + ", This side is '" + ThisType.ToString() + "'" +
                      " And other side must be '" + ThatType.ToString() + "'" +
                      " But that is '" + Status.ToString() + "'";
                Client.AddDebugInfo(ex);
                await Client.Disconncet();
                throw new InvalidOperationException(ex);
            }
        }
#endif

    }
}