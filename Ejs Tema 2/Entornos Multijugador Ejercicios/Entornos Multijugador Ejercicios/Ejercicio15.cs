using System;
using System.Threading;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio15
    {
        const int NHILOS = 4;
        static volatile int hilosCompletados = 0;

        static SemaphoreSlim emHilosCompletados = new SemaphoreSlim(1);
        static SemaphoreSlim scSiguienteLetra = new SemaphoreSlim(0);
        static SemaphoreSlim scPararUltimo = new SemaphoreSlim(0);
        static SemaphoreSlim scPararOtros = new SemaphoreSlim(0);


        public static void printLetras(object o)
        {
            char letra = (char)o;

            while (true) 
            {
                Write(letra);

                emHilosCompletados.Wait();
                hilosCompletados++;

                if (hilosCompletados < NHILOS)
                {
                    emHilosCompletados.Release();
                    if(letra != 'A') Thread.Sleep(1000);
                    scSiguienteLetra.Wait();
                    scPararUltimo.Release();
                    scPararOtros.Wait();
                }
                else 
                {
                    emHilosCompletados.Release();
                    Write('-');
                    hilosCompletados = 0;
                    scSiguienteLetra.Release(NHILOS - 1);
                    for (int i = 0; i < NHILOS - 1; i++)
                    {
                        scPararUltimo.Wait();
                    }
                    scPararOtros.Release(NHILOS - 1);
                }

            }
        }
        static void Main()
        {
            char c = 'A';
            for (int i = 0; i < 4; ++i)
            {
                new Thread(printLetras).Start(c++);
            }

        }
        static void Write(char s)
        {
            Thread.Sleep(10);
            Console.Write(s);
            Thread.Sleep(10);
        }
    }
}
