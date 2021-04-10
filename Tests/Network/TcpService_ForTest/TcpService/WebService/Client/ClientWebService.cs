using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;
using System.Net;

namespace Monsajem_Incs.Net.WebTest
{
    public class Client : Net.Base.Service.Client<EndPoint>
    {
        public Client() :
            base(new ClientSocket())
        { }
        private class ClientSocket : Net.Base.Socket.ClientSocket<EndPoint>
        {
            public System.Net.Sockets.Socket Socket;

            protected async override Task Inner_Connect(EndPoint Address)
            {
                this.Socket = new System.Net.Sockets.Socket(
                    System.Net.Sockets.AddressFamily.InterNetwork,
                    System.Net.Sockets.SocketType.Stream,
                    System.Net.Sockets.ProtocolType.Tcp);
                Socket.Connect(Address);
            }

            public async override Task<int> Recive(byte[] Buffer)
            {
                var Res = Socket.Receive(Buffer, System.Net.Sockets.SocketFlags.None);
                return Res;
            }

            protected override async Task Inner_Disconnect()
            {
                Socket.Close();
            }

            protected override async Task Inner_Send(byte[] Data)
            {
                Socket.Send(Data);
            }
        }
    }
}