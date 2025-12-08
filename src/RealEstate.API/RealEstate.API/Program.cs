using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Persistence;
using Serilog;


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

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(RealEstate.Application.Handlers.CreatePropertyHandler).Assembly));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<RealEstate.Application.Mappings.ApplicationProfile>());

builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyRepository, RealEstate.Infrastructure.Repositories.PropertyRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IOwnerRepository, RealEstate.Infrastructure.Repositories.OwnerRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyImageRepository, RealEstate.Infrastructure.Repositories.PropertyImageRepository>();
builder.Services.AddScoped<RealEstate.Application.Interfaces.IPropertyTraceRepository, RealEstate.Infrastructure.Repositories.PropertyTraceRepository>();
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