using AutoMapper;
using MixedMedia.Common.Dtos;
using MixedMedia.Common.Utilities;
using MixedMedia.Data.Entities;
using MixedMedia.Data.Repositories.Interfaces;
using MixedMedia.Domain.BusinessRules.ImageRules;
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
            ServiceResponse<List<ImageDto>> Response = new();

            try
            {
                var ImageList = await _imageRepository.GetAllImagesAsync();
                var ImageDtoList = new List<ImageDto>();

                foreach (var item in ImageList)
                {
                    ImageDtoList.Add(_mapper.Map<ImageDto>(item));
                }

                Response.Data = ImageDtoList;
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

        public async Task<ServiceResponse<ImageDto>> GetImageByIdAsync(Guid id)
        {
            ServiceResponse<ImageDto> Response = new();

            try
            {
                var Image = await _imageRepository.GetImageByIdAsync(id);
                var ImageDto = _mapper.Map<ImageDto>(Image);

                Response.Data = ImageDto;
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

        public async Task<Stream> GetImageFile(int token, string path, string fileName)
        {
            //TODO: add token check and flag once database has the table for them.
            try
            {
                return await LocalFileOperations.LoadImageFiles(path, fileName, _configuration);
            }
            catch (Exception ex)
            {
                return new MemoryStream();
            }
            
        }

        public async Task<ServiceResponse<ImageDto>> AddImagesAsync(ImageDto imageDto)
        {
            ServiceResponse<ImageDto> Response = new();

            try
            {
                //check to see if image already exists
                foreach (IFormFile img in imageDto.ImageDataList)
                {
                    if (await _imageRepository.CheckIfImageExistsAsync(img.FileName))
                    {
                        Response.Data = null;
                        Response.Success = false;
                        Response.Error = "DuplicateImage";
                        return Response;
                    }
                }

                if(!ImageRulesBase.RunAllValidations(imageDto.ImageDataList))
                {
                    Response.Data = null;
                    Response.Success = false;
                    Response.Error = "Invalid Image Files";
                    return Response;
                }

                var CurrentDate = DateTime.UtcNow;
                foreach (IFormFile img in imageDto.ImageDataList)
                {
                    ImageEntity Image = new ImageEntity()
                    {
                        Id = Guid.NewGuid(),
                        Description = imageDto.Description,
                        Date = CurrentDate,
                        Name = img.FileName,
                        Path = imageDto.Path
                    };

                    //TODO: add better Data field for response msg

                    if (!await _imageRepository.AddImageAsync(Image))
                    {
                        Response.Data = null;
                        Response.Success = false;
                        Response.Error = "RepoError";
                        return Response;
                    }
                }
                
                //add image file to local storage
                if (imageDto.ImageDataList == null || !await LocalFileOperations.SaveImageFiles(imageDto.ImageDataList, imageDto.Path, _configuration))
                {
                    Response.Data = null;
                    Response.Success = false;
                    Response.Error = "ImageStorageError";
                    return Response;
                }

                Response.Data = _mapper.Map<ImageDto>(imageDto);
                Response.Success = true;
                Response.Message = "Image Created";
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
