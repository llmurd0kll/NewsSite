using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Web.Infrastructure;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// DbContext
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();



// Localization
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var supportedCultures = new[] { "ru", "en" };
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var cultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    opt.DefaultRequestCulture = new RequestCulture("ru");
    opt.SupportedCultures = cultures;
    opt.SupportedUICultures = cultures;
    opt.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});

var app = builder.Build();

// Сидинг админа
await IdentitySeeder.SeedAsync(app.Services);

app.UseRequestLocalization();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
