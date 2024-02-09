using System;
using System.Threading;

namespace Entornos_Multijugador_Ejercicios
{
    internal class Ejercicio09_apartadoA
    {
        private const int N_FRAGMENTOS = 25;
        private const int N_HILOS = 10;
        private static volatile int[] fichero = new int[N_FRAGMENTOS];
        static Random _random = new Random();
        static volatile int idx=0;
        static volatile int numHilosActuales=0;
        static Mutex emIdx = new Mutex();
        static Mutex emNumHilosFinales = new Mutex();
        

        private static int DescargaDatos(int numFragmento)
        {
            Thread.Sleep(_random.Next(1000));
            return numFragmento * 2;
        }

        private static void MostrarFichero()
        {
            Console.WriteLine("--------------------------------------------------");
            WriteLine("te dice que: ");
            Console.Write("File = [");
            for (int i = 0; i < N_FRAGMENTOS; i++)
            {
                Console.Write(fichero[i] + ",");
            }
            Console.WriteLine("]");
        }

        public static void Downloader()
        {
            int currentIdx;
            
            while (true)
            {
                emIdx.WaitOne();
                if (idx < N_FRAGMENTOS)
                {
                    currentIdx = idx++;
                    emIdx.ReleaseMutex();
                    WriteLine(" ha descargado el fragmento "+ currentIdx);
                    fichero[currentIdx] = DescargaDatos(currentIdx);
                }
                else 
                {
                    emIdx.ReleaseMutex();
                    break;
                }
            }

            emNumHilosFinales.WaitOne();
            numHilosActuales++;
            if (numHilosActuales == N_HILOS) 
            {
                MostrarFichero();
            }
            emNumHilosFinales.ReleaseMutex();
        }
        static void WriteLine(String s)
        {
            Thread.Sleep(10);
            Console.WriteLine(Thread.CurrentThread.Name + ": " + s);
            Thread.Sleep(10);
        }
        public static void Main(String[] args)
        {
            Thread[] threads = new Thread[N_HILOS];
            for (int i = 0; i < N_HILOS; i++)
            {
                threads[i] = new Thread(Downloader);
                threads[i].Name = new string('\t', i) + "Hilo " + i;
                threads[i].Start();
            }
            
        }
    }
}
