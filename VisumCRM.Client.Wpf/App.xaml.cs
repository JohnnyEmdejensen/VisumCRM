using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using VisumCRM.Client.Wpf.Internals;

namespace VisumCRM.Client.Wpf
{

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
    {
        public static EventHelper EventHelper = new();
        public static Internals.LicenseInMemory? LicenseInMemory = null;
        public static AppShare AppShare = new();
        public static ToolsHelper ToolsHelper = new();
        public static DialogHelper DialogHelper = new();
        public static Repository.Repositories Repositories = new(Guid.Empty);
        public static Internals.DataShare DataShare = new();

        public App()
        {
            DataShare.ResumeEvents();
            if (Clipboard.ContainsText())
            {
                var firsttimeLicense = Clipboard.GetText();
                if (firsttimeLicense.Contains("VisumCRM License"))
                {
                    LicenseInMemory = Newtonsoft.Json.JsonConvert.DeserializeObject<Internals.LicenseInMemory>(firsttimeLicense);
                }
            }
        }   

    }

}
