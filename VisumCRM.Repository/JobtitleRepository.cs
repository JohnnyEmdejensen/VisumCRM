using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class JobtitleRepository
    {
        public delegate void JobtitleEventHandler(object sender, JobtitleEventArgs e);
        public event JobtitleEventHandler JobtitleCreated;
        public event JobtitleEventHandler JobtitleUpdated;
        public event JobtitleEventHandler JobtitleDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public JobtitleRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Jobtitle? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Jobtitles.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Jobtitle>? GetAll()
        {
            if (_context == null) return null;
            return _context.Jobtitles.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Jobtitle? Create(Entity.Jobtitle? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Jobtitles.Add(Source);
            _context.SaveChanges();
            JobtitleCreated?.Invoke(this, new JobtitleEventArgs(Source));
            return Source;
        }
        public Entity.Jobtitle? Update(Entity.Jobtitle? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Key = Source.Key;
            record.Value = Source.Value;
            _context.SaveChanges();
            JobtitleUpdated.Invoke(this, new JobtitleEventArgs(record));
            return record;
        }
        public Entity.Jobtitle? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            JobtitleDeleted?.Invoke(this, new JobtitleEventArgs(record));
            return record;
        }
    }

    public class JobtitleEventArgs
    {
        public Entity.Jobtitle Jobtitle;
        public JobtitleEventArgs(Entity.Jobtitle jobtitle)
        {
            Jobtitle = jobtitle;
        }
    }
}
