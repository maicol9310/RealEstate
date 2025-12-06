using AutoMapper;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.API.Mapping
{
    public class DomainToContractProfile : Profile
    {
        public DomainToContractProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.IdProperty.ToString()))
                .ForMember(d => d.ImageFile, o => o.MapFrom(s => s.Images.FirstOrDefault() != null ? s.Images.First().File : null))
                .ForMember(d => d.Owner, o => o.MapFrom(s => s.Owner));

            CreateMap<PropertyImage, PropertyImageDto>()
                .ForMember(d => d.IdPropertyImage, o => o.MapFrom(s => s.IdPropertyImage.ToString()));
            CreateMap<PropertyTrace, PropertyTraceDto>()
                .ForMember(d => d.IdPropertyTrace, o => o.MapFrom(s => s.IdPropertyTrace.ToString()))
                .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.IdProperty.ToString()));
            CreateMap<Owner, OwnerDto>()
                .ForMember(d => d.IdOwner, o => o.MapFrom(s => s.IdOwner.ToString()));
        }
    }
}
