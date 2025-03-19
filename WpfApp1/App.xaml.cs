using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;

namespace WpfApp1
{
    public partial class App : Application
    {
        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
            Debug.WriteLine("Excepcion");
            Debug.WriteLine(e.Exception.Message);
            Debug.WriteLine(e.Exception.StackTrace);

            string logName = "ExceptionLog - " + DateTime.Now.ToString("hh mm ss tt") + ".txt";
            string logMessage = e.Exception.Message + "\n" + e.Exception.StackTrace;
            File.WriteAllText(logName, logMessage);
            
            MessageBox.Show("Ha ocurrido una excepción no controlada: \n" + e.Exception.Message+"\n Se ha generado un archivo .log", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
