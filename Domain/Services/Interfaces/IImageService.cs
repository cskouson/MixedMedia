using MixedMedia.Common.Dtos;

namespace MixedMedia.Domain.Services.Interfaces
{
    public interface IImageService
    {
        Task<ServiceResponse<List<ImageDto>>> GetAllImagesAsync();
        Task<ServiceResponse<ImageDto>> GetImageByIdAsync(Guid id);
        Task<Stream> GetImageFile(int token, string path, string fileName);
        Task<ServiceResponse<ImageDto>> AddImagesAsync(ImageDto imageDto);

    }
}
