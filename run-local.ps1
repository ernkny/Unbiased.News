# Unbiased projesini local ortamda çalıştırmak için script

Write-Host "Unbiased projesini local ortamda başlatılıyor..." -ForegroundColor Green

# Önce çalışan tüm dotnet süreçlerini durduralım
Write-Host "Çalışan dotnet süreçleri durduruluyor..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | ForEach-Object { 
    $_.Kill()
    Write-Host "Servis durduruldu: $($_.Id)" -ForegroundColor Red
}

# API Gateway'i başlat
Write-Host "API Gateway başlatılıyor..." -ForegroundColor Cyan
Start-Process -FilePath "powershell" -ArgumentList "-Command `"cd $PSScriptRoot/Unbiased.ApiGateway && dotnet run --launch-profile http`"" -WindowStyle Normal

# 5 saniye bekle
Start-Sleep -Seconds 5

# News API'yi başlat
Write-Host "News API başlatılıyor..." -ForegroundColor Cyan
Start-Process -FilePath "powershell" -ArgumentList "-Command `"cd $PSScriptRoot/Unbiased.News.Api && dotnet run --launch-profile http`"" -WindowStyle Normal

# 5 saniye bekle
Start-Sleep -Seconds 5

# Playwright API'yi başlat
Write-Host "Playwright API başlatılıyor..." -ForegroundColor Cyan
Start-Process -FilePath "powershell" -ArgumentList "-Command `"cd $PSScriptRoot/Unbiased.Playwright.Api && dotnet run --launch-profile http`"" -WindowStyle Normal

# 5 saniye bekle
Start-Sleep -Seconds 5

# Log API'yi başlat
Write-Host "Log API başlatılıyor..." -ForegroundColor Cyan
Start-Process -FilePath "powershell" -ArgumentList "-Command `"cd $PSScriptRoot/Unbiased.Log.Api && dotnet run --launch-profile http`"" -WindowStyle Normal

# 5 saniye bekle
Start-Sleep -Seconds 5

# Identity API'yi başlat
Write-Host "Identity API başlatılıyor..." -ForegroundColor Cyan
Start-Process -FilePath "powershell" -ArgumentList "-Command `"cd $PSScriptRoot/Unbiased.Identity.Api && dotnet run --launch-profile http`"" -WindowStyle Normal

# 5 saniye bekle
Start-Sleep -Seconds 5

# Dashboard API'yi başlat
Write-Host "Dashboard API başlatılıyor..." -ForegroundColor Cyan
Start-Process -FilePath "powershell" -ArgumentList "-Command `"cd $PSScriptRoot/Unbiased.Dashboard.Api && dotnet run --launch-profile http`"" -WindowStyle Normal

# 5 saniye bekle
Start-Sleep -Seconds 5

# Servislerin çalışıp çalışmadığını kontrol et
Write-Host "Servislerin durumu kontrol ediliyor..." -ForegroundColor Yellow
$processes = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
Write-Host "Toplam çalışan dotnet süreci sayısı: $($processes.Count)" -ForegroundColor Cyan

Write-Host "Tüm servisler başlatıldı!" -ForegroundColor Green
Write-Host "API Gateway: http://localhost:5000" -ForegroundColor Cyan
Write-Host "News API: http://localhost:5001" -ForegroundColor Cyan
Write-Host "Playwright API: http://localhost:5002" -ForegroundColor Cyan
Write-Host "Log API: http://localhost:5003" -ForegroundColor Cyan
Write-Host "Identity API: http://localhost:5004" -ForegroundColor Cyan
Write-Host "Dashboard API: http://localhost:5005" -ForegroundColor Cyan

Write-Host "Servislere bağlantı testi yapılıyor..." -ForegroundColor Yellow
$endpoints = @(
    "http://localhost:5000",
    "http://localhost:5001",
    "http://localhost:5002",
    "http://localhost:5003",
    "http://localhost:5004",
    "http://localhost:5005"
)

foreach ($endpoint in $endpoints) {
    try {
        $response = Invoke-WebRequest -Uri $endpoint -Method Head -TimeoutSec 5 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Host "$endpoint erişilebilir." -ForegroundColor Green
        } else {
            Write-Host "$endpoint erişilebilir, ancak durum kodu: $($response.StatusCode)" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "$endpoint erişilemiyor. Hata: $_" -ForegroundColor Red
    }
} 