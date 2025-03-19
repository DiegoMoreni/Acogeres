using System;
using System.Diagnostics;

namespace WpfApp1
{
    public class DatosClientes
    {
        public string[] datos = new string[3];

        public DatosClientes() { }
        public DatosClientes(string nombre, string dni, string iban)
        {
            datos[0] = nombre;
            datos[1] = dni;
            datos[2] = iban;

            /*string accentedStr = nombre;
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            datos[3] = asciiStr;
            Debug.WriteLine(datos[3]);*/
        }

        public override string ToString()
        {
            return datos[0] + " " + datos[1] + " " + datos[2];
        }

        public override bool Equals(Object dc)
        {
            if (dc == null) return false;
            if (dc.GetType() != this.GetType())
                return false;

            DatosClientes temp = (DatosClientes)dc;

            return (temp.datos[0] == this.datos[0]) && (temp.datos[1] == this.datos[1]) && (temp.datos[2] == datos[2]);
        }

    }
}
