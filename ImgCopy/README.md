# About
Find all images in the given directories, and recursively copy them to a target directory.

## Requirements
- Dotnet 8 or later

## Usage

```console
dotnet run -- ./list-of-dirs.txt output-dir
```

- **list-of-dirs.txt**: A simple txt file with all directories to traverse. Each directory is delimited by a newline.
- **output-dir**: The directory to copy the images to.