using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class WorkinglogRepository
    {
        private Data.ApiContext _context;
        private Guid _license;
        public WorkinglogRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Workinglog? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Workinglogs.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Workinglog>? GetAll()
        {
            if (_context == null) return null;
            return _context.Workinglogs.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public List<Entity.Workinglog>? GetAllUserLog(long UserId)
        {
            if (_context == null) return null;
            return _context.Workinglogs.Where(x => x.RecordId == UserId && !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public List<Entity.Workinglog>? GetAllCompanyLog(long CompanyId)
        {
            if (_context == null) return null;
            return _context.Workinglogs.Where(x => x.RecordId == CompanyId && !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public List<Entity.Workinglog>? GetAllContactpersonLog(long ContactpersonId)
        {
            if (_context == null) return null;
            return _context.Workinglogs.Where(x => x.RecordId == ContactpersonId && !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Workinglog? Create(Entity.Workinglog? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Workinglogs.Add(Source);
            _context.SaveChanges();
            return Source;
        }
        public Entity.Workinglog? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            return record;
        }

    }
}
