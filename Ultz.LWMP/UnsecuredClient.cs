using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public class TcpLwmpClient : IClient
    {
        public TcpLwmpClient(TcpClient client)
        {
            Client = client;
            _stream = client.GetStream();
        }
        
        private Stream _stream;
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