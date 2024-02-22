using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio21
    {
        static void Cliente()
        {
            Thread.Sleep(100);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            string msg = "";

            while (msg != "EXIT")
            {
                msg = Console.ReadLine();
                Byte[] data = Encoding.UTF8.GetBytes(msg);

                clientSocket.SendTo(data, serverEP);
            }
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                clientSocket.Close();
            }
        }
        static void Servidor()
        {
            //Se crea el server
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint serverEP = new IPEndPoint(IPAddress.Any, 9050);
            serverSocket.Bind(serverEP);
            string msg = "";
            //Se reciben datos
            while (msg != "EXIT")
            {
                byte[] data = new byte[1024];
                EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
                int recv = serverSocket.ReceiveFrom(data, ref senderEP);

                msg = Encoding.UTF8.GetString(data, 0, recv);

                Console.WriteLine("He recibido el mensaje " + msg);
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

        static void Main()
        {
            new Thread(Servidor).Start();

            new Thread(Cliente).Start();
        }
    }
}
