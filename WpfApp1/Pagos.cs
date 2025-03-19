using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Pagos
    {
        public string Nombre = "";
        public double Horas = 0;
        public double Extras = 0;

        public Pagos(string n, double h, double e)
        {
            Nombre = n;
            Horas = h;
            Extras = e;
        }

        public Pagos()
        {
            Nombre = "Pepito";
            Horas = 2.25f;
            Extras = 3.5f;
        }

    }
}
