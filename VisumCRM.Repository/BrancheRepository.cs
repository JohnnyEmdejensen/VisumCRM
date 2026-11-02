using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class BrancheRepository
    {
        public delegate void BrancheEventHandler(object sender, BrancheEventArgs e);
        public event BrancheEventHandler? BrancheCreated;
        public event BrancheEventHandler? BrancheUpdate;
        public event BrancheEventHandler? BrancheDelete;
        private Data.ApiContext _context;
        private Guid _license;
        public BrancheRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Branche? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Branches.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Branche>? GetAll()
        {
            if (_context == null) return null;
            return _context.Branches.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Branche? Create(Entity.Branche? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Branches.Add(Source);
            _context.SaveChanges();
            BrancheCreated?.Invoke(this, new BrancheEventArgs(Source));
            return Source;
        }
        public Entity.Branche? Update(Entity.Branche? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Key = Source.Key;
            record.Value = Source.Value;
            _context.SaveChanges();
            BrancheUpdate?.Invoke(this, new BrancheEventArgs(record));
            return record;
        }
        public Entity.Branche? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            BrancheDelete?.Invoke(this, new BrancheEventArgs(record));
            return record;
        }
    }

    public class BrancheEventArgs
    {
        public Entity.Branche? Branche { get; set; }
        public BrancheEventArgs(Entity.Branche? branche)
        {
            Branche = branche;
        }
    }
}
