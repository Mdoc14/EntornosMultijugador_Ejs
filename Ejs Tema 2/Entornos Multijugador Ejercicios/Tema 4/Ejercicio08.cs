using System;
using System.Threading;

namespace Tema_4
{
    internal class Ejercicio08
    {
        const int N_PROCESOS = 5;
        Barrier barrera;
        void Proceso()
        {
            while (true)
            {
                Console.Write('A');
                barrera.SignalAndWait();
                Console.Write('B');
                barrera.SignalAndWait();
            }
        }

        void BarreraProcesos(Barrier b)
        {
            Console.Write(b.CurrentPhaseNumber % 2 == 0 ? '-' : '\n');
        }

        void Exec()
        {
            barrera = new Barrier(N_PROCESOS, BarreraProcesos);
            for (int i = 0; i < N_PROCESOS; i++)
            {
                new Thread(Proceso).Start();
            }
        }
        static void Main()
        {
            new Ejercicio08().Exec();
        }
    }
}
