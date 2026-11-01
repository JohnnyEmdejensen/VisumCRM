using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class EventHelper
    {
        public delegate void DefaultHandler(object? sender, EventArgs e);

        public event DefaultHandler? AuthenticationSuccess;
        public event DefaultHandler? AuthenticationCancled;

        public void RaiseAuthenticationSuccess()
        {
            AuthenticationSuccess?.Invoke( this, EventArgs.Empty);
        }   

        public void RaiseAuthenticationCancled()
        {
            AuthenticationCancled?.Invoke(this, EventArgs.Empty);
        }
    }
}
