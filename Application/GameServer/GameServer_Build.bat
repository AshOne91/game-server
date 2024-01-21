::REM "Consider updating git and svn"

::REM "Clean Build Folder"
::del /s /q "Build\*"

REM "Clean dotnet"
dotnet clean --output ../../Bin/GameServer/Win64 --configuration Debug

REM "Build start"
dotnet build --output ../../Bin/GameServer/Win64 --configuration Debug --runtime win-x64

REM "Start build and publish"
dotnet publish --output ../../Bin/GameServer/Win64 --configuration Debug --runtime win-x64 --self-contained false

REM "Finish!!!!!!!!"
pause