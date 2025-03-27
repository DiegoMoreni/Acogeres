using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class AddPagoDialog : Window
    {
        float h, extra;
        //DatosClientesReader dcr;
        List<DatosClientesWrapper> list;

        public AddPagoDialog( List<DatosClientesWrapper> nlist)
        {
            InitializeComponent();
            list = nlist;
            txtNombre.ItemsSource = list;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if ((String.IsNullOrWhiteSpace(txtNombre.Text)))
            {
                MessageBox.Show("Error: Se debe especificar un cliente", "Error al introducir datos del pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if(!float.TryParse(txtHoras.Text, out h))
            {
                MessageBox.Show("Error: El valor de las horas debe ser un número", "Error al introducir datos del pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(!float.TryParse(txtExtras.Text, out extra))
            {
                MessageBox.Show("Error: El valor de los extras debe ser un número", "Error al introducir datos del pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtComentario.Text))
                txtComentario.Text = "";

            /*if (dcr.FindClientes(txtNombre.Text).Count < 1)
            {
                MessageBox.Show("Error: El nombre introducido no se corresponde con ningún cliente", "Error al introducir datos del pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }*/

            this.DialogResult = true;
        }

        public PagosWrapper Answer
        {
            get { return new PagosWrapper(list.Find(x => x.asciiNombre == txtNombre.Text), h, extra, txtComentario.Text); }
        }
    }
}
