using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Magazyn_WPF.Models;
using Magazyn_WPF.ViewModels.Base;

namespace Magazyn_WPF.ViewModels
{
    public class ProductFormViewModel : ViewModelBase
    {
        // Czy to nowy produkt czy edycja
        public bool IsNew { get; }

        // Obiekt który edytujemy (wype³niany przy zapisie)
        public Produkt Produkt { get; }

        // Wynik — czy u¿ytkownik klikn¹³ Zapisz
        public bool Saved { get; private set; } = false;

        // Akcja zamykaj¹ca okno (ustawiana z zewn¹trz)
        public Action? CloseAction { get; set; }

        // ===== Pola formularza =====

        private string _nazwa = string.Empty;
        public string Nazwa
        {
            get => _nazwa;
            set => SetProperty(ref _nazwa, value);
        }

        private string _wybranaKategoria = string.Empty;
        public string WybranaKategoria
        {
            get => _wybranaKategoria;
            set => SetProperty(ref _wybranaKategoria, value);
        }

        private string _iloscText = string.Empty;
        public string IloscText
        {
            get => _iloscText;
            set => SetProperty(ref _iloscText, value);
        }

        private string _wybranaJednostka = string.Empty;
        public string WybranaJednostka
        {
            get => _wybranaJednostka;
            set => SetProperty(ref _wybranaJednostka, value);
        }

        private string _lokalizacja = string.Empty;
        public string Lokalizacja
        {
            get => _lokalizacja;
            set => SetProperty(ref _lokalizacja, value);
        }

        // ===== B³êdy walidacji =====

        private string _nazwaError = string.Empty;
        public string NazwaError
        {
            get => _nazwaError;
            set => SetProperty(ref _nazwaError, value);
        }

        private string _kategoriaError = string.Empty;
        public string KategoriaError
        {
            get => _kategoriaError;
            set => SetProperty(ref _kategoriaError, value);
        }

        private string _iloscError = string.Empty;
        public string IloscError
        {
            get => _iloscError;
            set => SetProperty(ref _iloscError, value);
        }

        private string _jednostkaError = string.Empty;
        public string JednostkaError
        {
            get => _jednostkaError;
            set => SetProperty(ref _jednostkaError, value);
        }

        private string _lokalizacjaError = string.Empty;
        public string LokalizacjaError
        {
            get => _lokalizacjaError;
            set => SetProperty(ref _lokalizacjaError, value);
        }

        // ===== Listy do ComboBoxów =====

        public ObservableCollection<string> Kategorie { get; } = new ObservableCollection<string>
        {
            "Elektronika", "Narzêdzia", "Meble", "Materia³y budowlane", "Inne"
        };

        public ObservableCollection<string> Jednostki { get; } = new ObservableCollection<string>
        {
            "szt.", "kg", "g", "l", "ml", "m", "m2", "kpl.", "op.", "worek", "rolka", "para", "karton"
        };

        // ===== Komendy =====

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ProductFormViewModel(Produkt? produkt = null)
        {
            IsNew = produkt == null;
            Produkt = produkt ?? new Produkt();

            // Wczytaj dane istniej¹cego produktu do pól
            if (!IsNew)
            {
                Nazwa = Produkt.Nazwa;
                WybranaKategoria = Produkt.Kategoria;
                IloscText = Produkt.Iloœæ.ToString();
                WybranaJednostka = Produkt.Jednostka;
                Lokalizacja = Produkt.Lokalizacja;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object? obj)
        {
            if (!Validate()) return;

            Produkt.Nazwa = Nazwa.Trim();
            Produkt.Kategoria = WybranaKategoria;
            Produkt.Iloœæ = int.Parse(IloscText);
            Produkt.Jednostka = WybranaJednostka;
            Produkt.Lokalizacja = Lokalizacja.Trim();

            Saved = true;
            CloseAction?.Invoke();
        }

        private void Cancel(object? obj)
        {
            Saved = false;
            CloseAction?.Invoke();
        }

        private bool Validate()
        {
            bool isValid = true;

            NazwaError = string.Empty;
            KategoriaError = string.Empty;
            IloscError = string.Empty;
            JednostkaError = string.Empty;
            LokalizacjaError = string.Empty;

            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                NazwaError = "Nazwa produktu jest wymagana.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(WybranaKategoria))
            {
                KategoriaError = "Wybierz kategoriê.";
                isValid = false;
            }

            if (!int.TryParse(IloscText, out int ilosc) || ilosc < 0)
            {
                IloscError = "Iloœæ musi byæ liczb¹ ca³kowit¹ wiêksz¹ lub równ¹ 0.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(WybranaJednostka))
            {
                JednostkaError = "Wybierz jednostkê.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Lokalizacja))
            {
                LokalizacjaError = "Lokalizacja jest wymagana.";
                isValid = false;
            }

            return isValid;
        }
    }
}