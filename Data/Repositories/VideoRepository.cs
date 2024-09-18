using Microsoft.EntityFrameworkCore;
using MixedMedia.Data.Entities;
using MixedMedia.Data.Repositories.Interfaces;

//TODO: Most of these can be reused, so I should put these in a base repo class
namespace MixedMedia.Data.Repositories
{
    public class VideoRepository :IVideoRepository
    {
        private readonly MixedDbContext _dbContext;

        public VideoRepository(MixedDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<ICollection<VideoEntity>> GetAllVideosAsync()
        {
            return await _dbContext.Videos.ToListAsync();
        }

        public async Task<VideoEntity> GetVideoByIdAsync(Guid id)
        {
            return await _dbContext.Videos.Where(x => x.Id == id).FirstAsync();
        }

        public async Task<VideoEntity> GetVideoByNameAsync(string name)
        {
            return await _dbContext.Videos.Where(x => x.Name == name).FirstAsync();
        }

        public async Task<bool> AddVideoAsync(VideoEntity video)
        {
            await _dbContext.Videos.AddAsync(video);
            return await Save();
        }

        public Task<bool> DeleteVideo(VideoEntity video)
        {
            _dbContext.Videos.Remove(video);
            return Save();
        }

        public async Task<bool> CheckIfVideoExistsAsync(string name)
        {
            int count = await _dbContext.Videos.Where(x => x.Name == name).CountAsync();
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
