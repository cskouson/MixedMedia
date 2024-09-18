using MixedMedia.Data.Entities;

namespace MixedMedia.Data.Repositories.Interfaces
{
    public interface IImageRepository
    {

        Task<ICollection<ImageEntity>> GetAllImagesAsync();
        Task<ImageEntity> GetImageByIdAsync(Guid id);
        Task<ImageEntity> GetImageByNameAsync(string name);
        Task<bool> AddImageAsync(ImageEntity image);
        Task<bool> CheckIfImageExistsAsync(string name);
        Task<bool> DeleteImage(ImageEntity image);

    }
}
