using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MTA_AuthAllocationManagementCert.Models.CustomIdentityEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models.Claims
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("ContactName", user.LastName ?? "[Click to edit profile]"));

            return identity;
        }
    }
}
