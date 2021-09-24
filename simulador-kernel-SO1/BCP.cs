using System;
using System.Collections.Generic;
using System.Text;

namespace simulador_kernel_SO1
{
    class BCP
    {
        private String Nombre;
        private int Prioridad;
        private int TiempoEjecucion;
        private int PID;
        private String Estado;
        private DateTime HoraCreacion;
        private TimeSpan TiempoRestante;

        public BCP(string nombre, int prioridad, int tiempoEjecucion)
        {
            this.Nombre = nombre;
            this.Prioridad = prioridad;
            this.TiempoEjecucion = tiempoEjecucion;
            this.tiempoRestante = TimeSpan.FromTicks(tiempoEjecucion * 10000000);
        }

        public string nombre { get => Nombre; set => Nombre = value; }
        public int prioridad { get => Prioridad; set => Prioridad = value; }
        public int tiempoEjecucion { get => TiempoEjecucion; set => TiempoEjecucion = value; }
        public int pid { get => PID; set => PID = value; }
        public string estado { get => Estado; set => Estado = value; }
        public DateTime horaCreacion { get => HoraCreacion; set => HoraCreacion = value; }
        public TimeSpan tiempoRestante { get => TiempoRestante; set => TiempoRestante = value; }


        public int CompareTo(BCP bcpComparar)
        {
            // A null value means that this object is greater.
            if (bcpComparar == null)
                return 1;

            else
                return this.prioridad.CompareTo(bcpComparar.prioridad);
        }

        public override string ToString()
        {
            return "________________________________________________________" +
                "\nNombre:\t\t\t" + this.nombre +
                "\nPrioridad:\t\t" + this.prioridad +
                "\nTiempo de ejecución:\t" + tiempoEjecucion +
                "\nPID:\t\t\t" + pid +
                "\nEstado:\t\t\t" + estado +
                "\nHora de creación:\t" + horaCreacion +
                "\nTiempo Restante:\t" + tiempoRestante;
        }
    }
}
