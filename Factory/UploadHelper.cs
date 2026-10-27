using System.Text.RegularExpressions;

namespace SelfKOrderWeb.Factory
{
    /// <summary>
    /// Shared checks for files uploaded into wwwroot.
    /// Keeps the existing "GUID_originalname.ext" naming so stored paths look the same as before,
    /// but strips any directory part from the client file name (no "..\" path traversal) and
    /// only accepts known image/video extensions (no .cshtml/.html/.exe dropped into wwwroot).
    /// </summary>
    public static class UploadHelper
    {
        public const long MaxImageBytes = 10L * 1024 * 1024;    // 10 MB
        public const long MaxMediaBytes = 200L * 1024 * 1024;   // 200 MB (ad videos)

        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg"
        };

        private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".webm", ".mov", ".m4v", ".ogg", ".ogv"
        };

        /// <summary>True when the file is a non-empty image within the size limit.</summary>
        public static bool IsAllowedImage(IFormFile? file) =>
            file != null && file.Length > 0 && file.Length <= MaxImageBytes &&
            ImageExtensions.Contains(Path.GetExtension(CleanName(file.FileName)));

        /// <summary>True when the file is a non-empty image or video within the size limit (ads, banners).</summary>
        public static bool IsAllowedMedia(IFormFile? file)
        {
            if (file == null || file.Length == 0 || file.Length > MaxMediaBytes) return false;
            var ext = Path.GetExtension(CleanName(file.FileName));
            return ImageExtensions.Contains(ext) || VideoExtensions.Contains(ext);
        }

        /// <summary>Unique stored name in the existing format: "{guid}_{original file name}".</summary>
        public static string UniqueName(IFormFile file) => Guid.NewGuid() + "_" + CleanName(file.FileName);

        /// <summary>Makes sure the target folder exists and returns its full path.</summary>
        public static string EnsureFolder(string webRootPath, string folder)
        {
            var full = Path.Combine(webRootPath, folder.Trim('\\', '/'));
            Directory.CreateDirectory(full);
            return full;
        }

        /// <summary>Writes the upload to {folder}/{storedName}. storedName must come from <see cref="UniqueName"/>.</summary>
        public static async Task SaveAsync(IFormFile file, string webRootPath, string folder, string storedName)
        {
            var fullFolder = EnsureFolder(webRootPath, folder);
            var fullPath = Path.GetFullPath(Path.Combine(fullFolder, storedName));
            if (!fullPath.StartsWith(Path.GetFullPath(fullFolder), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Invalid upload path.");

            await using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);
        }

        /// <summary>Client file name without any directory part and with unsafe characters replaced.</summary>
        private static string CleanName(string? fileName)
        {
            var name = (fileName ?? string.Empty).Replace('\\', '/');
            name = name[(name.LastIndexOf('/') + 1)..];
            name = Regex.Replace(name, @"[^\w.\- ()]", "_");
            name = name.Trim(' ', '.');
            return string.IsNullOrEmpty(name) ? "file" : name;
        }
    }
}
