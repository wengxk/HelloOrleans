rm -Force -Recurse ..\publish\client\*
dotnet publish ..\src\HelloOrleans.BlazorClient\HelloOrleans.BlazorClient.csproj -o ..\publish\client\  -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
pause