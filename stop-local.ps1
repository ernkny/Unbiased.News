# Unbiased projesinin local ortamda çalışan servislerini durdurmak için script

Write-Host "Unbiased projesinin local ortamda çalışan servisleri durduruluyor..." -ForegroundColor Yellow

# dotnet ile çalışan tüm servisleri durdur
Get-Process -Name "dotnet" | Where-Object { $_.MainWindowTitle -match "Unbiased" } | ForEach-Object { 
    $_.Kill()
    Write-Host "Servis durduruldu: $($_.MainWindowTitle)" -ForegroundColor Red
}

Write-Host "Tüm servisler durduruldu!" -ForegroundColor Green 