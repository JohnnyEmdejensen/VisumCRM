using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class PostalzoneRepository
    {
        public delegate void PostalzoneEventHandler(object sender, PostalzoneEventArgs e);
        public event PostalzoneEventHandler? PostalzoneCreated;
        public event PostalzoneEventHandler? PostalzoneUpdated;
        public event PostalzoneEventHandler? PostalzoneDeleted;

        private Data.ApiContext _context;
        private Guid _license;
        public PostalzoneRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Postalzone? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Postalzones.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public Entity.Postalzone? GetByCountryZipcodeCityState(long CountryId, string Zipcode, string City, string? State = null)
        {
            if (_context == null) return null;
            if (State == null)
            {
                return _context.Postalzones.FirstOrDefault(x => !x.IsDeleted && x.LicenseKey == _license && x.CountryId == CountryId && x.Zipcode == Zipcode);
            }
            else
            {
                return _context.Postalzones.FirstOrDefault(x => !x.IsDeleted && x.LicenseKey == _license && x.CountryId == CountryId && x.Zipcode == Zipcode && x.State == State);
            }
        }
        public List<Entity.Postalzone>? GetAll()
        {
            if (_context == null) return null;
            return _context.Postalzones.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Postalzone? Create(Entity.Postalzone? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Postalzones.Add(Source);
            _context.SaveChanges();
            PostalzoneCreated?.Invoke(this, new PostalzoneEventArgs(Source));
            return Source;
        }

        public Entity.Postalzone? Update(Entity.Postalzone? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.City = Source.City;
            record.CountryId = Source.CountryId;
            record.State = Source.State;
            record.Zipcode = Source.Zipcode;
            _context.SaveChanges();
            PostalzoneUpdated?.Invoke(this, new PostalzoneEventArgs(record));
            return record;
        }
        public Entity.Postalzone? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            PostalzoneDeleted?.Invoke(this, new PostalzoneEventArgs(record));
            return record;
        }
    }

    public class PostalzoneEventArgs
    {
        public Entity.Postalzone? Postalzone { get; set; }
        public PostalzoneEventArgs(Entity.Postalzone? postalzone)
        {
            Postalzone = postalzone;
        }
    }
}
