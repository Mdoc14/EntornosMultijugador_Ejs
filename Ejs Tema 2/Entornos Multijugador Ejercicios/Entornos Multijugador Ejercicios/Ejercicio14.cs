using System;
using System.Threading;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio14
    {
        const int NHILOS = 40;
        static volatile  int hilosCompletados = 0;
        static SemaphoreSlim emHilosCompletados = new SemaphoreSlim(1);
        static SemaphoreSlim scSiguienteLetra = new SemaphoreSlim(0);


        public static void printLetras() 
        {
            Write("A");

            emHilosCompletados.Wait();
            hilosCompletados++;
            if(hilosCompletados == NHILOS) 
            {
                scSiguienteLetra.Release(NHILOS);
            }
            emHilosCompletados.Release();
            
            scSiguienteLetra.Wait();
            Write("B");

        }
        static void Main() 
        {
           
            for(int i = 0; i < NHILOS; ++i) 
            {
                new Thread(printLetras).Start();
            }
        
        }
        static void Write(String s)
        {
            Thread.Sleep(10);
            Console.Write(s);
            Thread.Sleep(10);
        }

    }
}
