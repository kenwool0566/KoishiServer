using Serilog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public class TcpServer
    {
        private readonly IPEndPoint _endpoint;

        public TcpServer(IPEndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(_endpoint);
            listener.Start();

            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
            }

            catch (Exception ex)
            {
                Log.Error("TcpListener loop stopped unexpectedly: {Exception}", ex);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();

            try
            {
                Session session = new Session(stream);
                while (true)
                {
                    Packet packet = await Packet.ReadAsync(stream);
                    await Handler.HandlePacket(session, packet);
                }
            }
            
            catch (InvalidDataException ex)
            {
                Log.Error("Received Invalid Packet: {Exception}", ex);
            }

            catch
            {
                Log.Information("Client Disconnected.");
            }
        }
    }
}
