using System;
using System.IO;
using Spire.Xls;
using System.Data;

namespace WpfApp1
{
    class PagosWriter
    {
        string ruta = Environment.CurrentDirectory;
        string name = "ejemplo.xls";
        Opciones opciones;

        public PagosWriter(string nuevaRuta, string nuevoNombre)
        {
            if (!string.IsNullOrWhiteSpace(nuevaRuta))
            {
                ruta = nuevaRuta;
            }
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                name = nuevoNombre + ".xls";
            }
        }
        

        public PagosWriter(string nuevoNombre)
        {
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                name = nuevoNombre;
            }
        }
        public PagosWriter(string nuevoNombre, Opciones op)
        {
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                name = nuevoNombre;
            }
            opciones = op;
        }

        void CreateDataTable(DataTable dt, Pagos[] pagos)
        {
            dt.Columns.Add("Nombre",typeof(String));
            dt.Columns.Add("Horas", typeof(Double));
            dt.Columns.Add("Extras", typeof(Double));
            for(int i = 0; i < pagos.Length; i++)
            {
                dt.Rows.Add(pagos[i].Nombre, pagos[i].Horas, pagos[i].Extras);
            }
        }

        void CreateDataTable(DataTable dt, Pagos pagos)
        {
            dt.Columns.Add("Nombre", typeof(String));
            dt.Columns.Add("Horas", typeof(Double));
            dt.Columns.Add("Extras", typeof(Double));
            dt.Rows.Add(pagos.Nombre, pagos.Horas, pagos.Extras);
        }

        void AddSingleCell(DataTable dt, float valor)
        {
            dt.Columns.Add("Valor", typeof(float));
            dt.Rows.Add(valor);
        }

        public void SaveToExcel(Pagos[] pagos)
        {
            Workbook wb = new Workbook();
            wb.LoadFromFile(@".\Resources_NO_TOCAR\PlantillaAcompañamientosPagos.xlsx");
            Worksheet sheet = wb.Worksheets[0];

            CellRange locatedRange = sheet.AllocatedRange;

            DataTable dt = new DataTable();

            if (opciones != null)
            {
                dt = new DataTable();
                AddSingleCell(dt, opciones.precioHora);
                sheet.InsertDataTable(dt, false, 3, 2, true);
                dt = new DataTable();
                AddSingleCell(dt, opciones.IVA);
                sheet.InsertDataTable(dt, false, 4, 4, true);
            }
            dt = new DataTable();
            CreateDataTable(dt, pagos);

            sheet.InsertDataTable(dt, false, 7, 1, true);

            //string path = Path.Combine(ruta, name);
            wb.SaveToFile(name, ExcelVersion.Version97to2003);
        }
        public void SaveToExcel(Pagos p)
        {
            Workbook wb = new Workbook();
            wb.LoadFromFile(@".\Resources_NO_TOCAR\PlantillaAcompañamientosPagos.xlsx");
            Worksheet sheet = wb.Worksheets[0];

            CellRange locatedRange = sheet.AllocatedRange;

            DataTable dt = new DataTable();

            CreateDataTable(dt, p);

            sheet.InsertDataTable(dt, false, 7, 1, true);

            //string path = Path.Combine(ruta, name);
            wb.SaveToFile(name, ExcelVersion.Version97to2003);
        }
    }
}
 