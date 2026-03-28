using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Magazyn_WPF.Models;
using Magazyn_WPF.ViewModels.Base;
using Magazyn_WPF.Views;
using System.ComponentModel;
using System.Windows.Data;

namespace Magazyn_WPF.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
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

	
		//  WYSZUKIWARKA
		private string _wyszukiwanaFraza = string.Empty;
		public string WyszukiwanaFraza
		{
			get => _wyszukiwanaFraza;
			set
			{
				
				if (SetProperty(ref _wyszukiwanaFraza, value))
				{
					
					CollectionViewSource.GetDefaultView(Produkty).Refresh();
				}
			}
		}
		
		// Komendy CRUD
		public ICommand DeleteCommand { get; }
		public ICommand AddCommand { get; }
		public ICommand EditCommand { get; }


		public ICommand ClearSelectionCommand { get; }


		public MainViewModel()
		{
			LoadTestData();

			// Inicjalizacja komend
			DeleteCommand = new RelayCommand(DeleteProduct, CanModifyProduct);
			AddCommand = new RelayCommand(AddProduct);
			EditCommand = new RelayCommand(EditProduct, CanModifyProduct);
			ClearSelectionCommand = new RelayCommand(ClearSelection);

			
			CollectionViewSource.GetDefaultView(Produkty).Filter = FiltrujProdukty;
		}


		// LOGIKA DODAWANIA 
		private void AddProduct(object? obj)
		{
			var formWindow = new ProductFormWindow();
			if (formWindow.ShowDialog() == true)
			{
				var produkt = formWindow.Produkt;
				
				produkt.Id = Produkty.Any() ? Produkty.Max(p => p.Id) + 1 : 1;
				produkt.DataDodania = DateTime.Now;

				
				Produkty.Add(produkt);
				PrzeliczStatystyki();
			}
		}

		// LOGIKA EDYCJI 
		private void EditProduct(object? obj)
		{
			if (WybranyProdukt == null) return;

			
			var formWindow = new ProductFormWindow(WybranyProdukt);
			if (formWindow.ShowDialog() == true)
			{
				PrzeliczStatystyki();
			}
		}

		// metoda sprawdzaj¹ca czy coœ jest zaznaczone 

		private bool CanModifyProduct(object? obj)
		{
			return WybranyProdukt != null;
		}

		private void DeleteProduct(object? obj)
		{
			if (WybranyProdukt != null)
			{
				Produkty.Remove(WybranyProdukt);
				PrzeliczStatystyki();
			}
		}

		//  Metoda licz¹ca statystyki

		private void PrzeliczStatystyki()
		{
			if (Produkty == null) return;

			
			LiczbaProduktow = Produkty.Count;

			
			CalkowitaIloscWmagazynie = Produkty.Sum(p => p.Iloœæ);
		}

		// Logika filtrowania (Zwraca TRUE jeœli pokazaæ produkt, FALSE jeœli ukryæ)

		private bool FiltrujProdukty(object obj)
		{
			if (obj is Produkt produkt)
			{
				
				if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
					return true;

				// Szukamy po nazwie lub kategorii (ignorujemy wielkoœæ liter)
				return produkt.Nazwa.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase) ||
					   produkt.Kategoria.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
		private void LoadTestData()
		{
			Produkty = new ObservableCollection<Produkt>
			{
                //  NARZÊDZIA
				new Produkt { Id = 1, Nazwa = "Œruba M8", Kategoria = "Narzêdzia", Iloœæ = 500, Jednostka = "szt.", Lokalizacja = "Pó³ka A1", DataDodania = DateTime.Now.AddDays(-30) },
				new Produkt { Id = 2, Nazwa = "M³otek ciesielski", Kategoria = "Narzêdzia", Iloœæ = 15, Jednostka = "szt.", Lokalizacja = "Pó³ka A2", DataDodania = DateTime.Now.AddDays(-45) },
				new Produkt { Id = 3, Nazwa = "Wiertarka udarowa", Kategoria = "Narzêdzia", Iloœæ = 8, Jednostka = "szt.", Lokalizacja = "Rega³ B1", DataDodania = DateTime.Now.AddDays(-10) },
				new Produkt { Id = 4, Nazwa = "Zestaw kluczy p³askich", Kategoria = "Narzêdzia", Iloœæ = 20, Jednostka = "kpl.", Lokalizacja = "Pó³ka A3", DataDodania = DateTime.Now.AddDays(-100) },
				new Produkt { Id = 5, Nazwa = "Pi³a tarczowa", Kategoria = "Narzêdzia", Iloœæ = 5, Jednostka = "szt.", Lokalizacja = "Rega³ B2", DataDodania = DateTime.Now.AddDays(-5) },
				new Produkt { Id = 6, Nazwa = "Miarka zwijana 5m", Kategoria = "Narzêdzia", Iloœæ = 45, Jednostka = "szt.", Lokalizacja = "Pó³ka A4", DataDodania = DateTime.Now.AddDays(-60) },
				new Produkt { Id = 7, Nazwa = "Poziomica aluminiowa", Kategoria = "Narzêdzia", Iloœæ = 12, Jednostka = "szt.", Lokalizacja = "Rega³ B3", DataDodania = DateTime.Now.AddDays(-25) },
				new Produkt { Id = 21, Nazwa = "Œrubokrêt krzy¿akowy", Kategoria = "Narzêdzia", Iloœæ = 35, Jednostka = "szt.", Lokalizacja = "Szuflada A1", DataDodania = DateTime.Now.AddDays(-12) },
				new Produkt { Id = 22, Nazwa = "Klucz francuski", Kategoria = "Narzêdzia", Iloœæ = 18, Jednostka = "szt.", Lokalizacja = "Pó³ka A5", DataDodania = DateTime.Now.AddDays(-22) },
				new Produkt { Id = 23, Nazwa = "Szczypce uniwersalne", Kategoria = "Narzêdzia", Iloœæ = 25, Jednostka = "szt.", Lokalizacja = "Szuflada A2", DataDodania = DateTime.Now.AddDays(-5) },
				new Produkt { Id = 24, Nazwa = "Wyrzynarka", Kategoria = "Narzêdzia", Iloœæ = 6, Jednostka = "szt.", Lokalizacja = "Rega³ B4", DataDodania = DateTime.Now.AddDays(-40) },
				new Produkt { Id = 25, Nazwa = "Szlifierka k¹towa", Kategoria = "Narzêdzia", Iloœæ = 10, Jednostka = "szt.", Lokalizacja = "Rega³ B5", DataDodania = DateTime.Now.AddDays(-18) },
				new Produkt { Id = 26, Nazwa = "D³uto do drewna 12mm", Kategoria = "Narzêdzia", Iloœæ = 30, Jednostka = "szt.", Lokalizacja = "Pó³ka A6", DataDodania = DateTime.Now.AddDays(-60) },
				new Produkt { Id = 27, Nazwa = "Suwmiarka elektroniczna", Kategoria = "Narzêdzia", Iloœæ = 15, Jednostka = "szt.", Lokalizacja = "Szuflada A3", DataDodania = DateTime.Now.AddDays(-110) },
				new Produkt { Id = 28, Nazwa = "Klucze imbusowe", Kategoria = "Narzêdzia", Iloœæ = 40, Jednostka = "kpl.", Lokalizacja = "Pó³ka A7", DataDodania = DateTime.Now.AddDays(-33) },
				new Produkt { Id = 29, Nazwa = "Imad³o œlusarskie", Kategoria = "Narzêdzia", Iloœæ = 4, Jednostka = "szt.", Lokalizacja = "Stó³ 1", DataDodania = DateTime.Now.AddDays(-200) },
				new Produkt { Id = 30, Nazwa = "Nitownica rêczna", Kategoria = "Narzêdzia", Iloœæ = 14, Jednostka = "szt.", Lokalizacja = "Pó³ka A8", DataDodania = DateTime.Now.AddDays(-80) },

                //MATERIA£Y BUDOWLANE 
				new Produkt { Id = 8, Nazwa = "Klej monta¿owy", Kategoria = "Materia³y budowlane", Iloœæ = 25, Jednostka = "l", Lokalizacja = "Pó³ka C1", DataDodania = DateTime.Now.AddDays(-15) },
				new Produkt { Id = 9, Nazwa = "Pianka poliuretanowa", Kategoria = "Materia³y budowlane", Iloœæ = 40, Jednostka = "szt.", Lokalizacja = "Pó³ka C2", DataDodania = DateTime.Now.AddDays(-8) },
				new Produkt { Id = 10, Nazwa = "Cement 25kg", Kategoria = "Materia³y budowlane", Iloœæ = 100, Jednostka = "worek", Lokalizacja = "Paleta 1", DataDodania = DateTime.Now.AddDays(-2) },
				new Produkt { Id = 11, Nazwa = "Farba bia³a akrylowa", Kategoria = "Materia³y budowlane", Iloœæ = 60, Jednostka = "l", Lokalizacja = "Pó³ka C3", DataDodania = DateTime.Now.AddDays(-20) },
				new Produkt { Id = 12, Nazwa = "Gips szpachlowy", Kategoria = "Materia³y budowlane", Iloœæ = 30, Jednostka = "worek", Lokalizacja = "Paleta 2", DataDodania = DateTime.Now.AddDays(-12) },
				new Produkt { Id = 13, Nazwa = "P³yta OSB 18mm", Kategoria = "Materia³y budowlane", Iloœæ = 150, Jednostka = "szt.", Lokalizacja = "Hala B", DataDodania = DateTime.Now.AddDays(-50) },
				new Produkt { Id = 31, Nazwa = "Ceg³a pe³na", Kategoria = "Materia³y budowlane", Iloœæ = 2000, Jednostka = "szt.", Lokalizacja = "Plac zewnêtrzny", DataDodania = DateTime.Now.AddDays(-15) },
				new Produkt { Id = 32, Nazwa = "Pustak ceramiczny", Kategoria = "Materia³y budowlane", Iloœæ = 800, Jednostka = "szt.", Lokalizacja = "Plac zewnêtrzny", DataDodania = DateTime.Now.AddDays(-10) },
				new Produkt { Id = 33, Nazwa = "We³na mineralna 10cm", Kategoria = "Materia³y budowlane", Iloœæ = 45, Jednostka = "rolka", Lokalizacja = "Hala C", DataDodania = DateTime.Now.AddDays(-6) },
				new Produkt { Id = 34, Nazwa = "Folia izolacyjna", Kategoria = "Materia³y budowlane", Iloœæ = 120, Jednostka = "m2", Lokalizacja = "Pó³ka C4", DataDodania = DateTime.Now.AddDays(-90) },
				new Produkt { Id = 35, Nazwa = "Taœma malarska", Kategoria = "Materia³y budowlane", Iloœæ = 85, Jednostka = "szt.", Lokalizacja = "Pó³ka C5", DataDodania = DateTime.Now.AddDays(-2) },
				new Produkt { Id = 36, Nazwa = "Grunt uniwersalny 5L", Kategoria = "Materia³y budowlane", Iloœæ = 35, Jednostka = "l", Lokalizacja = "Pó³ka C6", DataDodania = DateTime.Now.AddDays(-18) },
				new Produkt { Id = 37, Nazwa = "Zaprawa murarska", Kategoria = "Materia³y budowlane", Iloœæ = 60, Jednostka = "worek", Lokalizacja = "Paleta 3", DataDodania = DateTime.Now.AddDays(-25) },
				new Produkt { Id = 38, Nazwa = "Silikon sanitarny", Kategoria = "Materia³y budowlane", Iloœæ = 50, Jednostka = "szt.", Lokalizacja = "Pó³ka C7", DataDodania = DateTime.Now.AddDays(-30) },
				new Produkt { Id = 39, Nazwa = "Ko³ki rozporowe 8x40", Kategoria = "Materia³y budowlane", Iloœæ = 500, Jednostka = "op.", Lokalizacja = "Pó³ka C8", DataDodania = DateTime.Now.AddDays(-100) },
				new Produkt { Id = 40, Nazwa = "Papa dachowa termozgrzewalna", Kategoria = "Materia³y budowlane", Iloœæ = 25, Jednostka = "rolka", Lokalizacja = "Hala C", DataDodania = DateTime.Now.AddDays(-40) },

                //  ELEKTRONIKA 
				new Produkt { Id = 14, Nazwa = "Lampka LED", Kategoria = "Elektronika", Iloœæ = 120, Jednostka = "szt.", Lokalizacja = "Rega³ D1", DataDodania = DateTime.Now.AddDays(-7) },
				new Produkt { Id = 15, Nazwa = "Przewód YDYp 3x1.5", Kategoria = "Elektronika", Iloœæ = 500, Jednostka = "m", Lokalizacja = "Bêben 1", DataDodania = DateTime.Now.AddDays(-1) },
				new Produkt { Id = 16, Nazwa = "Gniazdko podwójne", Kategoria = "Elektronika", Iloœæ = 85, Jednostka = "szt.", Lokalizacja = "Pó³ka D2", DataDodania = DateTime.Now.AddDays(-14) },
				new Produkt { Id = 17, Nazwa = "W³¹cznik pojedynczy", Kategoria = "Elektronika", Iloœæ = 60, Jednostka = "szt.", Lokalizacja = "Pó³ka D3", DataDodania = DateTime.Now.AddDays(-14) },
				new Produkt { Id = 18, Nazwa = "Bezpiecznik B16", Kategoria = "Elektronika", Iloœæ = 200, Jednostka = "szt.", Lokalizacja = "Szuflada E1", DataDodania = DateTime.Now.AddDays(-30) },
				new Produkt { Id = 41, Nazwa = "Przed³u¿acz bêbnowy 50m", Kategoria = "Elektronika", Iloœæ = 8, Jednostka = "szt.", Lokalizacja = "Rega³ D4", DataDodania = DateTime.Now.AddDays(-55) },
				new Produkt { Id = 42, Nazwa = "¯arówka LED E27", Kategoria = "Elektronika", Iloœæ = 300, Jednostka = "szt.", Lokalizacja = "Pó³ka D5", DataDodania = DateTime.Now.AddDays(-12) },
				new Produkt { Id = 43, Nazwa = "Puszka instalacyjna", Kategoria = "Elektronika", Iloœæ = 450, Jednostka = "szt.", Lokalizacja = "Pó³ka D6", DataDodania = DateTime.Now.AddDays(-4) },
				new Produkt { Id = 44, Nazwa = "Korytko kablowe 2m", Kategoria = "Elektronika", Iloœæ = 120, Jednostka = "szt.", Lokalizacja = "Hala B", DataDodania = DateTime.Now.AddDays(-80) },
				new Produkt { Id = 45, Nazwa = "Z³¹czka WAGO 3-pin", Kategoria = "Elektronika", Iloœæ = 1000, Jednostka = "szt.", Lokalizacja = "Szuflada E2", DataDodania = DateTime.Now.AddDays(-3) },
				new Produkt { Id = 46, Nazwa = "Miernik uniwersalny", Kategoria = "Elektronika", Iloœæ = 12, Jednostka = "szt.", Lokalizacja = "Szafa 1", DataDodania = DateTime.Now.AddDays(-150) },
				new Produkt { Id = 47, Nazwa = "Lutownica transformatorowa", Kategoria = "Elektronika", Iloœæ = 5, Jednostka = "szt.", Lokalizacja = "Szafa 1", DataDodania = DateTime.Now.AddDays(-45) },
				new Produkt { Id = 48, Nazwa = "Kabel sieciowy UTP 305m", Kategoria = "Elektronika", Iloœæ = 4, Jednostka = "karton", Lokalizacja = "Rega³ D7", DataDodania = DateTime.Now.AddDays(-11) },
				new Produkt { Id = 49, Nazwa = "Prze³¹cznik schodowy", Kategoria = "Elektronika", Iloœæ = 45, Jednostka = "szt.", Lokalizacja = "Pó³ka D8", DataDodania = DateTime.Now.AddDays(-6) },
				new Produkt { Id = 50, Nazwa = "Taœma izolacyjna", Kategoria = "Elektronika", Iloœæ = 180, Jednostka = "szt.", Lokalizacja = "Szuflada E3", DataDodania = DateTime.Now.AddDays(-2) },

                //  INNE / MEBLE 
				new Produkt { Id = 19, Nazwa = "Rêkawice robocze", Kategoria = "Inne", Iloœæ = 150, Jednostka = "para", Lokalizacja = "Pó³ka F1", DataDodania = DateTime.Now.AddDays(-40) },
				new Produkt { Id = 20, Nazwa = "Stó³ warsztatowy", Kategoria = "Meble", Iloœæ = 3, Jednostka = "szt.", Lokalizacja = "Hala A", DataDodania = DateTime.Now.AddDays(-90) },
				new Produkt { Id = 51, Nazwa = "Krzes³o warsztatowe obrotowe", Kategoria = "Meble", Iloœæ = 5, Jednostka = "szt.", Lokalizacja = "Hala A", DataDodania = DateTime.Now.AddDays(-20) },
				new Produkt { Id = 52, Nazwa = "Rega³ magazynowy metalowy", Kategoria = "Meble", Iloœæ = 12, Jednostka = "szt.", Lokalizacja = "Hala A", DataDodania = DateTime.Now.AddDays(-120) },
				new Produkt { Id = 53, Nazwa = "Szafka narzêdziowa na kó³kach", Kategoria = "Meble", Iloœæ = 2, Jednostka = "szt.", Lokalizacja = "Hala A", DataDodania = DateTime.Now.AddDays(-8) },
				new Produkt { Id = 54, Nazwa = "Okulary ochronne", Kategoria = "Inne", Iloœæ = 80, Jednostka = "szt.", Lokalizacja = "Pó³ka F2", DataDodania = DateTime.Now.AddDays(-15) },
				new Produkt { Id = 55, Nazwa = "Kask budowlany bia³y", Kategoria = "Inne", Iloœæ = 35, Jednostka = "szt.", Lokalizacja = "Pó³ka F3", DataDodania = DateTime.Now.AddDays(-55) },
				new Produkt { Id = 56, Nazwa = "Miot³a przemys³owa", Kategoria = "Inne", Iloœæ = 10, Jednostka = "szt.", Lokalizacja = "K¹t gospodarczy", DataDodania = DateTime.Now.AddDays(-200) },
				new Produkt { Id = 57, Nazwa = "Wiadro budowlane 20L", Kategoria = "Inne", Iloœæ = 60, Jednostka = "szt.", Lokalizacja = "Pó³ka F4", DataDodania = DateTime.Now.AddDays(-25) },
				new Produkt { Id = 58, Nazwa = "Tablica narzêdziowa œcienna", Kategoria = "Meble", Iloœæ = 15, Jednostka = "szt.", Lokalizacja = "Rega³ G1", DataDodania = DateTime.Now.AddDays(-40) },
				new Produkt { Id = 59, Nazwa = "Wózek magazynowy paletowy", Kategoria = "Inne", Iloœæ = 4, Jednostka = "szt.", Lokalizacja = "Hala B", DataDodania = DateTime.Now.AddDays(-1) },
				new Produkt { Id = 60, Nazwa = "Nauszniki przeciwha³asowe", Kategoria = "Inne", Iloœæ = 22, Jednostka = "szt.", Lokalizacja = "Pó³ka F5", DataDodania = DateTime.Now.AddDays(-18) }
			};

			
			PrzeliczStatystyki();
		}

		private void ClearSelection(object? obj)
		{
			WybranyProdukt = null; 
		}

	}
}