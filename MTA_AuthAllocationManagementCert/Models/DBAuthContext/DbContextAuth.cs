using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MTA_AuthAllocationManagementCert.Models.CustomIdentityEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models.DBAuthContext
{
    public class DbContextAuth : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbContextAuth(DbContextOptions<DbContextAuth> options) : base(options)
        {

        }
    }
}
