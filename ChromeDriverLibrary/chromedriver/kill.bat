@echo off
setlocal
echo Killing all chrome.exe processes...
taskkill /F /IM chrome.exe /T >nul
echo Killing all chromedriver.exe processes...
taskkill /F /IM chromedriver.exe /T >nul
echo Processes killed successfully.
endlocal