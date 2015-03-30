@echo off

rmdir NuGetPackage /s /q
mkdir NuGetPackage
mkdir NuGetPackage\RagingBool.Carcosa.0.0.0.0
mkdir NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib

copy package.nuspec NuGetPackage\RagingBool.Carcosa.0.0.0.0\RagingBool.Carcosa.0.0.0.0.nuspec
copy README.md NuGetPackage\RagingBool.Carcosa.0.0.0.0\README.md
copy LICENSE NuGetPackage\RagingBool.Carcosa.0.0.0.0\LICENSE

xcopy bin\net35\Release\RagingBool.Carcosa.Core_cs.dll NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net35\
xcopy bin\net35\Release\RagingBool.Carcosa.Core_cs.pdb NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net35\
xcopy bin\net35\Release\RagingBool.Carcosa.Core_cs.xml NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net35\
xcopy bin\net40\Release\RagingBool.Carcosa.Core_cs.dll NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Core_cs.pdb NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Core_cs.xml NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net40\
xcopy bin\net45\Release\RagingBool.Carcosa.Core_cs.dll NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Core_cs.pdb NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Core_cs.xml NuGetPackage\RagingBool.Carcosa.0.0.0.0\lib\net45\

cd NuGetPackage
nuget pack RagingBool.Carcosa.0.0.0.0\RagingBool.Carcosa.0.0.0.0.nuspec -Properties version=0.0.0.0
7z a -tzip RagingBool.Carcosa.0.0.0.0.zip RagingBool.Carcosa.0.0.0.0 RagingBool.Carcosa.0.0.0.0.nupkg

echo nuget push RagingBool.Carcosa.0.0.0.0.nupkg > push.cmd
echo pause >> push.cmd

pause