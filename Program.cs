using IdentityCore.Factory.RepositoryBrandDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Authoraization.IHandler;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using System;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SelfKOrderWebContextConnection") ?? throw new InvalidOperationException("Connection string 'SelfKOrderWebContextConnection' not found.");

builder.Services.AddDbContext<SelfKOrderWebContext>(options =>
   options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<SelfKOrderWebUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<SelfKOrderWebContext>();
//builder.Services.AddScoped<IUserClaimsPrincipalFactory<SelfKOrderWebUser>, CustomClaimsPrincipalFactory>();
//builder.Services.AddScoped<IUserClaimsPrincipalFactory<SelfKOrderWebUser>, CustomClaimsPrincipalFactory>();


// Add services to the container.

builder.Services.AddAuthentication();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
}); // add session
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IDataAccessService, DataAccessService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddScoped<IItIInventoryemRepo, InventoryRepo>();
builder.Services.AddScoped<IAccessFilterService, AccessFilterService>();
builder.Services.AddControllersWithViews().AddJsonOptions(options => {

    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.ConfigureApplicationCookie(options =>

{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
   // options.SlidingExpiration = true;
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // There is no HomeController, so "/Home/Error" itself returned 404 and errors showed a blank page.
    app.UseExceptionHandler("/Dashboard/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.UseStatusCodePages(async context => {
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.Redirect("/Identity/Account/Login?returnUrl=" + request.Path);
        ///Identity/Account/Login?ReturnUrl
    }

});
app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    // Don't redirect for login, register, static files, API, or already authenticated
    if (!context.User.Identity.IsAuthenticated &&
        !path.StartsWithSegments("/Identity/Account/Login") &&
        !path.StartsWithSegments("/css") &&
        !path.StartsWithSegments("/js") &&
        !path.StartsWithSegments("/lib") &&
        !path.StartsWithSegments("/images") &&
        !path.StartsWithSegments("/favicon.ico"))
    {
        // Redirect to login with returnUrl
        var returnUrl = WebUtility.UrlEncode(path + context.Request.QueryString);
        context.Response.Redirect($"/Identity/Account/Login?returnUrl={returnUrl}");
        return;
    }

    await next();
});
app.Use(async (context, next) =>
{
    // The old header sent X-Frame-Options: ALLOW, which is not a valid value and was ignored by browsers,
    // so framing was effectively allowed. It is left out to keep that behaviour. If nothing embeds this
    // admin site in an iframe on another domain, add: context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    await next.Invoke();
});

app.MapControllerRoute(
    name: "default",
 pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
