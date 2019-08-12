using System;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public interface IListener : IDisposable
    {
        void Start();
        void Stop();
        Task<IClient> AcceptClientAsync();
    }
}