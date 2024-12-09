using AutoMapper;
using CantinaAPI.Dtos;
using CantinaAPI.Models;

namespace CantinaAPI.Mappings
{
    public class AutoMappersProfiles: Profile
    {
        public AutoMappersProfiles()
        {
            CreateMap<MenuItemRequestDto, MenuItemModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<MenuItemModel, MenuItemRequestDto>();
        }
    }
}
