using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Magazyn_WPF.Models;
using Magazyn_WPF.ViewModels.Base;
using Magazyn_WPF.Views; // Dodane, aby ViewModel widzia³ ProductFormWindow

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
		}


		// --- LOGIKA DODAWANIA (Punkt 6) ---
		private void AddProduct(object? obj)
		{
			var formWindow = new ProductFormWindow();
			if (formWindow.ShowDialog() == true)
			{
				var produkt = formWindow.Produkt;
				// Generowanie nowego ID
				produkt.Id = Produkty.Any() ? Produkty.Max(p => p.Id) + 1 : 1;
				produkt.DataDodania = DateTime.Now;

				// Dodajemy do kolekcji - UI odœwie¿y siê SAMO!
				Produkty.Add(produkt);
			}
		}

		// --- LOGIKA EDYCJI (Punkt 7) ---
		private void EditProduct(object? obj)
		{
			if (WybranyProdukt == null) return;

			// Klonujemy obiekt, ¿eby nie zmieniaæ danych w tabeli zanim ktoœ kliknie "Zapisz"
			// Uwaga: w ProductFormWindow musisz mieæ konstruktor przyjmuj¹cy Produkt!
			var formWindow = new ProductFormWindow(WybranyProdukt);
			if (formWindow.ShowDialog() == true)
			{
				// W prawdziwym œrodowisku tutaj nast¹pi³by update w bazie danych.
				// Poniewa¿ pracujemy na ObservableCollection i zrobiliœmy binding, 
				// zmiana w³aœciwoœci w obiekcie mo¿e wymagaæ wymuszenia odœwie¿enia widoku
				// (w uproszczonym MVP na tym etapie wystarczy, ¿e dane w oknie siê zapisz¹)
			}
		}

		// Wspólna metoda sprawdzaj¹ca czy coœ jest zaznaczone (dla Usuñ i Edytuj)
		private bool CanModifyProduct(object? obj)
		{
			return WybranyProdukt != null;
		}

		private void DeleteProduct(object? obj)
		{
			if (WybranyProdukt != null)
			{
				Produkty.Remove(WybranyProdukt);
			}
		}

		private void LoadTestData()
		{
			Produkty = new ObservableCollection<Produkt>
			{
				new Produkt { Id = 1, Nazwa = "Œruba M8", Kategoria = "Narzêdzia", Iloœæ = 500, Jednostka = "szt.", Lokalizacja = "Pó³ka A1", DataDodania = DateTime.Now.AddDays(-30) },
				new Produkt { Id = 2, Nazwa = "Klej monta¿owy", Kategoria = "Materia³y budowlane", Iloœæ = 25, Jednostka = "l", Lokalizacja = "Pó³ka B3", DataDodania = DateTime.Now.AddDays(-15) },
				new Produkt { Id = 3, Nazwa = "Lampka LED", Kategoria = "Elektronika", Iloœæ = 120, Jednostka = "szt.", Lokalizacja = "Rega³ C2", DataDodania = DateTime.Now.AddDays(-7) }
			};
		}

		private void ClearSelection(object? obj)
		{
			WybranyProdukt = null; // Ustawienie na null automatycznie zablokuje przyciski Usuñ/Edytuj!
		}
	}
}