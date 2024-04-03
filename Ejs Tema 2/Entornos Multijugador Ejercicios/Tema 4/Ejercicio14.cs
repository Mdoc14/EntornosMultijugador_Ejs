using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Tema_4
{
    internal class Ejercicio14
    {
        const int N_TAREAS = 10;
        readonly Random _random = new Random();
        IList<Task<string>> _tasks = new List<Task<string>>(N_TAREAS);

        static void Main() 
        {
            new Ejercicio14().exec();
        }
        public void exec()
        {
            for (int i = 0; i < N_TAREAS; i++)
            {
                var task = new Task<string>(Metodo, i);
                _tasks.Add(task);
                task.Start();

            }
            while (_tasks.Count > 0)
            {
                int idx = Task.WaitAny(_tasks.ToArray());
                try
                {
                    Console.WriteLine($"Tarea {idx} finalizada con exito : {_tasks[idx].Result}");
                }
                catch ( Exception e ) 
                {
                    Console.WriteLine($"Tarea {idx} falló: {e.InnerException.Message}");
                }
                _tasks.RemoveAt(idx);
            }
        }
        public String Metodo(object i) 
        {
            Thread.Sleep(_random.Next(500));
            return _random.Next(10) > 2 ? $"Tarea {i} correcta": throw new TaskFailedException($"Tarea {i} con error");
        }
    }

    internal class TaskFailedException : Exception
    {
        public TaskFailedException(string message) : base(message)
        {
        }
    }
}
