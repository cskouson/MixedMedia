namespace MixedMedia.Common.Utilities
{
    public static class LocalFileOperations
    {
        public async static Task<bool> SaveImageFiles(List<IFormFile> imageList, string path, IConfiguration config)
        {
            string _baseImagePath = config["Configurations:BaseFilesPath"] + "mixed-media-images\\";

            Directory.CreateDirectory(Path.Combine(_baseImagePath, path));

            foreach (var image in imageList)
            {
                if (image.FileName == null || image.Length == 0)
                {
                    return false;
                }
                
                var fullPath = Path.Combine(_baseImagePath, path, image.FileName);
                
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                    stream.Close();
                }
            }
            return true;
        }

        public async static Task<bool> SaveVideoFile(List<IFormFile> videoList, string path, IConfiguration config)
        {
            string _baseVideoPath = config["Configurations:BaseFilesPath"] + "mixed-media-videos\\";

            Directory.CreateDirectory(Path.Combine(_baseVideoPath, path));

            foreach (var video in videoList)
            {
                if (video.FileName == null || video.Length == 0)
                {
                    return false;
                }

                var fullPath = Path.Combine(_baseVideoPath, path, video.FileName);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                    stream.Close();
                }
            }
            return true;
        }

        public async static Task<bool> SaveGenericFile(IFormFile file, string path, IConfiguration config)
        {
            string _baseFilePath = config["Configurations:BaseFilesPath"] + "mixed-media-generic\\";

            Directory.CreateDirectory(Path.Combine(_baseFilePath, path));

            if (file.FileName == null || file.Length == 0)
            {
                return false;
            }

            var fullPath = Path.Combine(_baseFilePath, path, file.FileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                stream.Close();
            }
            return true;
        }
    }
}
