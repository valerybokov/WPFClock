using System;
using System.Windows.Input;

namespace MVVM
{
    /// <summary>
    /// Команда от представления
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// Функция, выполняемая командой
        /// </summary>
        private readonly Action<T> action;

        /// <summary>
        /// Функция проверки разрешения на выполнение команды
        /// </summary>
        private readonly Predicate<T> canExecute;

        public RelayCommand(Action<T> executeFunc, Predicate<T> canExecuteFunc = null)
        {
            action = executeFunc;
            canExecute = canExecuteFunc;
        }
        
        /// <summary>
        /// Событие изменения разрешения на выполнение команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Метод проверки разрешения выполнения команды
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        /// <summary>
        /// Метод выполнения команды
        /// </summary>
        public void Execute(object parameter)
        {
            action((T)parameter);
        }
    }
}
