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
    /// Interaction logic for AuthenticateDialog.xaml
    /// </summary>
    public partial class AuthenticateDialog : Window
    {
        private Internals.FormSettingsClass _settings;
        public AuthenticateDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += AuthenticateDialog_ContentRendered;
            Closing += AuthenticateDialog_Closing;
            KeyDown += AuthenticateDialog_KeyDown;
            _settings.Load();
        }

        private void AuthenticateDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Close();
            }
        }

        private void AuthenticateDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void AuthenticateDialog_ContentRendered(object? sender, EventArgs e)
        {
            txtLicense.Focus();
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

        }

        private void imgRes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void imgCls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
