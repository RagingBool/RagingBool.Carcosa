@echo off

cd projects
msbuild RagingBool.Carcosa.net35.sln /t:Clean,Build /p:Configuration=Debug
msbuild RagingBool.Carcosa.net35.sln /t:Clean,Build /p:Configuration=Release
msbuild RagingBool.Carcosa.net40.sln /t:Clean,Build /p:Configuration=Debug
msbuild RagingBool.Carcosa.net40.sln /t:Clean,Build /p:Configuration=Release
msbuild RagingBool.Carcosa.net45.sln /t:Clean,Build /p:Configuration=Debug
msbuild RagingBool.Carcosa.net45.sln /t:Clean,Build /p:Configuration=Release

pause
