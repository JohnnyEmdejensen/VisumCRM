using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class LicenseRepository
    {
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
        public Entity.License? Create(Entity.License? Source)
        {
            if (_context == null || Source == null) return null;
            _context.Licenses.Add(Source);
            _context.SaveChanges();
            return Source;
        }
    }
}
