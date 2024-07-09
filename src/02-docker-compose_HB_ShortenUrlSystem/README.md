# README
## Prerequisite
- Create solution
```ps1
dotnet new sln -n ShortenUrlSystem -o src/02-docker-compose_HB_ShortenUrlSystem
```
- Create Web Server project
```ps1
dotnet new webapi -n WebServer.API -o src/02-docker-compose_HB_ShortenUrlSystem/WebServer.API;
dotnet sln src/02-docker-compose_HB_ShortenUrlSystem add (ls -r src/02-docker-compose_HB_ShortenUrlSystem/**/*.csproj)
```