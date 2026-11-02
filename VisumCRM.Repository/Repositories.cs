using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Repository
{
    public class Repositories
    {
        private Data.ApiContext? _context;
        public Repositories(Guid LicenseKey)
        {
            Data.ApiContextFactory factory = new();
            _context = factory.CreateDbContext(null);

            UserRepository = new UserRepository(_context, LicenseKey);
            CountryRepository = new CountryRepository(_context, LicenseKey);
            PostalzoneRepository = new PostalzoneRepository(_context, LicenseKey);
            LocationRepository = new LocationRepository(_context, LicenseKey);
            LicenseRepository = new LicenseRepository(_context, LicenseKey);
            CompanyRepository = new CompanyRepository(_context, LicenseKey);
            ContactpersonRepository = new ContactpersonRepository(_context, LicenseKey);
            BrancheRepository = new BrancheRepository(_context, LicenseKey);
            TargetgroupRepossitory = new TargetgroupRepossitory(_context, LicenseKey);
        }

        public UserRepository UserRepository { get; private set; }
        public CountryRepository CountryRepository { get; private set; }
        public PostalzoneRepository PostalzoneRepository { get; private set; }
        public LocationRepository LocationRepository { get; private set; }
        public LicenseRepository LicenseRepository { get; private set; }
        public CompanyRepository CompanyRepository { get; private set; }
        public ContactpersonRepository ContactpersonRepository { get; private set; }
        public BrancheRepository BrancheRepository { get; set; }
        public TargetgroupRepossitory TargetgroupRepossitory { get; set; }
    }
}
