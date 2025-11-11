@echo off
setlocal

:: Корневая папка проекта
set "ROOT=%~dp0"

echo Starting CurrencyShowcase services...

start "UserService" cmd /k "cd /d "%ROOT%UserService\WebApi\bin\Debug\net8.0" && set ASPNETCORE_ENVIRONMENT=Development && UserService.WebApi.exe"

timeout /t 1 /nobreak >nul

start "CurrencyService" cmd /k "cd /d "%ROOT%CurrencyService\WebApi\bin\Debug\net8.0" && set ASPNETCORE_ENVIRONMENT=Development && CurrencyService.WebApi.exe"

timeout /t 1 /nobreak >nul

start "FavoritesService" cmd /k "cd /d "%ROOT%FavoritesService\WebApi\bin\Debug\net8.0" && set ASPNETCORE_ENVIRONMENT=Development && FavoritesService.WebApi.exe"

timeout /t 1 /nobreak >nul

start "ApiGateway" cmd /k "cd /d "%ROOT%ApiGatewayService\WebApi\bin\Debug\net8.0" && set ASPNETCORE_ENVIRONMENT=Development && ApiGatewayService.WebApi.exe"

timeout /t 2 /nobreak >nul

start "Frontend" cmd /k "cd /d "%ROOT%frontend-app" && npm run dev"

echo All services started!
pause

endlocal