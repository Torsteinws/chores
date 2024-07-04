# About
Recursively find all image files in the current directory, and copy them to a new directory *image-collector-result*

# Prerequisite
- Dotnet 8

# Build
Build a single file executable with these commands. You can find the executable in `./publish`

- Windows
    ```console
    dotnet publish --runtime win-x64 --output publish/win-x64
    ```

- Linux
    ```console
    dotnet publish --runtime linux-x64 --output publish/linux-x64
    ```

See full list of supported supported OSes [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog).

# Usage
Move executable to the directory you want to start your image search, then start the executable by double clicking it. 
The result will be in a directory called image-collector-result.
