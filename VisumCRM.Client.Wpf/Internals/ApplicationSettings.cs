using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class ApplicationSettings
    {
        private DirectoryInfo _dir;
        private FileInfo _file;
        public ApplicationSettingsInfo Info { get; set; }
        public ApplicationSettings()
        {
            _dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Emdesoft Developments\\VisumCRM");
            if (!_dir.Exists) _dir.Create();
            _file = new FileInfo(_dir.FullName + "/VisumCRM.json");
            Info = new();
        }

        public void Load()
        {
            if (!_file.Exists)
            {
                Save();
            }
            string content = "";
            using (StreamReader sr = new(_file.FullName))
            {
                content = sr.ReadToEnd();
                sr.Close();
            }
            if (!string.IsNullOrEmpty(content))
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Internals.ApplicationSettingsInfo>(content);
                if (data != null)
                {
                    Info = data;
                    if (!string.IsNullOrEmpty( Info.ServerPath) && !string.IsNullOrEmpty(Info.DatabaseName))
                    {
                        //Server=localhost\\SQLEXPRESS;Database=VisumCRM;Trusted_Connection=True;TrustServerCertificate=True;
                        App.ConnectionString = "Server=" + Info.ServerPath + ";Database=" + Info.DatabaseName + ";";
                        if (Info.TrustedConnection)
                        {
                            App.ConnectionString += "Trusted_Connection=True;";
                        }
                        if (Info.TrustServerCertificate)
                        {
                            App.ConnectionString += "TrustServerCertificate=True;";
                        }
                    }
                }
            }
        }

        public void Save()
        {
            Internals.ApplicationSettingsInfo info = new();
            info.DatabaseName = Info.DatabaseName;
            info.Languages = Info.Languages;
            info.PrivateDocumentFolderPath = Info.PrivateDocumentFolderPath;
            info.PublicDocumentFolderPath = Info.PrivateDocumentFolderPath;
            info.SelectedLanguage = Info.SelectedLanguage;
            info.SelectedTheme = Info.SelectedTheme;
            info.ServerPath = Info.ServerPath;
            info.Themes = Info.Themes;
            info.TrustedConnection = Info.TrustedConnection;
            info.TrustServerCertificate = Info.TrustServerCertificate;
            info.UseAutoCompanyNumber = Info.UseAutoCompanyNumber;
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            using (StreamWriter sw = new StreamWriter(_file.FullName))
            {
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
