using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Windows;

namespace VisumCRM.Client.Wpf.Internals
{
    public class FormSettingsClass
    {
        private Window _dialog;
        private DirectoryInfo _dir;

        public FormSettingsClass(Window Dialog)
        {
            _dialog = Dialog;
            _dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Emdesoft Developments\\VisumCRM");
            if (!_dir.Exists)
            {
                _dir.Create();
            }
        }

        public void Load()
        {
            if (_dialog.Tag == null) return;
            FileInfo file = new FileInfo(_dir.FullName + "\\" + _dialog.Tag.ToString() + ".json");
            if (file.Exists)
            {
                string content = "";
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    content = sr.ReadToEnd();
                    sr.Close();
                }
                var info = Newtonsoft.Json.JsonConvert.DeserializeObject<Internals.DialogInfo>(content);
                if (info != null)
                {
                    if (info.IsMaxed)
                    {
                        _dialog.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        if (info.Top < 0 || info.Left < 0)
                        {
                            info.Top = 50;
                            info.Left = 50;
                        }
                        _dialog.Top = info.Top;
                        _dialog.Left = info.Left;
                        _dialog.Width = info.Width;
                        _dialog.Height = info.Height;
                    }
                }
            }
        }
        public void Save()
        {
            if (_dialog.Tag == null) return;
            FileInfo file = new FileInfo(_dir.FullName + "\\" + _dialog.Tag.ToString() + ".json");
            var info = new Internals.DialogInfo()
            {
                Top = _dialog.Top,
                Left = _dialog.Left,
                Width = _dialog.Width,
                Height = _dialog.Height,
                IsMaxed = _dialog.WindowState == WindowState.Maximized,
            };
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            using (StreamWriter sw = new StreamWriter(file.FullName))
            {
                sw.Write(content);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
