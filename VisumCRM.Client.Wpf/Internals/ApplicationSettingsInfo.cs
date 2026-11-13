using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class ApplicationSettingsInfo
    {
        public List<string> Languages { get; set; }
        public string SelectedLanguage { get; set; }
        public List<string> Themes { get; set; }
        public string SelectedTheme { get; set; }
        public string PrivateDocumentFolderPath { get; set; }
        public string PublicDocumentFolderPath { get; set; }
        public bool UseAutoCompanyNumber { get; set; }
        public string ServerPath { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string DatabaseName { get; set; }
        public bool TrustedConnection { get; set; }
        public bool TrustServerCertificate { get; set; }
        public ApplicationSettingsInfo()
        {
            PrivateDocumentFolderPath = "";
            PublicDocumentFolderPath = "";
            UseAutoCompanyNumber = false;
            ServerPath = "localhost\\SQLEXPRESS";
            DatabaseName = "VisumCRM";
            TrustedConnection = true;
            TrustServerCertificate = true;
            SelectedTheme = "Standard";
            SelectedLanguage = "English";
            Languages = new();
            Themes = new();
        }
    }
}
