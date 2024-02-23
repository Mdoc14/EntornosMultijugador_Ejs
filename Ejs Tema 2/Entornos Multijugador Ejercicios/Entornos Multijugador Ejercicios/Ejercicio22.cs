using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio22
    {
        const int serverPort = 9050;
        const int N_ITERACTIONS = 10;
        const int N_CLIENTS = 5;
        static volatile int clients = 0;
        static Mutex emClients = new Mutex();
        static Random random = new Random();

        static void Client()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);

            Byte[] data;
            string msg;

            // HANDSHAKE
            msg = "handshake";
            data = Encoding.UTF8.GetBytes(msg);
            clientSocket.SendTo(data, serverEP);

            // ASSIGN NEW CONNTHREAD
            EndPoint connThreadEP = new IPEndPoint(IPAddress.Any, 0);
            int recv = clientSocket.ReceiveFrom(data, ref connThreadEP);

            for (int i = 0; i < N_ITERACTIONS; i++)
            {
                Thread.Sleep(100 * random.Next(10));
                msg = "Mensaje " + i;
                data = Encoding.UTF8.GetBytes(msg);
                clientSocket.SendTo(data, connThreadEP);
            }

            msg = "EXIT";
            data = Encoding.UTF8.GetBytes(msg);
            clientSocket.SendTo(data, connThreadEP);

            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                clientSocket.Close();
            }
        }

        static void Server()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Any, serverPort);
            serverSocket.Bind(serverEP);

            Byte[] data = new byte[256];
            string msg;
            int dgramSize;
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, serverPort);

            
            while (true)
            {
                dgramSize = serverSocket.ReceiveFrom(data, ref remoteEP);
                new Thread(ConnectionThread).Start(remoteEP);

                emClients.WaitOne();
                if (++clients == N_CLIENTS) { break; }
                emClients.ReleaseMutex();   
            }

            try
            {
                serverSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                serverSocket.Close();
            }
        }

        static void ConnectionThread(object endPoint)
        {
            EndPoint clientEP = (IPEndPoint)endPoint;
            Socket connThreadSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            string msg = "handshake";
            Byte[] data = Encoding.UTF8.GetBytes(msg);
            int dgramSize;
            connThreadSocket.SendTo(data, clientEP);

            while (true)
            {
                dgramSize = connThreadSocket.ReceiveFrom(data, ref clientEP);
                msg = Encoding.UTF8.GetString(data, 0, dgramSize);
                if (msg == "EXIT") { break; }
                Console.WriteLine(clientEP.ToString() + ": " + msg);
            }

            try
            {
                connThreadSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                connThreadSocket.Close();
            }
        }

        static void Main()
        {
            new Thread(Server).Start();

            for (int i = 0; i < N_CLIENTS; i++)
            {
                new Thread(Client).Start();
            }
        }
    }
}
