using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add httpClient
builder.Services.AddHttpClient<IProductService, ProductService>(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])
    );

//authentication configuring
builder.Services.AddAuthentication(opt => 
{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", opt => 
    {
        opt.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
        opt.GetClaimsFromUserInfoEndpoint = true;
        opt.ClientId = "geek_shopping";
        opt.ClientSecret = "my_secret";
        opt.ResponseType = "code";
        opt.ClaimActions.MapJsonKey("role", "role", "role");
        opt.ClaimActions.MapJsonKey("sub", "sub", "sub");
        opt.TokenValidationParameters.NameClaimType = "name";
        opt.TokenValidationParameters.RoleClaimType = "name";
        opt.Scope.Add("geek_shopping");
        opt.SaveTokens = true;
    });

//depenency injection
//builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
