#!/usr/bin/env pwsh
# Hotel Management Database Setup Script
# Khởi chạy migrations, seed data, và chạy ứng dụng

Write-Host "🏨 Hotel Management API - Database Setup" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host ""

# 1. Clean old builds
Write-Host "🧹 Cleaning old builds..." -ForegroundColor Yellow
dotnet clean --nologo -q
dotnet clean
if ($?) {
    Write-Host "✅ Clean completed successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Clean failed" -ForegroundColor Red
}
Write-Host ""

# 2. Build project
Write-Host "🔨 Building project..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build completed successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 3. Unblock all files (Windows security)
Write-Host "🔓 Unblocking files from Windows security policy..." -ForegroundColor Yellow
Get-ChildItem -Path bin -Recurse -ErrorAction SilentlyContinue | Unblock-File -ErrorAction SilentlyContinue
Get-ChildItem -Path obj -Recurse -ErrorAction SilentlyContinue | Unblock-File -ErrorAction SilentlyContinue
Write-Host "✅ Files unblocked" -ForegroundColor Green
Write-Host ""

# 4. Drop existing database (optional)
$dropDb = Read-Host "Drop existing database and start fresh? (y/n)"
if ($dropDb -eq "y" -or $dropDb -eq "Y") {
    Write-Host "🗑️  Dropping database..." -ForegroundColor Yellow
    try {
        dotnet ef database drop -f --no-build 2>$null
        Write-Host "✅ Database dropped" -ForegroundColor Green
    } catch {
        Write-Host "⚠️  Could not drop database (might not exist or access denied)" -ForegroundColor Yellow
    }
}
Write-Host ""

# 5. Apply migrations
Write-Host "📊 Applying migrations to database..." -ForegroundColor Yellow
dotnet ef database update --no-build
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migrations applied successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Migration failed" -ForegroundColor Red
}
Write-Host ""

# 6. Run application
Write-Host "🚀 Starting Hotel Management API..." -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📍 API will be available at:" -ForegroundColor Green
Write-Host "   - HTTP: http://localhost:5000" -ForegroundColor Green
Write-Host "   - HTTPS: https://localhost:5001" -ForegroundColor Green
Write-Host "   - Swagger: https://localhost:5001/swagger" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Test Accounts:" -ForegroundColor Green
Write-Host "   - admin@test.com / 123456" -ForegroundColor Green
Write-Host "   - user@test.com / 123456" -ForegroundColor Green
Write-Host "   - receptionist@test.com / 123456" -ForegroundColor Green
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host ""

# Run application
dotnet run --no-build
