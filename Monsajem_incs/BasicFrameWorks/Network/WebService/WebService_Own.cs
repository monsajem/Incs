using static Monsajem_Incs.Collection.Array.Extentions;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Web.WebSocket.Server;
using Monsajem_Incs.Net.Base.Socket;
using System;
using static Monsajem_Incs.DelegateExtentions.Actions;

namespace Monsajem_Incs.Net.Web
{
    public class Server:
        Base.Service.Server<int>
    {
        public Server():base
            (new ServerSocket())
        {
            
        }
        private class WebSocket :
            Net.Base.Socket.ClientSocket<int>
        {
            public WebSocket(WebSocketSession Client)
            {
                Client.MessageReceived += (c, e) =>
                {
                    Recived(c);
                };
                this.Client = Client;
                this.IsConnected = true;
                Client.StartMessageLoop();
            }

            private WebSocketSession Client;

            public new void Recived(byte[] Data) => base.Recived(Data);

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
            public WebSocketServer wss;

            protected override void OnBeginService(int Address)
            {
                wss = new WebSocketServer();
                wss.Listen(Address);
            }

            protected override ClientSocket<int> OnWaitForAccep()
            {
                return new WebSocket(wss.Accept());
            }

            protected override void OnDisconnect()
            {
                wss.Stop();
            }
        }
    }
}
