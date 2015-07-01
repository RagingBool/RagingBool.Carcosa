@echo off

rmdir NuGetPackage /s /q
mkdir NuGetPackage
mkdir NuGetPackage\RagingBool.Carcosa.0.2.0.0
mkdir NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib

copy package.nuspec NuGetPackage\RagingBool.Carcosa.0.2.0.0\RagingBool.Carcosa.0.2.0.0.nuspec
copy README.md NuGetPackage\RagingBool.Carcosa.0.2.0.0\README.md
copy LICENSE NuGetPackage\RagingBool.Carcosa.0.2.0.0\LICENSE

xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.IO.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.IO.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.IO.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.IO.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.IO.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.IO.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Components.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Components.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Components.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.IO.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.IO.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.IO.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Message.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Message.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Message.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Xml.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Xml.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Core_cs.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Core_cs.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Core_cs.xml NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Devices.Midi_cs.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Devices.Midi_cs.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Devices.Midi_cs.xml NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Devices_cs.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Devices_cs.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy bin\net40\Release\RagingBool.Carcosa.Devices_cs.xml NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net40\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.IO.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.IO.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.IO.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.IO.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.IO.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.IO.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Media.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Components.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Components.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Components.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.IO.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.IO.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.IO.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Message.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Message.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Message.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Xml.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.Xml.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.Midi.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.XML NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy externals\lib_dotnet\MIDI.NET.0.1.0.0\lib\CannedBytes.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Core_cs.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Core_cs.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Core_cs.xml NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Devices.Midi_cs.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Devices.Midi_cs.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Devices.Midi_cs.xml NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Devices_cs.dll NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Devices_cs.pdb NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\
xcopy bin\net45\Release\RagingBool.Carcosa.Devices_cs.xml NuGetPackage\RagingBool.Carcosa.0.2.0.0\lib\net45\

cd NuGetPackage
nuget pack RagingBool.Carcosa.0.2.0.0\RagingBool.Carcosa.0.2.0.0.nuspec -Properties version=0.2.0.0
7z a -tzip RagingBool.Carcosa.0.2.0.0.zip RagingBool.Carcosa.0.2.0.0 RagingBool.Carcosa.0.2.0.0.nupkg

echo nuget push RagingBool.Carcosa.0.2.0.0.nupkg > push.cmd
echo pause >> push.cmd

pause