using System;
using System.Threading;

namespace Tema_4
{
    internal class Ejercicio02
    {
        void Mensaje()
        {
            try
            {
                Console.WriteLine("[Thread Mensaje] La vida es bella");
                Thread.Sleep(2000);
                Console.WriteLine("[Thread Mensaje] O no...");
                Thread.Sleep(2000);
                Console.WriteLine("[Thread Mensaje] Los pajaritos cantan");
                Thread.Sleep(2000);
                Console.WriteLine("[Thread Mensaje] Y molestan...");
            }
            catch(ThreadInterruptedException e)
            {
                Console.WriteLine("[Thread Mensaje] Se acabó!");
            }

        }
        void Exec() 
        {
            Thread mensaje = new Thread(Mensaje);
            mensaje.Start();

            for (int i = 0; i < 5; i++) 
            {
                if (!mensaje.IsAlive) 
                {
                    break;
                }
                Thread.Sleep(1000);
                Console.WriteLine("[Thread Main] Todavia esperando...");
            }
            if (mensaje.IsAlive)
            {
                mensaje.Interrupt();
                Console.WriteLine("[Thread Main] Cansado de esperar");
            }
            
            mensaje.Join();

            Console.WriteLine("[Thread Main] Por fin!!");


        }
        static void Main() 
        { 
            new Ejercicio02().Exec();
        }
    }
}
