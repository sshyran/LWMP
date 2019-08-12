using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultz.LWMP;

namespace Demo
{
    public class ConsoleClient : IClient
    {
        public static ConcurrentQueue<byte> Bytes { get; } = new ConcurrentQueue<byte>();
        
        public void Dispose()
        {
        }

        public Task<IEnumerable<byte>> ReadBytesAsync(int len)
        {
            var res = new byte[len];
            for (var i = 0; i < len;)
            {
                if (!Bytes.TryDequeue(out var b)) continue;
                res[i] = b;
                i++;
            }

            Console.WriteLine("Inbound> " + BitConverter.ToString(res).ToLower().Replace("-", ""));
            return Task.FromResult((IEnumerable<byte>)res);
        }

        public Task<bool> WriteBytesAsync(IEnumerable<byte> bytes)
        {
            Console.WriteLine("Outbound> " + BitConverter.ToString(bytes.ToArray()).ToLower().Replace("-", ""));
            return Task.FromResult(true);
        }
    }
}