using Monsajem_Incs.Net.Tcp.Socket;

namespace Monsajem_Incs.Net.Tcp
{
    public class Server :
        Base.Service.Server<System.Net.EndPoint>
    {
        private TcpServerSocket ServerSocket = new();
        public Server() :
             base(new TcpServerSocket())
        { }
    }
}
