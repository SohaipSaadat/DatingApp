using AutoMapper;
using DatingApplication.DTOs;
using DatingApplication.Entities;

namespace DatingApplication.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dist=> dist.PhotoUrl, opt=> opt.MapFrom(src=> src.Photos.FirstOrDefault(p=> p.IsMAin).Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(p => p.IsMAin).Url))
                .ForMember(d => d.ReciverPhotoUrl, o => o.MapFrom(s => s.Reciver.Photos.FirstOrDefault(p => p.IsMAin).Url));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}
