namespace ImgCopy;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        (FileInfo sourceFile, DirectoryInfo targetRoot) = ParseArgs(args);
        foreach(var dir in GetSourceDirectories(sourceFile.FullName))
        {
            // GEt the target directory
            var drive = Path.GetPathRoot(dir.FullName);
            if(drive is null) 
            {
                Console.Error.WriteLine("Skipping path without root: " + dir.FullName);
                continue;
            }
            string relativePath = Path.GetRelativePath(drive, dir.FullName);
            string targetDir = Path.Combine(targetRoot.FullName, relativePath);


            // Get all images and check if they exist
            var files = GetImageFiles(dir);
            if(files is null || !files.Any())
            {
                Console.WriteLine("No images found in directory: " + dir.FullName);
                continue;
            }

            // Create target directory if it does not exist
            if(!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copy images to targe directory
            foreach (var file in files) 
            {
                string newFilename = Path.Combine(targetDir, file.Name);
                if (File.Exists(newFilename))
                {
                    Console.WriteLine("Skipping, file already exists: " + newFilename);
                    continue;
                }

                try 
                {
                    File.Copy(file.FullName, newFilename);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Error copying file: \n" + file.FullName + " ----> " + newFilename + "\nReason: " + e.Message);
                }

            }
        }
    }

    private static (FileInfo, DirectoryInfo) ParseArgs(string[] args) {
        if (args.Length < 2) {
            Console.WriteLine("Usage: imgcopy <source> <destination>");
            Environment.Exit(1);
        }

        FileInfo sourceFile = new(args[0].Trim());
        DirectoryInfo destination = new(args[1].Trim());

        if (!sourceFile.Exists) {
            Console.WriteLine("Source file does not exist.");
            Environment.Exit(1);
            throw new InvalidOperationException();
        }

        if (!destination.Exists) {
            Directory.CreateDirectory(destination.FullName);
        }

        return (sourceFile, destination);
    }

    private static IEnumerable<DirectoryInfo> GetSourceDirectories(string path) 
    {
        using var reader = new StreamReader(path);
        foreach(var line in File.ReadLines(path))
        {
            var dir = new DirectoryInfo(line);
            if (dir.Exists)
            {
                yield return dir;
            }
        }
    }

    private static IEnumerable<FileInfo> GetImageFiles(DirectoryInfo dir)
    {
        var extensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".mp4", ".mov", ".webp", ".heic", ".tiff" };

        var files = dir
            .GetFiles("*.*", SearchOption.TopDirectoryOnly)
            .Where(file => extensions.Contains(file.Extension.ToLower()));

        return files;
    }    
}