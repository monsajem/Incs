using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Socket;
using System;
using System.Threading;

namespace Monsajem_Incs.Net.WebTest
{
    public class Server:
        Base.Service.Server<int>
    {
        public Server():base
            (new ServerSocket())
        { }

        private class Behavior
        {
            public Behavior(System.Net.Sockets.Socket S)
            {
                this.S = S;
            }

            public System.Net.Sockets.Socket S;

            public event Action Error;

            protected void OnError()
            {
                Error?.Invoke();
            }

            public event Action Closing;

            public void Send(byte[] Buffer)
            {
                S.BeginSend(Buffer,0,Buffer.Length,
                    System.Net.Sockets.SocketFlags.None,
                    null,null);
            }

            public void Close(){
                S.Close();
                Closing?.Invoke();
            }

            public event Action<byte[]> OnMessageEvent;

            public void StartReciving()
            {
                new Thread(() =>
                {
                    try
                    {
                        var Buffer = new byte[1024];
                        Recive:
                        var Recived = new byte[S.Receive(Buffer)];
                        System.Array.Copy(Buffer, 0, Recived, 0, Recived.Length);
                        OnMessageEvent(Recived);
                        goto Recive;
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        OnError();
                    }
                }).Start();
            }
        }
        private class WebSocket :
            Net.Base.Socket.ClientSocket<int>
        {
            public WebSocket(Behavior Client)
            {
                this.Client = Client;
                Client.OnMessageEvent += (e) => Recived(e);
                Client.Closing += () => IsConnected = false;
                Client.Error += () => Client.Close();
                this.IsConnected = true;
                Client.StartReciving();
            }

            private Behavior Client;

            protected async override Task Inner_Send(byte[] Data)
            {
                Client.Send(Data);
            }

            protected override async Task Inner_Disconnect()
            {
                Client.Close();
            }
        }
        private class ServerSocket :
        Net.Base.Socket.ServerSocket<int>
        {
            public System.Net.Sockets.Socket S;

            protected override void OnBeginService(int Address)
            {
                S = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.InterNetwork,
                System.Net.Sockets.SocketType.Stream,
                System.Net.Sockets.ProtocolType.Tcp);
                var address = new System.Net.IPEndPoint(System.Net.IPAddress.Any, Address);
                S.Bind(address); S.Listen(int.MaxValue);
            }

            protected override ClientSocket<int> OnWaitForAccep()
            {
                return new WebSocket(new Behavior(S.Accept()));
            }

            protected override void OnDisconnect()
            {
                S.Disconnect(true);
            }
        }
    }
}
