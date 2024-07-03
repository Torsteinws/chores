$imageExtensions = @(".jpg", ".jpeg", ".png", ".bmp", ".gif", ".mp4", ".mov", ".webp", ".heic", ".tiff")

$outputFile = "imgPaths.txt"
$OutputEncoding = [System.Text.Encoding]::UTF8

# Clear the output file content before starting
if (Test-Path $outputFile) {
    Clear-Content $outputFile
}

$size = 0
$count = 0
$i = 0

foreach ($drive in "C","D" ) {

    "--------------------------------------------------" | Out-File -FilePath $outputFile -Append
    " SEARCHING WINDOWS DRIVE: ${drive}:\              " | Out-File -FilePath $outputFile -Append
    "--------------------------------------------------" | Out-File -FilePath $outputFile -Append

    # Get all directories recursively with error handling
    Get-ChildItem -Path "${drive}:\" -Directory -Recurse -ErrorAction SilentlyContinue | 
        Where-Object {

            $i++
            if($i % 1000 -eq 0) {
                $sizeInMB = [math]::Floor($size / 1MB)
                Write-Host "Processed dirs: ${i}, Image count: ${count}, Total size: ${sizeInMB} MB"
            }

            # Check if any file inside the directory has an image extension
            $dir = $_
            $containsImage = Get-ChildItem -Path $dir.FullName -File -ErrorAction SilentlyContinue | 
                Where-Object { $_.Extension -in $imageExtensions } |
                Select-Object -First 1;

                if ($containsImage -ne $null) {

                    # Retrieve image files in the current directory
                    $imageFiles = Get-ChildItem -Path $dir.FullName -File -ErrorAction SilentlyContinue | 
                        Where-Object { $_.Extension -in $imageExtensions }
                
                    # Sum the sizes of all image files in the directory
                    $dirSize = ($imageFiles | Measure-Object -Property Length -Sum).Sum
                    $size += $dirSize

                    $dirCount = ($imageFiles | Measure-Object).Count
                    $count += $dirCount
                
                $true
            } else {
                $false
            }
        } |
        ForEach-Object {
 
            # Display the full path of the directory
            # Write-Host $_.FullName;
            $_.FullName | Out-File -FilePath $outputFile -Append
        }
}

# Display the total size of all images
$sizeInMB = [math]::Floor($size / 1MB)
Write-Host "Total size of all images: ${sizeInMB} MB"
$sizeInMB | Out-File -FilePath $outputFile -Append 