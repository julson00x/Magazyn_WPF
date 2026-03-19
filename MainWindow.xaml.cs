using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Magazyn_WPF.Models;

namespace Magazyn_WPF
{
    public partial class MainWindow : Window
    {
        private List<Produkt> produkty;

        public MainWindow()
        {
            InitializeComponent();
            LoadTestData();
        }

        private void LoadTestData()
        {
            produkty = new List<Produkt>
            {
                new Produkt
                {
                    Id = 1,
                    Nazwa = "Śruba M8",
                    Kategoria = "Narzędzia",
                    Ilość = 500,
                    Jednostka = "szt.",
                    Lokalizacja = "Półka A1",
                    DataDodania = DateTime.Now.AddDays(-30)
                },
                new Produkt
                {
                    Id = 2,
                    Nazwa = "Klej montażowy",
                    Kategoria = "Materiały budowlane",
                    Ilość = 25,
                    Jednostka = "l",
                    Lokalizacja = "Półka B3",
                    DataDodania = DateTime.Now.AddDays(-15)
                },
                new Produkt
                {
                    Id = 3,
                    Nazwa = "Lampka LED",
                    Kategoria = "Elektronika",
                    Ilość = 120,
                    Jednostka = "szt.",
                    Lokalizacja = "Regał C2",
                    DataDodania = DateTime.Now.AddDays(-7)
                },
                new Produkt
                {
                    Id = 4,
                    Nazwa = "Stół drewniany",
                    Kategoria = "Meble",
                    Ilość = 8,
                    Jednostka = "szt.",
                    Lokalizacja = "Hala główna",
                    DataDodania = DateTime.Now.AddDays(-45)
                },
                new Produkt
                {
                    Id = 5,
                    Nazwa = "Piła elektryczna",
                    Kategoria = "Narzędzia",
                    Ilość = 3,
                    Jednostka = "szt.",
                    Lokalizacja = "Magazyn 2",
                    DataDodania = DateTime.Now.AddDays(-20)
                },
                new Produkt
                {
                    Id = 6,
                    Nazwa = "Farba akrylowa",
                    Kategoria = "Materiały budowlane",
                    Ilość = 150,
                    Jednostka = "l",
                    Lokalizacja = "Półka D1",
                    DataDodania = DateTime.Now.AddDays(-10)
                }
            };

            DataGrid productDataGrid = ProductListView.FindName("ProductDataGrid") as DataGrid;
            if (productDataGrid != null)
            {
                productDataGrid.ItemsSource = produkty;
            }
        }
    }
}
