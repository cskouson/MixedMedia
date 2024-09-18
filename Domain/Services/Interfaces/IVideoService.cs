using MixedMedia.Common.Dtos;

namespace MixedMedia.Domain.Services.Interfaces
{
    public interface IVideoService
    {
        Task<ServiceResponse<List<VideoDto>>> GetAllVideosAsync();
        Task<ServiceResponse<VideoDto>> GetVideoByIdAsync(Guid id);
        Task<ServiceResponse<VideoDto>> AddVideosAsync(VideoDto videoDto);
    }
}
