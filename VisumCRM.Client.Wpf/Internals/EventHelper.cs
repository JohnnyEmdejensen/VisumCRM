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
        public event DefaultHandler? CountryCollectionChanged;
        public event DefaultHandler? PostalzoneColloectionChanged;
        public event DefaultHandler? LocationCollectionChanged;
        public event DefaultHandler? CompanyCollectionChanged;
        public event DefaultHandler? ContactpersonCollectionChanged;
        public event DefaultHandler? LocationModelCollectionChanged;
        public event DefaultHandler? BrancheCollectionChanged;
        public event DefaultHandler? TargetgroupCollectionChanged;
        public event DefaultHandler? UserCollectionChanged;
        public event DefaultHandler? CompanyModelCollectionChanged;
        public event DefaultHandler? CurrentCompanyChanged;

        public void RaiseCurrentCompanyChangedEvent()
        {
            CurrentCompanyChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseCompanyModelCollectionChangedEvent()
        {
            CompanyModelCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseUserCollectionChangedEvent()
        {
            UserCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseTargetgroupCollectionChangedEvent()
        {
            TargetgroupCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseBrancheCollectionChangedEvent()
        {
            BrancheCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseLocationModelCollectionChangedEvent()
        {
            LocationModelCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseContactpersonCollectionChangedEvent()
        {
            ContactpersonCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseCompanyCollectionChangedEvent()
        {
            CompanyCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseLocationCollectionChangedEvent()
        {
            LocationCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaisePostalzoneColloectionChangedEvent()
        {
            PostalzoneColloectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseCountryCollectionChangedEvent()
        {
            CountryCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseAuthenticationSuccess()
        {
            AuthenticationSuccess?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseAuthenticationCancled()
        {
            AuthenticationCancled?.Invoke(this, EventArgs.Empty);
        }
    }
}
