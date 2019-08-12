using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultz.LWMP;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(Listen);
            while (true)
            {
                var hex = Console.ReadLine();
                foreach (var b in Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16)))
                {
                    ConsoleClient.Bytes.Enqueue(b);
                }
            }
        }

        private static async Task Listen()
        {
            var listener = new LwmpListener(new ConsoleListener());
            listener.Start();
            var client = await listener.AcceptClientAsync();
            var message = await client.ReadMessageAsync();
            Console.WriteLine("Message> " + System.Text.Json.Serialization.JsonSerializer.ToString(message));
            var bytes = Encoding.UTF8.GetBytes("Bye!");
            var msg = new Message
            {
                Tag = message.Tag,
                Header = new byte[0],
                Body = bytes
            };
            await client.SendMessageAsync(msg);
        }
    }
}