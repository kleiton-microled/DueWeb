using System;
using System.Threading.Tasks;

namespace Sistema.DUE.Web.Helpers
{
    public static class RetryHelper
    {
        public static void RetryOnException(TimeSpan pausa, Action metodo)
        {
            do
            {
                try
                {
                    metodo();
                    break;
                }
                catch
                {
                    Task.Delay(pausa).Wait();
                }
            } while (true);
        }
    }
}