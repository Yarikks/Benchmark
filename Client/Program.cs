using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;

namespace Client
{
    class Program
    {
        static private Stopwatch watch = new Stopwatch();
        static void Main(string[] args)
        {
            using (var pipe = new NamedPipeClientStream("localhost", "psexecsvc", PipeDirection.InOut))
            {
                pipe.Connect(5000);
                pipe.ReadMode = PipeTransmissionMode.Message;
                do
                {
                    Console.Write(">");
                    string input = Console.ReadLine();

                    if(input == "send")
                    {
                        double size = 0;
                        watch.Start();
                        for (int i = 0; i < 1000000; i++)
                        {
                            byte[] bytes = Encoding.Default.GetBytes("Standart message sending to server!");
                            pipe.Write(bytes, 0, bytes.Length);
                            size += bytes.Length;
                        }
                        watch.Stop();

                        Console.WriteLine($"[*] Send: 1000000 messages\n" +
                                          $"[*] With size: {size / 1000000} Mbs\n" +
                                          $"[*] For {watch.ElapsedMilliseconds} ms\n");
                    }


                    Console.WriteLine();
                } while (true);
            }
        }
    }
}
