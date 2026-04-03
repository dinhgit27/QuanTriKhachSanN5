@echo off
REM Hotel Management Database Setup Script (Batch version)
REM Khởi chạy migrations, seed data, và chạy ứng dụng

color 0B
cls
echo.
echo ===============================================
echo   Hotel Management API - Database Setup
echo ===============================================
echo.

REM 1. Clean old builds
echo [1/5] Cleaning old builds...
dotnet clean
if %ERRORLEVEL% EQU 0 (
    echo [OK] Clean completed
) else (
    echo [FAIL] Clean failed
    pause
    exit /b 1
)
echo.

REM 2. Build project
echo [2/5] Building project...
dotnet build
if %ERRORLEVEL% EQU 0 (
    echo [OK] Build completed
) else (
    echo [FAIL] Build failed
    pause
    exit /b 1
)
echo.

REM 3. Apply migrations
echo [3/5] Applying migrations to database...
dotnet ef database update
if %ERRORLEVEL% EQU 0 (
    echo [OK] Migrations applied
) else (
    echo [WARN] Migrations may have failed, but continuing...
)
echo.

REM 4. Display info
echo [4/5] Database setup complete!
echo.
echo ===============================================
echo   Starting Hotel Management API...
echo ===============================================
echo.
echo API Endpoints:
echo   - Swagger UI: https://localhost:5001/swagger
echo   - API Base: https://localhost:5001/api
echo.
echo Test Accounts:
echo   - admin@test.com / 123456 (Admin)
echo   - user@test.com / 123456 (User)
echo   - receptionist@test.com / 123456 (Receptionist)
echo.
echo Press Ctrl+C to stop the application
echo ===============================================
echo.

REM 5. Run application
dotnet run
pause
