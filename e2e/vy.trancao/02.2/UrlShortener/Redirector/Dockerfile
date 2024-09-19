FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Redirector/Redirector.csproj", "Redirector/"]
COPY ["Common/Common.csproj", "Common/"]

RUN dotnet restore "./Redirector/Redirector.csproj"
RUN dotnet restore "./Common/Common.csproj"

COPY ./Redirector ./Redirector
COPY ./Common ./Common

WORKDIR "/src/Redirector"
RUN dotnet build "./Redirector.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Redirector.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS=81
ENTRYPOINT ["dotnet", "Redirector.dll"]
