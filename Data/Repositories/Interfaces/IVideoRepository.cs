using MixedMedia.Data.Entities;

namespace MixedMedia.Data.Repositories.Interfaces
{
    public interface IVideoRepository
    {
        Task<ICollection<VideoEntity>> GetAllVideosAsync();
        Task<VideoEntity> GetVideoByIdAsync(Guid id);
        Task<VideoEntity> GetVideoByNameAsync(string id);
        Task<bool> AddVideoAsync(VideoEntity video);
        Task<bool> CheckIfVideoExistsAsync(string name);
        Task<bool> DeleteVideo(VideoEntity video);
    }
}
