namespace ImageCollector;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        DirectoryInfo rootDir = new(Directory.GetCurrentDirectory());
        DirectoryInfo outputDir = GetOutputDirectory(rootDir);
        foreach(var file in GetImageFiles(rootDir))
        {
            Console.WriteLine(file.FullName);
        }
    }

    static DirectoryInfo GetOutputDirectory(DirectoryInfo baseDir)
    {
        var basePath = Path.Combine(baseDir.FullName, "image-collector-result");

        var availablePath = basePath;
        int i = 1;
        while(Directory.Exists(availablePath))
        {   
            availablePath = $"{basePath}-{i:D3}";
            i++;
        }

        DirectoryInfo outputDir = Directory.CreateDirectory(availablePath);

        return outputDir;
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