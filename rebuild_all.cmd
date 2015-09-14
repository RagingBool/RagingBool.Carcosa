@echo off

cd projects
msbuild RagingBool.Carcosa.net45.sln /t:Clean,Build /p:Configuration=Debug
msbuild RagingBool.Carcosa.net45.sln /t:Clean,Build /p:Configuration=Release

pause
