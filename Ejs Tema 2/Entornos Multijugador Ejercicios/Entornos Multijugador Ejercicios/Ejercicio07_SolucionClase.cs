using System;
using System.Threading;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio07_SolucionClase
    {
        const int N_PERSONAS = 5;
        const int N_ITERACCIONES = 4;

        static volatile int numPersonasMuseo = 0;
        static Mutex emNumPersonasMuseo = new Mutex();

        static volatile bool tengoRegalo = false;

        static void Persona()
        {
            bool tengoRegalo;

            for (int i = 0; i < N_ITERACCIONES; i++) 
            {
                emNumPersonasMuseo.WaitOne();
                WriteLine("Hola a los " + ++numPersonasMuseo);
                tengoRegalo = (numPersonasMuseo == 1);
                emNumPersonasMuseo.ReleaseMutex();

                if(tengoRegalo) 
                {
                    WriteLine("Tengo regalo xD");
                }
                else 
                {
                    WriteLine("Pues vaya...");
                }

                WriteLine("Que bonito!");
                WriteLine("Alucinante");

                emNumPersonasMuseo.WaitOne();
                WriteLine("Adios a los " + --numPersonasMuseo);
                emNumPersonasMuseo.ReleaseMutex();

                WriteLine("Paseando...");
            }
            
        }

        static void WriteLine(String s)
        {
            Thread.Sleep(10);
            Console.WriteLine(Thread.CurrentThread.Name + ": " + s);
            Thread.Sleep(10);
        }

        static void Main() 
        { 
            for (int i = 0; i < N_PERSONAS; i++) 
            { 
                Thread persona = new Thread(Persona);//La referencia va a la pila y el new al Montón
                persona.Name = new string('\t', i);
                persona.Start();
            }
        }
    }
}
