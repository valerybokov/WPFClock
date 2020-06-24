using ClockApplication.ViewModel;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ClockApplication
{
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel(this, new Utils.PositionHandler(this));

            DataContext = viewModel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.Initialize();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Deinitialize();
            
            base.OnClosing(e);
        }
    }
}