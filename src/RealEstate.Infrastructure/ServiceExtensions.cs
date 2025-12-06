using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Interfaces;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Infrastructure.Services;

namespace RealEstate.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<RealEstateDbContext>(options => options.UseSqlServer(config.GetConnectionString("RealEstateDb")));
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, FileService>();
            return services;
        }
    }
}
