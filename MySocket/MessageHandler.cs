namespace MySocket
{
    public interface IMessageHandler
    {
        void OnMessageReceived(string message);
    }


    public class MyMessageHandler : IMessageHandler
    {
        public void OnMessageReceived(string message)
        {
            Console.WriteLine($"Mensaje recibido: {message}");
        }
    }


}
