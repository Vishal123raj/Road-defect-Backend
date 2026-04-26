using AutoMapper;
using RoadDefect.Api.Models;
using RoadDefect.Api.DTOs.Auth;
using RoadDefect.Api.DTOs.Defects;
using RoadDefect.Api.DTOs.Users;
using RoadDefect.Api.DTOs.WorkOrders;

namespace RoadDefect.Api.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // -----------------------------
            // USER MAPPINGS
            // -----------------------------
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // handled manually

            CreateMap<User, AuthUserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // -----------------------------
            // DEFECT MAPPINGS
            // -----------------------------
            CreateMap<Defect, DefectListDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.Name))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<DefectCreateDto, Defect>()
                .ForMember(dest => dest.DefectType, opt => opt.Ignore())
                .ForMember(dest => dest.Severity, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<Defect, DefectDetailsDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.Name))
                .ForMember(dest => dest.RoadSegment, opt => opt.MapFrom(src => src.RoadSegment != null ? src.RoadSegment.Name : null))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.DefectType.ToString()))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl)));

            // -----------------------------
            // WORK ORDER MAPPINGS
            // -----------------------------
            CreateMap<WorkOrderCreateDto, WorkOrder>();

            CreateMap<WorkOrder, WorkOrderDetailsDto>()
                .ForMember(dest => dest.DefectTitle, opt => opt.MapFrom(src => src.Defect.Title))
                .ForMember(dest => dest.AssignedEngineer, opt => opt.MapFrom(src => src.AssignedToUser.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Updates, opt => opt.MapFrom(src =>
                    src.Updates.Select(u => $"{u.CreatedAt:g}: {u.Comment}")));
        }
    }
}
