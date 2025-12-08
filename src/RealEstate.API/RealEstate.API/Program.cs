using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application;
using RealEstate.Infrastructure.Persistence;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<RealEstateDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<RealEstate.Application.Mappings.ApplicationProfile>());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyRepository, RealEstate.Infrastructure.Repositories.PropertyRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IOwnerRepository, RealEstate.Infrastructure.Repositories.OwnerRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyImageRepository, RealEstate.Infrastructure.Repositories.PropertyImageRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyTraceRepository, RealEstate.Infrastructure.Repositories.PropertyTraceRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IUserRepository, RealEstate.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IUnitOfWork, RealEstate.Infrastructure.UnitOfWork.UnitOfWork>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IFileService, RealEstate.Infrastructure.Services.LocalFileService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();