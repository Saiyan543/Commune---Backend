using AutoMapper;
using Main.Slices.Accounts.EntityFramework_Jwt;
using Main.Slices.Accounts.Models.Dtos;
using Main.Slices.Discovery.Models.Dtos;

namespace Main.Global.Library.AutoMapper
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>().ReverseMap();
            CreateMap<UserForUpdateDto, User>().ReverseMap();
            CreateMap<UserAccountDto, User>().ReverseMap();
            CreateMap<ProfileDto, ProfileDb>().ReverseMap();
            CreateMap<ProfileView, ProfileDb>().ReverseMap();
        }
    }
}