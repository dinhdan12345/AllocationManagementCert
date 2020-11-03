using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models.DBAuthContext
{
    public static class RegisterDatabase
    {
        public static void RegisterDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            //we are dealing with real databases
            services.AddDbContext<DbContextAuth>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
