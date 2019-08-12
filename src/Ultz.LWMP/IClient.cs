using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ultz.LWMP
{
    public interface IClient : IDisposable
    {
        Task<IEnumerable<byte>> ReadBytesAsync(int len);
        Task<bool> WriteBytesAsync(IEnumerable<byte> bytes);
    }
}