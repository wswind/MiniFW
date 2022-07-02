@echo off

:begin
set /p projname=type project name:
echo project name is %projname%

if not exist %projname% (
    echo folder %projname% does not exist
    goto begin
)

cd %projname%
dotnet pack -c Release

:version
cd bin\Release
echo type nupkg package version:
set /p version=
set nupkgPah=%projname%.%version%.nupkg
if not exist %nupkgPah% (
    echo file %nupkgPah% does not exist
    goto version
)

echo copy %nupkgPah% to D:\\nuget_local
xcopy %nupkgPah% D:\nuget_local\


echo comfirm publish %nupkgPah% to nuget?£¨y/n£©
set /p confirm=
if %confirm% == y (
    dotnet nuget push %nupkgPah%  -k %NUGET_API_KEY% -s https://api.nuget.org/v3/index.json
    echo publish finish
) else (
    echo publish cancelled
)
pause