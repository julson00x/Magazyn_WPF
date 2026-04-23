using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Magazyn_WPF.Models;
using Magazyn_WPF.ViewModels.Base;
using Magazyn_WPF.Views;
using System.ComponentModel;
using System.Windows.Data;
using Magazyn_WPF.Services;

namespace Magazyn_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DataService _dataService = new DataService();

        private ObservableCollection<Produkt> _produkty;
        public ObservableCollection<Produkt> Produkty
        {
            get => _produkty;
            set => SetProperty(ref _produkty, value);
        }

        private Produkt? _wybranyProdukt;
        public Produkt? WybranyProdukt
        {
            get => _wybranyProdukt;
            set => SetProperty(ref _wybranyProdukt, value);
        }

        private int _liczbaProduktow;
        public int LiczbaProduktow
        {
            get => _liczbaProduktow;
            set => SetProperty(ref _liczbaProduktow, value);
        }

        private int _calkowitaIloscWmagazynie;
        public int CalkowitaIloscWmagazynie
        {
            get => _calkowitaIloscWmagazynie;
            set => SetProperty(ref _calkowitaIloscWmagazynie, value);
        }

        // Wyszukiwarka tekstowa
        private string _wyszukiwanaFraza = string.Empty;
        public string WyszukiwanaFraza
        {
            get => _wyszukiwanaFraza;
            set
            {
                if (SetProperty(ref _wyszukiwanaFraza, value))
                    CollectionViewSource.GetDefaultView(Produkty).Refresh();
            }
        }

        // Filtr kategorii
        private string _wybranaKategoria = "Wszystkie";
        public string WybranaKategoria
        {
            get => _wybranaKategoria;
            set
            {
                if (SetProperty(ref _wybranaKategoria, value))
                    CollectionViewSource.GetDefaultView(Produkty).Refresh();
            }
        }

        // Lista kategorii do ComboBoxa (z "Wszystkie" na pocz¹tku)
        public ObservableCollection<string> Kategorie { get; } = new ObservableCollection<string>
        {
            "Wszystkie", "Elektronika", "Narzêdzia", "Meble", "Materia³y budowlane", "Inne"
        };

        // Komendy
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public MainViewModel()
        {
            LoadData();

            DeleteCommand = new RelayCommand(DeleteProduct, CanModifyProduct);
            AddCommand = new RelayCommand(AddProduct);
            EditCommand = new RelayCommand(EditProduct, CanModifyProduct);
            ClearSelectionCommand = new RelayCommand(ClearSelection);

            CollectionViewSource.GetDefaultView(Produkty).Filter = FiltrujProdukty;
        }

        private void LoadData()
        {
            var zapisane = _dataService.LoadProducts();
            if (zapisane.Count > 0)
                Produkty = new ObservableCollection<Produkt>(zapisane);
            else
            {
                LoadTestData();
                _dataService.SaveProducts(Produkty);
            }
            PrzeliczStatystyki();
        }

        private void AddProduct(object? obj)
        {
            var formWindow = new ProductFormWindow();
            formWindow.ShowDialog();
            if (formWindow.Saved)
            {
                var produkt = formWindow.Produkt;
                produkt.Id = Produkty.Any() ? Produkty.Max(p => p.Id) + 1 : 1;
                produkt.DataDodania = DateTime.Now;
                Produkty.Add(produkt);
                PrzeliczStatystyki();
                _dataService.SaveProducts(Produkty);
            }
        }

        private void EditProduct(object? obj)
        {
            if (WybranyProdukt == null) return;
            var formWindow = new ProductFormWindow(WybranyProdukt);
            formWindow.ShowDialog();
            if (formWindow.Saved)
            {
                PrzeliczStatystyki();
                _dataService.SaveProducts(Produkty);
            }
        }

        private bool CanModifyProduct(object? obj) => WybranyProdukt != null;

        private void DeleteProduct(object? obj)
        {
            if (WybranyProdukt != null)
            {
                Produkty.Remove(WybranyProdukt);
                PrzeliczStatystyki();
                _dataService.SaveProducts(Produkty);
            }
        }

        private void PrzeliczStatystyki()
        {
            if (Produkty == null) return;
            LiczbaProduktow = Produkty.Count;
            CalkowitaIloscWmagazynie = Produkty.Sum(p => p.Iloœæ);
        }

        // Filtruje po frazie tekstowej ORAZ wybranej kategorii
        private bool FiltrujProdukty(object obj)
        {
            if (obj is not Produkt produkt) return false;

            bool pasujeFraza = string.IsNullOrWhiteSpace(WyszukiwanaFraza) ||
                               produkt.Nazwa.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase) ||
                               produkt.Kategoria.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);

            bool pasujeKategoria = WybranaKategoria == "Wszystkie" ||
                                   produkt.Kategoria == WybranaKategoria;

            return pasujeFraza && pasujeKategoria;
        }

        private void ClearSelection(object? obj) => WybranyProdukt = null;

        private void LoadTestData()
        {
            Produkty = new ObservableCollection<Produkt>
            {
                new Produkt { Id = 1, Nazwa = "Œruba M8", Kategoria = "Narzêdzia", Iloœæ = 500, Jednostka = "szt.", Lokalizacja = "Pó³ka A1", DataDodania = DateTime.Now.AddDays(-30) },
                new Produkt { Id = 2, Nazwa = "M³otek ciesielski", Kategoria = "Narzêdzia", Iloœæ = 15, Jednostka = "szt.", Lokalizacja = "Pó³ka A2", DataDodania = DateTime.Now.AddDays(-45) },
                new Produkt { Id = 3, Nazwa = "Wiertarka udarowa", Kategoria = "Narzêdzia", Iloœæ = 8, Jednostka = "szt.", Lokalizacja = "Rega³ B1", DataDodania = DateTime.Now.AddDays(-10) },
                new Produkt { Id = 4, Nazwa = "Zestaw kluczy p³askich", Kategoria = "Narzêdzia", Iloœæ = 20, Jednostka = "kpl.", Lokalizacja = "Pó³ka A3", DataDodania = DateTime.Now.AddDays(-100) },
                new Produkt { Id = 5, Nazwa = "Pi³a tarczowa", Kategoria = "Narzêdzia", Iloœæ = 5, Jednostka = "szt.", Lokalizacja = "Rega³ B2", DataDodania = DateTime.Now.AddDays(-5) },
                new Produkt { Id = 8, Nazwa = "Klej monta¿owy", Kategoria = "Materia³y budowlane", Iloœæ = 25, Jednostka = "l", Lokalizacja = "Pó³ka C1", DataDodania = DateTime.Now.AddDays(-15) },
                new Produkt { Id = 9, Nazwa = "Pianka poliuretanowa", Kategoria = "Materia³y budowlane", Iloœæ = 40, Jednostka = "szt.", Lokalizacja = "Pó³ka C2", DataDodania = DateTime.Now.AddDays(-8) },
                new Produkt { Id = 10, Nazwa = "Cement 25kg", Kategoria = "Materia³y budowlane", Iloœæ = 100, Jednostka = "worek", Lokalizacja = "Paleta 1", DataDodania = DateTime.Now.AddDays(-2) },
                new Produkt { Id = 14, Nazwa = "Lampka LED", Kategoria = "Elektronika", Iloœæ = 120, Jednostka = "szt.", Lokalizacja = "Rega³ D1", DataDodania = DateTime.Now.AddDays(-7) },
                new Produkt { Id = 15, Nazwa = "Przewód YDYp 3x1.5", Kategoria = "Elektronika", Iloœæ = 500, Jednostka = "m", Lokalizacja = "Bêben 1", DataDodania = DateTime.Now.AddDays(-1) },
                new Produkt { Id = 19, Nazwa = "Rêkawice robocze", Kategoria = "Inne", Iloœæ = 150, Jednostka = "para", Lokalizacja = "Pó³ka F1", DataDodania = DateTime.Now.AddDays(-40) },
                new Produkt { Id = 20, Nazwa = "Stó³ warsztatowy", Kategoria = "Meble", Iloœæ = 3, Jednostka = "szt.", Lokalizacja = "Hala A", DataDodania = DateTime.Now.AddDays(-90) },
            };
            PrzeliczStatystyki();
        }
    }
}