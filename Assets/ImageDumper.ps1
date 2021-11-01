Add-Type -Assembly PresentationCore
$img = [Windows.Clipboard]::GetImage()
if ($img -eq $null) {
    #Write-Host "Clipboard contains no image."
    Exit
}

$fcb = new-object Windows.Media.Imaging.FormatConvertedBitmap($img, [Windows.Media.PixelFormats]::Rgb24, $null, 0)
$file = $args[0]
#Write-Host ("`n Found picture. {0}x{1} pixel. Saving to {2}`n" -f $img.PixelWidth, $img.PixelHeight, $file)

$stream = [IO.File]::Open($file, "OpenOrCreate")
$encoder = New-Object Windows.Media.Imaging.PngBitmapEncoder
$encoder.Frames.Add([Windows.Media.Imaging.BitmapFrame]::Create($fcb))
$encoder.Save($stream)
$stream.Dispose()
Exit