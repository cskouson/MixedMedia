using AutoMapper;
using MixedMedia.Common.Dtos;
using MixedMedia.Common.Utilities;
using MixedMedia.Data.Entities;
using MixedMedia.Data.Repositories.Interfaces;
using MixedMedia.Domain.Services.Interfaces;

namespace MixedMedia.Domain.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public VideoService(IVideoRepository videoRepository, IMapper mapper, IConfiguration configuration)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<List<VideoDto>>> GetAllVideosAsync()
        {
            ServiceResponse<List<VideoDto>> response = new();

            try
            {
                var VideoList = await _videoRepository.GetAllVideosAsync();
                var VideoDtoList = new List<VideoDto>();

                foreach (var item in VideoList)
                {
                    VideoDtoList.Add(_mapper.Map<VideoDto>(item));
                }

                response.Data = VideoDtoList;
                response.Success = true;
                response.Message = "Ok";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return response;
        }
    }
}
