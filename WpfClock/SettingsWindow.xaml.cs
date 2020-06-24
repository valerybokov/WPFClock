using ClockApplication.ViewModel;
using System;
using System.Windows;

namespace ClockApplication
{
    partial class SettingsWindow : Window
    {
        readonly Settings settings;
        readonly SettingsViewModel settingsVm;
        readonly MainWindowViewModel mainVm;

        internal SettingsWindow(MainWindowViewModel mainVm, Settings settings)
        {
            InitializeComponent();

            this.mainVm = mainVm;
            this.settings = settings;
            settingsVm = new SettingsViewModel();

            DataContext = settingsVm;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            settingsVm.Initialize(settings, mainVm, result => DialogResult = result);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            settingsVm.Deinitialize();

            base.OnClosing(e);
        }
    }
}
