using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class ContactpersonRepository
    {
        public delegate void ContactpersonEventHandler(object sender, ContactpersonEventArgs e);
        public event ContactpersonEventHandler? ContactpersonCreated;
        public event ContactpersonEventHandler? ContactpersonUpdated;
        public event ContactpersonEventHandler? ContactpersonDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public ContactpersonRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Contactperson? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Contactpersons.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Contactperson>? GetAll()
        {
            if (_context == null) return null;
            return _context.Contactpersons.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Contactperson? Create(Entity.Contactperson? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Contactpersons.Add(Source);
            _context.SaveChanges();
            ContactpersonCreated?.Invoke(this, new ContactpersonEventArgs(Source));
            return Source;
        }
        public Entity.Contactperson? Update(Entity.Contactperson? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Birthday = Source.Birthday;
            record.Email = Source.Email;
            record.Firstname = Source.Firstname;
            record.LocationId = Source.LocationId;
            record.Middlename = Source.Middlename;
            record.Mobile = Source.Mobile;
            record.Phone = Source.Phone;
            record.Picture = Source.Picture;
            record.Surname = Source.Surname;
            _context.SaveChanges();
            ContactpersonUpdated?.Invoke(this, new ContactpersonEventArgs(record));
            return record;
        }

        public Entity.Contactperson? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            ContactpersonDeleted?.Invoke(this, new ContactpersonEventArgs(record));
            return record;
        }
    }

    public class ContactpersonEventArgs
    {
        public Entity.Contactperson? Contactperson { get; set ;}
        public ContactpersonEventArgs(Entity.Contactperson? contactperson)
        {
            Contactperson = contactperson;
        }
    }
}
