using AutoMapper;
using MixedMedia.Common.Dtos;
using MixedMedia.Common.Utilities;
using MixedMedia.Data.Entities;
using MixedMedia.Data.Repositories.Interfaces;
using MixedMedia.Domain.Services.Interfaces;

namespace MixedMedia.Domain.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ImageService(IImageRepository imageRepository, IMapper mapper, IConfiguration configuration)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<List<ImageDto>>> GetAllImagesAsync()
        {
            ServiceResponse<List<ImageDto>> response = new();

            try
            {
                var ImageList = await _imageRepository.GetAllImagesAsync();
                var ImageDtoList = new List<ImageDto>();

                foreach(var item in ImageList)
                {
                    ImageDtoList.Add(_mapper.Map<ImageDto>(item));
                }

                response.Data = ImageDtoList;
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

        public async Task<ServiceResponse<ImageDto>> GetImageByIdAsync(Guid id)
        {
            ServiceResponse<ImageDto> response = new();

            try
            {
                var Image = await _imageRepository.GetImageByIdAsync(id);
                var ImageDto = _mapper.Map<ImageDto>(Image);

                response.Data = ImageDto;
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

        public async Task<ServiceResponse<ImageDto>> AddImagesAsync(ImageDto imageDto)
        {
            ServiceResponse<ImageDto> response = new();

            try
            {
                //check to see if image already exists
                foreach(IFormFile img in imageDto.ImageDataList)
                {
                    if (await _imageRepository.CheckIfImageExistsAsync(img.FileName))
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Error = "DuplicateImage";
                        return response;
                    }
                }

                var currentDate = DateTime.UtcNow;
                foreach(IFormFile img in imageDto.ImageDataList)
                {
                    ImageEntity imageEntity = new ImageEntity()
                    {
                        Id = Guid.NewGuid(),
                        Description = imageDto.Description,
                        Date = currentDate,
                        Name = img.FileName,
                        Path = imageDto.Path
                    };

                    //TODO: add correct Data field for response msg

                    if (!await _imageRepository.AddImageAsync(imageEntity))
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Error = "RepoError";
                        return response;
                    }
                }
                
                //add image file to local storage
                if (imageDto.ImageDataList == null || !await LocalFileOperations.SaveImageFiles(imageDto.ImageDataList, imageDto.Path, _configuration))
                {
                    response.Data = null;
                    response.Success = false;
                    response.Error = "ImageStorageError";
                    return response;
                }

                response.Data = _mapper.Map<ImageDto>(imageDto);
                response.Success = true;
                response.Message = "Image Created";
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
