namespace ImageCollector;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        DirectoryInfo rootDir = new(Directory.GetCurrentDirectory());
        DirectoryInfo outputDir = GetOutputDirectory(rootDir);
        foreach(var srcFile in GetImageFiles(rootDir))
        {
            FileInfo dstFile;
            try 
            {
                dstFile = CreateUniqueFilename(outputDir, srcFile.Name);
            }
            catch (ArgumentException e)
            {
                Console.Error.WriteLine($"Error: Skipping file {srcFile.FullName}\nReason: {e.Message}");
                continue;
            }

            File.Copy(srcFile.FullName, dstFile.FullName);
        }
    }

    static DirectoryInfo GetOutputDirectory(DirectoryInfo baseDir)
    {
        var basePath = Path.Combine(baseDir.FullName, "Image Collector Result");

        var availablePath = basePath;
        int i = 1;
        while(Directory.Exists(availablePath))
        {   
            availablePath = $"{basePath} ({i})";
            i++;
        }

        DirectoryInfo outputDir = Directory.CreateDirectory(availablePath);

        return outputDir;
    }

    static FileInfo CreateUniqueFilename(DirectoryInfo baseDir, string desiredFilename) 
    {
        var baseName = Path.GetFileNameWithoutExtension(desiredFilename);
        var extension = Path.GetExtension(desiredFilename);

        var availablePath = Path.Combine(baseDir.FullName, desiredFilename);
        int i = 1;
        while(File.Exists(availablePath))
        {
            availablePath = Path.Combine(baseDir.FullName, $"{baseName} ({i}){extension}");
            i++;
        }
        
        return new FileInfo(availablePath);
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