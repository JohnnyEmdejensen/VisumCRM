using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VisumCRM.Data
{
    public class ApiContextFactory : IDesignTimeDbContextFactory<ApiContext>
    {
        public ApiContext CreateDbContext(string[]? args)
        {
            string connectionstring = "Server=localhost\\SQLEXPRESS;Database=VisumCRM;Trusted_Connection=True;TrustServerCertificate=True;";
            var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            return new ApiContext(optionsBuilder.Options);
        }
    }
}
