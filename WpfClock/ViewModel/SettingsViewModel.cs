using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ClockApplication.ViewModel
{
    class SettingsViewModel : BindableBase
    {
        #region fields
        Settings settings;
        bool buttonOKClicked;
        ICommand changeForegroundCommand, changeBackgroundCommand;
        MainWindowViewModel mainVM;
        Brush clockBackground, foreground;
        bool autoload, showSeconds;
        bool loading = true;
        bool topMost;
        Action<bool> closeWindow;
        #endregion

        #region methods
        internal void Initialize(Settings settings, MainWindowViewModel mainVM, Action<bool> closeWindow)
        {
            this.settings = settings;
            this.mainVM = mainVM;
            this.closeWindow = closeWindow;
            TopMost = settings.TopMost;
            Foreground = settings.Foreground;
            ClockBackground = settings.ClockBackground;

            Autoload = settings.Autoload;
            ShowSeconds = settings.ShowSeconds;
            loading = false;
        }

        /// <summary>
        /// Deinitialize the view model before window will be closed
        /// </summary>
        internal void Deinitialize()
        {
            if (!buttonOKClicked)
            {
                mainVM.Foreground = settings.Foreground;
                mainVM.ClockBackground = settings.ClockBackground;

                if (settings.ShowSeconds != ShowSeconds)
                    mainVM.ShowSeconds = settings.ShowSeconds;
            }

            loading = true;
        }

        private void OnButtonClick()
        {
            buttonOKClicked = true;

            settings.ShowSeconds = ShowSeconds;
            settings.Foreground = Foreground;
            settings.ClockBackground = ClockBackground;
            settings.TopMost = topMost;

            if (settings.Autoload != Autoload)
                ChangeAutoload();

            //set DialogResult value and close SettingsWindow
            closeWindow(true);
        }

        private Brush GetColor(Brush color)
        {
            using (var cd = new System.Windows.Forms.ColorDialog())
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));

            return color;
        }

        private void ChangeAutoload()
        {
            //настройки автозагрузки изменены
            string linkpath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                mainVM.Title + ".lnk");

            try//для создания и удаления файла
            {
                if (Autoload)//установить автозагрузку
                {
                    var app = System.Reflection.Assembly.GetExecutingAssembly().Location;

                    if (MyShortcut.Shortcut.Create(
                        app, linkpath,
                        "Программа " + mainVM.Title, app, 0))//0 - icon index
                        settings.Autoload = true;
                    else
                        MessageBox.Show("Не удалось выполнить операцию", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //убрать автозагрузку
                    if (System.IO.File.Exists(linkpath))
                        System.IO.File.Delete(linkpath);

                    settings.Autoload = false;
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Не удалось выполнить операцию", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Autoload = !Autoload;
            }
        }
        #endregion

        #region properties
        public Brush Foreground
        {
            get => foreground;
            private set
            {
                if (!loading)
                    mainVM.Foreground = value;

                foreground = value;
                OnPropertyChanged();
            }
        }

        public Brush ClockBackground
        {
            get => clockBackground;
            private set
            {
                if (!loading)
                    mainVM.ClockBackground = value;

                clockBackground = value;
                OnPropertyChanged();
            }
        }

        public bool Autoload
        {
            get => autoload;
            set {
                autoload = value;
                OnPropertyChanged();
            }
        }
        
        public bool ShowSeconds
        {
            get => showSeconds;
            set {
                if (!loading)
                    mainVM.ShowSeconds = value;

                showSeconds = value;
                OnPropertyChanged();
            }
        }

        public bool TopMost
        {
            get => topMost;
            set
            {
                if (!loading)
                    mainVM.TopMost = value;

                topMost = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand ChangeBackgroundCommand
        {
            get
            {
                if (changeBackgroundCommand == null)
                    changeBackgroundCommand = new Command(
                        () => ClockBackground = GetColor(ClockBackground));

                return changeBackgroundCommand;
            }
        }

        public ICommand ChangeForegroundCommand
        {
            get
            {
                if (changeForegroundCommand == null)
                    changeForegroundCommand = new Command(
                        () => Foreground = GetColor(Foreground));

                return changeForegroundCommand;
            }
        }

        public ICommand KeyCommand => new Command(() => closeWindow(false));

        public ICommand ButtonClickCommand => new Command(OnButtonClick);
        #endregion
    }
}
