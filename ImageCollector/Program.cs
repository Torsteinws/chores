namespace ImageCollector;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        DirectoryInfo rootDir = new(Directory.GetCurrentDirectory());
        DirectoryInfo outputDir = GetOutputDirectory(rootDir);

        Console.WriteLine("Searching for files...");
        var files = GetImageFiles(rootDir);

        int currentCount = 0;
        int successCount = 0;
        int totalCount = files.Count();
        
        Console.WriteLine($"Files found: {totalCount}\n");

        if(totalCount > 0)
        {
            Console.Write("Copying files: 0%");
        }

        foreach(var srcFile in GetImageFiles(rootDir))
        {
            currentCount++;
            if(currentCount % 20 == 0)
            {
                Console.SetCursorPosition(15, Console.CursorTop);
                double percent = Math.Round((double)currentCount / totalCount * 100, 1, MidpointRounding.ToPositiveInfinity);
                Console.Write($"{percent:00.0}%".PadRight(6));
            }


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
            successCount++;
        }

        if(totalCount > 0) 
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("Copying files: 100%       ");
        }

        if(successCount > 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nCopied {successCount} {(successCount == 1 ? "file" : "files")} to: {outputDir.FullName}");
            Console.ResetColor();
        }

        if(successCount == 0)
        {
            Directory.Delete(outputDir.FullName);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
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

        var excludedDirs = dir.GetDirectories("Image Collector Result*", SearchOption.TopDirectoryOnly)
                              .Select(d => d.FullName);

        var files = dir
            .GetFiles("*.*", SearchOption.AllDirectories)
            .Where(file => 
                extensions.Contains(file.Extension.ToLower()) &&
                !excludedDirs.Any(path => file.DirectoryName?.StartsWith(path) ?? false)
            ); 

        return files;        
    }
}