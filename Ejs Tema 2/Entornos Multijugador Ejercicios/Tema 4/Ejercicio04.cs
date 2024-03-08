using System;
using System.Threading;
using System.Xml.Serialization;

namespace Tema_4
{
    internal class Ejercicio04
    {
        static void Main()
        {
            new ProdConsN().Exec();
        }
    }
    class ProdConsN
    {
        volatile float producto;
        SincConN producido = new SincConN(false);
        SincConN consumido = new SincConN(true);
        public void Productor()
        {
            Random random = new Random();
            int cache;
            while (true)
            {
                cache = random.Next(1, 10);
                consumido.Await();
                producto = cache;
                Console.WriteLine("[PROD] Producto: {0}", producto);
                producido.Signal();
            }
        }
        public void Consumidor()
        {
            while (true)
            {
                producido.Await();
                Console.WriteLine("[CONS] Producto: {0}", producto);
                Console.ReadLine();
                consumido.Signal();
            }
        }
        public void Exec()
        {
            new Thread(() => Productor()).Start();
            new Thread(() => Consumidor()).Start();
        }
    }

    class SincConN
    {
        volatile bool producido;

        public SincConN(bool producido)
        {
            this.producido = producido;
        }
        public void Signal()
        {
            producido = true;
        }

        public void Await()
        {
            while (!producido) ;
            producido = false;
        }
    }
}
