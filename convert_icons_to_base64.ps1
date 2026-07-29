# Convert generated PNGs to base64 for embedding in resx
$iconsDir = "D:\Working Folder\NT.IPTV\NT.IPTV\toolbar-icons"

# Array of icons to process
$icons = @("btnRefresh", "btnGlobalSearch")

foreach ($icon in $icons) {
	$pngPath = Join-Path $iconsDir "$icon.png"
	if (Test-Path $pngPath) {
		$bytes = [System.IO.File]::ReadAllBytes($pngPath)
		$base64 = [Convert]::ToBase64String($bytes)

		# Output in chunks for readability (Visual Studio uses 76 char lines)
		Write-Host "`n<!-- $icon -->"
		Write-Host "<data name=`"$icon.Image`" type=`"System.Drawing.Bitmap, System.Drawing`" mimetype=`"application/x-microsoft.net.object.bytearray.base64`">"
		Write-Host "  <value>"

		# Split into 76-character lines
		$lineLength = 76
		for ($i = 0; $i -lt $base64.Length; $i += $lineLength) {
			$line = $base64.Substring($i, [Math]::Min($lineLength, $base64.Length - $i))
			Write-Host "    $line"
		}

		Write-Host "  </value>"
		Write-Host "</data>"
	}
}
