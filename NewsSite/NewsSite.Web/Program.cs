using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Web.Data;
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
// AddLocalization Ч включает инфраструктуру локализации и указывает папку дл€ ресурсов.
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    // AddViewLocalization Ч позвол€ет использовать ресурсы в Razor-представлени€х.
    .AddViewLocalization()
    // AddDataAnnotationsLocalization Ч локализует сообщени€ валидации DataAnnotations.
    .AddDataAnnotationsLocalization();

var supportedCultures = new[] { "ru", "en" };
// RequestLocalizationOptions Ч список поддерживаемых культур и способ их выбора.
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var cultures = new[] { "ru", "en" }.Select(c => new CultureInfo(c)).ToList();
    opt.DefaultRequestCulture = new RequestCulture("ru");
    opt.SupportedCultures = cultures;
    opt.SupportedUICultures = cultures;

    // пор€док важен: сначала query, потом cookie
    opt.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider()
    };
});


var app = builder.Build();

// UseRequestLocalization Ч активирует middleware локализации.
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

using (var scope = app.Services.CreateScope())
    {
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAndAdmin(userManager, roleManager);
    }
