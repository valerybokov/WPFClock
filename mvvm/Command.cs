using System;
using System.Windows.Input;

namespace MVVM
{
    /// <summary>
    /// Класс самой простой комманды
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Делегат на функцию, выполняемую командой
        /// </summary>
        private Action action;

        public Command(Action action)
        {
            this.action = action;
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
            return true;
        }

        /// <summary>
        /// Метод выполнения команды
        /// </summary>
        public void Execute(object parameter)
        {
            action();
        }
    }
}
