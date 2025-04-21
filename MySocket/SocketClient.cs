using System;
using System.Net.Sockets;
using System.Text;

namespace MySocket
{
    public class SocketClient : ISocketClient
    {
        private readonly string serverIp;
        private readonly int serverPort;
        private readonly RetryPolicy retryPolicy;

        public SocketClient(string ip, int port, int maxRetries = 3, int retryDelayMs = 1000)
        {
            serverIp = ip;
            serverPort = port;
            retryPolicy = new RetryPolicy(maxRetries, retryDelayMs);
        }

        public void SendMessage(string message)
        {
            retryPolicy.Execute(() =>
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(serverIp, serverPort);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }

                    client.Close();
                    Console.WriteLine("Mensaje enviado exitosamente.");
                }
            });
        }
    }
}


