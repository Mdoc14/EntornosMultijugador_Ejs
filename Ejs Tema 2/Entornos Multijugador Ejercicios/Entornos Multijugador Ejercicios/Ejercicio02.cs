using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio02
    {
        static volatile int peticion;
        static volatile bool servido;
        static volatile bool mandado;
        static volatile int N_MAX = 50;

        static Random rand = new Random();

        static void Servidor()
        {
            for (int i = 0; i < N_MAX; i++)
            {
                //Espera al cliente
                while (!mandado) ;
                Console.WriteLine("2.Peticion en el servidor: " + peticion);
                //Suma el numero
                peticion++;
                Console.WriteLine("3.Servidor actualiza la peticion: " + peticion);
                //Le dice al cliente que ya la tiene
                mandado = false;
                servido = true;
            }
        }
        static void Cliente()
        {
            for (int i = 0; i < N_MAX; i++)
            {
                //Manda el entero al servidor
                peticion = rand.Next(10);
                Console.WriteLine("1.Peticion del cliente: " + peticion);
                mandado = true;
                //Espera a que el servidor se lo mande actualizado
                while (!servido) ;
                //Lo imprime por pantalla
                Console.WriteLine("4.Recibido por el servidor: " + peticion + "\n");
                servido = false;
            }

        }
        static void Main()
        {
            new Thread(Cliente).Start();
            new Thread(Servidor).Start();
        }
    }
}
