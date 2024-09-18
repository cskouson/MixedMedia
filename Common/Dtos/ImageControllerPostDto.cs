namespace MixedMedia.Common.Dtos
{
    public class ImageControllerPostDto
    {
        public string Path { get; set; }
        public string? Description { get; set; }
        public List<IFormFile> ImageDataList { get; set; }
    }
}
