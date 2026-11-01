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
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Window
    {
        private Entity.User? _user;
        public Internals.FormSettingsClass _settings;

        public UserDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += UserDialog_ContentRendered;
            Closing += UserDialog_Closing;
            KeyDown += UserDialog_KeyDown;
            _settings.Load();
            dgUserLog.SizeChanged += DgUserLog_SizeChanged;
        }

        private void DoClose()
        {
            Close();
        }
        private void DoSave(bool CloseAfter)
        {
            _user = DataContext as Entity.User;
            if (_user != null)
            {

                if (_user.Id == 0)
                {
                    _user = App.Repositories.UserRepository.Create(_user);
                }
                else
                {
                    _user = App.Repositories.UserRepository.Update(_user);
                }
            }
            if (CloseAfter)
            {
                DoClose();
            }
        }

        private void SetGui()
        {
            _user = DataContext as Entity.User;
            btnAdd.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveClose.IsEnabled = false;
            btnDelete.IsEnabled = false;

            if (_user != null)
            {
                if (_user.Id > 0)
                {
                    btnAdd.IsEnabled = true;
                    btnSave.IsEnabled = true;
                    btnSaveClose.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                }
                else
                {
                    btnAdd.IsEnabled = false;
                    btnSave.IsEnabled = true;
                    btnSaveClose.IsEnabled = true;
                    btnDelete.IsEnabled = false;
                    txtFirstname.Focus();
                }
            }
            else
            {
                btnAdd.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnSaveClose.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void UserDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DoClose();
            }
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Key == Key.S)
                {
                    DoSave(false);
                }
            }
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                if (e.Key == Key.S)
                {
                    DoSave(true);
                }
            }
        }

        private void UserDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void UserDialog_ContentRendered(object? sender, EventArgs e)
        {
            _user = DataContext as Entity.User;
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
        private void DgUserLog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = dgUserLog.ActualWidth;
            dgUserLog.Columns[2].Width = width - 120;
        }
    }
}
