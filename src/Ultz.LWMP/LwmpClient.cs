using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public class LwmpClient : IDisposable
    {
        public LwmpClient(IClient client)
        {
            UnderlyingClient = client;
        }

        public LwmpClient(IPAddress address, int port)
        {
            var client = new TcpClient();
            client.Connect(address, port);
            UnderlyingClient = new UnsecuredClient(client);
        }

        public LwmpClient(IPAddress address, int port, bool secure, string hostname)
        {
            var client = new TcpClient();
            client.Connect(address, port);
            UnderlyingClient = secure ? (IClient) new SecuredClient(client, hostname) : new UnsecuredClient(client);
        }

        public IClient UnderlyingClient { get; }

        public async Task<Message> ReadMessageAsync()
        {
            var tagBytes = await ReadPartAsync();
            var headerBytes = await ReadPartAsync();
            var payloadBytes = await ReadPartAsync();
            return new Message
            {
                Body = payloadBytes.ToArray(),
                Header = headerBytes.ToArray(),
                Tag = Encoding.UTF8.GetString(tagBytes.ToArray())
            };
        }

        public async Task SendMessageAsync(Message msg)
        {
            await UnderlyingClient.WriteBytesAsync(GetPart(Encoding.UTF8.GetBytes(msg.Tag)));
            await UnderlyingClient.WriteBytesAsync(GetPart(msg.Header));
            await UnderlyingClient.WriteBytesAsync(GetPart(msg.Body));
        }

        private static IEnumerable<byte> GetPart(byte[] data)
        {
            foreach (var enumerable in data.Split(255))
            {
                var bytes = enumerable.ToArray();
                yield return (byte) bytes.Length;
                foreach (var b in bytes)
                {
                    yield return b;
                }
            }
        }

        private async Task<(IEnumerable<byte> Chunk, bool IsMore)> ReadChunkAsync()
        {
            var length = (await UnderlyingClient.ReadBytesAsync(1)).MoveOne();
            return (await UnderlyingClient.ReadBytesAsync(length), length == 255);
        }

        private async Task<List<byte>> ReadPartAsync()
        {
            var list = new List<byte>();
            var more = true;
            while (more)
            {
                var (chunk, isMore) = await ReadChunkAsync();
                more = isMore;
                list.AddRange(chunk);
            }

            return list;
        }

        public void Dispose()
        {
            UnderlyingClient?.Dispose();
        }
    }
}