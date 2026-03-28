using System;
using System.Windows.Input;

namespace Magazyn_WPF.ViewModels.Base
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
//------ Odpowiada za powiazanie logiki biznesowej z interfejsem uzytkownika, pozwala na wykonywanie akcji w odpowiedzi
//------ na zdarzenia UI, takie jak klikniêcia przycisków. RelayCommand implementuje interfejs ICommand,
// ------ co umo¿liwia jego u¿ycie w XAML do powi¹zania z elementami interfejsu u¿ytkownika. 