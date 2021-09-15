using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;

namespace Model
{
    public class ApplicationUserClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole> 
    {

        public ApplicationUserClaimsFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("Fullname", user.Fullname));

            return identity; 


        }
    }
}
