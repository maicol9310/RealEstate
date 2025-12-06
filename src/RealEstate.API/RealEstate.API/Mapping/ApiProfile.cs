using AutoMapper;
using RealEstate.Domain.Entities;
using RealEstate.Contracts.DTOs;

namespace RealEstate.API.Mappings
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<Property, PropertyDto>();
            CreateMap<PropertyImage, PropertyImageDto>();
            CreateMap<PropertyTrace, PropertyTraceDto>();
            CreateMap<Owner, OwnerDto>();
        }
    }
}