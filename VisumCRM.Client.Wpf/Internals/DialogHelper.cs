using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace VisumCRM.Client.Wpf.Internals
{
    public class DialogHelper
    {

        public void ShowAppsettingsDialog()
        {
            Dialogs.AppSettingsDialog frm = new Dialogs.AppSettingsDialog()
            {

            };
            frm.Show();
        }

        public void ShowContactperson(Entity.Contactperson? Contactperson)
        {
            Dialogs.ContatcpersonDialog frm = new Dialogs.ContatcpersonDialog()
            {
                DataContext = Contactperson,
            };
            frm.Show();
        }
        public void ShowBranche(Entity.Branche? Branche)
        {
            Dialogs.BrancheDialog frm = new Dialogs.BrancheDialog()
            {
                DataContext = Branche,
            };
            frm.Show();
        }
        public void ShowJobtitle(Entity.Jobtitle? Jobtitle)
        {
            Dialogs.JobtitleDialog frm = new Dialogs.JobtitleDialog()
            {
                DataContext = Jobtitle,
            };
            frm.Show();
        }
        public void ShowCompany(Entity.Company? Company)
        {
            if (Company != null)
            {
                Dialogs.CompanyDialog dialog = new();
                if (!CompanyAlreadyOpen(Company))
                {
                    Dialogs.CompanyDialog frm = new Dialogs.CompanyDialog
                    {
                        DataContext = Company,
                    };
                    frm.Show();
                }
            }
            else
            {
                Dialogs.CompanyDialog frm = new Dialogs.CompanyDialog
                {
                    DataContext = Company,
                };
                frm.Show();
            }
        }
        public void ShowAuthenticate()
        {
            Dialogs.AuthenticateDialog frm = new Dialogs.AuthenticateDialog();
            frm.Show();
        }
        public void ShowUser(Entity.User? User)
        {
            Dialogs.UserDialog frm = new Dialogs.UserDialog
            {
                DataContext = User,
            };
            frm.Show();
        }
        private bool CompanyAlreadyOpen(Entity.Company Company)
        {
            bool result = false;
            foreach (var dialog in App.Current.Windows)
            {
                if (dialog is Dialogs.CompanyDialog)
                {
                    var form = dialog as Dialogs.CompanyDialog;
                    if (form != null)
                    {
                        var company = form.DataContext as Entity.Company;
                        if (company != null && company.Id == Company.Id)
                        {
                            result = true;
                            form.Topmost = true;
                            Thread.Sleep(500);
                            form.Topmost = false;
                        }
                    }
                }
            }
            return result;
        }
    }
}
