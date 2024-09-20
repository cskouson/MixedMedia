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
            ServiceResponse<List<VideoDto>> Response = new();

            try
            {
                var VideoList = await _videoRepository.GetAllVideosAsync();
                var VideoDtoList = new List<VideoDto>();

                foreach (var item in VideoList)
                {
                    VideoDtoList.Add(_mapper.Map<VideoDto>(item));
                }

                Response.Data = VideoDtoList;
                Response.Success = true;
                Response.Message = "Ok";
            }
            catch (Exception ex)
            {
                Response.Data = null;
                Response.Success = false;
                Response.Message = "Error";
                Response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return Response;
        }

        public async Task<ServiceResponse<VideoDto>> GetVideoByIdAsync(Guid id)
        {
            ServiceResponse<VideoDto> Response = new();

            try
            {
                var Video = await _videoRepository.GetVideoByIdAsync(id);
                var VideoDto = _mapper.Map<VideoDto>(Video);

                Response.Data = VideoDto;
                Response.Success = true;
                Response.Message = "Ok";
            }
            catch (Exception ex)
            {
                Response.Data = null;
                Response.Success = false;
                Response.Message = "Error";
                Response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return Response;
        }

        public async Task<ServiceResponse<VideoDto>> AddVideosAsync(VideoDto videoDto)
        {
            ServiceResponse<VideoDto > Response = new();

            try
            {
                //check to see if video already exists
                foreach(IFormFile vid in videoDto.VideoDataList)
                {
                    if(await _videoRepository.CheckIfVideoExistsAsync(vid.FileName))
                    {
                        Response.Data = null;
                        Response.Success = false;
                        Response.Error = "DuplicateVideo";
                        return Response;
                    }
                }

                //TODO: hook up business rule validations

                var CurrentDate = DateTime.UtcNow;
                foreach(IFormFile vid in videoDto.VideoDataList)
                {
                    VideoEntity Video = new VideoEntity()
                    {
                        Id = Guid.NewGuid(),
                        Description = videoDto.Description,
                        Date = CurrentDate,
                        Name = vid.FileName,
                        Path = videoDto.Path
                    };

                    //TODO: add better Data field for response

                    if(!await _videoRepository.AddVideoAsync(Video))
                    {
                        Response.Data = null;
                        Response.Success = false;
                        Response.Error = "RepoError";
                        return Response;
                    }
                }

                //add image file to local storage
                if(videoDto.VideoDataList == null || !await LocalFileOperations.SaveVideoFile(videoDto.VideoDataList, videoDto.Path, _configuration))
                {
                    Response.Data = null;
                    Response.Success = false;
                    Response.Error = "VideoStorageError";
                }
                Response.Data = _mapper.Map<VideoDto>(videoDto);
                Response.Success = true;
                Response.Message = "Video Created";
            }
            catch (Exception ex)
            {
                Response.Data = null;
                Response.Success = false;
                Response.Message = "Error";
                Response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return Response;
        }
    }
}
