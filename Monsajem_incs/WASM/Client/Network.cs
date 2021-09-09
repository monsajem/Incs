using System;
using static MonsajemData.DataBase;
using MonsajemData;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.KeyValue;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Net.Web;
using Monsajem_Incs.Net.Base.Service;
using WebAssembly.Browser.DOM;
using System.Reflection;
using System.Threading.Tasks;
using static WASM_Global.Publisher;

namespace Monsajem_Client
{
    public class Network
    {
        public static string Service_IpAddress;
        public static int Service_Port;
        public static bool ShowMessages = true;
        internal static Client Server = new Client();

        public static async Task Connect(
             Func<IAsyncOprations, Task> Ac)
          {
              //try
              //{
              if (Service_IpAddress == null)
                  throw new Exception("server ip address is empty");
              if(Service_Port == 0)
                  throw new Exception("server Port address is empty");
              if (ShowMessages)
                UserControler.Publish.ShowAction("در حال اتصال");
              await Server.Connect(new EndPoint()
              {
                  IpAddress = Service_IpAddress,
                  Port = Service_Port
              }, Ac);
            //}
            //catch (Exception ex)
            //{
            //    throw new ThisException("ارتباط با سرویس دهنده بر قرار نشد");
            //}
        }

        public static Func<IAsyncOprations, Task> BaseLogin=async (c)=> { };

        public static async Task RemoteAndRun(Action Ac)
        {
            await Remote(Ac);
            Ac();
        }
        public static async Task Remote(Action Ac)
        {
            await Connect(async (rq) =>
            {
                 await BaseLogin?.Invoke(rq);
                 if(ShowMessages)
                 UserControler.Publish.ShowAction("در حال انجام عملیات");
                 await rq.RunOnOtherSide(Ac);
            });
            if(ShowMessages)
            UserControler.Publish.HideAction();
        }
        public static async Task Remote<UserType>(Action<UserType> Ac)
            where UserType : User<UserType>
        {
            await Connect(async (rq) =>
            {
                await BaseLogin?.Invoke(rq);
                if (ShowMessages)
                UserControler.Publish.ShowAction("در حال انجام عملیات");
                await rq.RunOnOtherSide(Ac);
            });
            if (ShowMessages)
                UserControler.Publish.HideAction();
        }
        public static async Task<t> Remote<t>(Func<t> Ac)
        {
            t Result=default;
            await Connect(async (rq) =>
            {
                await BaseLogin?.Invoke(rq);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال انجام عملیات");
                Result = await rq.RunOnOtherSide(Ac);
            });
            return Result;
        }
        public static async Task<t> Remote<t,UserType>(Func<UserType, t> Ac)
            where UserType : User<UserType>
        {
            t Result=default;
            await Connect(async (rq) =>
            {
                await BaseLogin?.Invoke(rq);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال انجام عملیات");
                Result = await rq.RunOnOtherSide(Ac);
            });
            return Result;
        }

        public static async Task Remote(
            Action<ISyncOprations> ServerSide,
            Func<IAsyncOprations, Task> ClientSide)
        {
            await Connect(async (rq) =>
            {
                await BaseLogin?.Invoke(rq);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال ارسال درخواست");
                await rq.RunOnOtherSide(ServerSide);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال انجام عملیات");
                await ClientSide(rq);
            });
            if(ShowMessages)
                UserControler.Publish.HideAction();
        }

        public static async Task Remote(
            Func<IAsyncOprations, Task> ServerSide,
            Func<IAsyncOprations, Task> ClientSide)
        {
            await Connect(async (rq) =>
            {
                await BaseLogin?.Invoke(rq);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال ارسال درخواست");
                await rq.RunOnOtherSide(ServerSide);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال انجام عملیات");
                await ClientSide(rq);
            });
            if (ShowMessages)
                UserControler.Publish.HideAction();
        }

        public static async Task Remote<UserType>(
            Action<ISyncOprations, UserType> ServerSide,
            Func<IAsyncOprations, Task> ClientSide)
            where UserType:User<UserType>
        {
            await Connect(async (rq) =>
            {
                await BaseLogin?.Invoke(rq);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال ارسال درخواست");
                await rq.RunOnOtherSide(ServerSide);
                if (ShowMessages)
                    UserControler.Publish.ShowAction("در حال انجام عملیات");
                await ClientSide(rq);
            });
            if (ShowMessages)
                UserControler.Publish.HideAction();
        }
    }
}