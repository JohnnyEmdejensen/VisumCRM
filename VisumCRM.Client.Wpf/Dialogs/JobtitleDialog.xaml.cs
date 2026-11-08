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
    /// Interaction logic for JobtitleDialog.xaml
    /// </summary>
    public partial class JobtitleDialog : Window
    {
        private Entity.Jobtitle? _jobtitle;
        public Internals.FormSettingsClass _settings;
        public JobtitleDialog()
        {
            InitializeComponent();
            _settings = new(this);
            ContentRendered += JobtitleDialog_ContentRendered;
            Closing += JobtitleDialog_Closing;
            KeyDown += JobtitleDialog_KeyDown;
            dgJobtitles.SizeChanged += DgJobtitles_SizeChanged;            
            _settings.Load();
        }

        private List<Entity.Jobtitle>? ApplyFilter(List<Jobtitle>? Jobtitles)
        {
            if (Jobtitles == null) return null;
            if (!string.IsNullOrEmpty(txtFilter.Text))
                return Jobtitles.Where(x => x.Key.ToLower().Contains(txtFilter.Text.ToLower()) || x.Value.ToLower().Contains(txtFilter.Text.ToLower())).ToList();
            else
                return Jobtitles;
        }


        private void DoAdd()
        {
            _jobtitle = new Jobtitle();
            DataContext = _jobtitle;
            SetGui();
            txtKey.Focus();
        }
        private void DoClose()
        {
            Close();
        }
        private void DoDelete()
        {
            _jobtitle = DataContext as Entity.Jobtitle;
            string? message = App.Current.Resources["messagebox.message.suretodeletethisjobtitle"].ToString();
            string? title = App.Current.Resources["messagebox.title.deletejobtitle"].ToString();
            if (message != null && title != null && _jobtitle != null)
            {
                ImageSource src = new BitmapImage(new Uri("/Images/delete_24.png", UriKind.Relative));
                imgDialogState.Source = src;
                if (MessageBox.Show(string.Format( message.Replace("|", Environment.NewLine), _jobtitle.Value), title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _jobtitle = App.Repositories.JobtitleRepository.Delete(_jobtitle.Id);
                    if (_jobtitle != null)
                    {
                        App.ToolsHelper.LogWriter(0, "The user has marked the jobtitle deleted.", _jobtitle.Id);
                    }
                    _jobtitle = null;
                    DataContext = _jobtitle;
                    SetGui();
                }
            }
            imgDialogState.Source = null;
        }
        private void DoSave(bool CloseAfter)
        {
            _jobtitle = DataContext as Entity.Jobtitle;
            if (_jobtitle != null)
            {
                if (_jobtitle.Id == 0)
                {
                    _jobtitle = App.Repositories.JobtitleRepository.Create(_jobtitle);
                }
                else
                {
                    _jobtitle = App.Repositories.JobtitleRepository.Create(_jobtitle);
                }
                DataContext = _jobtitle;
                SetGui();
            }
        }

        private void FillJobtitleControl()
        {
            using (new Internals.WorkhandlerContainer("FillJobtitleControl"))
            {
                dgJobtitles.BeginInit();
                dgJobtitles.ItemsSource = ApplyFilter(App.DataShare.JobtitleCollection);
                dgJobtitles.EndInit();
            }

        }

        private void SetGui()
        {
            _jobtitle = DataContext as Entity.Jobtitle;
            btnAdd.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveClose.IsEnabled = false;
            btnDelete.IsEnabled = false;
            if (_jobtitle != null)
            {
                if (_jobtitle.Id > 0)
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
                    //txtKey.Focus();
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

        private void JobtitleDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DoClose();
            }
        }

        private void JobtitleDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.Save();
        }

        private void JobtitleDialog_ContentRendered(object? sender, EventArgs e)
        {
            _jobtitle = DataContext as Entity.Jobtitle;
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DoDelete();
        }

        private void dgJobtitles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _jobtitle = dgJobtitles.SelectedItem as Entity.Jobtitle;
            DataContext = _jobtitle;
            SetGui();
            txtKey.Focus();
        }
        private void DgJobtitles_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = dgJobtitles.ActualWidth;
            dgJobtitles.Columns[2].Width = width - 64 - 2;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillJobtitleControl();

        }
    }
}
