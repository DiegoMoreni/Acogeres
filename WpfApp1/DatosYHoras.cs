using System;

namespace WpfApp1
{
    class DatosYHoras
    {
        public string Nombre = "Pepito Grillo";
        public string DNI = "12345678D";
        public string IBAN = "ES45 5654 6544 5640 5454";
        public double Horas = 2;
        public double Extra = 3;
        public string Notas = "Cacatua";

        public string info = "";

        public DatosYHoras() { }
        public DatosYHoras(DatosClientes d, Pagos p, string n)
        {
            Nombre = d.datos[0];
            DNI = d.datos[1];
            IBAN = d.datos[2];
            Horas = p.Horas;
            Extra = p.Extras;
            Notas = n;
        }

        public void ParseInfo()
        {
            bool h = false;
            //info = Nombre + "\n" + DNI + "\n" + IBAN + "\n";
            info = "\n" + DNI + "\n" + IBAN + "\n";
            if (Horas > 0)
            {
                int hInt = (int)Horas;
                info += hInt + " hora";
                if(hInt > 1)
                {
                    info += "s";
                }
                double minutos = (Horas - hInt) * 60;
                if(minutos > 0)
                {
                    info += " y " + minutos + " minutos";
                }
                h = true;
            }
            if (Extra > 0)
            {
                if (h) info += " + ";
                info += Extra + "€";
            }
            info += ".";
            if (!String.IsNullOrEmpty(Notas))//(Notas != "")
            {
                info += "\nNota: " + Notas;
            }
            info += "\n";
            Console.WriteLine(info);
        }
    }
}
