using System;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public class LwmpListener : IDisposable
    {
        public LwmpListener(IListener listener)
        {
            UnderlyingListener = listener;
        }

        public IListener UnderlyingListener { get; }

        public void Start()
        {
            UnderlyingListener.Start();
        }

        public void Stop()
        {
            UnderlyingListener.Stop();
        }

        public async Task<LwmpClient> AcceptClientAsync()
        {
            return new LwmpClient(await UnderlyingListener.AcceptClientAsync());
        }

        public void Dispose()
        {
            UnderlyingListener?.Dispose();
        }
    }
}