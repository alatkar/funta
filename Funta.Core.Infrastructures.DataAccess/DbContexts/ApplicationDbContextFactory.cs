using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Funta.Core.Infrastructures.DataAccess
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<AppContextDb>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AppContextDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppContextDb>();
            var connectionString = _configuration.GetConnectionString("connectionString");
            builder.UseSqlServer(connectionString);
            return new AppContextDb(builder.Options);
        }
    }
}
