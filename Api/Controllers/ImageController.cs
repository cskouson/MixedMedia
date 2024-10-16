﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MixedMedia.Common.Dtos;
using MixedMedia.Domain.Services.Interfaces;

namespace MixedMedia.Api.Controllers
{
    [ApiController]
    [Route("/image")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ImageController(IImageService imageService, IMapper mapper) 
        {
            _imageService = imageService;
            _mapper = mapper;
        }

        [Route("GetAllImages")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImageDto>))]
        public async Task<IActionResult> GetAllImages()
        {
            var Images = await _imageService.GetAllImagesAsync();
            return Ok(Images);
        }

        [Route("GetImageById")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDto))]
        public async Task<IActionResult> GetImageById(Guid id)
        {
            var Image = await _imageService.GetImageByIdAsync(id);
            return Ok(Image);
        }

        [Route("GetImageFile")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetImageFile(int token, string path, string fileName )
        {
            Stream image = await _imageService.GetImageFile(token, path, fileName);

            return File(image, "application/octet", fileName);
        }

        [Route("UploadImages")]
        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] ImageControllerPostDto dto)
        {
            var Images = await _imageService.AddImagesAsync(_mapper.Map<ImageDto>(dto));
            return  Ok(Images);
        }
    }
}
