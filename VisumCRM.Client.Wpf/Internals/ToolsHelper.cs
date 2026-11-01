using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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


    }

}
