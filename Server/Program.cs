using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Server
{
    class Program
    {
        static private Stopwatch watch = new Stopwatch();
        static void Main(string[] args)
        {
            using (var pipe = new NamedPipeServerStream(
                   "psexecsvc",
                   PipeDirection.InOut,
                   NamedPipeServerStream.MaxAllowedServerInstances,
                   PipeTransmissionMode.Message))
            {
                Console.WriteLine("[*] Waiting for client connection...");
                pipe.WaitForConnection();
                Console.WriteLine("[*] Client connected.");
                while (true)
                {
                    double size = 0;
                    watch.Start();
                    for(int i = 0; i < 1000000; i++)
                    {
                        var messageBytes = ReadMessage(pipe);
                        var line = Encoding.UTF8.GetString(messageBytes);
                        size += messageBytes.Length;
                    }
                    watch.Stop();

                    Console.WriteLine($"[*] Received: 1000000 messages\n" +
                                      $"[*] With size: {size/1000000} Mbs\n" +
                                      $"[*] For {watch.ElapsedMilliseconds} ms\n");
                }
            }
        }

        private static byte[] ReadMessage(PipeStream pipe)
        {
            byte[] buffer = new byte[1024];
            using (var ms = new MemoryStream())
            {
                do
                {
                    var readBytes = pipe.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, readBytes);
                }
                while (!pipe.IsMessageComplete);

                return ms.ToArray();
            }
        }
    }
}
