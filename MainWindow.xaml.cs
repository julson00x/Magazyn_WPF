using System.Windows;
using Magazyn_WPF.ViewModels;

namespace Magazyn_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Punkt 4: Ustawienie DataContext. 
            // Od teraz to MainViewModel dostarcza dane i logikę dla całego okna.
            DataContext = new MainViewModel();
        }
    }
}