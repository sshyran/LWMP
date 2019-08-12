using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public class SecuredListener : IListener
    {
        public SecuredListener(TcpListener listener, X509Certificate2 certificate)
        {
            Listener = listener;
            Certificate = certificate;
        }

        public X509Certificate2 Certificate { get; }

        public TcpListener Listener { get; }

        public void Start()
        {
            Listener.Start();
        }

        public void Stop()
        {
            Listener.Stop();
        }

        public async Task<IClient> AcceptClientAsync()
        {
            return new SecuredClient(await Listener.AcceptTcpClientAsync(), Certificate);
        }

        public void Dispose()
        {
        }
    }
}