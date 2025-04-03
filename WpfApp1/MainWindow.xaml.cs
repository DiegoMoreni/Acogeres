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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Diagnostics;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        string Version = "0.5.0";

        List<DatosClientesWrapper> itemsDatosClientes = new List<DatosClientesWrapper>();
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        DatosClientesReader dcr = new DatosClientesReader();

        int numPagos;

        List<PagosWrapper> itemsPagosWrapper = new List<PagosWrapper>();

        Opciones opciones = new();
        static string opcionesPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Resources_NO_TOCAR");
        string opcionesFilePath = opcionesPath + @"\Opciones.json";

        bool pagosModificado = false;
        bool clientesModificado = false;
        public MainWindow()
        {
            InitializeComponent();

            opciones = OpenOrCreateOptions();
            UpdateOptions();
            dcr.PrepareData();
            foreach(DatosClientes dc in dcr.datos)
            {
                itemsDatosClientes.Add(new DatosClientesWrapper(dc));
            }
            lvDatosClientes.ItemsSource = itemsDatosClientes;

            lvPagos.ItemsSource = itemsPagosWrapper;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvDatosClientes.ItemsSource);
            view.Filter = UserFilter;
            view.SortDescriptions.Add(new SortDescription("Nombre", ListSortDirection.Ascending));

            CollectionView viewPagos = (CollectionView)CollectionViewSource.GetDefaultView(lvPagos.ItemsSource);
            UpdateNumPagos();
            txtVersion.Content = "Versión: " + Version;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

            // Falta añadir logica para controlar cuando estan modificadas las cosas y cambiar el mensaje de acuerdo a ello.

            if (pagosModificado)
            {
                MessageBoxResult result = MessageBox.Show("Hay pagos sin guardar. \nSi sales sin guardar se perderán esos pagos. \nPara guardarlos debes generar un archivo de pagos.\n¿Quieres salir de todas formas?", "Aviso", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }

            }
            else
            {
                if (clientesModificado)
                {
                    MessageBoxResult result = MessageBox.Show("Hay datos de clientes sin guardar.\nSi sales sin guardar se perderán esos datos.\n¿Quieres guardarlos antes de salir?", "Aviso", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            dcr.AddAllData(itemsDatosClientes);
                            e.Cancel = false;
                            return;
                        case MessageBoxResult.No:
                            e.Cancel = false;
                            return;
                        case MessageBoxResult.Cancel:
                            e.Cancel = true;
                            return;
                    }
                }
            }
        }

        private void HandleColumnHeaderSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (sizeChangedEventArgs.NewSize.Width <= 80)
            {
                sizeChangedEventArgs.Handled = true;
                if(((GridViewColumnHeader)sender).Column == null)
                {
                    // La Column a veces puede ser null. Es necesario comprobarlo para que no salte excepcion.
                    // Si es null se salta y listo.
                    //Debug.WriteLine("SENDER ES NULL??");
                    return;
                }
                ((GridViewColumnHeader)sender).Column.Width = 80;
            }
        }

        private bool UserFilter(object item)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                return true;
            }
            else
            {
                string accentedStr = txtFilter.Text;
                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

                //return (item as DatosClientesWrapper).Nombre.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                return (item as DatosClientesWrapper).asciiNombre.IndexOf(asciiStr, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvDatosClientes.ItemsSource).Refresh();
        }

        private void tabItem_Click(object sender, MouseButtonEventArgs e)
        {
            txtFilter.Text = "";
            CollectionViewSource.GetDefaultView(lvDatosClientes.ItemsSource).Refresh();
        }

        private void btnAddCliente_Click(object sender, RoutedEventArgs e)
        {
            AddClienteDialog dialog = new AddClienteDialog();
            if(dialog.ShowDialog() == true)
            {
                DatosClientesWrapper cliente = dialog.Answer;
                itemsDatosClientes.Add(cliente);
                CollectionViewSource.GetDefaultView(lvDatosClientes.ItemsSource).Refresh();
                //dcr.AddDataToFile(dialog.Answer.ToDatosClientes());
                MessageBox.Show("Se han añadido el cliente: " + dialog.Answer.ToString(), "Cliente añadido", MessageBoxButton.OK);
                lvDatosClientes.SelectedItem = cliente;
                lvDatosClientes.ScrollIntoView(lvDatosClientes.SelectedItem);
                clientesModificado = true;
            }
        }

        private void btnEditCliente_Click(object sender, RoutedEventArgs e)
        {
            if (lvDatosClientes.SelectedItem == null)
                return;
            EditClienteDialog dialog = new EditClienteDialog((DatosClientesWrapper)lvDatosClientes.SelectedItem);
            if(dialog.ShowDialog() == true)
            {
                //dcr.EditDataFromFile(((DatosClientesWrapper)lvDatosClientes.SelectedItem).ToDatosClientes(), dialog.Answer.ToDatosClientes());
                itemsDatosClientes.Remove((DatosClientesWrapper)lvDatosClientes.SelectedItem);
                itemsDatosClientes.Add(dialog.Answer);
            }
            CollectionViewSource.GetDefaultView(lvDatosClientes.ItemsSource).Refresh();
        }

        private void btnDeleteCliente_Click(object sender, RoutedEventArgs e)
        {
            if (lvDatosClientes.SelectedItem == null)
                return;
            MessageBoxResult result = MessageBox.Show("¿Eliminar el cliente "+ ((DatosClientesWrapper)lvDatosClientes.SelectedItem) + "? Este cambio es permanente y no se puede deshacer.", "Aviso", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                //dcr.RemoveDataFromFile(((DatosClientesWrapper)lvDatosClientes.SelectedItem).ToDatosClientes());
                itemsDatosClientes.Remove((DatosClientesWrapper)lvDatosClientes.SelectedItem);
                CollectionViewSource.GetDefaultView(lvDatosClientes.ItemsSource).Refresh();
            }
        }

        private void lvDatosHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if(listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvDatosClientes.Items.SortDescriptions.Clear();
            }
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvDatosClientes.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void btnAddPago_Click(object sender, RoutedEventArgs e)
        {
            AddPagoDialog dialog = new AddPagoDialog(itemsDatosClientes);
            if (dialog.ShowDialog() == true)
            {
                itemsPagosWrapper.Add(dialog.Answer);
                CollectionViewSource.GetDefaultView(lvPagos.ItemsSource).Refresh();
                pagosModificado = true;
            }
            UpdateNumPagos();
        }

        private void btnEditPago_Click(object sender, RoutedEventArgs e)
        {
            if (lvPagos.SelectedItem == null)
                return;
            EditPagoDialog dialog = new EditPagoDialog((PagosWrapper)lvPagos.SelectedItem, itemsDatosClientes);
            if(dialog.ShowDialog() == true)
            {
                itemsPagosWrapper.Remove((PagosWrapper)lvPagos.SelectedItem);
                itemsPagosWrapper.Add(dialog.Answer);
            }
            CollectionViewSource.GetDefaultView(lvPagos.ItemsSource).Refresh();
            UpdateNumPagos();
        }

        
        private void btnTestBug_Click(object sender, RoutedEventArgs e)
        {
            string s = null;
            s.Trim();
        }


        private void btnDeletePago_Click(object sender, RoutedEventArgs e)
        {
            if (lvPagos.SelectedItem == null)
                return;
            MessageBoxResult result = MessageBox.Show("¿Eliminar el siguiente pago?\n" + ((PagosWrapper)lvPagos.SelectedItem) + "\nEste cambio es permanente y no se puede deshacer.", "Aviso", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                itemsPagosWrapper.Remove((PagosWrapper)lvPagos.SelectedItem);
                CollectionViewSource.GetDefaultView(lvPagos.ItemsSource).Refresh();
                if(itemsPagosWrapper.Count == 0)
                {
                    pagosModificado = false;
                }
            }
            UpdateNumPagos();
        }

        private void lvPagosHeader_Click(object sender, RoutedEventArgs e)
        {
            // Actualmente, está desactivada la opción de ordenar la lista de pagos alfabéticamente.
            // Si se quiere volver a activar, hay que añadir al GridViewColumnHeader de la ListView
            // lvPagos la opción "Click="lvPagosHeader_Click"
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvPagos.Items.SortDescriptions.Clear();
            }
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvPagos.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void UpdateNumPagos()
        {
            numPagos = itemsPagosWrapper.Count;
            txtNumPagos.Content = " El archivo contendrá " +numPagos+" pagos.";
        }

        private void btnGenerarArchivosPagos_Click(object sender, RoutedEventArgs e)
        {
            if(numPagos < 1)
            {
                MessageBox.Show("Error: No se puede generar un archivo con 0 pagos", "Error al generar archivos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog dialog = new();
            dialog.Filter = "Excel (*.xls)|*.xls";
            dialog.Title = "Guardar archivo de pagos";
            if (string.IsNullOrWhiteSpace(opciones.rutaPagos))
            {
                dialog.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                dialog.InitialDirectory = opciones.rutaPagos;
            }

            if (dialog.ShowDialog() == true)
            {
                PagosWriter pWriter = new(dialog.FileName, opciones);
                List<Pagos> pagosList = new();
                foreach (PagosWrapper pw in itemsPagosWrapper)
                {
                    pagosList.Add(pw.GetPagos());
                }
                pWriter.SaveToExcel(pagosList.ToArray());
                dcr.AddAllData(itemsDatosClientes);
                MessageBox.Show("Se han generado el archivo de Pagos.", "Archivos generados", MessageBoxButton.OK);
                clientesModificado = false;
                pagosModificado = false;
            }
        }

        private void btnGenerarArchivosDyH_Click(object sender, RoutedEventArgs e)
        {
            if (numPagos < 1)
            {
                MessageBox.Show("Error: No se puede generar un archivo con 0 pagos", "Error al generar archivos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog dialog = new();
            dialog.Filter = "Documento de Word (*.docx)|*.docx";
            dialog.Title = "Guardar archivo de datos y horas";
            if (string.IsNullOrWhiteSpace(opciones.rutaDyH))
            {
                dialog.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                dialog.InitialDirectory = opciones.rutaDyH;
            }
            if (dialog.ShowDialog() == true)
            {
                DatosYHorasWriter dyhWriter = new(dialog.FileName);
                List<DatosYHoras> dyhList = new();
                foreach (PagosWrapper pw in itemsPagosWrapper)
                {
                    Pagos p = pw.GetPagos();
                    //dyhList.Add(new(dcr.FindClientes(pw.Nombre)[0], p, pw.Comentario));
                    dyhList.Add(new(itemsDatosClientes.Find(x => x.Nombre == pw.Nombre || x.asciiNombre == pw.Nombre).ToDatosClientes(), p, pw.Comentario));
                }
                dyhWriter.WriteToDoc(dyhList.ToArray());
                MessageBox.Show("Se han generado el archivo de Datos y Horas.", "Archivos generados", MessageBoxButton.OK);
                pagosModificado = false;
            }
        }

        Opciones OpenOrCreateOptions()
        {
            Opciones tempOpciones;
            
            DirectoryInfo di = Directory.CreateDirectory(opcionesPath);
            try
            {
                using StreamReader streamReader = new(opcionesFilePath);
                var jsonFile = streamReader.ReadToEnd();
                JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
                tempOpciones = JsonSerializer.Deserialize<Opciones>(jsonFile, _options);
                Debug.WriteLine("Funciona: " + tempOpciones.IVA + tempOpciones.precioHora);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: " + e.Message);
                tempOpciones = new() { IVA = 10, precioHora = 15, rutaPagos = "", rutaDyH = "" };
                string json = JsonSerializer.Serialize(tempOpciones);
                File.WriteAllText(opcionesPath+@"\Opciones.json", json);
            }
            return tempOpciones;
        }

        void UpdateOptions()
        {
            string json = JsonSerializer.Serialize(opciones);
            Task.Run(() => File.WriteAllText(opcionesPath + @"\Opciones.json", json));


            txtIVA.Content = opciones.IVA;
            txtPrecioHora.Content = opciones.precioHora;
            txtRutaPagos.Content = "Pagos: "+ opciones.rutaPagos;
            txtRutaPagosOpciones.Content = opciones.rutaPagos;
            txtRutaDyH.Content = "Datos y Horas: "+ opciones.rutaDyH;
            txtRutaDyHOpciones.Content = opciones.rutaDyH;
        }

        private void btnModifIVA_Click(object sender, RoutedEventArgs e)
        {
            IntroducirValorDialog dialog = new IntroducirValorDialog();
            if(dialog.ShowDialog() == true)
            {
                opciones.IVA = float.Parse(dialog.Answer);
            }
            UpdateOptions();
        }
        private void btnModifPrecio_Click(object sender, RoutedEventArgs e)
        {
            IntroducirValorDialog dialog = new IntroducirValorDialog();
            if (dialog.ShowDialog() == true)
            {
                opciones.precioHora = float.Parse(dialog.Answer);
            }
            UpdateOptions();
        }
        private void btnModifRutaPagos_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == true)
            {
                //txtRutaPagos.Content = dialog.SelectedPath;
                //txtRutaPagosOpciones.Content = dialog.SelectedPath;
                opciones.rutaPagos = dialog.SelectedPath;
                UpdateOptions();
            }
        }
        private void btnModifRutaDyH_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == true)
            {
                //txtRutaDyH.Content = dialog.SelectedPath;
                //txtRutaDyHOpciones.Content = dialog.SelectedPath;
                opciones.rutaDyH = dialog.SelectedPath;
                UpdateOptions();
            }
        }

       
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }

    public class DatosClientesWrapper
    {
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string IBAN { get; set; }

        public string asciiNombre { get; set; }

        public DatosClientesWrapper() { }
        public DatosClientesWrapper(DatosClientes dc)
        {
            Nombre = dc.datos[0];
            DNI = dc.datos[1];
            IBAN = dc.datos[2];
            asciiNombre = "";

            string accentedStr = dc.datos[0]; 
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            asciiNombre = asciiStr;
        }

        public void UpdateAscii()
        {
            string accentedStr = Nombre;
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            asciiNombre = asciiStr;
        }

        public override string ToString()
        {
            return Nombre;
            // + " (DNI: " + DNI + ", IBAN: " + IBAN + ")";
        }

        public DatosClientes ToDatosClientes()
        {
            return new DatosClientes(Nombre, DNI, IBAN);
        }

    }
    public class PagosWrapper
    {
        public string Nombre { get; set; }
        public float Horas { get; set; }
        public float Extra { get; set; }
        public string Comentario { get; set; }
        public string DNI { get; set; }
        public string IBAN { get; set; }

        public PagosWrapper() { }
        public PagosWrapper(string n, float h, float e, string c) 
        {
            Nombre = n;
            Horas = h;
            Extra = e;
            Comentario = c;
        }
        public PagosWrapper(DatosClientesWrapper cliente ,float h, float e, string c)
        {
            Nombre = cliente.Nombre;
            DNI = cliente.DNI;
            IBAN = cliente.IBAN;
            Horas = h;
            Extra = e;
            Comentario = c;
        }

        public override string ToString()
        {
            return Nombre + "\n" + Horas + " Horas + " + Extra + "€\nNota: " + Comentario;
        }

        // Hacer metodo ToDatos
        public Pagos GetPagos()
        {
            return new Pagos(Nombre, Horas, Extra);
        }

    }


}
