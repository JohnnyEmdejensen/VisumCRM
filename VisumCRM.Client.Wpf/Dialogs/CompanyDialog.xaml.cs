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
    /// Interaction logic for CompanyDialog.xaml
    /// </summary>
    public partial class CompanyDialog : Window
    {
        private Entity.Company? _company;
        private Internals.FormSettingsClass _settings;
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
        }


        private void DoAdd()
        {
            _company = new Company();
            DataContext = _company;
            SetGui();
            txtNumber.Focus();
        }
        private void DoClose()
        {
            Close();
        }
        private void DoDelete()
        {

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
                }
                else
                {
                    _company = App.Repositories.CompanyRepository.Update(_company);
                }
                DataContext = _company;
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
        }

        private void CompanyDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void CompanyDialog_ContentRendered(object? sender, EventArgs e)
        {            
            _company = DataContext as Entity.Company;
            if (_company != null)
                App.ToolsHelper.LogWriter(1, "The user has viewed the company details.", _company.Id);
            SetGui();
            ViewDetails();
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
    }
}
