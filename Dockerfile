
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:8080;http://+:8081"
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["AuthService.API/AuthService.API.csproj", "AuthService.API/"]
COPY ["AuthService.BO/AuthService.BO.csproj", "AuthService.BO/"]
COPY ["AuthService.DAO/AuthService.DAO.csproj", "AuthService.DAO/"]
COPY ["AuthService.Repository/AuthService.Repository.csproj", "AuthService.Repository/"]
COPY ["AuthService.Service/AuthService.Service.csproj", "AuthService.Service/"]

RUN dotnet restore "AuthService.API/AuthService.API.csproj"

COPY . .
WORKDIR "/src/AuthService.API"
RUN dotnet build "./AuthService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER app
ENTRYPOINT ["dotnet", "AuthService.API.dll"]