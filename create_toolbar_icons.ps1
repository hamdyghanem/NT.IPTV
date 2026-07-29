# PowerShell script to create professional toolbar icons for frmCategories
# These will be generated as transparent PNGs

Add-Type -AssemblyName System.Drawing

# Define icon specifications
$iconSize = 64  # 64x64 pixels for good scaling
$backgroundColor = [System.Drawing.Color]::Transparent

# Create icons directory
$iconsDir = "$PSScriptRoot\NT.IPTV\toolbar-icons"
if (-not (Test-Path $iconsDir)) {
	New-Item -ItemType Directory -Path $iconsDir -Force | Out-Null
}

# Helper function to create a transparent bitmap
function New-TransparentBitmap {
	param([int]$width, [int]$height)
	$bitmap = New-Object System.Drawing.Bitmap($width, $height, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
	$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
	$graphics.Clear([System.Drawing.Color]::Transparent)
	return $bitmap, $graphics
}

# Helper function to create icon with simple shapes (using built-in .NET drawing)
function New-IconImage {
	param([string]$name, [scriptblock]$drawBlock)

	$bitmap, $graphics = New-TransparentBitmap $iconSize $iconSize
	$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
	$graphics.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAlias

	# Invoke the drawing block
	& $drawBlock $graphics $iconSize

	$graphics.Dispose()

	$path = Join-Path $iconsDir "$name.png"
	$bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
	$bitmap.Dispose()

	Write-Host "Created: $path"
	return $path
}

# Icon 1: Live TV - Play/Triangle
$playIcon = New-IconImage "btnLive" {
	param($g, $size)
	$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(0, 150, 136), 2)
	$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(0, 150, 136))

	$points = @(
		[System.Drawing.PointF](10, 10),
		[System.Drawing.PointF](10, $size - 10),
		[System.Drawing.PointF]($size - 10, $size / 2)
	)
	$g.FillPolygon($brush, $points)
	$brush.Dispose()
}

# Icon 2: Movies - Film Reel
$moviesIcon = New-IconImage "btnMovies" {
	param($g, $size)
	$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(233, 30, 99), 2)
	$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(233, 30, 99))

	# Draw film reel circles
	$g.DrawEllipse($pen, 5, 5, 18, 18)
	$g.DrawEllipse($pen, 41, 5, 18, 18)
	$g.DrawEllipse($pen, 5, 41, 18, 18)
	$g.DrawEllipse($pen, 41, 41, 18, 18)

	# Draw center film strip
	$g.FillRectangle($brush, 20, 15, 24, 34)

	$pen.Dispose()
	$brush.Dispose()
}

# Icon 3: Series - Stacked Bars
$seriesIcon = New-IconImage "btnSeries" {
	param($g, $size)
	$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(63, 81, 181))

	# Draw stacked rectangles
	$g.FillRectangle($brush, 8, 8, 48, 10)
	$g.FillRectangle($brush, 8, 22, 48, 10)
	$g.FillRectangle($brush, 8, 36, 48, 10)
	$g.FillRectangle($brush, 8, 50, 48, 10)

	$brush.Dispose()
}

# Icon 4: Global Search - Magnifying Glass
$searchIcon = New-IconImage "btnGlobalSearch" {
	param($g, $size)
	$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 152, 0), 3)
	$pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round

	# Draw circle (search lens)
	$g.DrawEllipse($pen, 8, 8, 32, 32)

	# Draw handle
	$g.DrawLine($pen, 42, 42, 56, 56)

	$pen.Dispose()
}

# Icon 5: Refresh - Curved Arrow
$refreshIcon = New-IconImage "btnRefresh" {
	param($g, $size)
	$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(76, 175, 80), 3)
	$pen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
	$pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
	$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(76, 175, 80))

	# Draw curved arrow path
	$path = New-Object System.Drawing.Drawing2D.GraphicsPath
	$path.AddArc(12, 12, 40, 40, 45, 270)
	$g.DrawPath($pen, $path)
	$path.Dispose()

	# Draw arrowhead
	$points = @(
		[System.Drawing.PointF](52, 12),
		[System.Drawing.PointF](52, 28),
		[System.Drawing.PointF](44, 20)
	)
	$g.FillPolygon($brush, $points)

	$pen.Dispose()
	$brush.Dispose()
}

# Icon 6: Logout - Exit/Door
$logoutIcon = New-IconImage "btnLogout" {
	param($g, $size)
	$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(244, 67, 54), 2)
	$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(244, 67, 54))

	# Draw door rectangle
	$g.DrawRectangle($pen, 12, 8, 32, 48)

	# Draw door knob
	$g.FillEllipse($brush, 38, 28, 4, 4)

	# Draw arrow pointing out
	$g.DrawLine($pen, 18, 32, 28, 32)
	$arrowPoints = @(
		[System.Drawing.PointF](28, 32),
		[System.Drawing.PointF](24, 28),
		[System.Drawing.PointF](26, 32)
	)
	$g.FillPolygon($brush, $arrowPoints)

	$pen.Dispose()
	$brush.Dispose()
}

Write-Host "`nAll toolbar icons created successfully in: $iconsDir"
Write-Host "`nNext steps:"
Write-Host "1. Open frmCategories.Designer.cs"
Write-Host "2. Update button images to reference the new PNG files from the toolbar-icons folder"
Write-Host "3. Or use the Designer to load these images into the resx file via drag-and-drop"
