using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio01
    {
        static volatile int producto;
        static volatile bool producido;
        static volatile bool consumido;
        static volatile int N_MAX = 10;

        static Random rand = new Random();

        static void Productor()
        {
            for (int i = 0; i < N_MAX; i++)
            {
                producto = rand.Next(10);
                Console.WriteLine("Productor genera " + producto);
                producido = true;
                while (!consumido) ;
                consumido = false;
            }
        }
        static void Consumidor()
        {
            for (int i = 0; i < N_MAX; i++)
            {
                while (!producido) ;
                Console.WriteLine("\tConsumidor recibe " + producto);
                producido = false;
                consumido = true;
            }
        }
        static void Main()
        {
            new Thread(Productor).Start();
            new Thread(Consumidor).Start();
        }
    }
}
