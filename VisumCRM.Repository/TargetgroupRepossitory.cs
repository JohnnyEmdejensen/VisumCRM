using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class TargetgroupRepossitory
    {
        public delegate void TargetgroupEventHandler(object sender, TargetgroupEventArgs e);
        public event TargetgroupEventHandler? TargetgroupCreated;
        public event TargetgroupEventHandler? TargetgroupUpdated;
        public event TargetgroupEventHandler? TargetgroupDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public TargetgroupRepossitory(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Targetgroup? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Targetgroups.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Targetgroup>? GetAll()
        {
            if (_context == null) return null;
            return _context.Targetgroups.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Targetgroup? Create(Entity.Targetgroup? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Targetgroups.Add(Source);
            _context.SaveChanges();
            TargetgroupCreated?.Invoke(this, new TargetgroupEventArgs(Source));
            return Source;
        }
        public Entity.Targetgroup? Update(Entity.Targetgroup? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Key = Source.Key;
            record.Value = Source.Value;
            _context.SaveChanges();
            TargetgroupUpdated?.Invoke(this, new TargetgroupEventArgs(record));
            return record;
        }
        public Entity.Targetgroup? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            TargetgroupDeleted?.Invoke(this, new TargetgroupEventArgs(record));
            return record;
        }
    }

    public class TargetgroupEventArgs
    {
        public Entity.Targetgroup? Targetgroup { get; set; }
        public TargetgroupEventArgs(Entity.Targetgroup? targetgroup)
        {
            Targetgroup = targetgroup;
        }
    }
}
