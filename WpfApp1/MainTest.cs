using System;
using System.Collections.Generic;

namespace WpfApp1
{
    class MainTest
    {
        static void dfMain(string[] args)
        {
            //TestDatosReader();
            TestAll();
        }

        static void TestAll()
        {
            DatosClientesReader dcr = new DatosClientesReader();
            dcr.PrepareData();

            string nombre = "Cacatua";
            float horas = 5;
            float extra = 3;

            List<DatosClientes> busqueda = dcr.FindClientes(nombre);
            if (busqueda.Count == 0)
            {
                Console.WriteLine("No hay resultados");
                return;
            }

            Pagos p = new Pagos(nombre, horas, extra);
            DatosYHoras dyh = new DatosYHoras(busqueda[0],p,"");
            
            PagosWriter pw = new PagosWriter("","");
            DatosYHorasWriter dyhw = new("","");
            pw.SaveToExcel(p);
            dyhw.WriteToDoc(dyh);
        }

        static void TestDatosReader()
        {
            DatosClientesReader dcr = new DatosClientesReader();
            dcr.PrepareData();

            List<DatosClientes> busqueda = dcr.FindClientes("María");
            busqueda.ForEach(Console.WriteLine);
            if(busqueda.Count == 0)
            {
                Console.WriteLine("No hay resultados");
            }

            dcr.AddDataToFile(new DatosClientes("Pepito Grillo", "asdasd", "asdasdas"));

            DatosClientes d2 = new DatosClientes("Pepito Grillo", "sdffsdf", "asdasdas");
            dcr.AddDataToFile(d2);

            busqueda = dcr.FindClientes("Pep");
            busqueda.ForEach(Console.WriteLine);

            DatosClientes d3 = new DatosClientes("Cacatua", "sdf34df", "asdasd34as");
            dcr.AddDataToFile(d3);

            busqueda = dcr.FindClientes("Cac");
            busqueda.ForEach(Console.WriteLine);

        }

        static void TestDYHWriter()
        {
            DatosYHorasWriter p = new DatosYHorasWriter("","");

            DatosYHoras[] datos = new DatosYHoras[5];
            for (int i = 0; i < datos.Length; i++)
            {
                datos[i] = new DatosYHoras();
            }

            p.WriteToDoc(datos);
        }

        static void TestPagosWriter()
        {
            PagosWriter pW = new PagosWriter("","");
            Pagos[] pagos = new Pagos[5];
            for (int i = 0; i < pagos.Length; i++)
            {
                pagos[i] = new Pagos();
            }
            pW.SaveToExcel(pagos);
        }

    }
}
