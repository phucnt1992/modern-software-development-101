FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/SimpleHealthcheckApi/SimpleHealthcheckApi.csproj", "SimpleHealthcheckApi/"]
RUN dotnet restore "SimpleHealthcheckApi/SimpleHealthcheckApi.csproj"
COPY src/SimpleHealthcheckApi SimpleHealthcheckApi/
WORKDIR "/src/SimpleHealthcheckApi"
RUN dotnet build "SimpleHealthcheckApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SimpleHealthcheckApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleHealthcheckApi.dll"]
