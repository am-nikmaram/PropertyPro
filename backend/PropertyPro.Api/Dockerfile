# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/PropertyPro.Api/PropertyPro.Api.csproj", "backend/PropertyPro.Api/"]
COPY ["backend/PropertyPro.Application/PropertyPro.Application.csproj", "backend/PropertyPro.Application/"]
COPY ["backend/PropertyPro.Domain/PropertyPro.Domain.csproj", "backend/PropertyPro.Domain/"]
COPY ["backend/PropertyPro.Infrastructure/PropertyPro.Infrastructure.csproj", "backend/PropertyPro.Infrastructure/"]
RUN dotnet restore "./backend/PropertyPro.Api/PropertyPro.Api.csproj"
COPY . .
WORKDIR "/src/backend/PropertyPro.Api"
RUN dotnet build "./PropertyPro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PropertyPro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropertyPro.Api.dll"]