FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebServer/WebServer.csproj", "WebServer/"]
COPY ["Common/Common.csproj", "Common/"]

RUN dotnet restore "./WebServer/WebServer.csproj"
RUN dotnet restore "./Common/Common.csproj"

COPY ./WebServer ./WebServer
COPY ./Common ./Common

WORKDIR "/src/WebServer"
RUN dotnet build "./WebServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS=80
ENTRYPOINT ["dotnet", "WebServer.dll"]
