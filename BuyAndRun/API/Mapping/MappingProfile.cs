using System.Linq;
using API.Models.Auction;
using API.Models.Category;
using API.Models.Lot;
using API.Models.User;
using AutoMapper;
using BLL.Resources.Auction;
using BLL.Resources.Category;
using BLL.Resources.Identity.User;
using BLL.Resources.Lot;

namespace API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuctionResource, AuctionModel>()
                .ForMember(r => r.Moderators, m => m.MapFrom(m => m.Moderators.Select(u => u.UserName)));

            CreateMap<CategoryResource, CategoryModel>()
                .ForMember(r => r.StartedBy, m => m.MapFrom(m => m.StartedBy.UserName));

            CreateMap<UserResource, UserModel>()
                .ForMember(r => r.Roles, m => m.MapFrom(m => m.Roles.Select(r => r.Name)));

            CreateMap<CreateUserResource, CreateUserModel>()
                .ForMember(r => r.UserName, m => m.MapFrom(m => m.UserName))
                .ReverseMap();
            
            CreateMap<UserResource, WinnerUserModel>()
                .ReverseMap();

            CreateMap<LotResource, LotModel>()
                .ForMember(r => r.User, m => m.MapFrom(m => m.User));

            CreateMap<CreateLotResource, CreateLotModel>()
                .ForMember(r => r.User, m => m.MapFrom(m => m.User))
                .ReverseMap();
            
            CreateMap<CreateLotModel, CreateLotResource>()
                .ForMember(r => r.User, m => m.MapFrom(m => m.User))
                .ReverseMap();
            

            CreateMap<CreateCategoryResource, CreateCategoryModel>()
                .ForMember(r => r.StartedBy, m => m.MapFrom(m => m.StartedBy.UserName))
                .ReverseMap();

            CreateMap<UpdateCategoryResource, UpdateCategoryModel>()
                .ReverseMap();
        }
    }
}
