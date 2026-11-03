using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class UserRepository
    {
        public delegate void UserEventHandler(object sender, UserEventArgs e);
        public event UserEventHandler? UserCreated;
        public event UserEventHandler? UserUpdated;
        public event UserEventHandler? UserDeleted;

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
        public List<Entity.User>? GetAllSpecial()
        {
            if (_context == null) return null;
            var records = _context.Users.Where(x => !x.IsDeleted && x.LicenseKey == _license).Select(c => new { c.Id, c.Firstname, c.Middlename, c.Surname, c.Birthday, c.Email, c.LastVisit, c.LocationId, c.Mobile, c.Phone, c.Picture, c.SecurityLevel, c.TimeUsed });

            List<Entity.User> list = new();
            foreach (var item in records)
            {
                Entity.User user = new Entity.User
                {
                    Id = item.Id,
                    Birthday = item.Birthday,
                    Email = item.Email,
                    Firstname = item.Firstname,
                    LastVisit = item.LastVisit,
                    LocationId = item.LocationId,
                    Middlename = item.Middlename,
                    Mobile = item.Mobile,
                    Phone = item.Phone,
                    Picture = item.Picture,
                    SecurityLevel = item.SecurityLevel,
                    Surname = item.Surname,
                    TimeUsed = item.TimeUsed
                };
                list.Add(user);
            }
            return list;
        }
        public Entity.User? Create(Entity.User? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Users.Add(Source);
            _context.SaveChanges();
            UserCreated?.Invoke(this, new UserEventArgs(Source));
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
            UserUpdated?.Invoke(this, new UserEventArgs(record));
            return record;
        }
        public Entity.User? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            UserDeleted?.Invoke(this, new UserEventArgs(record));
            return record;
        }
    }

    public class UserEventArgs
    {
        public Entity.User? User { get; set; }
        public UserEventArgs(Entity.User? user)
        {
            User = user;
        }
    }
}
