using System.Configuration;
using System.Data;
using System.Windows;
using VisumCRM.Client.Wpf.Internals;

namespace VisumCRM.Client.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DialogHelper DialogHelper = new();
        public static Repository.Repositories Repositories = new(Guid.Empty);
    }

}
