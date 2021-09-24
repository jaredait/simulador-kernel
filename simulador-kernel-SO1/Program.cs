using System;
using System.Collections.Generic;

namespace simulador_kernel_SO1
{
    class Program
    {
        private static List<BCP> TodosProcesos = new List<BCP>();
        private static List<BCP> Preparados = new List<BCP>(); 
        private List<BCP> Suspendidos = new List<BCP>();


        static void Main(string[] args)
        {
            /*
            DateTime date1 = new DateTime(2010, 1, 1, 8, 0, 0);
            DateTime date2 = new DateTime(2010, 1, 1, 8, 0, 15);

            // Calculate the interval between the two dates.
            TimeSpan interval = date2 - date1;
            Console.WriteLine("{0} - {1} = {2}", date2, date1, interval.TotalSeconds);
            */


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

            // ordenar la lista Preparados segun su prioridad en orden ascendente
            Preparados.Sort(delegate (BCP x, BCP y)
            {
                return x.prioridad.CompareTo(y.prioridad);
            });
        }

        private void EjecutarProceso()
        {
            BCP bcp = Preparados[0];
            Preparados.RemoveAt(0);
            bcp.tiempoRestante = TimeSpan.FromTicks(bcp.tiempoEjecucion * 10000000);

            Console.WriteLine("\nEjecutando:\n{0}\n", bcp.ToString());

            int a = bcp.tiempoEjecucion;
            while (a >= 0)
            {
                Console.Write("\rTiempo restante {0:00}", a);
                System.Threading.Thread.Sleep(1000);
                bcp.tiempoRestante = TimeSpan.FromTicks(a * 10000000);
                a--;
            }

            Console.WriteLine("\n\n\nPROCESO TERMINADO\n{0}\n", bcp.ToString());

            Console.ReadKey();
        }
    }
}
