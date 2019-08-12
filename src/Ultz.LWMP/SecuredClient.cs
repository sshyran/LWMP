using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public class SecuredClient : IClient
    {
        // Server ctor
        public SecuredClient(TcpClient client, X509Certificate certificate)
        {
            Client = client;
            _stream = new SslStream(client.GetStream());
            _stream.AuthenticateAsServer(certificate);
        }

        // Client ctor
        public SecuredClient(TcpClient client, string hostname)
        {
            Client = client;
            _stream = new SslStream(client.GetStream());
            _stream.AuthenticateAsClient(hostname);
        }
        
        private SslStream _stream;
        public TcpClient Client { get; }

        public async Task<IEnumerable<byte>> ReadBytesAsync(int len)
        {
            var read = 0;
            var amt = len;
            var buffer = new byte[len];
            while (amt > 0 && (read = await _stream.ReadAsync(buffer, 0, amt)) > 0)
            {
                amt -= read;
            }

            if (amt > 0)
            {
                throw new EndOfStreamException();
            }

            return buffer;
        }

        public async Task<bool> WriteBytesAsync(IEnumerable<byte> bytes)
        {
            if (!_stream.CanWrite) return false;
            var arr = bytes.ToArray();
            await _stream.WriteAsync(arr, 0, arr.Length);
            return true;
        }

        public void Dispose()
        {
            _stream?.Dispose();
            Client?.Dispose();
        }
    }
}