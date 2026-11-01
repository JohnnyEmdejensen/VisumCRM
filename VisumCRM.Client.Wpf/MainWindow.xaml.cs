using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisumCRM.Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Internals.FormSettingsClass _settings;
        private System.Timers.Timer _clock;
        public MainWindow()
        {
            InitializeComponent();
            _clock = new System.Timers.Timer();
            _clock.Interval = 5000;
            _clock.Elapsed += _clock_Elapsed;
            SetClock();
            _clock.Enabled = true;
            _settings = new Internals.FormSettingsClass(this);
            ContentRendered += MainWindow_ContentRendered;
            KeyDown += MainWindow_KeyDown;
            Closing += MainWindow_Closing;
            _settings.Load();
        }

        private void SetClock()
        {
            Dispatcher.Invoke(new Action(() => {
                lblClock.Content = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            }));
        }
        private void _clock_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _clock.Enabled = false;
            SetClock();
            _clock.Enabled = true;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _clock.Enabled = false;            
            _settings.Save();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {

        }

        private void txtTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void imgMin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void imgMax_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Maximized;
            imgRes.Visibility = Visibility.Visible;
            imgMax.Visibility = Visibility.Collapsed;
        }

        private void imgRes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Normal;
            imgRes.Visibility = Visibility.Collapsed;
            imgMax.Visibility = Visibility.Visible;
        }

        private void imgCls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void mnuCreateUser_Click(object sender, RoutedEventArgs e)
        {
            Entity.User usr = new();
            App.DialogHelper.ShowUser(usr);
        }
    }
}