using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MixedMedia.Data.Entities;
using MixedMedia.Data.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;

//TODO: Most of these can be reused, so I should put these in a base repo class
namespace MixedMedia.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly MixedDbContext _dbContext;

        public ImageRepository(MixedDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<ICollection<ImageEntity>> GetAllImagesAsync()
        {
            return await _dbContext.Images.ToListAsync();
        }

        public async Task<ImageEntity> GetImageByIdAsync(Guid id)
        {
            return await _dbContext.Images.Where(x => x.Id == id).FirstAsync();
        }

        public async Task<ImageEntity> GetImageByNameAsync(string name)
        {
            return await _dbContext.Images.Where(x => x.Name == name).FirstAsync();
        }

        public async Task<bool> AddImageAsync(ImageEntity image)
        {
            await _dbContext.Images.AddAsync(image);
            return await Save();
        }

        public Task<bool> DeleteImage(ImageEntity image)
        {
            _dbContext.Images.Remove(image);
            return Save();
        }

        public async Task<bool> CheckIfImageExistsAsync(string name)
        {
            int count = await _dbContext.Images.Where(x => x.Name == name).CountAsync();
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
