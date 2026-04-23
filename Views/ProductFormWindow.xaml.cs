using System.Windows;
using Magazyn_WPF.Models;
using Magazyn_WPF.ViewModels;

namespace Magazyn_WPF.Views
{
    public partial class ProductFormWindow : Window
    {
        private readonly ProductFormViewModel _viewModel;

        public Produkt Produkt => _viewModel.Produkt;

        public ProductFormWindow(Produkt? produkt = null)
        {
            InitializeComponent();
            _viewModel = new ProductFormViewModel(produkt);
            _viewModel.CloseAction = () => Close();
            DataContext = _viewModel;
        }

        // Czy użytkownik kliknął Zapisz
        public bool Saved => _viewModel.Saved;
    }
}