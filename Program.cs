namespace ImgCopy;

using System.IO;

internal class Program
{
    static void Main(string[] args)
    {
        var (sourceFile, destination) = ParseArgs(args);
        var files = ParseFile(sourceFile.FullName);
            
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

    private static List<string> ParseFile(string path) {
        var paths = new List<string>();
        using (var reader = new StreamReader(path)) {
            string? line = reader.ReadLine();
            while (line is not null) {
                paths.Add(line.Trim());
            }
        }
        return paths;
    }
}