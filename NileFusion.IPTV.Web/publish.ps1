param(
    [string]$ResourceGroup = "AudioCloud_group",
    [string]$AppName = "NTIPTV"
)

Set-Location $PSScriptRoot

Write-Host "Building frontend..."
npm run build

if (-not (Test-Path "dist")) {
    throw "Build output folder 'dist' was not found."
}

$zipPath = Join-Path $PSScriptRoot "dist.zip"
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Write-Host "Creating deployment zip..."
Compress-Archive -Path (Join-Path $PSScriptRoot "dist\*") -DestinationPath $zipPath -Force

Write-Host "Deploying to Azure App Service..."
az webapp deploy --resource-group $ResourceGroup --name $AppName --src-path $zipPath --type zip --clean true

Write-Host "Deployment complete."
