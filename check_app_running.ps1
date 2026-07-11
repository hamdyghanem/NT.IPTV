$apps = @("next", "iplay", "lionz")
foreach ($app in $apps) {
    $procs = Get-Process -Name "*$app*" -ErrorAction SilentlyContinue
    if ($procs) {
        foreach ($p in $procs) {
            $path = ""
            try { $path = $p.MainModule.FileName } catch {}
            Write-Host "FOUND RUNNING APP: $($p.Name) (PID: $($p.Id))" -ForegroundColor Green
            Write-Host "  Path: $path" -ForegroundColor White
            
            # Show its active network connections
            $conns = Get-NetTCPConnection -ErrorAction SilentlyContinue | Where-Object { $_.OwningProcess -eq $p.Id }
            if ($conns) {
                Write-Host "  Connections:" -ForegroundColor Yellow
                foreach ($c in $conns) {
                    Write-Host "    $($c.LocalAddress):$($c.LocalPort) -> $($c.RemoteAddress):$($c.RemotePort) ($($c.State))"
                }
            } else {
                Write-Host "  No active TCP connections." -ForegroundColor Gray
            }
            Write-Host ""
        }
    } else {
        Write-Host "App '$app' is not running." -ForegroundColor Yellow
    }
}
