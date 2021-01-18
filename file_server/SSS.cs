using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace file_server
{
    class SSS : Socket
    {       

        public SSS() : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            base.Bind(new IPEndPoint(IPAddress.Any, 5000));
            base.Listen(10);
            BeginAccept(callback, this);
        }

        private void callback(IAsyncResult ar)
        {
            Client client = new Client(EndAccept(ar));
            //Global.clientList.Add(client);
            BeginAccept(callback, this);
        }
    }
}
