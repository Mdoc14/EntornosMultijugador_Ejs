using System;
using System.Threading;

namespace Tema_4
{
    internal class Ejercicio01
    {
        SemaphoreSlim sem = new SemaphoreSlim(0);
        volatile int producto;

        void Productor()
        {
            Random random = new Random();
            producto = random.Next(10);
            sem.Release();    
        }

        void Consumidor()
        {
            sem.Wait();
            Console.WriteLine("Producto: {0}", producto);
        }

        void Exec()
        {
            new Thread(Productor).Start();
            new Thread(Consumidor).Start();
        }

        static void Main(string[] args)
        {
            new Ejercicio01().Exec();
        }
    }
}
