using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class UserRepository
    {
        private Data.ApiContext _context;
        private Guid _license;
        public UserRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.User? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Users.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public Entity.User? GetByLogin(string Login)
        {
            if (_context == null) return null;
            return _context.Users.FirstOrDefault(x => x.Login == Login && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.User>? GetAll()
        {
            if (_context == null) return null;
            return _context.Users.Where(x => !x.IsDeleted && x.LicenseKey == _license && x.SecurityLevel != 999).ToList();
        }
        public Entity.User? Create(Entity.User? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Users.Add(Source);
            _context.SaveChanges();
            return Source;
        }
        public Entity.User? Update(Entity.User? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Firstname = Source.Firstname;
            record.Middlename = Source.Middlename;
            record.Surname = Source.Surname;
            record.Phone = Source.Phone;
            record.Mobile = Source.Mobile;
            record.Email = Source.Email;
            record.LocationId = Source.LocationId;
            record.Picture = Source.Picture;
            record.Birthday = Source.Birthday;
            record.Login = Source.Login;
            record.Password = Source.Password;
            record.LastVisit = Source.LastVisit;
            record.TimeUsed = Source.TimeUsed;
            record.SecurityLevel = Source.SecurityLevel;
            record.IsInactive = Source.IsInactive;
            _context.SaveChanges();
            return record;
        }
        public Entity.User? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            return record;
        }
    }
}
