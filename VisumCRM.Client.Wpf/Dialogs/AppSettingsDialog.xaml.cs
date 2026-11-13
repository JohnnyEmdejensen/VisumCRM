using Microsoft.Win32;
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

namespace VisumCRM.Client.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for AppSettingsDialog.xaml
    /// </summary>
    public partial class AppSettingsDialog : Window
    {
        private Internals.FormSettingsClass _settings;
        public AppSettingsDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += AppSettingsDialog_ContentRendered;
            Closing += AppSettingsDialog_Closing;
            KeyDown += AppSettingsDialog_KeyDown;
            FillControls();
            _settings.Load();
        }

        private bool AnythingMissing()
        {
            bool result = false;
            if (string.IsNullOrEmpty(App.AppSettings.Info.ServerPath))
                result = true;
            if (string.IsNullOrEmpty(App.AppSettings.Info.DatabaseName))
                result = true;
            if (!App.AppSettings.Info.TrustedConnection)
            {
                if (string.IsNullOrEmpty(App.AppSettings.Info.Login))
                    result = true;
                if (string.IsNullOrEmpty(App.AppSettings.Info.Password))
                    result = true;
            }
            return result;
        }

        private void FillControls()
        {
            using (new Internals.WorkhandlerContainer("FillControls"))
            {
                FillLanguagesControl();
                FillThemesControl();
            }
        }
        private void FillLanguagesControl()
        {
            using (new Internals.WorkhandlerContainer("FillLanguagesControl"))
            {
                cboLanguages.BeginInit();
                cboLanguages.ItemsSource = App.AppSettings.Info.Languages;
                cboLanguages.EndInit();
            }
        }
        private void FillThemesControl()
        {
            using (new Internals.WorkhandlerContainer("FillThemesControl"))
            {
                cboThemes.BeginInit();
                cboThemes.ItemsSource = App.AppSettings.Info.Themes;
                cboThemes.EndInit();
            }
        }
        private void DoClose()
        {
            Close();
        }

        private void DoSave()
        {
            using (new Internals.WorkhandlerContainer("DoSave"))
            {
                App.AppSettings.Info.PrivateDocumentFolderPath = txtPrivateFolder.Text;
                App.AppSettings.Info.PublicDocumentFolderPath = txtPublicFolder.Text;
                if (chkUseAutoNumber.IsChecked.HasValue)
                    App.AppSettings.Info.UseAutoCompanyNumber = chkUseAutoNumber.IsChecked.Value;
                else
                    App.AppSettings.Info.UseAutoCompanyNumber = false;
                var language = cboLanguages.SelectedItem as string;
                if (language != null)
                    App.AppSettings.Info.SelectedLanguage = language;
                else
                    App.AppSettings.Info.SelectedLanguage = "English";
                var theme = cboThemes.SelectedItem as string;
                if (theme != null)
                    App.AppSettings.Info.SelectedTheme = theme;
                else
                    App.AppSettings.Info.SelectedTheme = "Standard";

                App.AppSettings.Info.ServerPath = txtServerPath.Text;
                App.AppSettings.Info.DatabaseName = txtDatabaseName.Text;
                if (chkUseTrusted.IsChecked.HasValue)
                    App.AppSettings.Info.TrustedConnection = chkUseTrusted.IsChecked.Value;
                if (chkTrustCert.IsChecked.HasValue)
                    App.AppSettings.Info.TrustServerCertificate = chkTrustCert.IsChecked.Value;
                if (!AnythingMissing())
                {
                    App.AppSettings.Save();
                    DoClose();
                }
                else
                {
                    string? message = App.Current.Resources["messagebox.message.theappsttingsismissinginformation"].ToString();
                    string? title = App.Current.Resources["messagebox.title.saveappsettings"].ToString();
                    if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(title))
                    {
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void AppSettingsDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DoClose();
            }
        }

        private void AppSettingsDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void AppSettingsDialog_ContentRendered(object? sender, EventArgs e)
        {
            btnCreateDatabase.IsEnabled = true;

            grdLogininfo.Visibility = Visibility.Collapsed;
            if (!string.IsNullOrEmpty(App.AppSettings.Info.SelectedLanguage))
                cboLanguages.SelectedItem = App.AppSettings.Info.SelectedLanguage;
            if (!string.IsNullOrEmpty(App.AppSettings.Info.SelectedTheme))
                cboThemes.SelectedItem = App.AppSettings.Info.SelectedTheme;
            txtPrivateFolder.Text = App.AppSettings.Info.PrivateDocumentFolderPath;
            txtPublicFolder.Text = App.AppSettings.Info.PublicDocumentFolderPath;
            chkUseAutoNumber.IsChecked = App.AppSettings.Info.UseAutoCompanyNumber;
            txtServerPath.Text = App.AppSettings.Info.ServerPath;
            txtDatabaseName.Text = App.AppSettings.Info.DatabaseName;
            chkTrustCert.IsChecked = App.AppSettings.Info.TrustServerCertificate;
            chkUseTrusted.IsChecked = App.AppSettings.Info.TrustedConnection;
            if (!App.AppSettings.Info.TrustedConnection)
            {
                grdLogininfo.Visibility = Visibility.Visible;
            }
            if (App.Repositories.Databasename == App.AppSettings.Info.DatabaseName)
            {
                if (App.Repositories.DatabaseObject.CanConnect())
                {
                    btnCreateDatabase.IsEnabled = false;
                }
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
            DoClose();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DoSave();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DoClose();
        }

        private void btnPublicFolder_Click(object sender, RoutedEventArgs e)
        {
            string? message = App.Current.Resources["folderdialog.message.getfoldertostorepublicfiles"].ToString();
            string? title = App.Current.Resources["folderdialog.title.getpublicfolder"].ToString();
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(title))
            {
                OpenFolderDialog ofd = new OpenFolderDialog();
                ofd.Title = title;
                ofd.Multiselect = false;
                ofd.ShowDialog();
                if (!string.IsNullOrEmpty(ofd.FolderName))
                {
                    txtPublicFolder.Text = ofd.FolderName;
                }
            }
        }

        private void btnPrivateFolder_Click(object sender, RoutedEventArgs e)
        {
            string? message = App.Current.Resources["folderdialog.message.getfoldertostoreprivatefiles"].ToString();
            string? title = App.Current.Resources["folderdialog.title.getprivatefolder"].ToString();
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(title))
            {
                OpenFolderDialog ofd = new OpenFolderDialog();
                ofd.Title = title;
                ofd.Multiselect = false;
                ofd.ShowDialog();
                if (!string.IsNullOrEmpty(ofd.FolderName))
                {
                    txtPrivateFolder.Text = ofd.FolderName;
                }
            }

        }

        private void cboLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
