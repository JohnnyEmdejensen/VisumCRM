using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VisumCRM.Client.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for CompanyDialog.xaml
    /// </summary>
    public partial class CompanyDialog : Window
    {
        private CVRData? _cvrdata = null;
        private Entity.Company? _company;
        private Internals.FormSettingsClass _settings;
        private List<Entity.Workinglog>? _logs;
        private IEnumerable<TextBox>? _textboxes;
        public CompanyDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += CompanyDialog_ContentRendered;
            Closing += CompanyDialog_Closing;
            KeyDown += CompanyDialog_KeyDown;
            App.EventHelper.BrancheCollectionChanged += EventHelper_BrancheCollectionChanged;
            imgDialogState.Source = null;
            _settings.Load();
            FillControls();
            dgCompanyLog.SizeChanged += DgCompanyLog_SizeChanged;
        }


        private void DoAdd()
        {
            _company = new Entity.Company();
            if (App.AppShare.CurrentUser != null)
            {
                _company.SalespersonId = App.AppShare.CurrentUser.Id;
            }
            DataContext = _company;
            SetGui();
            ViewDetails();
            txtNumber.Focus();
        }
        private void DoClose()
        {
            bool allow = true;
            _company = DataContext as Entity.Company;
            if (_company != null)
            {
                if (_company.Id == 0)
                {
                    var message = App.Current.Resources["messagebox.message.suretocancelthecompanycreation"].ToString();
                    var title = App.Current.Resources["messagebox.title.cancelcompanycreation"].ToString();
                    if (message != null && title != null)
                    {
                        if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            allow = false;
                        }
                    }
                }
            }
            if (allow)
                Close();
        }
        private void DoDelete()
        {
            _company = DataContext as Entity.Company;
            if (_company != null)
            {
                string? message = App.Current.Resources["messagebox.message.suretodeletethiscompany"].ToString();
                string? title = App.Current.Resources["messagebox.title.deletecompany"].ToString();
                if (message != null && title != null)
                {
                    if(MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _company = App.Repositories.CompanyRepository.Delete(_company.Id);
                        if (_company != null)
                        {
                            App.ToolsHelper.LogWriter(0, "The user has marked the company deleted.", _company.Id);
                        }
                    }
                    _company = null;
                    DataContext = _company;
                    SetGui();
                    ViewDetails();
                }
            }
        }
        private void DoSave(bool CloseAfter)
        {
            _company = DataContext as Entity.Company;
            if (_company != null)
            {
                var locationId = App.ToolsHelper.ParseLocation(cboCountry.Text, txtZipcode.Text, txtCity.Text, txtStreet.Text, txtHouse.Text, txtFloor.Text, txtDoor.Text, null);
                if (locationId.HasValue)
                    _company.LocationId = locationId.Value;
                if (_company.Id == 0)
                {
                    _company = App.Repositories.CompanyRepository.Create(_company);
                    if (_company != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has created the company.", _company.Id);
                        App.ToolsHelper.LogWriter(1, "Created", _company.Id);
                    }
                }
                else
                {
                    _company = App.Repositories.CompanyRepository.Update(_company);
                    if (_company != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has updated the company.", _company.Id);
                        App.ToolsHelper.LogWriter(1, "Updated.", _company.Id);
                    }
                }
                DataContext = _company;
                if (_cvrdata != null && _cvrdata.direktion != null && _cvrdata.direktion.Count > 0 && _company != null)
                {
                    string message = App.Current.Resources["messagebox.message.theimportedcvrdatacontainspersondate"].ToString() ?? "";
                    string title = App.Current.Resources["messagebox.title.cvrdatahascontactperson"].ToString() ?? "";
                    if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        List<Entity.Contactperson> contactpersons = new();
                        foreach (var person in _cvrdata.direktion)
                        {
                            var contactperson = new Entity.Contactperson();
                            contactperson.EmployeerId = _company.Id;
                            if (person.name != null)
                            {
                                var parts = person.name.Split(' ');
                                if (parts.Length > 0)
                                {
                                    if (parts.Length == 1)
                                    {
                                        contactperson.Firstname = parts[0];
                                    }
                                    if (parts.Length == 2)
                                    {
                                        contactperson.Firstname = parts[0];
                                        contactperson.Surname = parts[1];
                                    }
                                    if (parts.Length >= 3 )
                                    {
                                        for (int i = 1; i < parts.Length - 1; i++)
                                        {
                                            if (!string.IsNullOrEmpty(contactperson.Middlename))
                                            {
                                                contactperson.Middlename += " " + parts[i];
                                            }
                                            else
                                            {
                                                contactperson.Middlename = parts[i];
                                            }
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(person.address) && !string.IsNullOrEmpty(person.zipcode) && !string.IsNullOrEmpty(person.city))
                            {
                                string street = "";
                                string house = "";
                                string? floor;
                                string? door;
                                if (App.ToolsHelper.ParseAddress(person.address, out street, out house, out floor, out door))
                                {
                                    contactperson.LocationId = App.ToolsHelper.ParseLocation("Danamrk", person.zipcode, person.city, street, house, floor, door);
                                }
                            }
                            contactpersons.Add(contactperson);
                        }
                        if (contactpersons.Count > 0)
                        {
                            foreach (var item in contactpersons)
                            {
                                var newItem = App.Repositories.ContactpersonRepository.Create(item);
                            }
                        }
                    }
                }
                SetGui();
            }
            if (CloseAfter)
            {
                DoClose();
            }
        }
        private void FillControls()
        {
            using (new Internals.WorkhandlerContainer("FillControls"))
            {
                FillCountryControl();
                FillSalespersonControl();
                FillBrancheControl();
                FillTargetgroupControl();
            }
        }

        private void FillTargetgroupControl()
        {
            using (new Internals.WorkhandlerContainer("FillTargetgroupControl"))
            {
                App.ToolsHelper.FillTargetgroupControl(cboTargetgroups);
            }
        }
        private void FillSalespersonControl()
        {
            using (new Internals.WorkhandlerContainer("FillSalespersonControl"))
            {
                App.ToolsHelper.FillSalespersonControl(cboSalespersons);
            }
        }

        private void FillBrancheControl()
        {
            using (new Internals.WorkhandlerContainer("FillBrancheControl"))
            {
                App.ToolsHelper.FillBrancheControl(cboBranches);
            }
        }
        private void FillCountryControl()
        {
            using (new Internals.WorkhandlerContainer("FillCountryControl"))
            {
                App.ToolsHelper.FillCountryControl(cboCountry);
            }
        }

        private void SetEnvironments()
        {
            _textboxes = App.ToolsHelper.FindVisualChildren<TextBox>(this);
            foreach (var ctrl in _textboxes)
            {
                ctrl.GotFocus += Ctrl_GotFocus;
            }
        }


        private void SetGui()
        {
            _company = DataContext as Entity.Company;
            btnAdd.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveClose.IsEnabled = false;
            btnDelete.IsEnabled = false;
            if (_company != null)
            {
                if (_company.Id > 0)
                {
                    btnAdd.IsEnabled = true;
                    btnSave.IsEnabled = true;
                    btnSaveClose.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                    imgDialogState.Source = null;
                }
                else
                {
                    btnAdd.IsEnabled = false;
                    btnSave.IsEnabled = true;
                    btnSaveClose.IsEnabled = true;
                    btnDelete.IsEnabled = false;
                    ImageSource src = new BitmapImage(new Uri("/Images/add_24.png", UriKind.Relative));
                    imgDialogState.Source = src;
                    txtNumber.Focus();
                }
            }
            else
            {
                btnAdd.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnSaveClose.IsEnabled = false;
                btnDelete.IsEnabled = false;
                imgDialogState.Source = null;
            }
        }

        private void ViewDetails()
        {
            txtStreet.Text = "";
            txtHouse.Text = "";
            txtFloor.Text = "";
            txtFloor.Text = "";
            txtZipcode.Text = "";
            txtCity.Text = "";
            cboCountry.SelectedValue = null;
            _company = DataContext as Entity.Company;
            if (_company == null || App.DataShare.LocationModelCollection == null) return;
            var location = App.DataShare.LocationModelCollection.FirstOrDefault(x => x.Id == _company.LocationId);
            if (location == null) return;
            txtStreet.Text = location.Street;
            txtHouse.Text = location.House;
            txtFloor.Text = location.Floor;
            txtFloor.Text = location.Door;
            txtZipcode.Text = location.Zipcode;
            txtCity.Text = location.City;
            cboCountry.SelectedValue = location.CountryId;
        }

        private void CompanyDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DoClose();
            }
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Key == Key.N)
                {
                    e.Handled = true;
                    btnAdd.Focus();
                    DoAdd();
                }
                if (e.Key == Key.D)
                {
                    e.Handled = true;
                    DoDelete();
                }
                if (e.Key == Key.S)
                {
                    DoSave(false);
                }
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    if (e.Key == Key.S)
                    {
                        DoSave(true);
                    }
                }
            }
        }

        private void CompanyDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void CompanyDialog_ContentRendered(object? sender, EventArgs e)
        {
            SetEnvironments();
            _company = DataContext as Entity.Company;
            if (_company != null)
            {
                lblName.Content = _company.Name;
                lblAddress.Content = App.ToolsHelper.GetAddress(_company.LocationId);
                lblPostalzone.Content = App.ToolsHelper.GetPostalzone(_company.LocationId);
                lblPhone.Content = "Phone: " + _company.Phone;
                lblPhone2.Content = "Phone: " + _company.Phone2;
                lblEmail.Content = "Email: " + _company.Email;
                _logs = App.Repositories.WorkinglogRepository.GetAllCompanyLog(_company.Id);
                List<Models.WorkingLogModel> logs = new List<Models.WorkingLogModel>();
                if (_logs != null)
                {
                    _logs = _logs.OrderByDescending(x => x.LogTime).ToList();
                    foreach (var log in _logs)
                    {
                        Models.WorkingLogModel wl = new Models.WorkingLogModel
                        {
                            FormattetTime = App.ToolsHelper.ConvertLongToDateTime(log.LogTime),
                            Message = log.Message,
                        };
                        if (App.DataShare.UserCollection != null)
                        {
                            var user = App.DataShare.UserCollection.FirstOrDefault(x => x.Id == log.UserId);
                            if (user != null)
                            {
                                user.SetFullName();
                                wl.UserName = user.Fullname ?? "";
                            }
                        }
                        logs.Add(wl);
                    }
                }
                dgCompanyLog.BeginInit();
                dgCompanyLog.ItemsSource = logs;
                dgCompanyLog.EndInit();
                App.ToolsHelper.LogWriter(1, "The user has viewed the company details.", _company.Id);
            }
            SetGui();
            ViewDetails();
            if (_company != null) txtNumber.Focus();
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
            DoClose();

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DoAdd();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DoSave(false);
        }

        private void btnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            DoSave(true);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DoDelete();
        }

        private void Branche_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var branche = new Entity.Branche();
            App.DialogHelper.ShowBranche(branche);
        }

        private void Targetgroup_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void EventHelper_BrancheCollectionChanged(object? sender, EventArgs e)
        {
            using (new Internals.WorkhandlerContainer("EventHelper_BrancheCollectionChanged"))
            {
                FillBrancheControl();
            }
        }
        private void Ctrl_GotFocus(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            ctrl.SelectAll();
        }
        private void DgCompanyLog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = dgCompanyLog.ActualWidth;
            dgCompanyLog.Columns[3].Width = width - 120 - 100 - 32;
        }

        private void btnCVR_Click(object sender, RoutedEventArgs e)
        {
            _company = DataContext as Entity.Company;
            if (!string.IsNullOrEmpty(txtTaxnumber.Text))
            {
                var api = new CVRRegister();
                var result = api.GetCompany(txtTaxnumber.Text);
                if (_company != null && result != null)
                {
                    _cvrdata = result;
                    if (_company.Id == 0 && !btnAdd.IsEnabled)
                    {
                        _company = new();
                        if (App.AppShare.CurrentUser != null)
                        {
                            _company.SalespersonId = App.AppShare.CurrentUser.Id;
                        }
                        _company.Name = result.name ?? "";
                        txtStreet.Text = result.address;
                        txtZipcode.Text = result.zipcode;
                        txtCity.Text = result.city;
                        _company.Website = result.website;
                        _company.Email = result.email;
                        _company.Phone = result.phone;
                        _company.Taxnumber = result.vat ?? "";
                        _company.Founded = App.ToolsHelper.FormatStringDateToLongDate(result.startdate);
                        if (result.employees.HasValue)
                            _company.Employees = result.employees.Value;
                        if (result.country != null && result.country == "DK" && App.DataShare.CountryCollection != null)
                        {
                            var item = App.DataShare.CountryCollection.FirstOrDefault(x => x.Name == "Danmark");
                            if (item != null)
                                cboCountry.SelectedValue = item.Id;
                        }
                        if (App.DataShare.BrancheCollection != null && result.industrycode.HasValue)
                        {
                            var branche = App.DataShare.BrancheCollection.FirstOrDefault(x => x.Key == result.industrycode.Value.ToString());
                            if (branche == null)
                            {
                                branche = new();
                                branche.Key = result.industrycode.Value.ToString();
                                branche.Value = result.industrydesc ?? "";
                                branche = App.Repositories.BrancheRepository.Create(branche);
                            }
                            if (branche != null)
                            {
                                _company.BrancheId = branche.Id;
                            }
                        }
                        _company.SetVisualDates();
                        txtFounded.Text = _company.VisualFundedDate;
                        DataContext = _company;
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
