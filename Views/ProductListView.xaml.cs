using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Magazyn_WPF.Models;

namespace Magazyn_WPF.Views
{
    public partial class ProductListView : UserControl
    {
        public ProductListView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var formWindow = new ProductFormWindow();
            if (formWindow.ShowDialog() == true)
            {
                var produkt = formWindow.Produkt;
                produkt.Id = ((List<Produkt>)ProductDataGrid.ItemsSource).Max(p => p.Id) + 1;
                produkt.DataDodania = DateTime.Now;
                ((List<Produkt>)ProductDataGrid.ItemsSource).Add(produkt);
                ProductDataGrid.Items.Refresh();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductDataGrid.SelectedItem as Produkt;
            if (selectedProduct == null)
            {
                MessageBox.Show("Proszę wybrać produkt do edycji", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var formWindow = new ProductFormWindow(selectedProduct);
            if (formWindow.ShowDialog() == true)
            {
                ProductDataGrid.Items.Refresh();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductDataGrid.SelectedItem as Produkt;
            if (selectedProduct == null)
            {
                MessageBox.Show("Proszę wybrać produkt do usunięcia", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Czy na pewno chcesz usunąć produkt \"{selectedProduct.Nazwa}\"?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ((List<Produkt>)ProductDataGrid.ItemsSource).Remove(selectedProduct);
                ProductDataGrid.Items.Refresh();
            }
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditButton_Click(null, null);
        }
    }
}

