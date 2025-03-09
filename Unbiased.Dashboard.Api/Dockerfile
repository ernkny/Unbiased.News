# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Unbiased.Dashboard.Api/Unbiased.Dashboard.Api.csproj", "Unbiased.Dashboard.Api/"]
COPY ["Unbiased.Dashboard.Application/Unbiased.Dashboard.Application.csproj", "Unbiased.Dashboard.Application/"]
COPY ["Unbiased.Dashboard.Infrastructure/Unbiased.Dashboard.Infrastructure.csproj", "Unbiased.Dashboard.Infrastructure/"]
COPY ["Unbiased.Dashboard.Domain/Unbiased.Dashboard.Domain.csproj", "Unbiased.Dashboard.Domain/"]
COPY ["Unbiased.Dashboard.Common/Unbiased.Dashboard.Common.csproj", "Unbiased.Dashboard.Common/"]
COPY ["Unbiased.Shared.Dtos/Unbiased.Shared.Dtos.csproj", "Unbiased.Shared.Dtos/"]
COPY ["Unbiased.Shared.Extensions/Unbiased.Shared.Extensions.csproj", "Unbiased.Shared.Extensions/"]
COPY ["Unbiased.Log.Domain/Unbiased.Log.Domain.csproj", "Unbiased.Log.Domain/"]
RUN dotnet restore "./Unbiased.Dashboard.Api/Unbiased.Dashboard.Api.csproj"
COPY . .
WORKDIR "/src/Unbiased.Dashboard.Api"
RUN dotnet build "./Unbiased.Dashboard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unbiased.Dashboard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unbiased.Dashboard.Api.dll"]