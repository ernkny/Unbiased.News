# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Unbiased.News.Api/Unbiased.News.Api.csproj", "Unbiased.News.Api/"]
COPY ["Unbiased.News.Application/Unbiased.News.Application.csproj", "Unbiased.News.Application/"]
COPY ["Unbiased.News.Domain/Unbiased.News.Domain.csproj", "Unbiased.News.Domain/"]
COPY ["Unbiased.News.Common/Unbiased.News.Common.csproj", "Unbiased.News.Common/"]
COPY ["Unbiased.News.Infrastructure/Unbiased.News.Infrastructure.csproj", "Unbiased.News.Infrastructure/"]
COPY ["Unbiased.Shared.Dtos/Unbiased.Shared.Dtos.csproj", "Unbiased.Shared.Dtos/"]
COPY ["Unbiased.Shared.Extensions/Unbiased.Shared.Extensions.csproj", "Unbiased.Shared.Extensions/"]
COPY ["Unbiased.Log.Domain/Unbiased.Log.Domain.csproj", "Unbiased.Log.Domain/"]
RUN dotnet restore "./Unbiased.News.Api/Unbiased.News.Api.csproj"
COPY . .
WORKDIR "/src/Unbiased.News.Api"
RUN dotnet build "./Unbiased.News.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unbiased.News.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unbiased.News.Api.dll"]