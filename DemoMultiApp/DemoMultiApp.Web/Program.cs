using DemoMultiApp.Web.APIService.Interface;
using DemoMultiApp.Web.APIService.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.Cookie.HttpOnly = true;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
      options.Cookie.SameSite = SameSiteMode.Lax;
  });

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Account/Login";
    option.LogoutPath = "/Account/Login";
    option.AccessDeniedPath = "/";
    option.SlidingExpiration = true;
});

builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
