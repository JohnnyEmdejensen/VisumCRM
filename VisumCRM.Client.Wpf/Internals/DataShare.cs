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

        public DataShare()
        {

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
                var countries = App.Repositories.CountryRepository.GetAll();
            }
        }
        public void GetPostalzones()
        {
            using (new Internals.WorkhandlerContainer("GetPostalzones"))
            {
                var postalzones = App.Repositories.PostalzoneRepository.GetAll();
            }
        }
        public void GetLocations()
        {
            using (new Internals.WorkhandlerContainer("GetLocations"))
            {
                var loctions = App.Repositories.LocationRepository.GetAll();
            }
        }
        public void GetCompanies()
        {
            using (new Internals.WorkhandlerContainer("GetCompanies"))
            {
                
            }
        }
        public void GetContactpersons()
        {
            using (new Internals.WorkhandlerContainer("GetContactpersons"))
            {

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
