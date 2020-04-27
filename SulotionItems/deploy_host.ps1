rm -Force -Recurse ..\publish\host\*
dotnet publish ..\src\HelloOrleans.SiloHost\HelloOrleans.SiloHost.csproj -o ..\publish\host\  -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
pause