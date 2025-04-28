namespace PaParchar.Infrastructure.Options
{
    public class FileStorageOptions
    {
        public const string SectionName = "FileStorage";
        public string[] AllowedMimeTypes { get; set; } = [ "image/jpeg", "image/png", "image/gif", "image/webp" ];
        public long MaxFileSize { get; set; } = 5 * 1024 * 1024;
        public string[] AllowedExtensions { get; set; } = [ ".jpg", ".jpeg", ".png", ".gif", ".webp" ];
        public string FolderPrefix { get; set; } = "images";
        public bool UseDateFolders { get; set; } = false;
    }
} 