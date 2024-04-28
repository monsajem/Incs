using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Net.Tcp.Socket;

namespace Monsajem_Incs.Net.Tcp
{
    public class Client :
        Client<System.Net.EndPoint>
    {
        public Client() :
            base(new TcpClientSocket())
        { }
    }
}
