namespace MixedMedia.Domain.BusinessRules.ImageRules
{
    public static class ImageTypeRule
    {
        static string[] ImageTypes = {"apng", "png", "avif", "gif", "jpg",
                                       "jpeg", "jfif", "pjpeg", "pjp", "svg",
                                       "webp", "tiff", "tif", "psd", "raw", "bmp",
                                       "svgz", "ai", "pdf"};
        
        public static bool AreValidImageTypes(List<IFormFile> images)
        {
            foreach (var image in images)
            {
                string[] separateExtension = image.FileName.Split('.');
                if ( !ImageTypes.Contains(separateExtension[1]) )
                    return false;
            }
            return true;
        }
    }
}
