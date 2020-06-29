using AutoMapper;
using BLL.Mapping.ValueResolvers.AuctionResolvers;
using BLL.Resources.Auction;
using BLL.Resources.Category;
using BLL.Resources.Identity.Role;
using BLL.Resources.Identity.User;
using BLL.Resources.Lot;
using DAL.Entities;
using DAL.Entities.Identity;

namespace BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, AuctionResource>()
                .ForMember(m => m.Id, f => f.MapFrom(f => f.AuctionId))
                .ForMember(m => m.LotsCount, f => f.MapFrom<LotsCountAuctionResolver>())
                .ReverseMap();
            CreateMap<CreateAuctionResource, Auction>();

            CreateMap<Category, CategoryResource>()
                .ForMember(m => m.Id, t => t.MapFrom(t => t.CategoryId))
                .ForMember(m => m.StartedBy, t => t.MapFrom(t => t.User))
                .ForMember(m => m.ParentAuction, t => t.MapFrom(t => t.Auction))
                .ReverseMap();
            CreateMap<CreateCategoryResource, Category>();

            CreateMap<Lot, LotResource>()
                .ForMember(m => m.Id, p => p.MapFrom(p => p.LotId))
                .ReverseMap();
            CreateMap<CreateLotResource, Lot>()
                .ForMember(m => m.User, p => p.MapFrom(p => p.User))
                .ReverseMap();

            CreateMap<User, UserResource>()
                .ReverseMap();
            CreateMap<CreateUserResource, User>();

            CreateMap<Role, RoleResource>();
            CreateMap<UserToRole, RoleResource>()
                .IncludeMembers(u => u.Role);

            CreateMap<AuctionToModerator, UserResource>()
                .IncludeMembers(u => u.User);
            CreateMap<AddModeratorToAuctionResource, AuctionToModerator>();
        }
    }
}
