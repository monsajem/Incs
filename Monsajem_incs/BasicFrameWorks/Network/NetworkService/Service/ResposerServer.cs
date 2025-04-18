﻿using Monsajem_Incs.Net.Base.Socket;
using System;
using System.Threading;

namespace Monsajem_Incs.Net.Base.Service
{
    public class Server<AddressType>
    {
        private ServerSocket<AddressType> ServerSocket;

        public Server(
            ServerSocket<AddressType> ServerSocket)
        {
            this.ServerSocket = ServerSocket;
        }

        public void StartServicing(
            AddressType Address,
            Action<ISyncOprations> Service)
        {
            new Thread(() =>
            {
                ServerSocket.BeginService(Address);
                while (true)
                {
                    var Client = ServerSocket.WaitForAccept();
                    new Thread(() =>
                    {
                        Service(new SyncOprations<AddressType>(Client, true));
#if DEBUG
                        Client.AddDebugInfo("end.");
#endif
                        Client.Disconncet().Wait();
                    }).Start();
                }
            }).Start();
        }
    }
}