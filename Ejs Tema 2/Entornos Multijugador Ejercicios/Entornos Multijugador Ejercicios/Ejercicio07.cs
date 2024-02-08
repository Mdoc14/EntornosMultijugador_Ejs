using System;
using System.Threading;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio07
    {
        const int NUM_PERSONAS = 10;
        const int NUM_ITERACIONES = 10;
        static volatile int visitantesActuales = 0;
        static volatile String personaRegalo;
        static Mutex cambioVisitantes = new Mutex();
        static void Persona()
        {
            for (int i = 0; i < NUM_ITERACIONES; i++)
            {


                cambioVisitantes.WaitOne();
                if (visitantesActuales == 0) 
                {
                    personaRegalo = Thread.CurrentThread.Name;
                }
                visitantesActuales++;
                WriteLine("Hola, somos " + visitantesActuales.ToString());
                cambioVisitantes.ReleaseMutex();

                Museo();

                cambioVisitantes.WaitOne();
                if (personaRegalo == Thread.CurrentThread.Name)
                {
                    personaRegalo = " ";
                }
                visitantesActuales--;
                WriteLine("Adios a los " + visitantesActuales.ToString());
                cambioVisitantes.ReleaseMutex();

                for (int j = 0; j < NUM_ITERACIONES; j++)
                {
                    WriteLine("paseando ...");
                }
            }
        }

        static void Museo()
        {
            if (personaRegalo != Thread.CurrentThread.Name) 
            {
                WriteLine("No tengo regalo");
            }
            else
            {
                WriteLine("Tengo regalo");
            }
            WriteLine("Que bonito");
            WriteLine("Alucinante");
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < NUM_PERSONAS; i++)
            {
                Thread persona = new Thread(Persona);
                persona.Name = new string('\t', i) + "Persona " + i;
                persona.Start();
            }

        }

        static void WriteLine(String s)
        {
            Thread.Sleep(10);
            Console.WriteLine(Thread.CurrentThread.Name + ": " + s);
            Thread.Sleep(10);
        }
    }
}
