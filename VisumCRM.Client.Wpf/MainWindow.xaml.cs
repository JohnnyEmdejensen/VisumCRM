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
using VisumCRM.Client.Wpf.Internals;

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
            App.EventHelper.AuthenticationCancled += EventHelper_AuthenticationCancled;
            App.EventHelper.AuthenticationSuccess += EventHelper_AuthenticationSuccess;
            App.EventHelper.CompanyModelCollectionChanged += EventHelper_CompanyModelCollectionChanged;
            dgCompanies.SizeChanged += DgCompanies_SizeChanged;
            dgCompanies.SelectionChanged += DgCompanies_SelectionChanged;
            dgCompanies.MouseDoubleClick += DgCompanies_MouseDoubleClick;
            _settings.Load();
        }

        private void FetchAllData() 
        {
            using (new Internals.WorkhandlerContainer("FetchAllData"))
            {
                App.DataShare.GetAllData();
            }
        }

        private void SetClock()
        {
            Dispatcher.Invoke(new Action(() => {
                lblClock.Content = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            }));
        }

        private void StartFirstTimeGuide()
        {
            if (App.LicenseInMemory != null)
            {                
                Entity.License? lic = new Entity.License()
                {
                    Email = App.LicenseInMemory.Email,
                    LicenseKey = App.LicenseInMemory.Key,
                    Phone = App.LicenseInMemory.Phone,
                    Name = App.LicenseInMemory.Company,
                    Licensename = App.LicenseInMemory.Name                  
                };
                var locationid = App.ToolsHelper.ParseLocation(App.LicenseInMemory.Country, App.LicenseInMemory.Zipcode, App.LicenseInMemory.City, App.LicenseInMemory.Street, App.LicenseInMemory.House, App.LicenseInMemory.Floor, App.LicenseInMemory.Door, null);
                if (locationid.HasValue)
                    lic.LocationId = locationid.Value;
                lic = App.Repositories.LicenseRepository.Create(lic);
                if (lic != null)
                {
                    App.Repositories = new Repository.Repositories(lic.LicenseKey);
                }
                Entity.User? usr = new Entity.User()
                {
                    Firstname = "Master",
                    Login = "MA",
                    Password = "master",
                    SecurityLevel = 999,
                    Surname = "Login",
                };
                usr = App.Repositories.UserRepository.Create(usr);
               
            }
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
            StartFirstTimeGuide();
            if (App.AppShare.CurrentUser == null)
            {
                App.DialogHelper.ShowAuthenticate();
            }
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
        private void EventHelper_AuthenticationCancled(object? sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void EventHelper_AuthenticationSuccess(object? sender, EventArgs e)
        {
            if (App.AppShare.CurrentUser != null)
            {
                App.DataShare.ResumeEvents();

                lblUser.Content = App.AppShare.CurrentUser.Fullname;
                App.AppShare.CurrentDatabase = App.Repositories.Databasename;
                lblDatabase.Content = App.AppShare.CurrentDatabase;
                if (App.AppShare.CurrentUser.SecurityLevel == 999)
                {
                    var allUsers = App.Repositories.UserRepository.GetAll();
                    if (allUsers == null || allUsers.Count == 0 )
                    {
                        string? message = App.Current.Resources["messagebox.message.nousersexists"].ToString();
                        if (message != null)
                            message = message.Replace("|", "\n");
                        MessageBox.Show(message, App.Current.Resources["messagebox.title.nousersexists"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                        Entity.User usr = new()
                        {
                            SecurityLevel = 9
                        };
                        App.DialogHelper.ShowUser(usr);
                    }
                }
                else
                {
                    using (new Internals.WorkhandlerContainer("Authenticated"))
                    {
                        FetchAllData();
                    }
                }
            }
        }

        private void mnuCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            App.DialogHelper.ShowCompany(new Entity.Company());
        }
        private void EventHelper_CompanyModelCollectionChanged(object? sender, EventArgs e)
        {
            using (new Internals.WorkhandlerContainer("EventHelper_CompanyModelCollectionChanged"))
            {
                dgCompanies.BeginInit();
                dgCompanies.ItemsSource = App.DataShare.CompanyModelCollection;
                dgCompanies.EndInit();
            }
        }
        private void DgCompanies_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = dgCompanies.ActualWidth;
            dgCompanies.Columns[2].Width = width - 100 - 120 - 54 - 120 - 120 - 80 - 80 - 80 - 2;
        }
        private void DgCompanies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = dgCompanies.SelectedItem as Models.CompanyModel;
            if (item == null || App.DataShare.CompanyCollection == null) return;
            var company = App.DataShare.CompanyCollection.FirstOrDefault(x => x.Id == item.Id);
            App.DialogHelper.ShowCompany(company);
        }

        private void DgCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}