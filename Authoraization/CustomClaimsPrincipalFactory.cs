using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Data;
using System.Security.Claims;

namespace SelfKOrderWeb.Authoraization
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<SelfKOrderWebUser, ApplicationRole>
    {
        private readonly SelfKOrderWebContext _dbContext;

        public CustomClaimsPrincipalFactory(
            UserManager<SelfKOrderWebUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            SelfKOrderWebContext dbContext)
            : base(userManager, roleManager, optionsAccessor)
        {
            _dbContext = dbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(SelfKOrderWebUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var userPerm = await _dbContext.UserPermissions.FirstOrDefaultAsync(p => p.Id == user.Id);

            if (userPerm != null)
            {
                identity.AddClaim(new Claim("CompID", userPerm.CompID.ToString()));
                identity.AddClaim(new Claim("BrandID", userPerm.BrandID.ToString()));
                identity.AddClaim(new Claim("FranchID", userPerm.FranchID.ToString()));
                identity.AddClaim(new Claim("BranchID", userPerm.BranchID.ToString()));
                identity.AddClaim(new Claim("IsRBSUser", userPerm.IsRBSUser.ToString().ToLower()));
                identity.AddClaim(new Claim("IsSuperMainAdmin", userPerm.IsSuperMainAdmin.ToString().ToLower()));
            }

            return identity;
        }
    }
}
