## See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

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
COPY ["Unbiased.ApiGateway/Unbiased.ApiGateway.csproj", "Unbiased.ApiGateway/"]
COPY ["Unbiased.ApiGateway.Application/Unbiased.ApiGateway.Application.csproj", "Unbiased.ApiGateway.Application/"]
COPY ["Unbiased.ApiGateway.Infrastructure/Unbiased.ApiGateway.Infrastructure.csproj", "Unbiased.ApiGateway.Infrastructure/"]
COPY ["Unbiased.ApiGateway.Domain/Unbiased.ApiGateway.Domain.csproj", "Unbiased.ApiGateway.Domain/"]
COPY ["Unbiased.ApiGateway.Common/Unbiased.ApiGateway.Common.csproj", "Unbiased.ApiGateway.Common/"]
COPY ["Unbiased.Shared.Extensions/Unbiased.Shared.Extensions.csproj", "Unbiased.Shared.Extensions/"]
COPY ["Unbiased.Log.Domain/Unbiased.Log.Domain.csproj", "Unbiased.Log.Domain/"]
COPY ["Unbiased.Shared.Dtos/Unbiased.Shared.Dtos.csproj", "Unbiased.Shared.Dtos/"]
RUN dotnet restore "./Unbiased.ApiGateway/Unbiased.ApiGateway.csproj"
COPY . .
WORKDIR "/src/Unbiased.ApiGateway"
RUN dotnet build "./Unbiased.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unbiased.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unbiased.ApiGateway.dll"]