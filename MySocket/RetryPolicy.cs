using System;
using System.Threading;

namespace MySocket
{
    public class RetryPolicy
    {
        private readonly int maxRetries;
        private readonly int delayMilliseconds;

        public RetryPolicy(int maxRetries = 3, int delayMilliseconds = 1000)
        {
            this.maxRetries = maxRetries;
            this.delayMilliseconds = delayMilliseconds;
        }

        public void Execute(Action action)
        {
            int attempt = 0;

            while (true)
            {
                try
                {
                    action();
                    return; // Éxito
                }
                catch (Exception ex)
                {
                    attempt++;

                    Console.WriteLine($"[RetryPolicy] Error en intento {attempt}: {ex.Message}");

                    if (attempt >= maxRetries)
                    {
                        throw new Exception($"Falló después de {maxRetries} intentos.", ex);
                    }

                    Thread.Sleep(delayMilliseconds);
                }
            }
        }
    }
}
