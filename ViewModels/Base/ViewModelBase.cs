using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Magazyn_WPF.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
// to klasa bazowa dla wszystkich ViewModeli w aplikacji, implementuj¹ca interfejs INotifyPropertyChanged.
// Umo¿liwia powiadamianie interfejsu u¿ytkownika o zmianach w³aœciwoœci, co jest kluczowe dla aktualizacji danych wyœwietlanych na ekranie.
