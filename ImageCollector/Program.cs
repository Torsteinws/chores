namespace ImageCollector;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        DirectoryInfo rootDir = new(Directory.GetCurrentDirectory());
        foreach(var file in GetImageFiles(rootDir))
        {
            Console.WriteLine(file.FullName);
        }
    }


    static IEnumerable<FileInfo> GetImageFiles(DirectoryInfo dir)
    {
        var extensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".mp4", ".mov", ".webp", ".heic", ".tiff" };

        var files = dir
            .GetFiles("*.*", SearchOption.AllDirectories)
            .Where(file => extensions.Contains(file.Extension.ToLower()));

        return files;        
    }

}