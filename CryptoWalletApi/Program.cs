using System.Reflection;
using System.Text;
using CryptoWalletApi;
using CryptoWalletApi.Authorization;
using CryptoWalletApi.Entities;
using CryptoWalletApi.Middlewares;
using CryptoWalletApi.Models.Validators;
using CryptoWalletApi.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("JwtBearer").Bind(authenticationSettings);
// Add services to the container.

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPasswordHasher<Wallet>, PasswordHasher<Wallet>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ErrorHandlingMiddleware, ErrorHandlingMiddleware>();

builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = authenticationSettings.Issuer,
        ValidateIssuer = true,
        ValidIssuer = authenticationSettings.Issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Key)),
    };
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("EntityOwner", builder => builder.AddRequirements(new EntityOwnerRequirement()));
//});
builder.Services.AddScoped<IAuthorizationHandler, EntityOwnerRequirementHandler>();

builder.Services.AddDbContext<CryptoWalletDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CryptoWalletConnectionString"));
});
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<CryptoWalletDbContext>();

if (!dbContext.Roles.Any())
{
    var roleSeeder = new Seeder(dbContext);
    await roleSeeder.SeedRoles();
}
if (!dbContext.Currencies.Any())
{
    var currSeeder = new Seeder(dbContext);
    await currSeeder.SeedCurrencies();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cfg =>
{
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
    cfg.AllowAnyOrigin();
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
