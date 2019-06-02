using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                     .ForMember(dest => dest.photourl, opt =>
                     {
                         opt.MapFrom(src => src.photos.FirstOrDefault(p => p.IsMain).url);
                     })
                     .ForMember(dest => dest.Age, opt =>
                     {
                         opt.ResolveUsing(d => d.dateofbirth.CalculateAge());
                     });

            CreateMap<User, userForDetailedDto>()
                     .ForMember(dest => dest.photourl, opt =>
                     {
                         opt.MapFrom(src => src.photos.FirstOrDefault(p => p.IsMain).url);
                     })
                     .ForMember(dest => dest.Age, opt =>
                     {
                         opt.ResolveUsing(d => d.dateofbirth.CalculateAge());
                     });

            CreateMap<Photo, photosForDetailsDtos>();
            CreateMap<ruserFromUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();

        }

    }
}
