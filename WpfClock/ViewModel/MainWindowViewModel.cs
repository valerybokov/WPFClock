using ClockApplication.Utils;
using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ClockApplication.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        #region fields
        readonly string zero = "0";
        readonly Settings settings;
        string hours, minutes, seconds;
        readonly PositionHandler positionHandler;
        Window window;
        Brush clockBackground = Brushes.White, foreground = Brushes.Blue;
        bool isLoading = true, settingsLoaded;
        Visibility bSettingsVisibility = Visibility.Hidden;
        double top, left;
        readonly DispatcherTimer timerHours, timerWindowState;
        GridLength secondsWidth = new GridLength(1, GridUnitType.Star), marginWidth = new GridLength(3);
        ICommand mouseMoveCommand, mouseLeaveCommand, mouseUpCommand, mouseDownCommand;
        ICommand windowActivatedCommand, windowDeactivatedCommand, windowMouseLeaveCommand;
        ICommand settingsCommand, buttonMouseEnterCommand, buttonMouseLeaveCommand;
        #endregion

        #region methods
        public MainWindowViewModel(Window window, PositionHandler positionHandler)
        {
            this.positionHandler = positionHandler;
            this.window = window;

            settings = new Settings();
            settings.Load(DataLoaded);

            timerHours = new DispatcherTimer();
            timerHours.Tick += new EventHandler(HoursTimer_Tick);
            timerHours.Interval = TimeSpan.FromSeconds(1);

            timerWindowState = new DispatcherTimer();
            timerWindowState.Tick += new EventHandler(WindowStateTimer_Tick);
            timerWindowState.Interval = TimeSpan.FromMilliseconds(1500f);
        }

        public void Initialize()
        {
            int i = 5 * 3;//3 минуты
            //если настройки не загрузились, ждать их загрузки, но не более 3х минут
            while (!settingsLoaded && i > 0)
            {
                System.Threading.Thread.Sleep(200);
                --i;
            }

            ClockBackground = settings.ClockBackground;
            Foreground = settings.Foreground;

            if (settings.IsDefault)
            {
                SetTopAndLeft();
            }
            else
            {
                Top = settings.Y;
                Left = settings.X;
            }

            if (settings.ShowSeconds)
                MarginWidth = new GridLength(3);
            else
                ShowSeconds = false;

            HoursTimer_Tick(null, null);

            timerHours.Start();
            isLoading = false;
        }

        public void Deinitialize()
        {
            timerHours.Stop();
            timerWindowState.Stop();

            timerHours.Tick -= HoursTimer_Tick;
            timerWindowState.Tick -= WindowStateTimer_Tick;

            if (positionHandler.PositionChanged)
            {
                settings.X = Left;
                settings.Y = Top;
                settings.Save();
            }
        }

        private void SetTopAndLeft()
        {
            Top = SystemParameters.PrimaryScreenHeight - (window.Height + 100);//100 условно высота панели задач
            Left = SystemParameters.PrimaryScreenWidth - (window.Width + 30);// 30 - отступ от края
        }

        private void DataLoaded(IAsyncResult ar)
        {
            var asRez = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
            var del = (Func<bool>)asRez.AsyncDelegate;

            settingsLoaded = true;

            if (!del.EndInvoke(ar))
                MessageBox.Show(
                        "Не удалось прочитать настройки",
                        "Clock",
                        MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void HoursTimer_Tick(object sender, EventArgs e)
        {
            var t = DateTime.Now;
            var h = t.Hour.ToString();
            var m = t.Minute.ToString();

            if (h.Length == 1)
                h = zero + h;
            if (m.Length == 1)
                m = zero + m;

            hours = h;
            minutes = m;
            OnPropertyChanged(nameof(Hours));
            OnPropertyChanged(nameof(Minutes));

            if (settings.ShowSeconds)
            {
                var s = t.Second.ToString();

                if (s.Length == 1)
                    s = zero + s;

                seconds = s;

                OnPropertyChanged(nameof(Seconds));
            }
        }

        private void WindowStateTimer_Tick(object sender, EventArgs e)
        {
            timerWindowState.Stop();
            OnWindowDeactivated();
        }

        private void OnWindowActivated()
        {
            if (!isLoading)
            {
                /*Курсор вошел на кнопку. Остановить таймер - кнопку не скрывать*/
                timerWindowState.Stop();
                SettingsVisibility = Visibility.Visible;
            }
        }

        private void OnWindowDeactivated()
        {
            SettingsVisibility = Visibility.Hidden;
        }

        private void OnOpenSettings()
        {
            var s = new SettingsWindow(this, settings);
            s.Owner = window;

            if (s.ShowDialog() == true)
            {
                settings.X = window.Left;
                settings.Y = window.Top;

                if (!settings.Save())
                {
                    MessageBox.Show(
                                "Не удалось сохранить настройки",
                                "Clock",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region properties
        internal bool ShowSeconds
        {
            set
            {
                if (value)//добавить секунды
                {
                    window.Width += ((window.Width - 3) / 2) + 3;//3 - расстояния между ячейками

                    MarginWidth = new GridLength(3);
                    SecondsWidth = new GridLength(1, GridUnitType.Star);//NOTE set Width in the Stars
                }
                else//убрать секунды
                {
                    window.Width -= ((window.Width - 6) / 3) + 3;//6 - два расстояния между ячейками

                    SecondsWidth = new GridLength(0);
                    MarginWidth = new GridLength(0);
                }
                //NOTE это задается позиция окна
                SetTopAndLeft();
            }
        }

        public string Title => "Clock";

        public bool TopMost
        {
            get => settings.TopMost;
            internal set
            {
                settings.TopMost = value;
                OnPropertyChanged();
            }
        }
        public Visibility SettingsVisibility
        {
            get => bSettingsVisibility;
            private set
            {
                bSettingsVisibility = value;
                OnPropertyChanged();
            }
        }

        public Brush Foreground
        {
            get => foreground;
            internal set
            {
                foreground = value;
                OnPropertyChanged();
            }
        }

        public Brush ClockBackground
        {
            get => clockBackground;
            internal set
            {
                clockBackground = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Margin between minutes and seconds
        /// </summary>
        public GridLength MarginWidth
        {
            get => marginWidth;
            private set
            {
                marginWidth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property to show/hide seconds
        /// </summary>
        public GridLength SecondsWidth
        {
            get => secondsWidth;
            private set
            {
                secondsWidth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Верхняя координата окна.
        /// </summary>
        /// <remarks>однонаправленная привязка не работает</remarks>
        public double Top {
            get => top;
            set {
                top = value;
                OnPropertyChanged();
            }
        }
        public double Left {
            get => left;
            set {
                left = value;
                OnPropertyChanged();
            }
        }
        public string Hours => hours;
        public string Minutes => minutes;
        public string Seconds => seconds;
        #endregion

        #region commands
        #region mouse commands for digits (textblocks)
        public ICommand MouseUpCommand
        {
            get
            {
                if (mouseUpCommand == null)
                    mouseUpCommand = new RelayCommand<MouseButtonEventArgs>(positionHandler.MouseUp);

                return mouseUpCommand;
            }
        }

        public ICommand MouseDownCommand
        {
            get
            {
                if (mouseDownCommand == null)
                    mouseDownCommand = new RelayCommand<MouseButtonEventArgs>(positionHandler.MouseDown);

                return mouseDownCommand;
            }
        }

        public ICommand MouseMoveCommand  {
            get {
                if (mouseMoveCommand == null)
                    mouseMoveCommand = new RelayCommand<MouseEventArgs>(positionHandler.MouseMove);

                return mouseMoveCommand;
            }
        }

        public ICommand MouseLeaveCommand {
            get {
                if (mouseLeaveCommand == null)
                    mouseLeaveCommand = new RelayCommand<MouseEventArgs>(positionHandler.MouseLeave);

                return mouseLeaveCommand;
            }
        }
        #endregion

        #region window commands      
        public ICommand WindowMouseLeaveCommand
        {
            get
            {
                if (windowMouseLeaveCommand == null)
                    windowMouseLeaveCommand = new Command(
                        /*Курсор вышел за пределы кнопки. Запустить таймер на скрытие кнопки.*/
                        timerWindowState.Start
                    );

                return windowMouseLeaveCommand;
            }
        }

        /// <summary>
        /// Uses like handler of window-mouse activated and window-mouse enter
        /// </summary>
        public ICommand WindowActivatedCommand
        {
            get
            {
                if (windowActivatedCommand == null)
                    windowActivatedCommand = new Command(OnWindowActivated);

                return windowActivatedCommand;
            }
        }

        public ICommand WindowDeactivatedCommand
        {
            get
            {
                if (windowDeactivatedCommand == null)
                    windowDeactivatedCommand = new Command(OnWindowDeactivated);

                return windowDeactivatedCommand;
            }
        }
        #endregion

        #region button Settings commands
        public ICommand ButtonMouseEnterCommand
        {
            get
            {
                if (buttonMouseEnterCommand == null)
                    buttonMouseEnterCommand = new Command(timerWindowState.Stop);

                return buttonMouseEnterCommand;
            }
        }
        
        public ICommand ButtonMouseLeaveCommand
        {
            get
            {
                if (buttonMouseLeaveCommand == null)
                    buttonMouseLeaveCommand = new Command(
                        /*Курсор вышел за пределы кнопки. Запустить таймер на скрытие кнопки.*/
                        timerWindowState.Start
                    );

                return buttonMouseLeaveCommand;
            }
        }

        /// <summary>
        /// Click button to open settings
        /// </summary>
        public ICommand OpenSettingsCommand
        {
            get
            {
                if (settingsCommand == null)
                    settingsCommand = new Command(OnOpenSettings);

                return settingsCommand;
            }
        }
        #endregion
        #endregion
    }
}
