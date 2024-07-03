namespace ImgCopy;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        var (sourceFile, destination) = ParseArgs(args);
        foreach(var dir in GetSourceDirectories(sourceFile.FullName))
        {
            Console.WriteLine(dir.FullName);

            // foreach (var file in dir.GetFiles())
            // {
            //     var destFile = Path.Combine(destination.FullName, file.Name);
            //     file.CopyTo(destFile, true);
            // }
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
}