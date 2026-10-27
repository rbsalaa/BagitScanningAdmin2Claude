// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SelfKOrderWeb.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SelfKOrderWeb.Data;

namespace SelfKOrderWeb.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<SelfKOrderWebUser> _userManager;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly SelfKOrderWebContext _dbContext;
        public LoginModel(SelfKOrderWebContext dbContext,SignInManager<SelfKOrderWebUser> signInManager, UserManager<SelfKOrderWebUser> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;  // <-- Initialize the injected _dbContext
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl ??= Url.Content("~/");
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        //    if (!ModelState.IsValid)
        //        return Page();

        //    var user = await _userManager.FindByNameAsync(Input.Email);

        //    if (user != null && await _userManager.CheckPasswordAsync(user, Input.Password))
        //    {
        //        var userPerm = await _dbContext.UserPermissions
        //            .FirstOrDefaultAsync(p => p.Id == user.Id);

        //        if (userPerm == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "No permissions assigned.");
        //            return Page();
        //        }

        //        var claims = new List<Claim>
        //{
        //    new Claim("CompID", userPerm.CompID.ToString()),
        //    new Claim("BrandID", userPerm.BrandID.ToString()),
        //    new Claim("FranchID", userPerm.FranchID.ToString()),
        //     new Claim("BranchID", userPerm.BranchID.ToString()),
        //    new Claim("IsRBSUser", userPerm.IsRBSUser.ToString().ToLower()),
        //    new Claim("IsSuperMainAdmin", userPerm.IsSuperMainAdmin.ToString().ToLower())

        //};

        //        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        //        var claims2 = principal.Claims;
        //        await _signInManager.SignInWithClaimsAsync(user, Input.RememberMe, claims);
        //        //await _signInManager.SignInWithClaimsAsync(user, Input.RememberMe, claims);
        //        //await _signInManager.SignInAsync(user, Input.RememberMe);
        //        _logger.LogInformation("User logged in with custom claims.");
        //        return LocalRedirect(returnUrl);
        //    }

        //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    return Page();
        //}
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
                return Page();

            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.Content("~/");

            var user = await _userManager.FindByNameAsync(Input.Email);
            if (user != null)
            {
                // Goes through SignInManager so failed attempts count towards Identity's lockout
                // (5 wrong passwords lock the account for 5 minutes) instead of allowing unlimited guesses.
                var check = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: true);
                if (check.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    ModelState.AddModelError(string.Empty, "Too many failed attempts. Please try again in a few minutes.");
                    return Page();
                }
                if (!check.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                // Load user permissions (custom table)
                var userPerm = await _dbContext.UserPermissions
                    .FirstOrDefaultAsync(p => p.Id == user.Id);

                if (userPerm == null)
                {
                    ModelState.AddModelError(string.Empty, "No permissions assigned.");
                    return Page();
                }

                // Build identity manually with standard and custom claims
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                identity.AddClaim(new Claim("CompID", userPerm.CompID.ToString()));
                identity.AddClaim(new Claim("BrandID", userPerm.BrandID.ToString()));
                identity.AddClaim(new Claim("FranchID", userPerm.FranchID.ToString()));
                identity.AddClaim(new Claim("BranchID", userPerm.BranchID.ToString()));
                identity.AddClaim(new Claim("IsRBSUser", userPerm.IsRBSUser.ToString().ToLower()));
                identity.AddClaim(new Claim("IsSuperMainAdmin", userPerm.IsSuperMainAdmin.ToString().ToLower()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });

                _logger.LogInformation("User logged in with custom claims.");
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

    }
}
