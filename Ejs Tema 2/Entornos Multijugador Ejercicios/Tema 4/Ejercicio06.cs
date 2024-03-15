using System;
using System.Threading;

namespace Tema_4
{
    internal class Ejercicio06
    {
        const int N_PERSONAS = 5;
        int nPersonasMuseo = 0;
        readonly object emPersonasMuseo = new object();
        void Persona()
        {
            bool regalo = false;
            lock (emPersonasMuseo)
            {
                nPersonasMuseo++;
                regalo = nPersonasMuseo == 1 ? true : false;
            }
            if (regalo) Console.Write("Tengo regalo");

            Console.WriteLine("Que bonito");
            Console.WriteLine("Alucinante");

            lock (emPersonasMuseo)
            {
                nPersonasMuseo--;
            }
        }
        void Exec()
        { 
            for (int i = 0; i < N_PERSONAS; i++)
            {
                new Thread(Persona).Start();
            }
        
        }

        static void Main()
        {
            new Ejercicio06().Exec();
        }
    }

}
