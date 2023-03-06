using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace MySignalR
{
    public class Client
    {
        private TcpClient client;

        public Client(int port = 7777)
        {
            client = new TcpClient("127.0.0.1", port);
        }

        public void Send(object data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            using MemoryStream ms = new();
            ms.Write(BitConverter.GetBytes(bytes.Length));
            ms.Write(bytes);
            NetworkStream destination = client.GetStream();
            ms.WriteTo(destination);
        }
    }
}