using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class AddClienteDialog : Window
    {
        public AddClienteDialog()
        {
            InitializeComponent();
            txtNombre.Focus();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if ((String.IsNullOrWhiteSpace(txtNombre.Text)) || (String.IsNullOrWhiteSpace(txtDNI.Text)) || (String.IsNullOrWhiteSpace(txtIBAN.Text)))
            {
                MessageBox.Show("Error: Se deben introducir todos los datos del nuevo cliente (nombre, DNI, IBAN)", "Error al introducir datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
        }

        public DatosClientesWrapper Answer
        {
            get { return new DatosClientesWrapper() { Nombre = txtNombre.Text, DNI = txtDNI.Text, IBAN = txtIBAN.Text }; }
        }

    }
}
