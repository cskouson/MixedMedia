using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MixedMedia.Common.Dtos;
using MixedMedia.Domain.Services.Interfaces;

namespace MixedMedia.Api.Controllers
{
    [ApiController]
    [Route("/video")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;

        public VideoController(IVideoService videoService, IMapper mapper)
        {
            _videoService = videoService;
            _mapper = mapper;
        }

        [Route("GetAllVideos")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =  typeof(List<VideoDto>))]
        public async Task<IActionResult> GetAllVideos()
        {
            var Videos = await _videoService.GetAllVideosAsync();
            return Ok(Videos);
        }

        [Route("GetVideoById")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VideoDto))]
        public async Task<IActionResult> GetVideoById(Guid id)
        {
            var Video = await _videoService.GetVideoByIdAsync(id);
            return Ok(Video);
        }

        [Route("UploadVideos")]
        [HttpPost]
        public async Task<IActionResult> UploadVideo([FromForm] VideoControllerPostDto dto)
        {
            var Videos = await _videoService.AddVideosAsync(_mapper.Map<VideoDto>(dto));
            return Ok(Videos);
        }
    }
}
