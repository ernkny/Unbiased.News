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
COPY ["Unbiased.Playwright.Api/Unbiased.Playwright.Api.csproj", "Unbiased.Playwright.Api/"]
COPY ["Unbiased.Playwright.Application/Unbiased.Playwright.Application.csproj", "Unbiased.Playwright.Application/"]
COPY ["Unbiased.Playwright.Common/Unbiased.Playwright.Common.csproj", "Unbiased.Playwright.Common/"]
COPY ["Unbiased.Playwright.Domain/Unbiased.Playwright.Domain.csproj", "Unbiased.Playwright.Domain/"]
COPY ["Unbiased.Playwright.Infrastructure/Unbiased.Playwright.Infrastructure.csproj", "Unbiased.Playwright.Infrastructure/"]
COPY ["Unbiased.Shared.Extensions/Unbiased.Shared.Extensions.csproj", "Unbiased.Shared.Extensions/"]
COPY ["Unbiased.Log.Domain/Unbiased.Log.Domain.csproj", "Unbiased.Log.Domain/"]
COPY ["Unbiased.Shared.Dtos/Unbiased.Shared.Dtos.csproj", "Unbiased.Shared.Dtos/"]
RUN dotnet restore "./Unbiased.Playwright.Api/Unbiased.Playwright.Api.csproj"
COPY . .
WORKDIR "/src/Unbiased.Playwright.Api"
RUN dotnet build "./Unbiased.Playwright.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unbiased.Playwright.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unbiased.Playwright.Api.dll"]