using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Magazyn_WPF.Models;

namespace Magazyn_WPF.Views
{
    public partial class ProductFormWindow : Window
    {
        public Produkt Produkt { get; set; }
        public bool IsNew { get; set; }

        public ProductFormWindow(Produkt produkt = null)
        {
            InitializeComponent();
            IsNew = produkt == null;
            Produkt = produkt ?? new Produkt();
            LoadData();
        }

        private void LoadData()
        {
            if (!IsNew)
            {
                NazwaTextBox.Text = Produkt.Nazwa;
                KategoriaComboBox.SelectedItem = KategoriaComboBox.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(x => x.Content.ToString() == Produkt.Kategoria);
                IlośćTextBox.Text = Produkt.Ilość.ToString();
                JednostkaTextBox.Text = Produkt.Jednostka;
                LokalizacjaTextBox.Text = Produkt.Lokalizacja;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NazwaTextBox.Text))
            {
                MessageBox.Show("Nazwa produktu jest wymagana!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Produkt.Nazwa = NazwaTextBox.Text;
            Produkt.Kategoria = (KategoriaComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            
            if (int.TryParse(IlośćTextBox.Text, out int ilość))
            {
                Produkt.Ilość = ilość;
            }
            
            Produkt.Jednostka = JednostkaTextBox.Text;
            Produkt.Lokalizacja = LokalizacjaTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
