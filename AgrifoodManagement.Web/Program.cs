using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util;
using AgrifoodManagement.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Terminal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Syncfusion.Licensing;
using AgrifoodManagement.Web.Controllers;

var builder = WebApplication.CreateBuilder(args);

SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJ8S0d+X1JPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9mSXxTcERnWn1deXBRRmI=;Mgo+DSMBMAY9C3t2XVhhQlJHfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTH5RdkNiXXxcc31RQ2Vb");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationMediatR();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<CartController>();

// Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    if (path == "/" || path == "/Home" || path == "/Home/Index")
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            var userType = context.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if (userType == "Seller")
            {
                context.Response.Redirect("/Producer/Announcements");
                return;
            }
            else if (userType == "Buyer")
            {
                context.Response.Redirect("/Consumer/Home");
                return;
            }
        }
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
