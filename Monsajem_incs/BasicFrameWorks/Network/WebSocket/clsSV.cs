using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Monsajem_Incs.Net.Web.WebSocket.Server
{
    public class WebSocketServer
    {
        private bool _listening;
        private TcpListener server;
        public void Close() => _listening = false;

        public void Listen(int port)
        {
            if (_listening) throw new Exception("Already listening!");
            _listening = true;

            server = new TcpListener(IPAddress.Any, port);
            server.Start();
        }

        public WebSocketSession Accept()
        {
            var session = new WebSocketSession(server.AcceptTcpClient());
            session.Start();
            return session;
        }

        public void Stop()
        {
            server.Stop();
        }
    }
}