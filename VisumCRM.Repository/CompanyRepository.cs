using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class CompanyRepository
    {
        public delegate void CompanyEventHandler(object sender, CompanyEventArgs e);
        public event CompanyEventHandler? CompanyCreated;
        public event CompanyEventHandler? CompanyUpdated;
        public event CompanyEventHandler? CompanyDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public CompanyRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Company? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Companies.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Company>? GetAll()
        {
            if (_context == null) return null;
            return _context.Companies.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }

        public Entity.Company? Create(Entity.Company? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Companies.Add(Source);
            _context.SaveChanges();
            CompanyCreated?.Invoke(this, new CompanyEventArgs(Source));
            return Source;
        }

        public Entity.Company? Update(Entity.Company? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Email = Source.Email;
            record.LocationId = Source.LocationId;
            record.Logo = Source.Logo;
            record.Name = Source.Name;
            record.Nickname = Source.Nickname;
            record.Number = Source.Number;
            record.Phone = Source.Phone;
            record.Phone2 = Source.Phone2;
            record.Taxnumber = Source.Taxnumber;
            record.Website = Source.Website;
            record.Founded = Source.Founded;
            record.Employees = Source.Employees;
            _context.SaveChanges();
            CompanyUpdated?.Invoke(this, new CompanyEventArgs(record));
            return record;
        }
        public Entity.Company? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            CompanyDeleted?.Invoke(this, new CompanyEventArgs(record));
            return record;
        }
    }

    public class CompanyEventArgs
    {
        public Entity.Company? Company { get; set; }
        public CompanyEventArgs(Entity.Company? company)
        {
            Company = company;
        }
    }
}
