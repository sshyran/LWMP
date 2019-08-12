using System.Collections.Generic;

namespace Ultz.LWMP
{
    public struct Message
    {
        public string Tag { get; set; }
        public byte[] Header { get; set; }
        public byte[] Body { get; set; }
    }
}