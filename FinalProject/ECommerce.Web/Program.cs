using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Core;
using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Users;
using ECommerce.Fascet;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Profiles;
using ECommerce.Membership;
using ECommerce.Membership.Services;
using ECommerce.Utility;
using ECommerce.Utility.ChatHub;
using ECommerce.Web;
using ECommerce.Web.Areas.Vendor.Profiles;
using ECommerce.Web.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var assemblyName = Assembly.GetExecutingAssembly().FullName;


// AppSettings Configuration
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();



#region SerilogConfiguration
// Serilog Configuration
builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration));
#endregion



#region AutofacDependencyConfiguration 

//Autofac Dependency Configure

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WebModule(configuration));

    //Added MembershipModule
    containerBuilder.RegisterModule(new InfrastructureModule(configuration["ConnectionStrings:DefaultConnection"], assemblyName));
    containerBuilder.RegisterModule(new CoreModule(configuration["ConnectionStrings:DefaultConnection"], assemblyName));
    containerBuilder.RegisterModule(new MembershipModule());
    containerBuilder.RegisterModule(new FascetModule());

});
#endregion



// Add services to the container.


#region DbContextBindings
builder.Services.AddDbContext<CoreDbContext>(options =>
    options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"],
                                                m => m.MigrationsAssembly(assemblyName)));
#endregion 


builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<StoreSubDomainChecker>();

//Automapper Configuration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<InfrastructureProfile>();
    cfg.AddProfile<VendorProfile>();
    cfg.AddProfile<WebProfile>();
});



#region Configuring Identity
//Configuring Membership Services
builder.Services
    .AddIdentity<ApplicationUser, Role>()
    .AddEntityFrameworkStores<CoreDbContext>()
    .AddUserManager<UserManager>()
    .AddRoleManager<RoleManager>()
    .AddSignInManager<SignInManager>()
    .AddDefaultTokenProviders();

//Configuring IdentityOption which replace Default Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // SignIn settings
    options.SignIn.RequireConfirmedAccount = false;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

#endregion

#region External Login Configuration
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = builder.Configuration["App:GoogleClientId"];
    option.ClientSecret = builder.Configuration["App:GoogleClientSecret"];
});
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.ClientId = builder.Configuration["App:FacebookClientId"];
    option.ClientSecret = builder.Configuration["App:FacebookClientSecret"];
});
#endregion

#region Authorization Configuration
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("MerchantPolicy", policy =>
    //{
    //    policy.RequireAuthenticatedUser();
    //    policy.RequireRole("Vendor");
    //    policy.RequireRole("Distributor");
    //});
});
#endregion


builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, MyCustomProvider>();

try
{
    var app = builder.Build();

    Log.Information("Application starting up");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
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


    app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ChatHub>("/messagehub");
    });

    //app.MapRazorPages(); this must be commented out.

    app.Run();
}
catch (Exception ex)
{

    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
