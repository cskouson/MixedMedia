namespace MixedMedia.Common.Dtos
{
    public class VideoControllerPostDto
    {
        public string Path { get; set; }
        public string? Description { get; set; }
        public List<IFormFile> VideoDataList { get; set; }
    }
}
