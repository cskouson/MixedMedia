using MixedMedia.Common.Dtos;
using MixedMedia.Data.Entities;
using AutoMapper;

namespace MixedMedia.Common.Mapping
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ImageEntity, ImageDto>().ReverseMap();
            CreateMap<ImageControllerPostDto, ImageDto>().ReverseMap();

            CreateMap<VideoEntity, VideoDto>().ReverseMap();
            CreateMap<VideoControllerPostDto, VideoDto>().ReverseMap();
        }
         
    }
}
