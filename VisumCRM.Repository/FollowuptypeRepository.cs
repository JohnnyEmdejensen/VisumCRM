using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class FollowuptypeRepository
    {
        public delegate void FollowuptypeEventHandler(object sender, FollowuptypeEventArgs e);
        public event FollowuptypeEventHandler? FollowuptypeCreated;
        public event FollowuptypeEventHandler? FollowuptypeUpdated;
        public event FollowuptypeEventHandler? FollowuptypeDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public FollowuptypeRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Followuptype? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Followuptypes.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Followuptype>? GetAll()
        {
            if (_context == null) return null;
            return _context.Followuptypes.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Followuptype? Create(Entity.Followuptype? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Followuptypes.Add(Source);
            _context.SaveChanges();
            FollowuptypeCreated?.Invoke(this, new FollowuptypeEventArgs(Source));
            return Source;
        }
        public Entity.Followuptype? Update(Entity.Followuptype? Source)
        {            
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Key = Source.Key;
            record.Value = Source.Value;
            _context.SaveChanges();
            FollowuptypeUpdated?.Invoke(this, new FollowuptypeEventArgs(record));
            return record;
        }
        public Entity.Followuptype? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            FollowuptypeDeleted?.Invoke(this, new FollowuptypeEventArgs(record));
            return record;
        }
    }
    public class FollowuptypeEventArgs
    {
        public Entity.Followuptype Followuptype { get; set; }
        public FollowuptypeEventArgs(Entity.Followuptype followuptype)
        {
            Followuptype = followuptype;
        }
    }
}
