using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection;
using DemoMultiApp.Data.Model;
using DemoMultiApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DemoMultiApp.Core.Interface;
using DemoMultiApp.Core.Reposiotry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Connection String
builder.Services.AddDbContext<DemoDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add Identity Options
builder.Services.AddIdentity<UserModel, RoleModel>().AddRoles<RoleModel>().AddEntityFrameworkStores<DemoDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireNonAlphanumeric = false;
});

// Configurar JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//Interface/Repository contract
builder.Services.AddScoped<IFunctionRepository, FunctionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Add AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

//Use Cors
app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//Add Migration
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DemoDbContext>();
context.Database.Migrate();

//User/Role Seeders
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();
    var roles = new[] { "System", "Supervisor", "Operator" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            RoleModel rolemodel = new RoleModel();
            rolemodel.Name = role;
            await roleManager.CreateAsync(rolemodel);
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();

    string userName = "System";
    string password = "Temporary01";
    string roleName = "System";
    if (await userManager.FindByNameAsync(userName) == null)
    {
        UserModel user = new UserModel();
        user.UserName = userName;
        user.IsActive = true;
        var a = await userManager.CreateAsync(user, password);
        if (a.Succeeded)
        {
            var b = await userManager.AddToRoleAsync(user, roleName);
            if (!a.Succeeded)
            {
                await userManager.DeleteAsync(user);
            }
        }
    }
}

app.Run();


