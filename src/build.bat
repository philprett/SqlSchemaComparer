@echo off

if not exist "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\vsdevcmd\core\vsdevcmd_start.bat" goto novisualstudio
call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\vsdevcmd\core\vsdevcmd_start.bat"
call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\vsdevcmd\core\msbuild.bat"
call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\vsdevcmd\core\vsdevcmd_end.bat"

CD /D "%~dp0"

:doit
msbuild SqlSchemaComparer\SqlSchemaComparer.csproj /p:Configuration=Release /t:Rebuild
if errorlevel 1 goto builderror

start SqlSchemaComparer\bin\Release

goto :EOF


:novisualstudio
echo We could not find the Visual Studio 2017 command line tools.
pause
goto :EOF

:builderror
pause
goto :EOF