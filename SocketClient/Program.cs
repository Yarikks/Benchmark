using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    public static class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static private Stopwatch watch = new Stopwatch();
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                double bytes = 0;
                watch.Start();
                for(int i = 0; i < 1000000; i++)
                {
                    string msg = "Default text for sending to server";
                    byte[] data = Encoding.Unicode.GetBytes(msg);

                    socket.Send(data);
                    bytes += data.Length;
                }
                watch.Stop();

                Console.WriteLine($"Messages send: 1000000\nAll size: {bytes/1000000} Mb\nTime elapsed: {watch.ElapsedMilliseconds} ms");
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
