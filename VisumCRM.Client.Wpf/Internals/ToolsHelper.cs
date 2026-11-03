using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace VisumCRM.Client.Wpf.Internals
{
    public class ToolsHelper
    {
        public long? ParseLocation(string Countryname, string Zipcode, string City, string Street, string House, string? Floor = null, string? Door = null, string? State = null)
        {
            long? result = null;
            var Country = App.Repositories.CountryRepository.GetCountryByName(Countryname);
            if (Country == null)
            {
                Country = new()
                {
                    Name = Countryname,
                    IsoCode = "",
                    Number = 0,                    
                };
                Country = App.Repositories.CountryRepository.Create(Country);
            }
            if (Country != null)
            {
                var postalzone = App.Repositories.PostalzoneRepository.GetByCountryZipcodeCityState(Country.Id, Zipcode, City, State);
                if (postalzone == null)
                {
                    postalzone = new()
                    {
                        CountryId = Country.Id,
                        Zipcode = Zipcode,
                        City = City,
                        State = State
                    };
                    postalzone = App.Repositories.PostalzoneRepository.Create(postalzone);
                }
                if (postalzone != null)
                {
                    var location = App.Repositories.LocationRepository.GetByPostalzoneStreetHouse(postalzone.Id, Street, House, Floor, Door);
                    if (location == null)
                    {
                        location = new()
                        {
                            PostalzoneId = postalzone.Id,
                            Street = Street,
                            House = House,
                            Floor = Floor,
                            Door = Door
                        };
                        location = App.Repositories.LocationRepository.Create(location);
                    }
                    if (location != null)
                    {
                        result = location.Id;
                    }
                }
            }
            return result;
        }

        public void FillSalespersonControl(ComboBox Ctrl)
        {
            Ctrl.BeginInit();
            if (App.DataShare.UserCollection != null && App.DataShare.UserCollection.Count > 0)
                Ctrl.ItemsSource = App.DataShare.UserCollection.OrderBy(x => x.Fullname).ToList();
            else
                Ctrl.ItemsSource = App.DataShare.UserCollection;
            Ctrl.DisplayMemberPath = "Fullname";
            Ctrl.SelectedValuePath = "Id";
            Ctrl.EndInit();
        }
        public void FillCountryControl(ComboBox Ctrl)
        {
            Ctrl.BeginInit();
            if (App.DataShare.CountryCollection != null && App.DataShare.CountryCollection.Count > 0)
                Ctrl.ItemsSource = App.DataShare.CountryCollection.OrderBy(x => x.Name).ToList();
            else
                Ctrl.ItemsSource = App.DataShare.CountryCollection;
            Ctrl.DisplayMemberPath = "Name";
            Ctrl.SelectedValuePath = "Id";
            Ctrl.EndInit();
        }
        public void FillTargetgroupControl(ComboBox Ctrl)
        {
            Ctrl.BeginInit();
            if (App.DataShare.TargetgroupCollection != null && App.DataShare.TargetgroupCollection.Count > 0)
                Ctrl.ItemsSource = App.DataShare.TargetgroupCollection.OrderBy(x => x.Value).ToList();
            else
                Ctrl.ItemsSource = App.DataShare.TargetgroupCollection;
            Ctrl.DisplayMemberPath = "Value";
            Ctrl.SelectedValuePath = "Id";
            Ctrl.EndInit();
        }
    }

}
