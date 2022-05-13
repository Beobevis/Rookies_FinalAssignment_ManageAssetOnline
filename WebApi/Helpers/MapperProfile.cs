using AutoMapper;
using WebApi.Models.Assets;
using WebApi.Models.Assignments;
using WebApi.Models.Categories;
using WebApi.Entities;
using WebApi.Models.ReturnRequest;

namespace WebApi.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Asset, AssetCreateModel>();
            CreateMap<AssetCreateModel, Asset>();
            CreateMap<Asset, AssetUpdateModel>();
            CreateMap<AssetUpdateModel, Asset>();
            CreateMap<Asset, AssetModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));

            CreateMap<AssetModel, Asset>();
            CreateMap<Assignment, AssignmentCreateModel>();
            CreateMap<AssignmentCreateModel, Assignment>();
            CreateMap<Assignment, AssignmentUpdateModel>();
            CreateMap<AssignmentUpdateModel, Assignment>();
            CreateMap<HistoricalAssignmentModel, Assignment>();
            CreateMap<Assignment, HistoricalAssignmentModel>()
            .ForMember(dest => dest.AssignTo, opt => opt.MapFrom(src => src.AssignTo.Username))
            .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedBy.Username));

            CreateMap<Assignment, AssignmentModel>()
            .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => (src.Asset != null) ? src.Asset.AssetCode : null))
            .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => (src.Asset != null) ? src.Asset.AssetName : null))
            .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => (src.Asset != null) ? src.Asset.Specification : null))
            .ForMember(dest => dest.AssignTo, opt => opt.MapFrom(src => src.AssignTo.Username))
            .ForMember(dest => dest.AssignToFirstname, opt => opt.MapFrom(src => src.AssignTo.Firstname))
            .ForMember(dest => dest.AssignToLastname, opt => opt.MapFrom(src => src.AssignTo.Lastname))
            .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedBy.Username));

            CreateMap<AssignmentModel, Assignment>();
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();

            CreateMap<ReturnRequest, ReturnRequestModel>()
            .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => (src.Asset != null) ? src.Asset.AssetCode : null))
            .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => (src.Asset != null) ? src.Asset.AssetName : null))
            .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.RequestedBy.Username))
            .ForMember(dest => dest.AcceptedBy, opt => opt.MapFrom(src => (src.AcceptedBy != null) ? src.AcceptedBy.Username : null));
            
            CreateMap<ReturnRequestModel, ReturnRequest>();
        }
    }
}