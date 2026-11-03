using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VisumCRM.Client.Wpf.Models
{
    public class LocationModel
    {
        public long Id { get; set; }
        public string Street { get; set; } = "";
        public string House { get; set; } = "";
        public string? Floor { get; set; }
        public string? Door { get; set; }
        public long PostalzoneId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string? Address { get; set; }

        public string City { get; set; } = "";
        public string Zipcode { get; set; } = "";
        public string? State { get; set; }
        public long CountryId { get; set; }

        public string CountryName { get; set; } = "";
        public string CountryIsoCode { get; set; } = "";
        public int CountryNumber { get; set; }
        public byte[]? Flag { get; set; }


        public void SetAddress()
        {
            var a = Street + " " + House;
            if (!string.IsNullOrEmpty(Floor))
                a += ", " + Floor + ".";
            if (!string.IsNullOrEmpty(Door))
            {
                a += " " + Door + ".";
            }
            else
            {
                if (!string.IsNullOrEmpty(Floor))
                    a += " " + App.Current.Resources["text.translate.floor"].ToString();
            }
            Address = a;
        }

    }
}
