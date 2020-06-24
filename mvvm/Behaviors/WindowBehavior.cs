using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM
{
    public class WindowBehavior
    {
        #region window activated
        public static readonly DependencyProperty ActivatedCommandProperty =
                    DependencyProperty.RegisterAttached(
                    "ActivatedCommand", typeof(ICommand), typeof(WindowBehavior),
                    new UIPropertyMetadata(new PropertyChangedCallback(ActivatedCommandChanged)));

        private static void ActivatedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (Window)d;
            element.Activated += Window_Activated;
        }

        private static void Window_Activated(object sender, EventArgs e)
        {
            ICommand command = GetActivatedCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetActivatedCommand(UIElement element, ICommand value)
        {
            element.SetValue(ActivatedCommandProperty, value);
        }

        public static ICommand GetActivatedCommand(UIElement element)
        {
            return (ICommand)element.GetValue(ActivatedCommandProperty);
        }
        #endregion

        #region window deactivated

        public static readonly DependencyProperty DeactivatedCommandProperty =
            DependencyProperty.RegisterAttached(
            "DeactivatedCommand", typeof(ICommand), typeof(WindowBehavior),
            new UIPropertyMetadata(new PropertyChangedCallback(DectivatedCommandChanged)));

        private static void DectivatedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (Window)d;
            element.Deactivated += Window_Deactivated;
        }

        private static void Window_Deactivated(object sender, EventArgs e)
        {
            ICommand command = GetDeactivatedCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetDeactivatedCommand(UIElement element, ICommand value)
        {
            element.SetValue(DeactivatedCommandProperty, value);
        }

        public static ICommand GetDeactivatedCommand(UIElement element)
        {
            return (ICommand)element.GetValue(DeactivatedCommandProperty);
        }
        #endregion
    }
}
