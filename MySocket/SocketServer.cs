using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySocket
{
    public class SocketServer : ISocketServer
    {
        private readonly int port;
        private bool isRunning = false;

        public SocketServer(int port)
        {
            this.port = port;
        }

        public void StartListening(IMessageHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            isRunning = true;

            Task.Run(() =>
            {
                try
                {
                    TcpListener listener = new TcpListener(IPAddress.Any, port);
                    listener.Start();
                    Console.WriteLine($"Servidor escuchando en el puerto {port}...");

                    while (isRunning)
                    {
                        if (!listener.Pending())
                        {
                            Thread.Sleep(100); // Evita ocupar CPU innecesariamente
                            continue;
                        }

                        TcpClient client = listener.AcceptTcpClient();

                        Task.Run(() =>
                        {
                            try
                            {
                                using (NetworkStream stream = client.GetStream())
                                {
                                    byte[] buffer = new byte[1024];
                                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                                    handler.OnMessageReceived(message);
                                }
                                client.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[Error en cliente] {ex.Message}");
                            }
                        });
                    }

                    listener.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error en servidor] {ex.Message}");
                }
            });
        }
    }
}



