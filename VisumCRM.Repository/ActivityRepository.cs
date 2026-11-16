using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class ActivityRepository
    {
        public delegate void ActivityEventHandler(object source, ActivityEventArgs e);
        public event ActivityEventHandler? ActivityCreated;
        public event ActivityEventHandler? ActivityUpdated;
        public event ActivityEventHandler? ActivityDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public ActivityRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Activity? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Activities.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Activity>? GetAll()
        {
            if (_context == null) return null;
            return _context.Activities.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Activity? Create(Entity.Activity? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Activities.Add(Source);
            _context.SaveChanges();
            ActivityCreated?.Invoke(this, new ActivityEventArgs(Source));
            return Source;
        }
        public Entity.Activity? Update(Entity.Activity? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.ActivityEnd = Source.ActivityEnd;
            record.ActivityStart = Source.ActivityStart;
            record.ActivityType = Source.ActivityType;
            record.CompanyId = Source.CompanyId;
            record.Conclution = Source.Conclution;
            record.ContactpersonId = Source.ContactpersonId;
            record.FollowuptypeId = Source.FollowuptypeId;
            record.IsComplete = Source.IsComplete;
            record.LocationId = Source.LocationId;
            record.Reason = Source.Reason;
            record.SalespersonId = Source.SalespersonId;
            record.Subject = Source.Subject;
            record.TimeBilled = Source.TimeBilled;
            record.TimeUsed = Source.TimeUsed;
            _context.SaveChanges();
            ActivityUpdated?.Invoke(this, new ActivityEventArgs(record));
            return record;
        }
        public Entity.Activity? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            ActivityDeleted?.Invoke(this, new ActivityEventArgs(record));
            return record;
        }
    }
    public class ActivityEventArgs
    {
        public Entity.Activity Activity { get; set; }
        public ActivityEventArgs(Entity.Activity activity)
        {
            Activity = activity;
        }
    }
}
