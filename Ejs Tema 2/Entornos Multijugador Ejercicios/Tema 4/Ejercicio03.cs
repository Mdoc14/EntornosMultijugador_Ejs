using System;
using System.Threading;
using System.Xml.Serialization;

namespace Tema_4
{
    internal class Ejercicio03
    {
        static void Main() 
        {
            new ProdCons().Exec();
        }
    }
    class ProdCons
    {
        volatile float producto;
        SincCon sinc = new SincCon();
        public void Productor()
        {
            Random random = new Random();
            producto = random.Next(1, 10);
            sinc.Signal();
        }
        public void Consumidor()
        {
            sinc.Await();
            Console.WriteLine("Producto: {0}", producto);
        }
        public void Exec()
        {
            new Thread(() => Productor()).Start();
            new Thread(() => Consumidor()).Start();
        }
    }

    class SincCon
    {
        volatile bool producido = false;
        public void Signal()
        {
            producido = true;
        }

        public void Await()
        {
            while (!producido);
            producido = false;
        }
    }
}
