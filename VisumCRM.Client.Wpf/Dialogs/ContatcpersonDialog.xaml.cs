using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VisumCRM.Entity;

namespace VisumCRM.Client.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for ContatcpersonDialog.xaml
    /// </summary>
    public partial class ContatcpersonDialog : Window
    {
        private Entity.Contactperson? _contactperson;
        private Internals.FormSettingsClass _settings;
        private IEnumerable<TextBox>? _textboxes;
        public ContatcpersonDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += ContatcpersonDialog_ContentRendered;
            Closing += ContatcpersonDialog_Closing;
            KeyDown += ContatcpersonDialog_KeyDown;
            FillControls();
            _settings.Load();
        }

        private void DoAdd()
        {
            _contactperson = new();
            DataContext = _contactperson;
            SetGui();
            ViewDetails();
            txtFirstname.Focus();
        }

        private void DoClose()
        {
            Close();
        }
        private void DoDelete()
        {
            _contactperson = DataContext as Entity.Contactperson;
            if (_contactperson != null)
            {
                string? message = App.Current.Resources["messagebox.message.suretodeletethiscontactperson"].ToString();
                string? title = App.Current.Resources["messagebox.title.deletecontactperson"].ToString();
                if (message != null && title != null)
                {
                    _contactperson.SetFullname();
                    message = message.Replace("|", Environment.NewLine) + _contactperson.Fullname;
                    if (MessageBox.Show(message,title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _contactperson = App.Repositories.ContactpersonRepository.Delete(_contactperson.Id);
                    }
                    DataContext = null;
                    _contactperson = null;
                    SetGui();
                    ViewDetails();
                }
            }
        }

        private void DoSave(bool CloseAfter)
        {
            _contactperson = DataContext as Entity.Contactperson;
            if (_contactperson != null)
            {
                var locationId = App.ToolsHelper.ParseLocation(cboCountry.Text, txtZipcode.Text, txtCity.Text, txtStreet.Text, txtHouse.Text, txtFloor.Text, txtDoor.Text);
                if (locationId != null)
                    _contactperson.LocationId = locationId;
                if(!string.IsNullOrEmpty(txtBirthday.Text))
                {
                    long l = 0;
                    if (long.TryParse(txtBirthday.Text, out l))
                    {
                        _contactperson.Birthday = l;
                    }
                }
                if (_contactperson.Id == 0)
                {
                    _contactperson = App.Repositories.ContactpersonRepository.Create(_contactperson);
                    if (_contactperson != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has created the contactperson.", _contactperson.Id);
                        App.ToolsHelper.LogWriter(2, "Created", _contactperson.Id);
                    }
                }
                else
                {
                    _contactperson = App.Repositories.ContactpersonRepository.Update(_contactperson);
                    if (_contactperson != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has updated the contactperson.", _contactperson.Id);
                        App.ToolsHelper.LogWriter(2, "Updated", _contactperson.Id);
                    }
                }
                DataContext = _contactperson;
                SetGui();
                ViewDetails();
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
                FillEmployeeControl();
            }
        }
        private void FillCountryControl()
        {
            using (new Internals.WorkhandlerContainer("FillCountryControl"))
            {
                App.ToolsHelper.FillCountryControl(cboCountry);
            }
        }
        private void FillSalespersonControl()
        {
            using (new Internals.WorkhandlerContainer("FillSalespersonControl"))
            {
                App.ToolsHelper.FillSalespersonControl(cboSalespersons);
            }
        }
        private void FillEmployeeControl()
        {
            using (new Internals.WorkhandlerContainer("FillEmployeeControl"))
            {
                App.ToolsHelper.FillEmployeeControl(cboEmployees);
            }
        }
        private void SetEnvironments()
        {
            _textboxes = App.ToolsHelper.FindVisualChildren<TextBox>(this);
            foreach (var ctrl in _textboxes)
            {
                ctrl.GotFocus += Ctrl_GotFocus; ;
            }
        }


        private void SetGui()
        {
            _contactperson = DataContext as Entity.Contactperson;
            btnAdd.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveClose.IsEnabled = false;
            btnDelete.IsEnabled = false;
            if (_contactperson != null)
            {
                if (_contactperson.Id > 0)
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
                    txtFirstname.Focus();
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
            txtDoor.Text = "";
            txtZipcode.Text = "";
            txtCity.Text = "";
            cboCountry.SelectedValue = null;

            _contactperson = DataContext as Entity.Contactperson;
            if (_contactperson == null || App.DataShare.LocationModelCollection == null) return;
            var location = App.DataShare.LocationModelCollection.FirstOrDefault(x => x.Id == _contactperson.LocationId);
            if (location == null) return;
            txtStreet.Text = location.Street;
            txtHouse.Text = location.House;
            txtFloor.Text = location.Floor;
            txtDoor.Text = location.Door;
            txtZipcode.Text = location.Zipcode;
            txtCity.Text = location.City;
            cboCountry.SelectedValue = location.CountryId;
        }


        private void ContatcpersonDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DoClose();
            }
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Key == Key.S)
                {
                    e.Handled = true;
                    btnSave.Focus();
                    DoSave(false);
                }
                if (e.Key == Key.N)
                {
                    e.Handled = true;
                    DoAdd();
                }
                if (e.Key == Key.D)
                {
                    e.Handled = true;
                    btnDelete.Focus();
                    DoDelete();
                }
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    if (e.Key == Key.S)
                    {
                        e.Handled = true;
                        btnSaveClose.Focus();
                        DoSave(true);
                    }
                }
            }
        }

        private void ContatcpersonDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void ContatcpersonDialog_ContentRendered(object? sender, EventArgs e)
        {
            SetEnvironments();
            _contactperson = DataContext as Entity.Contactperson;
            SetGui();
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
        private void Ctrl_GotFocus(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            ctrl.SelectAll();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DoDelete();
        }

        private void lblJobtitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var jobtitle = new Entity.Jobtitle();
            App.DialogHelper.ShowJobtitle(jobtitle);
        }
    }
}
