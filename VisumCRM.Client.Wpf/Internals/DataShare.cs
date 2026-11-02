using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class DataShare
    {
        public List<Entity.Company>? CompanyCollection { get; set; }
        public List<Entity.Contactperson>? ContactpersonCollection { get; set; }
        public List<Entity.Country>? CountryCollection { get; set; }
        public List<Entity.Postalzone>? PostalzoneCollection { get; set; }
        public List<Entity.Location>? LocationCollection { get; set; }
        public List<Models.LocationModel>? LocationModelCollection { get; set; }

        public DataShare()
        {

        }

        private void BuildLocationModel()
        {
            if (CountryCollection == null || CountryCollection.Count == 0) return;
            if (PostalzoneCollection == null || PostalzoneCollection.Count == 0) return;
            if (LocationCollection == null || LocationCollection.Count == 0) return;
            using (new Internals.WorkhandlerContainer("BuildLocationModel"))
            {
                LocationModelCollection = new();
                foreach (var location in LocationCollection)
                {
                    Models.LocationModel model = new Models.LocationModel
                    {
                        Street = location.Street,
                        House = location.House,
                        Floor = location.Floor,
                        Door = location.Door,
                        PostalzoneId = location.PostalzoneId,
                    };
                    var postalzone = PostalzoneCollection.FirstOrDefault(x => x.Id == location.PostalzoneId);
                    if (postalzone != null)
                    {
                        model.City = postalzone.City;
                        model.Zipcode = postalzone.Zipcode;
                        model.State = postalzone.State;
                        model.CountryId = postalzone.CountryId;
                        var country = CountryCollection.FirstOrDefault(x => x.Id == postalzone.CountryId);
                        if (country != null)
                        {
                            model.CountryName = country.Name;
                            model.CountryNumber = country.Number;
                            model.CountryIsoCode = country.IsoCode;
                        }
                    }
                    model.SetAddress();
                    LocationModelCollection.Add(model);
                }
            }
            if (LocationModelCollection != null && LocationModelCollection.Count > 0)
            {
                LocationModelCollection = LocationModelCollection.OrderBy(x => x.CountryName).ThenBy(x => x.Zipcode).ThenBy(x => x.Street).ThenBy(x => x.House).ToList();
                App.EventHelper.RaiseLocationModelCollectionChangedEvent();
            }
        }
        public void GetAllData()
        {
            using (new Internals.WorkhandlerContainer("GetAllData"))
            {
                GetCountries();
                GetPostalzones();
                GetLocations();
                GetCompanies();
                GetContactpersons();
            }
        }
        public void GetCountries()
        {
            using (new Internals.WorkhandlerContainer("GetCountries"))
            {
                CountryCollection = App.Repositories.CountryRepository.GetAll();
                App.EventHelper.RaiseCountryCollectionChangedEvent();
            }
        }
        public void GetPostalzones()
        {
            using (new Internals.WorkhandlerContainer("GetPostalzones"))
            {
                PostalzoneCollection = App.Repositories.PostalzoneRepository.GetAll();
                App.EventHelper.RaisePostalzoneColloectionChangedEvent();
            }
        }
        public void GetLocations()
        {
            using (new Internals.WorkhandlerContainer("GetLocations"))
            {
                LocationCollection = App.Repositories.LocationRepository.GetAll();
                App.EventHelper.RaiseLocationCollectionChangedEvent();
            }
        }
        public void GetCompanies()
        {
            using (new Internals.WorkhandlerContainer("GetCompanies"))
            {
                CompanyCollection = App.Repositories.CompanyRepository.GetAll();
                App.EventHelper.RaiseCompanyCollectionChangedEvent();
            }
        }
        public void GetContactpersons()
        {
            using (new Internals.WorkhandlerContainer("GetContactpersons"))
            {
                ContactpersonCollection = App.Repositories.ContactpersonRepository.GetAll();
                App.EventHelper.RaiseContactpersonCollectionChangedEvent();
            }
        }

        public void ResumeEvents()
        {
            App.Repositories.CountryRepository.CountryCreated += CountryRepository_CountryCreated;
            App.Repositories.CountryRepository.CountryDeleted += CountryRepository_CountryDeleted;
            App.Repositories.CountryRepository.CountryUpdated += CountryRepository_CountryUpdated;
            App.Repositories.LocationRepository.LocationCreated += LocationRepository_LocationCreated;
            App.Repositories.LocationRepository.LocationDeleted += LocationRepository_LocationDeleted;
            App.Repositories.LocationRepository.LocationUpdated += LocationRepository_LocationUpdated;
            App.Repositories.PostalzoneRepository.PostalzoneCreated += PostalzoneRepository_PostalzoneCreated;
            App.Repositories.PostalzoneRepository.PostalzoneDeleted += PostalzoneRepository_PostalzoneDeleted;
            App.Repositories.PostalzoneRepository.PostalzoneUpdated += PostalzoneRepository_PostalzoneUpdated;
            App.Repositories.UserRepository.UserCreated += UserRepository_UserCreated;
            App.Repositories.UserRepository.UserDeleted += UserRepository_UserDeleted;
            App.Repositories.UserRepository.UserUpdated += UserRepository_UserUpdated;
            App.Repositories.CompanyRepository.CompanyCreated += CompanyRepository_CompanyCreated;
            App.Repositories.CompanyRepository.CompanyDeleted += CompanyRepository_CompanyDeleted;
            App.Repositories.CompanyRepository.CompanyUpdated += CompanyRepository_CompanyUpdated;
            App.Repositories.ContactpersonRepository.ContactpersonCreated += ContactpersonRepository_ContactpersonCreated;
            App.Repositories.ContactpersonRepository.ContactpersonDeleted += ContactpersonRepository_ContactpersonDeleted;
            App.Repositories.ContactpersonRepository.ContactpersonUpdated += ContactpersonRepository_ContactpersonUpdated;
            App.EventHelper.CountryCollectionChanged += EventHelper_CountryCollectionChanged;
            App.EventHelper.PostalzoneColloectionChanged += EventHelper_PostalzoneColloectionChanged;
            App.EventHelper.LocationCollectionChanged += EventHelper_LocationCollectionChanged;
            App.EventHelper.CompanyCollectionChanged += EventHelper_CompanyCollectionChanged;
            App.EventHelper.ContactpersonCollectionChanged += EventHelper_ContactpersonCollectionChanged;
        }

        private void EventHelper_ContactpersonCollectionChanged(object? sender, EventArgs e)
        {

        }

        private void EventHelper_CompanyCollectionChanged(object? sender, EventArgs e)
        {

        }

        private void EventHelper_LocationCollectionChanged(object? sender, EventArgs e)
        {
            BuildLocationModel();
        }

        private void EventHelper_PostalzoneColloectionChanged(object? sender, EventArgs e)
        {
            BuildLocationModel();
        }

        private void EventHelper_CountryCollectionChanged(object? sender, EventArgs e)
        {
            BuildLocationModel();
        }

        private void ContactpersonRepository_ContactpersonUpdated(object sender, Repository.ContactpersonEventArgs e)
        {

        }

        private void ContactpersonRepository_ContactpersonDeleted(object sender, Repository.ContactpersonEventArgs e)
        {

        }

        private void ContactpersonRepository_ContactpersonCreated(object sender, Repository.ContactpersonEventArgs e)
        {

        }

        private void CompanyRepository_CompanyUpdated(object sender, Repository.CompanyEventArgs e)
        {

        }

        private void CompanyRepository_CompanyDeleted(object sender, Repository.CompanyEventArgs e)
        {

        }

        private void CompanyRepository_CompanyCreated(object sender, Repository.CompanyEventArgs e)
        {

        }

        private void UserRepository_UserUpdated(object sender, Repository.UserEventArgs e)
        {
        }

        private void UserRepository_UserDeleted(object sender, Repository.UserEventArgs e)
        {

        }

        private void UserRepository_UserCreated(object sender, Repository.UserEventArgs e)
        {

        }

        private void PostalzoneRepository_PostalzoneUpdated(object sender, Repository.PostalzoneEventArgs e)
        {

        }

        private void PostalzoneRepository_PostalzoneDeleted(object sender, Repository.PostalzoneEventArgs e)
        {

        }

        private void PostalzoneRepository_PostalzoneCreated(object sender, Repository.PostalzoneEventArgs e)
        {

        }

        private void LocationRepository_LocationUpdated(object sender, Repository.LocationEventArgs e)
        {

        }

        private void LocationRepository_LocationDeleted(object sender, Repository.LocationEventArgs e)
        {

        }

        private void LocationRepository_LocationCreated(object sender, Repository.LocationEventArgs e)
        {

        }

        private void CountryRepository_CountryUpdated(object sender, Repository.CountryEventArgs e)
        {

        }

        private void CountryRepository_CountryDeleted(object sender, Repository.CountryEventArgs e)
        {

        }

        private void CountryRepository_CountryCreated(object sender, Repository.CountryEventArgs e)
        {

        }
    }
}
