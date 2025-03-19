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
    public partial class IntroducirValorDialog : Window
    {
        public IntroducirValorDialog()
        {
            InitializeComponent();
            txtValor.Focus();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtValor.Text, out _))
            {
                MessageBox.Show("Error: El valor introducido debe ser un número", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtValor.Text))
            {
                MessageBox.Show("Error: Se debe introducir el nuevo valor", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            

            this.DialogResult = true;
        }

        public string Answer
        {
            get { return txtValor.Text; }
        }

    }
}
