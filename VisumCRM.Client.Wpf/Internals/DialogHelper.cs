using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class DialogHelper
    {
        public void ShowBranche(Entity.Branche? Branche)
        {
            Dialogs.BrancheDialog frm = new Dialogs.BrancheDialog()
            {
                DataContext = Branche,
            };
            frm.Show();
        }
        public void ShowCompany(Entity.Company? Company)
        {
            Dialogs.CompanyDialog frm = new Dialogs.CompanyDialog
            {
                DataContext = Company,
            };
            frm.Show();
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
    }
}
