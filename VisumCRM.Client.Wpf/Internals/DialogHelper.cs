using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class DialogHelper
    {
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
