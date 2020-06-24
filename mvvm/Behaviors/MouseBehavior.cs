using System;
using System.Windows;
using System.Windows.Input;

namespace MVVM
{
    public class MouseBehavior
    {
        #region MouseUp

        public static readonly DependencyProperty MouseUpCommandProperty =
                            DependencyProperty.RegisterAttached(
                            "MouseUpCommand", typeof(ICommand), typeof(MouseBehavior),
                            new UIPropertyMetadata(new PropertyChangedCallback(MouseUpCommandChanged)));

        private static void MouseUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.MouseUp += new MouseButtonEventHandler(Mouse_Up);
        }

        public static void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            ICommand command = GetMouseUpCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetMouseUpCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseUpCommandProperty, value);
        }

        public static ICommand GetMouseUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseUpCommandProperty);
        }
        #endregion

        #region MouseMove

        public static readonly DependencyProperty MouseMoveCommandProperty =
                            DependencyProperty.RegisterAttached(
                            "MouseMoveCommand", typeof(ICommand), typeof(MouseBehavior),
                            new UIPropertyMetadata(new PropertyChangedCallback(MouseMoveCommandChanged)));

        private static void MouseMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.MouseMove += new MouseEventHandler(Mouse_Move);
        }

        public static void Mouse_Move(object sender, MouseEventArgs e)
        {
            ICommand command = GetMouseMoveCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetMouseMoveCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseMoveCommandProperty, value);
        }

        public static ICommand GetMouseMoveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseMoveCommandProperty);
        }
        #endregion

        #region MouseLeave

        public static readonly DependencyProperty MouseLeaveCommandProperty =
                            DependencyProperty.RegisterAttached(
                            "MouseLeaveCommand", typeof(ICommand), typeof(MouseBehavior),
                            new UIPropertyMetadata(new PropertyChangedCallback(MouseLeaveCommandChanged)));

        private static void MouseLeaveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.MouseLeave += new MouseEventHandler(Mouse_Leave);
        }

        public static void Mouse_Leave(object sender, MouseEventArgs e)
        {
            ICommand command = GetMouseLeaveCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetMouseLeaveCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseLeaveCommandProperty, value);
        }

        public static ICommand GetMouseLeaveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeaveCommandProperty);
        }
        #endregion

        #region MouseEnter

        public static readonly DependencyProperty MouseEnterCommandProperty =
                            DependencyProperty.RegisterAttached(
                            "MouseEnterCommand", typeof(ICommand), typeof(MouseBehavior),
                            new UIPropertyMetadata(new PropertyChangedCallback(MouseEnterCommandChanged)));

        private static void MouseEnterCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.MouseEnter += new MouseEventHandler(Mouse_Enter);
        }

        static void Mouse_Enter(object sender, MouseEventArgs e)
        {
            ICommand command = GetMouseEnterCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetMouseEnterCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseLeaveCommandProperty, value);
        }

        public static ICommand GetMouseEnterCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseEnterCommandProperty);
        }
        #endregion

        #region MouseDown

        public static readonly DependencyProperty MouseDownCommandProperty =
                            DependencyProperty.RegisterAttached(
                            "MouseDownCommand", typeof(ICommand), typeof(MouseBehavior),
                            new UIPropertyMetadata(new PropertyChangedCallback(MouseDownCommandChanged)));

        private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.MouseDown += new MouseButtonEventHandler(Mouse_Down);
        }

        public static void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            ICommand command = GetMouseDownCommand((UIElement)sender);
            command.Execute(e);
        }

        public static void SetMouseDownCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseDownCommandProperty, value);
        }

        public static ICommand GetMouseDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseDownCommandProperty);
        }
        #endregion
    }
}
