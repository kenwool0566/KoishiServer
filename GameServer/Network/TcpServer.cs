using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public class TcpServer
    {
        private Socket? _listener;

        public void Start(IPEndPoint endpoint)
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(endpoint);
            _listener.Listen(1);
            AcceptLoop();
        }

        private void AcceptLoop()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += (s, e) => ProcessAccept(e);
            AcceptAsync(args);
        }

        private void AcceptAsync(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            if (_listener == null)
            {
                return;
            }

            if (!_listener.AcceptAsync(args)) ProcessAccept(args);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Socket? socket = e.AcceptSocket;

            if (socket == null)
            {
                return;
            }

            ClientConnection conn = new ClientConnection(socket);
            AcceptAsync(e);
        }
    }
}
