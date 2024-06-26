using INTEX_AURORA_BRICKS.Data;
using INTEX_AURORA_BRICKS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Configuration;
using System.Net;

using Microsoft.ML.OnnxRuntime;

using System;




var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AuroraConnection");
builder.Services.AddDbContext<AuroraContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<Customers>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuroraContext>();

builder.Services.AddControllersWithViews();

// Add authentication services
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//// HTTPS Redirection
//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
//    options.HttpsPort = 443; // Set the HTTPS port your app is using
//});

//// HSTS?
//builder.Services.AddHsts(options =>
//{
//    options.Preload = true;
//    options.IncludeSubDomains = true;
//    options.MaxAge = TimeSpan.FromDays(60);
//    options.ExcludedHosts.Add("example.com");
//    options.ExcludedHosts.Add("www.example.com");
//});

// HSTS
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60); // Start with a low value for testing
                                            // Optionally exclude specific hosts
                                            // options.ExcludedHosts.Add("example.com");
});
// HTTPS REDIRECTION
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
});

services.AddDbContext<AuroraContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuroraConnection"));
});


services.AddDefaultIdentity<Customers>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 11;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 2;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<AuroraContext>();

// Used for cart sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDatabaseDeveloperPageExceptionFilter();
services.AddControllersWithViews();

services.AddSingleton<InferenceSession>(new InferenceSession("fraud_model.onnx"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("Customer"))
    {
        await roleManager.CreateAsync(new IdentityRole("Customer"));
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// CSP
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; " +
        "font-src 'self' https://fonts.googleapis.com https://fonts.gstatic.com; " +
        "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
        "script-src 'self' 'unsafe-inline' https://ajax.googleapis.com https://cdnjs.cloudflare.com https://cdn-cookieyes.com https://intex-4-7.azurewebsites.net; " +
        "img-src https: data:;" +
        "frame-src 'self' https://intex-4-7.azurewebsites.net; " +
        "base-uri 'self'; " +
        "form - action 'self' https://intex-4-7.azurewebsites.net/*" + // Allow form submissions to this specific URL
        "upgrade-insecure-requests; " +
        "connect-src 'self' https://log.cookieyes.com https://cdn-cookieyes.com");

    await next();
});








// For cart sessions
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
