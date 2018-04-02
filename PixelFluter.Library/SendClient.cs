using System;
using System.Net;
using System.Net.Sockets;

namespace PixelFluter.Library
{
    public class SendClient : IDisposable
    {
        private static Random Random = new Random();  
        private Socket Socket = null;
        public SendClient()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Send(PixelPackage cmdPart, Config config)
        {
            Console.WriteLine("Start Socket: {0}", cmdPart.Index);
            while(true)
            {
                try
                {
                    using(var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        socket.Connect(config.Ip, config.Port);
                        while(true)
                        {
                            socket.Send(cmdPart.Bytes);
                        }
                        socket.Close();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception in Socket {0}: {1} - Renew pipe!", cmdPart.Index, ex.Message);
                }
            }
        }

        public void Dispose()
        {
            Socket.Close();
            Socket.Dispose();
        }
    }
}