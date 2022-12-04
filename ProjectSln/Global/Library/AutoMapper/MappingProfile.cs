using AutoMapper;
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.DbModels;
using Main.Slices.Accounts.Models.Dtos.In;
using Main.Slices.Accounts.Models.Dtos.Out;
using Main.Slices.Discovery.Models.Dtos.Db;
using Main.Slices.Discovery.Models.Dtos.Out;

namespace Main.Global.Library.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>().ReverseMap();
            CreateMap<UserForUpdateDto, User>().ReverseMap();
            CreateMap<UserAccountDto, User>().ReverseMap();
            CreateMap<ProfileDto, ProfileDbModel>().ReverseMap();
            CreateMap<SearchProfileDto, ProfileDbModel>().ReverseMap();
        }
    }
}