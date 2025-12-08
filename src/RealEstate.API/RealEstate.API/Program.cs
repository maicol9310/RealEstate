using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add DbContext
builder.Services.AddDbContext<RealEstateDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register MediatR (scans assemblies)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(RealEstate.Application.Handlers.CreatePropertyHandler).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<RealEstate.Application.Mappings.ApplicationProfile>());

// DI: Repositories, UoW, Services
builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyRepository, RealEstate.Infrastructure.Repositories.PropertyRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IOwnerRepository, RealEstate.Infrastructure.Repositories.OwnerRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyImageRepository, RealEstate.Infrastructure.Repositories.PropertyImageRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IUnitOfWork, RealEstate.Infrastructure.UnitOfWork.UnitOfWork>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IFileService, RealEstate.Infrastructure.Services.LocalFileService>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
