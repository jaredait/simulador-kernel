using System;
using System.Collections.Generic;
using System.Timers;

namespace simulador_kernel_SO1
{
    class Program
    {
        private static List<BCP> TodosProcesos = new List<BCP>();
        private static List<BCP> Preparados = new List<BCP>(); 
        private static List<BCP> Suspendidos = new List<BCP>();

        private static System.Timers.Timer aTimer;

        static void Main(string[] args)
        {
            Program program = new Program();
            int opcionMenu = 0;
            bool continuar = true;

            do
            {
                Console.Clear();
                program.ImprimirMenu();
                opcionMenu = Convert.ToInt32(Console.ReadLine());

                switch (opcionMenu)
                {
                    case 1:
                        program.CrearProceso();
                        break;

                    case 2:
                        program.EjecutarProceso();
                        break;

                    case 3:
                        program.ReanudarProceso();
                        break;

                    case 4:
                        program.MatarProceso();
                        break;

                    case 5:
                        program.VerTodosProcesos();
                        break;

                    case 6:
                        program.VerProcesosPreparados();
                        break;

                    case 7:
                        program.VerProcesosSuspendidos();
                        break;

                    case 8:
                        program.Salir();
                        break;
                    default:
                        break;
                }

                continuar = opcionMenu == 8 ? false : true;
            } while (continuar);

  
        }


        private void ImprimirMenu()
        {
            Console.WriteLine("1. Crear Proceso");
            Console.WriteLine("2. Ejecutar Procesos");
            Console.WriteLine("3. Reanudar Proceso");
            Console.WriteLine("4. Matar Proceso");
            Console.WriteLine("5. Ver todos los procesos");
            Console.WriteLine("6. Ver procesos preparados");
            Console.WriteLine("7. Ver procesos suspendidos");
            Console.WriteLine("8. Salir");
            Console.WriteLine("______________________________");
        }

        //  1
        private void CrearProceso()
        {
            string nombreProceso;
            int prioridad;
            int tiempoEjecucion;

            Console.WriteLine("Nombre del proceso:");
            nombreProceso = Console.ReadLine();
            Console.WriteLine("Prioridad: (máxima prioridad es 0)");
            prioridad = Convert.ToInt32(Console.ReadLine()); 
            Console.WriteLine("Tiempo de ejecución: (en segundos)");
            tiempoEjecucion = Convert.ToInt32(Console.ReadLine());

            BCP bcp = new BCP(nombreProceso, prioridad, tiempoEjecucion);

            // asignar parametros al BCP
            bcp.pid = TodosProcesos.Count;
            bcp.estado = "P";
            bcp.horaCreacion = DateTime.Now;

            TodosProcesos.Add(bcp);
            Preparados.Add(bcp);

            OrdenarPreparados();

        }

        private static void OrdenarPreparados()
        {
            Preparados.Sort(delegate (BCP x, BCP y)
            {
                return x.prioridad.CompareTo(y.prioridad);
            });
        }

        //  2
        private void EjecutarProceso()
        {
            BCP bcp = Preparados[0];

            SetTimer();
            Console.WriteLine("\nPresiona ENTER para finalizar el proceso\n");

            Console.WriteLine("\nEjecutando:\n{0}\n", bcp.ToString());
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            if(bcp.tiempoRestante != TimeSpan.FromTicks(0))
            {
                Console.WriteLine(bcp.ToString());
                Console.WriteLine("\nPROCESO SUSPENDIDO");
                SuspenderProceso();
                Console.ReadKey();
            }

        }
        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            BCP bcp = Preparados[0];
            Console.WriteLine("\r\nTiempo restante: " + bcp.tiempoRestante);
            bcp.tiempoRestante -= TimeSpan.FromTicks(10000000);
            if (bcp.tiempoRestante == TimeSpan.FromTicks(0))
            {
                aTimer.Stop();
                aTimer.Dispose();

                Console.WriteLine("________________________________________________________");
                Console.WriteLine(bcp.ToString());
                EliminarProceso();
                Console.WriteLine("\nPROCESO TERMINADO");
            }
        }

        //  3
        private void ReanudarProceso()
        {
            int pidReanudar = 0;
            BCP bcp;

            Console.Clear();
            VerProcesosSuspendidos();
            Console.Write("\nPID: ");
            pidReanudar = Convert.ToInt32(Console.ReadLine());

            bcp = buscarPorPIDEnSuspendidos(pidReanudar);
            if (bcp != null)
            {
                Suspendidos.Remove(bcp);
                bcp.estado = "P";
                Preparados.Add(bcp);
                OrdenarPreparados();
            } else
            {
                Console.WriteLine("Proceso no existe en Suspendido");
            }
        }

        private static BCP buscarPorPIDEnSuspendidos(int pid)
        {
            foreach(BCP temp in Suspendidos)
            {
                if (temp.pid == pid)
                    return temp;
            }
            return null;
        }
        private static BCP buscarPorPIDEnTodos(int pid)
        {
            foreach (BCP temp in TodosProcesos)
            {
                if (temp.pid == pid)
                    return temp;
            }
            return null;
        }

        private void SuspenderProceso()
        {
            BCP bcp = Preparados[0];
            Preparados.Remove(bcp);
            bcp.estado = "S";
            Suspendidos.Add(bcp);
        }

        //  4
        private void MatarProceso()
        {
            int pidMatar = 0;
            BCP bcp;

            Console.Clear();
            VerTodosProcesos();
            Console.Write("\nPID: ");
            pidMatar = Convert.ToInt32(Console.ReadLine());

            bcp = buscarPorPIDEnTodos(pidMatar);
            if (bcp != null)
            {
                Suspendidos.Remove(bcp);
                Preparados.Remove(bcp);
                TodosProcesos.Remove(bcp);
                bcp = null;
            } else
            {
                Console.WriteLine("No existe el proceso");
            }

        }


        private static void EliminarProceso()
        {
            BCP bcp = Preparados[0];
            Preparados.Remove(bcp);
            TodosProcesos.Remove(bcp);
            bcp = null;
        }
    
        //  5
        private void VerTodosProcesos()
        {
            DateTime ahorita;
            TimeSpan edad;
            Console.Clear();
            Console.WriteLine("Procesos del sistema: ");
            foreach(BCP temp in TodosProcesos)
            {
                Console.WriteLine(temp.ToString() + "\n");
                ahorita = DateTime.Now;
                edad = ahorita - temp.horaCreacion;
                Console.WriteLine("Edad:\t\t\t{0}\n", edad);
            }
            Console.ReadKey();
        }

        //  6
        private void VerProcesosPreparados()
        {
            DateTime ahorita;
            TimeSpan edad;
            Console.Clear();
            Console.WriteLine("Procesos preparados: ");
            foreach (BCP temp in Preparados)
            {
                Console.WriteLine(temp.ToString() + "\n");
                ahorita = DateTime.Now;
                edad = ahorita - temp.horaCreacion;
                Console.WriteLine("Edad:\t\t\t{0}\n", edad);
            }
            Console.ReadKey();
        }

        //  7
        private void VerProcesosSuspendidos()
        {
            DateTime ahorita;
            TimeSpan edad;
            Console.Clear();
            Console.WriteLine("Procesos suspendidos: ");
            foreach (BCP temp in Suspendidos)
            {
                Console.WriteLine(temp.ToString() + "\n");
                ahorita = DateTime.Now;
                edad = ahorita - temp.horaCreacion;
                Console.WriteLine("Edad:\t\t\t{0}\n", edad);
            }
            Console.ReadKey();
        }

        //  8
        private void Salir()
        {
            Preparados = null;
            Suspendidos = null;
            TodosProcesos = null;

            Console.WriteLine("Procesos destruidos");
            Console.ReadKey();
        }
    }
}
