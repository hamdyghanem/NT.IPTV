# Create a professional settings/gear icon
Add-Type -AssemblyName System.Drawing

$iconSize = 64
$bitmap = New-Object System.Drawing.Bitmap($iconSize, $iconSize, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.Clear([System.Drawing.Color]::Transparent)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias

# Draw gear/cog icon (settings)
$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(156, 39, 176), 2)  # Purple
$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(156, 39, 176))

# Draw center circle
$graphics.FillEllipse($brush, 24, 24, 16, 16)

# Draw outer gear teeth
$center = 32
$outerRadius = 28
$innerRadius = 22

# Create 8 gear teeth
$pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
$pen.LineWidth = 3

for ($i = 0; $i -lt 8; $i++) {
	$angle = ($i * 45) * [Math]::PI / 180
	$x1 = $center + $innerRadius * [Math]::Cos($angle)
	$y1 = $center + $innerRadius * [Math]::Sin($angle)
	$x2 = $center + $outerRadius * [Math]::Cos($angle)
	$y2 = $center + $outerRadius * [Math]::Sin($angle)
	$graphics.DrawLine($pen, [single]$x1, [single]$y1, [single]$x2, [single]$y2)
}

$graphics.Dispose()

$iconPath = "D:\Working Folder\NT.IPTV\NT.IPTV\Resources\btnSettings.png"
$bitmap.Save($iconPath, [System.Drawing.Imaging.ImageFormat]::Png)
$bitmap.Dispose()

Write-Host "Settings icon created: $iconPath"
