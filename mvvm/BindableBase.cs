using System.ComponentModel;

namespace MVVM
{
    /// <summary>
    /// Базовый класс для уведомления представлений об изменении свойств их моделей
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Метод для уведомления об изменении свойства модели представления - наследника.
        /// Требуется вызывать при изменении свойства модели представления, к которому есть привязка в представлении!
        /// </summary>
        /// <param name="property"> Имя измененного свойства </param>
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Событие изменения свойства модели представления - наследника
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
