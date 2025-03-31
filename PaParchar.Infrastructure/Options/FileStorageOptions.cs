namespace PaParchar.Infrastructure.Options
{
    public class FileStorageOptions
    {
        public const string SectionName = "FileStorage";

        // Tipos MIME permitidos
        public string[] AllowedMimeTypes { get; set; } = { "image/jpeg", "image/png", "image/gif", "image/webp" };
        
        // Tamaño máximo del archivo (en bytes), por defecto 5MB
        public long MaxFileSize { get; set; } = 5 * 1024 * 1024;
        
        // Extensiones de archivo permitidas
        public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Prefijo para las carpetas en el bucket
        public string FolderPrefix { get; set; } = "images";
        
        // Usar estructura de carpetas por fecha (true) o plana (false)
        public bool UseDateFolders { get; set; } = true;
    }
} 