using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASP.NETCoreIdentityNetGenDevTest.Areas.Identity.Data;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ASPNETCoreIdentityNetGenDevTestContextConnection") ?? throw new InvalidOperationException("Connection string 'ASPNETCoreIdentityNetGenDevTestContextConnection' not found.");

builder.Services.AddDbContext<ASPNETCoreIdentityNetGenDevTestContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ASPNETCoreIdentityNetGenDevTestContext>();

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();
#region Authorization
AddAuthorizationPolicy(builder.Services);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void AddAuthorizationPolicy(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Administrator"));
        options.AddPolicy("RequireReporter", policy => policy.RequireRole("Manager"));
    });
}