using System.Windows;
using Magazyn_WPF.ViewModels;

namespace Magazyn_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}