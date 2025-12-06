using AutoMapper;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(d => d.ImageFile, opt => opt.Ignore());

            CreateMap<PropertyImage, PropertyImageDto>();
            CreateMap<PropertyTrace, PropertyTraceDto>();
            CreateMap<Owner, OwnerDto>();
        }
    }
}