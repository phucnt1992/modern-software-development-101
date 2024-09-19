FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
COPY . /src
WORKDIR /src
RUN dotnet build "HealthCheck.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthCheck.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS=80
ENTRYPOINT ["dotnet", "HealthCheck.dll"]