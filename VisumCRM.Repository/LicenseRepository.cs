using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class LicenseRepository
    {
        public delegate void LicenseEventHandler(object sender, LicenseEventArgs e);
        public event LicenseEventHandler? LicenseCreated;
        public event LicenseEventHandler? LicenseUpdated;
        public event LicenseEventHandler? LicenseDeleted;

        private Data.ApiContext _context;
        private Guid _license;
        public LicenseRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.License? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Licenses.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public Entity.License? GetByName(string Name)
        {
            if (_context == null) return null;
            return _context.Licenses.FirstOrDefault(x => x.Licensename == Name && !x.IsDeleted);
        }
        public Entity.License? Create(Entity.License? Source)
        {
            if (_context == null || Source == null) return null;
            _context.Licenses.Add(Source);
            _context.SaveChanges();
            LicenseCreated?.Invoke(this, new LicenseEventArgs(Source));
            return Source;
        }
        public Entity.License? Update(Entity.License? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Email = Source.Email;
            record.IsInactive = Source.IsInactive;
            record.LicenseKey = Source.LicenseKey;
            record.Licensename = Source.Licensename;
            record.Name = Source.Name;
            record.Phone = Source.Phone;
            record.Taxnumber = Source.Taxnumber;
            _context.SaveChanges();
            LicenseUpdated?.Invoke(this, new LicenseEventArgs(record));
            return record;
        }
    }
    public class LicenseEventArgs
    {
        public Entity.License? License { get; set; }
        public LicenseEventArgs(Entity.License? license)
        {
            License = license;
        }
    }
}
