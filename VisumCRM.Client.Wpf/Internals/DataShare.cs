using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
        public List<Entity.Branche>? BrancheCollection { get; set; }
        public List<Entity.Targetgroup>? TargetgroupCollection { get; set; }
        public List<Entity.User>? UserCollection { get; set; }
        public List<Models.LocationModel>? LocationModelCollection { get; set; }
        public List<Models.KeyValueModel>? BrancheModelCollection { get; set; }
        public List<Models.KeyValueModel>? TargetgroupModelCollection { get; set; }
        public List<Models.CompanyModel>? CompanyModelCollection { get; set; }
        public List<Models.ContactpersonModel>? ContactpersonModelCollection { get; set; }
        public DataShare()
        {

        }

        private void BuildBrancheModel()
        {
            if (BrancheCollection == null || BrancheCollection.Count == 0) return;
            using (new Internals.WorkhandlerContainer("BuildBrancheModel"))
            {
                BrancheModelCollection = new();
                foreach (var branche in BrancheCollection)
                {
                    var b = new Models.KeyValueModel
                    {
                        Id = branche.Id,
                        Key = branche.Key,
                        Value = branche.Value
                    };
                    BrancheModelCollection.Add(b);
                }
                if (BrancheModelCollection != null && BrancheModelCollection.Count > 0)
                {
                    BrancheModelCollection = BrancheModelCollection.OrderBy(x => x.Value).ToList();
                }
            }
        }
        private void BuildTargetgroupModel()
        {
            if (TargetgroupCollection == null || TargetgroupCollection.Count == 0) return;
            using (new Internals.WorkhandlerContainer("BuildTargetgroupModel"))
            {
                TargetgroupModelCollection = new();
                foreach (var targetgroup in TargetgroupCollection)
                {
                    var t = new Models.KeyValueModel
                    {
                        Id = targetgroup.Id,
                        Key = targetgroup.Key,
                        Value = targetgroup.Value
                    };
                    TargetgroupModelCollection.Add(t);
                }
                if (TargetgroupModelCollection != null && TargetgroupModelCollection.Count > 0)
                {
                    TargetgroupModelCollection = TargetgroupModelCollection.OrderBy(x => x.Value).ToList();
                }
            }
        }
        private void BuildContactpersonModel()
        {
            if (ContactpersonCollection == null || ContactpersonCollection.Count == 0) return;
            using (new Internals.WorkhandlerContainer("BuildContactpersonModel"))
            {
                ContactpersonModelCollection = new();
                foreach (var contactperson in ContactpersonCollection)
                {
                    contactperson.SetFullname();
                    var c = new Models.ContactpersonModel
                    {
                        Birthday = contactperson.Birthday,
                        Email = contactperson.Email,
                        EmployeerId = contactperson.EmployeerId,
                        Firstname = contactperson.Firstname,
                        LocationId = contactperson.LocationId,
                        Fullname = contactperson.Fullname,
                        Middlename = contactperson.Middlename,
                        Mobile = contactperson.Mobile,
                        Phone = contactperson.Phone,
                        Picture = contactperson.Picture,
                        SalespersonId = contactperson.SalespersonId,
                        Surname = contactperson.Surname,
                        Id = contactperson.Id,
                    };
                    if (App.DataShare.LocationModelCollection != null)
                    {
                        var location = App.DataShare.LocationModelCollection.FirstOrDefault(x => x.Id == contactperson.LocationId);
                        if (location != null)
                        {
                            location.SetAddress();
                            c.Address = location.Address;
                            c.Zipcode = location.Zipcode;
                            c.City = location.City;
                            c.Country = location.CountryName;
                        }
                    }
                    if (App.DataShare.UserCollection != null)
                    {
                        var user = App.DataShare.UserCollection.FirstOrDefault(x => x.Id == contactperson.SalespersonId);
                        if (user != null)
                        {
                            user.SetFullname();
                            c.SalespersonName = user.Fullname;
                        }
                    }
                    if (App.DataShare.CompanyCollection != null)
                    {
                        var company = App.DataShare.CompanyCollection.FirstOrDefault(x => x.Id == contactperson.EmployeerId);
                        if (company != null)
                        {
                            c.EmployeeName = company.Name;
                        }
                    }
                    ContactpersonModelCollection.Add(c);
                }
                if (ContactpersonModelCollection != null)
                {
                    ContactpersonModelCollection = ContactpersonModelCollection.OrderBy(x => x.Fullname).ToList();                    
                }
            }
            App.EventHelper.RaiseContactpersonModelCollectionChangedEvent();
        }
        private void BuildCompanyModel()
        {
            if (CompanyCollection == null || CompanyCollection.Count == 0) return;

            using (new Internals.WorkhandlerContainer("BuildCompanyModel"))
            {
                CompanyModelCollection = new();
                foreach (var company in CompanyCollection)
                {
                    var c = new Models.CompanyModel
                    {
                        BrancheId = company.BrancheId,
                        Email = company.Email,
                        Id = company.Id,
                        LocationId = company.LocationId,
                        Logo = company.Logo,
                        Name = company.Name,
                        Nickname = company.Nickname,
                        Phone = company.Phone,
                        Number = company.Number,
                        Phone2 = company.Phone2,
                        SalespersonId = company.SalespersonId,
                        TargetgroupId = company.TargetgroupId,
                        Taxnumber = company.Taxnumber,
                        Website = company.Website,                        
                    };
                    if (App.DataShare.LocationModelCollection != null)
                    {
                        var location = App.DataShare.LocationModelCollection.FirstOrDefault(x => x.Id == company.LocationId);
                        if (location != null)
                        {
                            location.SetAddress();
                            c.Address = location.Address;
                            c.Zipcode = location.Zipcode;
                            c.City = location.City;
                            c.Country = location.CountryName;
                        }
                        CompanyModelCollection.Add(c);
                    }
                    if (App.DataShare.BrancheCollection != null && App.DataShare.BrancheCollection.Count > 0)
                    {
                        var branche = App.DataShare.BrancheCollection.FirstOrDefault(x => x.Id == company.BrancheId);
                        if (branche != null)
                        {
                            c.Branche = branche.Value;
                        }
                    }
                    if (App.DataShare.TargetgroupCollection != null && App.DataShare.TargetgroupCollection.Count > 0)
                    {
                        var targetgroup = App.DataShare.TargetgroupCollection.FirstOrDefault(x => x.Id == company.TargetgroupId);
                        if (targetgroup != null)
                        {
                            c.Targetgroup = targetgroup.Value;
                        }
                    }
                }
                if (CompanyModelCollection != null && CompanyModelCollection.Count > 0)
                {
                    CompanyModelCollection = CompanyModelCollection.OrderBy(x => x.Name).ToList();
                    App.EventHelper.RaiseCompanyModelCollectionChangedEvent();
                }
            }
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
                        Id = location.Id,
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
                GetUsers();
                GetBranches();
                GetTargetgroups();
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
        public void GetUsers()
        {
            using (new Internals.WorkhandlerContainer("GetUsers"))
            {
                UserCollection = App.Repositories.UserRepository.GetAll();
                App.EventHelper.RaiseUserCollectionChangedEvent();
            }
        }
        public void GetBranches()
        {
            using (new Internals.WorkhandlerContainer("GetBranches"))
            {
                var records = App.Repositories.BrancheRepository.GetAll();
                if (records != null && records.Count > 0)
                {
                    records = records.OrderBy(x => x.Key).ToList();
                }
                BrancheCollection = records;
                App.EventHelper.RaiseBrancheCollectionChangedEvent();
            }
        }

        public void GetTargetgroups()
        {
            using (new Internals.WorkhandlerContainer("GetTargetgroups"))
            {
                var records = App.Repositories.TargetgroupRepossitory.GetAll();
                if (records != null && records.Count > 0)
                {
                    records = records.OrderBy(x => x.Key).ToList();
                }
                TargetgroupCollection = records;
                App.EventHelper.RaiseTargetgroupCollectionChangedEvent();
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
            App.Repositories.BrancheRepository.BrancheUpdate += BrancheRepository_BrancheUpdate;
            App.Repositories.BrancheRepository.BrancheCreated += BrancheRepository_BrancheCreated;
            App.Repositories.BrancheRepository.BrancheDelete += BrancheRepository_BrancheDelete;
            App.Repositories.TargetgroupRepossitory.TargetgroupCreated += TargetgroupRepossitory_TargetgroupCreated;
            App.Repositories.TargetgroupRepossitory.TargetgroupDeleted += TargetgroupRepossitory_TargetgroupDeleted;
            App.Repositories.TargetgroupRepossitory.TargetgroupUpdated += TargetgroupRepossitory_TargetgroupUpdated;

            App.EventHelper.CountryCollectionChanged += EventHelper_CountryCollectionChanged;
            App.EventHelper.PostalzoneColloectionChanged += EventHelper_PostalzoneColloectionChanged;
            App.EventHelper.LocationCollectionChanged += EventHelper_LocationCollectionChanged;
            App.EventHelper.CompanyCollectionChanged += EventHelper_CompanyCollectionChanged;
            App.EventHelper.ContactpersonCollectionChanged += EventHelper_ContactpersonCollectionChanged;
            App.EventHelper.BrancheCollectionChanged += EventHelper_BrancheCollectionChanged;
            App.EventHelper.TargetgroupCollectionChanged += EventHelper_TargetgroupCollectionChanged;
            App.EventHelper.UserCollectionChanged += EventHelper_UserCollectionChanged;
        }

        private void EventHelper_UserCollectionChanged(object? sender, EventArgs e)
        {

        }

        private void EventHelper_TargetgroupCollectionChanged(object? sender, EventArgs e)
        {
            BuildTargetgroupModel();
        }

        private void EventHelper_BrancheCollectionChanged(object? sender, EventArgs e)
        {
            BuildBrancheModel();
        }

        private void TargetgroupRepossitory_TargetgroupUpdated(object sender, Repository.TargetgroupEventArgs e)
        {
            GetTargetgroups();
        }

        private void TargetgroupRepossitory_TargetgroupDeleted(object sender, Repository.TargetgroupEventArgs e)
        {
            GetTargetgroups();
        }

        private void TargetgroupRepossitory_TargetgroupCreated(object sender, Repository.TargetgroupEventArgs e)
        {
            GetTargetgroups();
        }

        private void BrancheRepository_BrancheDelete(object sender, Repository.BrancheEventArgs e)
        {
            GetBranches();
        }

        private void BrancheRepository_BrancheCreated(object sender, Repository.BrancheEventArgs e)
        {
            GetBranches();
        }

        private void BrancheRepository_BrancheUpdate(object sender, Repository.BrancheEventArgs e)
        {
            GetBranches();
        }

        private void EventHelper_ContactpersonCollectionChanged(object? sender, EventArgs e)
        {
            BuildContactpersonModel();
        }

        private void EventHelper_CompanyCollectionChanged(object? sender, EventArgs e)
        {
            BuildCompanyModel();
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
