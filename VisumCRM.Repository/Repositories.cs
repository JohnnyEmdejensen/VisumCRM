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
        }

        public UserRepository UserRepository { get; private set; }
    }
}
