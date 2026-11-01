using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class PostalzoneRepository
    {
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
            return record;
        }
        public Entity.Postalzone? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            return record;
        }
    }
}
