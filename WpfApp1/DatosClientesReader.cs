using System;
using Spire.Xls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;

namespace WpfApp1
{
    public class DatosClientesReader
    {
        Workbook wb = new Workbook();
        public List<DatosClientes> datos = new List<DatosClientes>();

        public void PrepareData()
        {
            OpenDataFile();
            GetDataFromFile();
        }

        void OpenDataFile()
        {
            wb.LoadFromFile(@".\Resources_NO_TOCAR\DatosClientes.xlsx");
        }

        void GetDataFromFile()
        {
            Worksheet sheet = wb.Worksheets[0];
            CellRange locatedRange = sheet.AllocatedRange;

            for (int i = 1; i < locatedRange.Rows.Length; i++)
            {
                DatosClientes temp = new DatosClientes();

                for (int j = 0; j < 3; j++)
                {

                    temp.datos[j] = locatedRange[i + 1, j + 1].Value;

                    //Console.Write(locatedRange[i + 1, j + 1].Value + "  ");
                    // Los datos se obtienen aquí, cada valor de j es una celda del excel.
                    // Ejemplo: j = 0 -> nombre
                    //          j = 1 -> dni
                    //          j = 2 -> iban
                }
                datos.Add(temp);
            }
        }

        public void AddDataToFile(DatosClientes dc)
        {
            Worksheet ws = wb.Worksheets[0];
            DataTable dt = new DataTable();

            if (datos.Contains(dc))
            {
                Console.WriteLine("El cliente ya está en la lista " + dc.ToString());
                return;
            }
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("DNI", typeof(string));
            dt.Columns.Add("IBAN", typeof(string));
            dt.Rows.Add(dc.datos[0], dc.datos[1], dc.datos[2]);
            ws.InsertRow(2);
            ws.InsertDataTable(dt, false, 2, 1, true);

            //Esto igual da problemas.
            //ws.Protect("", SheetProtectionType.All);
            
            wb.Save();
        }

        public void AddAllData(List<DatosClientesWrapper> data)
        {
            foreach (DatosClientesWrapper dc in data) { 
                AddDataToFile(dc.ToDatosClientes()); 
            }
        }

        public List<DatosClientes> FindClientes(string datosABuscar)
        {
            List<DatosClientes> busqueda = new List<DatosClientes>();

            //busqueda = datos.Where(x => x.datos[0].Contains(datosABuscar)).ToList();
            
            string accentedStr = datosABuscar;
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

            //busqueda = datos.Where(x => x.datos[3].Contains(asciiStr)).ToList();

            busqueda = datos.Where(x => CultureInfo.CurrentCulture.CompareInfo.IndexOf(x.datos[0], asciiStr, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != -1).ToList<DatosClientes>();

            return busqueda;
        }

        public void RemoveDataFromFile(DatosClientes dc)
        {
            Worksheet ws = wb.Worksheets[0];
            DataTable dt = new DataTable();

            if (!datos.Contains(dc))
            {
                Console.WriteLine("No hay ningún cliente con ese nombre+dni+iban. " + dc.ToString());
                return;
            }
            int row = datos.FindIndex(a => a.datos[0] == dc.datos[0] && a.datos[1] == dc.datos[1] && a.datos[2] == dc.datos[2]);
            if (row == -1)
            {
                Console.WriteLine("Datos no encontrados");
                return;
            }
            Console.WriteLine("Datos encontrados en la fila: " + row);
            Console.WriteLine(datos[row]);
            ws.DeleteRow(row + 2);
            // Se usa row +2 en lugar de row:
            // -> +1 porque las listas empiezan con index 0, pero las hojas de calculo con indice 1
            // -> +1 porque la primera fila es la cabecera de la tabla
            // Total: row +1 +1
            wb.Save();

        }

        public void EditDataFromFile(DatosClientes old, DatosClientes nuevo)
        {
            RemoveDataFromFile(old);
            AddDataToFile(nuevo);
        }

    }
}
