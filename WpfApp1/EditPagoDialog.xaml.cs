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
    public partial class EditPagoDialog : Window
    {

        float h, extra;
        List<DatosClientesWrapper> list;

        public EditPagoDialog(PagosWrapper p, List<DatosClientesWrapper> nList)
        {
            InitializeComponent();
            list = nList;
            txtNombre.ItemsSource = list;
            txtNombre.Text = p.Nombre;
            Debug.WriteLine("p: "+p.Nombre+" / t: "+txtNombre.SelectedItem);
            txtHoras.Text = p.Horas.ToString();
            txtExtras.Text = p.Extra.ToString();
            txtComentario.Text = p.Comentario;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(txtHoras.Text, out h))
            {
                MessageBox.Show("Error: El valor de las horas debe ser un número", "Error al introducir datos del pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!float.TryParse(txtExtras.Text, out extra))
            {
                MessageBox.Show("Error: El valor de los extras debe ser un número", "Error al introducir datos del pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
        }

        public PagosWrapper Answer
        {
            get { return new PagosWrapper() { Nombre = txtNombre.Text, Horas = h, Extra = extra, Comentario = txtComentario.Text }; }
        }

    }
}
