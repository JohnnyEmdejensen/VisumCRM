using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class CountryRepository
    {
        private Data.ApiContext _context;
        private Guid _license;
        public CountryRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Country? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Countries.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public Entity.Country? GetCountryByName(string Name)
        {
            if (_context == null) return null;
            var country = _context.Countries.FirstOrDefault(x => x.Name == Name && !x.IsDeleted && x.LicenseKey == _license);
            return country;
        }
        public List<Entity.Country>? GetAll()
        {
            if (_context == null) return null;
            return _context.Countries.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Country? Create(Entity.Country? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Countries.Add(Source);
            _context.SaveChanges();
            return Source;
        }
        public Entity.Country? Update(Entity.Country? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Flag = Source.Flag;
            record.IsoCode = Source.IsoCode;
            record.Name = Source.Name;
            record.Number = Source.Number;
            _context.SaveChanges();
            return record;
        }
        public Entity.Country? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            return record;
        }
    }
}
