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

# Install necessary dependencies for Playwright
USER root
RUN apt-get update && \
    apt-get install -y wget gnupg2 libatk1.0-0 libatk-bridge2.0-0 libcups2 libdrm2 libxkbcommon0 \
    libxcomposite1 libxdamage1 libxfixes3 libxrandr2 libgbm1 libasound2 \
    ca-certificates curl unzip xvfb \
    # Firefox dependencies
    libdbus-glib-1-2 libxt6 \
    # Chrome dependencies
    libglib2.0-0 libnss3 libnspr4 libxss1 libx11-xcb1 \
    # GTK and additional dependencies for Playwright
    libgtk-3-0 libpangocairo-1.0-0 libpango-1.0-0 \
    libcairo-gobject2 libcairo2 libgdk-pixbuf-2.0-0 libfontconfig1 \
    libxcursor1 libxi6 libxtst6 \
    # Additional dependencies
    fonts-liberation fonts-noto-color-emoji && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Create and set permissions for Playwright directories
RUN mkdir -p /ms-playwright/firefox-1465/firefox && \
    mkdir -p /ms-playwright/chromium-1140/chrome-linux && \
    chmod -R 777 /ms-playwright && \
    chmod -R 777 /app

# Download and install Firefox
RUN cd /ms-playwright/firefox-1465 && \
    wget -q "https://playwright.azureedge.net/builds/firefox/1465/firefox-ubuntu-20.04.zip" -O firefox.zip && \
    unzip firefox.zip && \
    rm firefox.zip && \
    chmod -R 777 /ms-playwright/firefox-1465

# Download and install Chromium
RUN cd /ms-playwright/chromium-1140 && \
    wget -q "https://playwright.azureedge.net/builds/chromium/1140/chromium-linux.zip" -O chromium.zip && \
    unzip chromium.zip && \
    rm chromium.zip && \
    chmod -R 777 /ms-playwright/chromium-1140

# Switch back to non-root user for security
USER $APP_UID

ENTRYPOINT ["dotnet", "Unbiased.Playwright.Api.dll"]