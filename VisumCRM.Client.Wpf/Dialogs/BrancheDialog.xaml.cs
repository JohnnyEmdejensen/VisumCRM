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
    /// Interaction logic for BrancheDialog.xaml
    /// </summary>
    public partial class BrancheDialog : Window
    {
        private Entity.Branche? _branche;
        private Internals.FormSettingsClass _settings;
        private IEnumerable<TextBox>? _textboxes;
        public BrancheDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += BrancheDialog_ContentRendered;
            Closing += BrancheDialog_Closing;
            KeyDown += BrancheDialog_KeyDown;
            dgBranches.SizeChanged += DgBranches_SizeChanged;
            App.EventHelper.BrancheCollectionChanged += EventHelper_BrancheCollectionChanged;
            _settings.Load();
            FillControls();
        }

        private void DoAdd()
        {
            _branche = new();
            DataContext = _branche;
            SetGui();
            txtKey.Focus();
        }
        private void DoClose()
        {
            Close();
        }
        private void DoDelete()
        {

        }
        private void FillControls()
        {
            using (new Internals.WorkhandlerContainer("FillControls"))
            {
                FillBranchedControl();
            }
        }
        private void FillBranchedControl()
        {
            using (new Internals.WorkhandlerContainer("FillBranchedControl"))
            {
                dgBranches.BeginInit();
                dgBranches.ItemsSource = App.DataShare.BrancheCollection;
                dgBranches.EndInit();
            }
        }
        private void DoSave(bool CloseAfter)
        {
            _branche = DataContext as Entity.Branche;
            if (_branche != null)
            {
                if (_branche.Id == 0)
                {
                    _branche = App.Repositories.BrancheRepository.Create(_branche);
                }
                else
                {
                    _branche = App.Repositories.BrancheRepository.Update(_branche);
                }
                DataContext = _branche;
                SetGui();
            }
            if (CloseAfter)
            {
                DoClose();
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
            _branche = DataContext as Entity.Branche;
            btnAdd.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveClose.IsEnabled = false;
            btnDelete.IsEnabled = false;
            if (_branche != null)
            {
                if (_branche.Id > 0)
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
        private void BrancheDialog_KeyDown(object sender, KeyEventArgs e)
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
                    DoSave(false);
                }
                if (e.Key == Key.N)
                {
                    DoAdd();
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
        private void BrancheDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void BrancheDialog_ContentRendered(object? sender, EventArgs e)
        {
            _branche = DataContext as Entity.Branche;
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
        private void EventHelper_BrancheCollectionChanged(object? sender, EventArgs e)
        {
            using (new Internals.WorkhandlerContainer("EventHelper_BrancheCollectionChanged"))
            {
                FillBranchedControl();
            }
        }
        private void dgBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _branche = dgBranches.SelectedItem as Entity.Branche;
            DataContext = _branche;
            SetGui();
            txtKey.Focus();
        }
        private void Ctrl_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = sender as TextBox;
            if (item != null)
            {
                item.SelectAll();
            }
        }
        private void DgBranches_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = dgBranches.ActualWidth;
            dgBranches.Columns[2].Width = width - 64 - 2; 
        }

    }
}
