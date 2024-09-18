namespace MixedMedia.Common.Dtos
{
    public class VideoDto
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Path { get; set; }
        public List<IFormFile>? VideoDataList { get; set; }
    }
}
