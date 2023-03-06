using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySignalR
{
    public class Server
    {
        private TcpListener server;
        public event Action<JObject> MessageReceived = delegate { };

        public Server(int port = 7777)
        {
            server = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            server.Start();
            server.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
        }

        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpClient client = server.EndAcceptTcpClient(ar);
            Console.WriteLine("Client connected.");

            Thread clientThread = new(() => HandleClient(client));
            clientThread.Start();
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] sizeBuffer = new byte[4];
            byte[] buffer;
            while (stream.Read(sizeBuffer, 0, 4) == 4)
            {
                int size = BitConverter.ToInt32(sizeBuffer);
                buffer = new byte[size];
                _ = stream.Read(buffer, 0, size);
                MessageReceived?.Invoke(JObject.Parse(Encoding.UTF8.GetString(buffer)));
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}