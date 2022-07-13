using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// add EntityFramework context
builder.Services.AddDbContext<SqlServerContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("data"))
);

// Add Configs for Identity Server
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SqlServerContext>()
    .AddDefaultTokenProviders();

var identityBuilder = builder.Services.AddIdentityServer(opt =>
    {
        opt.Events.RaiseErrorEvents = true;
        opt.Events.RaiseInformationEvents = true;
        opt.Events.RaiseFailureEvents = true;
        opt.Events.RaiseSuccessEvents = true;
        opt.EmitStaticAudienceClaim = true;
    }).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
      .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
      .AddInMemoryClients(IdentityConfiguration.Clients)
      .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

identityBuilder.AddDeveloperSigningCredential();
/////////////////////////////////////////////////

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

using (var serviceScope = app.Services.CreateScope()) 
{
    var service = serviceScope.ServiceProvider.GetService<IDbInitializer>();
    service.Initialize();
}

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
