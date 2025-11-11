@echo off
echo Stopping CurrencyShowcase services...

taskkill /im "UserService.WebApi.exe"
taskkill /im "CurrencyService.WebApi.exe"
taskkill /im "FavoritesService.WebApi.exe"
taskkill /im "ApiGatewayService.WebApi.exe"

timeout /t 6 /nobreak >nul

taskkill /im "node.exe"

echo All services stopped!
pause