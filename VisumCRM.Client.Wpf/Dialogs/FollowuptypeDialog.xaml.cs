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
    /// Interaction logic for FollowuptypeDialog.xaml
    /// </summary>
    public partial class FollowuptypeDialog : Window
    {
        private IEnumerable<TextBox>? _textboxes;

        private Internals.FormSettingsClass _settings;
        private Entity.Followuptype? _followuptype;
        public FollowuptypeDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += FollowuptypeDialog_ContentRendered;
            Closing += FollowuptypeDialog_Closing;
            KeyDown += FollowuptypeDialog_KeyDown;
            FillControls();
            _settings.Load();
        }

        private void DoAdd()
        {
            _followuptype = new();
            DataContext = _followuptype;
            SetGui();
            txtKey.Focus();
        }
        private void DoClose()
        {
            Close();
        }
        private void DoDelete()
        {
            _followuptype = DataContext as Entity.Followuptype;
            var message = App.Current.Resources["messagebox.message.suretodeletethisfollowuptype"].ToString();
            var titlw = App.Current.Resources["messagebox.title.deletefollowuptype"].ToString();
            if (message != null && titlw != null && _followuptype != null)
            {
                if (MessageBox.Show(string.Format(message.Replace("|", Environment.NewLine), _followuptype.Value), titlw, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _followuptype = App.Repositories.FollowuptypeRepository.Delete(_followuptype.Id);
                    if (_followuptype != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has marked the followuptype as deleted.", _followuptype.Id);
                    }
                }

            }
        }
        private void DoSave(bool CloseAfter)
        {
            _followuptype = DataContext as Entity.Followuptype;
            if (_followuptype != null)
            {
                if (_followuptype.Id == 0)
                {
                    _followuptype = App.Repositories.FollowuptypeRepository.Create(_followuptype);
                    if (_followuptype != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has created the followuptype.", _followuptype.Id);
                    }
                }
                else
                {
                    _followuptype = App.Repositories.FollowuptypeRepository.Update(_followuptype);
                    if (_followuptype != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has updated the followuptype.", _followuptype.Id);
                    }
                }
                DataContext = _followuptype;
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
                FillFollowupsControl();
            }
        }
        private void FillFollowupsControl()
        {
            using (new Internals.WorkhandlerContainer("FillFollowupsControl"))
            {
                dgFollowupTypes.BeginInit();
                dgFollowupTypes.ItemsSource = App.DataShare.FollowuptypeCollection;
                dgFollowupTypes.EndInit();
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
            _followuptype = DataContext as Entity.Followuptype;
            btnAdd.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveClose.IsEnabled = false;
            btnDelete.IsEnabled = false;
            if (_followuptype != null)
            {
                if (_followuptype.Id > 0)
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
                    txtKey.Focus();
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
        private void FollowuptypeDialog_KeyDown(object sender, KeyEventArgs e)
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
                    btnAdd.Focus();
                    DoAdd();
                }
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    if (e.Key == Key.S)
                    {
                        e.Handled = true;
                        btnSave.Focus();
                        DoSave(true);
                    }
                }
            }
        }

        private void FollowuptypeDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void FollowuptypeDialog_ContentRendered(object? sender, EventArgs e)
        {
            _followuptype = DataContext as Entity.Followuptype;
            SetGui();
            SetEnvironments();
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
        private void Ctrl_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = sender as TextBox;
            if (item != null)
            {
                item.SelectAll();
            }
        }

        private void dgFollowupTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _followuptype = dgFollowupTypes.SelectedItem as Entity.Followuptype;
            DataContext = _followuptype;
            txtKey.Focus();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
