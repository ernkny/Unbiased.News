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
COPY ["Unbiased.Log.Api/Unbiased.Log.Api.csproj", "Unbiased.Log.Api/"]
COPY ["Unbiased.Log.Application/Unbiased.Log.Application.csproj", "Unbiased.Log.Application/"]
COPY ["Unbiased.Log.Common/Unbiased.Log.Common.csproj", "Unbiased.Log.Common/"]
COPY ["Unbiased.Log.Domain/Unbiased.Log.Domain.csproj", "Unbiased.Log.Domain/"]
COPY ["Unbiased.Log.Infrastructure/Unbiased.Log.Infrastructure.csproj", "Unbiased.Log.Infrastructure/"]
COPY ["Unbiased.Shared.Extensions/Unbiased.Shared.Extensions.csproj", "Unbiased.Shared.Extensions/"]
COPY ["Unbiased.Shared.Dtos/Unbiased.Shared.Dtos.csproj", "Unbiased.Shared.Dtos/"]
RUN dotnet restore "./Unbiased.Log.Api/Unbiased.Log.Api.csproj"
COPY . .
WORKDIR "/src/Unbiased.Log.Api"
RUN dotnet build "./Unbiased.Log.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unbiased.Log.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unbiased.Log.Api.dll"]