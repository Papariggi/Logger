using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerClient
{
    public class Client
    {
        private static StringBuilder clientBuffer = new StringBuilder();
        private const string host = "192.168.1.48";

        private static void run(string hostname, int port)
        {

        }

        public static void appendToBuffer(StringBuilder str)
        {
            if ((str.Length + clientBuffer.Length) >=
                clientBuffer.MaxCapacity)
            {
                clientBuffer.Remove(0, str.Length);
            }

            clientBuffer.Append(str);
        }

        private static void sendBuffToServer()
        {
            while (true)
            {
                Thread.Sleep(5000);
                if (clientBuffer.Length == 0) continue;

                UdpClient client = new UdpClient();
                client.Connect(host, 3000);
                byte[] data = Encoding.UTF8.GetBytes(clientBuffer.ToString());
                int sendedBytes = client.Send(data, data.Length);

                if (sendedBytes == 0) { continue; }

                client.Close();
                clientBuffer.Clear();
            }
        }
    }
}

