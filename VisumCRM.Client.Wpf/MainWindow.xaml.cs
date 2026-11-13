using Microsoft.EntityFrameworkCore;
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
using VisumCRM.Entity;

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
            App.EventHelper.ContactpersonModelCollectionChanged += EventHelper_ContactpersonModelCollectionChanged;
            App.EventHelper.CurrentCompanyChanged += EventHelper_CurrentCompanyChanged;
            App.EventHelper.CurrentContactpersonChanged += EventHelper_CurrentContactpersonChanged;
            dgCompanies.SizeChanged += DgCompanies_SizeChanged;
            dgContactpersons.SizeChanged += DgContactpersons_SizeChanged;
            dgCompanies.SelectionChanged += DgCompanies_SelectionChanged;
            dgContactpersons.SelectionChanged += DgContactpersons_SelectionChanged;
            dgCompanies.MouseDoubleClick += DgCompanies_MouseDoubleClick;
            dgContactpersons.MouseDoubleClick += DgContactpersons_MouseDoubleClick;
            _settings.Load();
            pageDashboard.IsSelected = true;
            if (_settings._values != null)
            {
                foreach (string item in _settings._values)
                {
                    if (item.StartsWith("ActiveTab:"))
                    {
                        var value = item.Replace("ActiveTab:", "");
                        if (value == "Companies")
                            pageCompanies.IsSelected = true;
                        if (value == "Activities")
                            pageCompanies.IsSelected = true;
                    }
                }
            }
        }

        private List<Models.ContactpersonModel>? ApplyContactpersonFilter(List<Models.ContactpersonModel>? Contactpersons)
        {            
            List<Models.ContactpersonModel>? result = App.DataShare.ContactpersonModelCollection;
            if (App.DataShare.ContactpersonModelCollection != null)
            {
                if (App.AppShare.CurrentCompany != null && result != null)
                    result = result.Where(x => x.EmployeerId == App.AppShare.CurrentCompany.Id).ToList();
            }
            return result;
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
                    App.Repositories = new Repository.Repositories(lic.LicenseKey, App.ConnectionString);
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
            if (App.AppShare.CurrentUser != null)
                App.ToolsHelper.LogWriter(0, "The user has left the syetm.", App.AppShare.CurrentUser.Id);
            List<string> values = new List<string>();
            string activeTab = "Dashboard";
            if (pageCompanies.IsSelected)
                activeTab = "Companies";
            if (pageactivities.IsSelected)
                activeTab = "Activities";
            values.Add("ActiveTab:" + activeTab);
            _settings = new FormSettingsClass(this, values);
            _settings.Save();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            //Data.ApiContextFactory factory = new Data.ApiContextFactory();
            //var dbcontext = factory.CreateDbContext(new string[] { "Server=localhost\\SQLEXPRESS;Database=VisumCRM_test;Trusted_Connection=True;TrustServerCertificate=True;" });
            //dbcontext.Database.Migrate();

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
        private void EventHelper_ContactpersonModelCollectionChanged(object? sender, EventArgs e)
        {
            using (new Internals.WorkhandlerContainer("EventHelper_ContactpersonModelCollectionChanged"))
            {
                dgContactpersons.BeginInit();
                dgContactpersons.ItemsSource = ApplyContactpersonFilter(App.DataShare.ContactpersonModelCollection);
                dgContactpersons.EndInit();
            }
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

        private void DgContactpersons_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = dgContactpersons.SelectedItem as Models.ContactpersonModel;
            if (item == null || App.DataShare.ContactpersonCollection == null) return;
            var contactperson = App.DataShare.ContactpersonCollection.FirstOrDefault(x => x.Id == item.Id);
            App.DialogHelper.ShowContactperson(contactperson);
        }
        private void DgCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = dgCompanies.SelectedItem as Models.CompanyModel;
            if (item != null && App.DataShare.CompanyCollection != null)
            {
                App.AppShare.CurrentCompany = App.DataShare.CompanyCollection.FirstOrDefault(x => x.Id == item.Id);
                if (App.AppShare.CurrentCompany != null)
                    App.EventHelper.RaiseCurrentCompanyChangedEvent();
            }
        }

        private void DgContactpersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = dgContactpersons.SelectedItem as Models.ContactpersonModel;
            if (item != null && App.DataShare.ContactpersonCollection != null)
            {
                App.AppShare.CurrentContactperson = App.DataShare.ContactpersonCollection.FirstOrDefault(x => x.Id == item.Id);
                if (App.AppShare.CurrentContactperson != null)
                    App.EventHelper.RaiseCurrentContactpersonChangedEvent();
            }
        }
        private void DgContactpersons_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = dgContactpersons.ActualWidth;
            dgContactpersons.Columns[1].Width = width - 80 - 80 - 180 - 180 - 140 - 2;
        }

        private void mnuContactperson_Click(object sender, RoutedEventArgs e)
        {
            Entity.Contactperson contactperson = new();
            if (App.AppShare.CurrentCompany != null) contactperson.EmployeerId = App.AppShare.CurrentCompany.Id;
            App.DialogHelper.ShowContactperson(contactperson);
        }

        private void btnBranches_Click(object sender, RoutedEventArgs e)
        {
            App.DialogHelper.ShowBranche(null);
        }
        private void btnJobtitles_Click(object sender, RoutedEventArgs e)
        {
            App.DialogHelper.ShowJobtitle(null);
        }
        private void EventHelper_CurrentCompanyChanged(object? sender, EventArgs e)
        {
            btnActionWebsite.IsEnabled = false;
            if (App.AppShare.CurrentCompany == null) return;
            lblActiveCompany.Content = App.AppShare.CurrentCompany.Name;
            if (!string.IsNullOrEmpty(App.AppShare.CurrentCompany.Website))
            {
                btnActionWebsite.IsEnabled = true;
            }
            using (new Internals.WorkhandlerContainer("EventHelper_CurrentCompanyChanged"))
            {
                dgContactpersons.BeginInit();
                dgContactpersons.ItemsSource = ApplyContactpersonFilter(App.DataShare.ContactpersonModelCollection);
                dgContactpersons.EndInit();
            }

        }
        private void EventHelper_CurrentContactpersonChanged(object? sender, EventArgs e)
        {
            if (App.AppShare.CurrentContactperson == null) return;
            lblActiveContactperson.Content = App.AppShare.CurrentContactperson.Fullname;
        }

        private void btnActionWebsite_Click(object sender, RoutedEventArgs e)
        {
            if (App.AppShare.CurrentCompany != null && !string.IsNullOrEmpty( App.AppShare.CurrentCompany.Website))
            {
                App.ToolsHelper.OpenWebsite(App.AppShare.CurrentCompany.Website);
            }
        }

        private void tglDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (!tglDashboard.IsChecked.HasValue) return;
            if (tglDashboard.IsChecked.Value)
            {
                pageDashboard.Visibility = Visibility.Visible;
                grdDashboard.Visibility = Visibility.Visible;
            }
            else
            {
                pageDashboard.Visibility = Visibility.Collapsed;
                grdDashboard.Visibility = Visibility.Collapsed;
            }
        }

        private void tglCompanies_Click(object sender, RoutedEventArgs e)
        {
            if (!tglCompanies.IsChecked.HasValue) return;
            if (tglCompanies.IsChecked.Value)
            {
                pageCompanies.Visibility = Visibility.Visible;
                grdCompanies.Visibility = Visibility.Visible;
            }
            else
            {
                pageCompanies.Visibility = Visibility.Collapsed;
                grdCompanies.Visibility = Visibility.Collapsed;
            }
        }

        private void tglActivities_Click(object sender, RoutedEventArgs e)
        {
            if (!tglActivities.IsChecked.HasValue) return;
            if (tglActivities.IsChecked.Value)
            {
                pageactivities.Visibility = Visibility.Visible;
                grdActivities.Visibility = Visibility.Visible;
            }
            else
            {
                pageactivities.Visibility = Visibility.Collapsed;
                grdActivities.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAppSettings_Click(object sender, RoutedEventArgs e)
        {
            App.DialogHelper.ShowAppsettingsDialog();
        }
    }
}