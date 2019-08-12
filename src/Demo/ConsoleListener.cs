using System;
using System.Threading.Tasks;
using Ultz.LWMP;

namespace Demo
{
    public class ConsoleListener : IListener
    {
        public void Dispose()
        {
        }

        public void Start()
        {
            Console.WriteLine("Started");
        }

        public void Stop()
        {
            Console.WriteLine("Stop");
        }

        public Task<IClient> AcceptClientAsync()
        {
            return Task.FromResult<IClient>(new ConsoleClient());
        }
    }
}