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
COPY ["Unbiased.Identity.Api/Unbiased.Identity.Api.csproj", "Unbiased.Identity.Api/"]
COPY ["Unbiased.Identity.Application/Unbiased.Identity.Application.csproj", "Unbiased.Identity.Application/"]
COPY ["Unbiased.Identity.Common/Unbiased.Identity.Common.csproj", "Unbiased.Identity.Common/"]
COPY ["Unbiased.Identity.Infrastructure/Unbiased.Identity.Infrastructure.csproj", "Unbiased.Identity.Infrastructure/"]
COPY ["Unbiased.Identity.Domain/Unbiased.Identity.Domain.csproj", "Unbiased.Identity.Domain/"]
COPY ["Unbiased.Shared.Dtos/Unbiased.Shared.Dtos.csproj", "Unbiased.Shared.Dtos/"]
COPY ["Unbiased.Shared.Extensions/Unbiased.Shared.Extensions.csproj", "Unbiased.Shared.Extensions/"]
COPY ["Unbiased.Log.Domain/Unbiased.Log.Domain.csproj", "Unbiased.Log.Domain/"]
RUN dotnet restore "./Unbiased.Identity.Api/Unbiased.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Unbiased.Identity.Api"
RUN dotnet build "./Unbiased.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unbiased.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unbiased.Identity.Api.dll"]