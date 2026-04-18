# Unbiased - Unbiased News & Content Platform

Unbiased is a **.NET 8 based microservice news and content platform** that automatically scrapes news sources, regenerates them with AI, and serves them to consumers through multiple channels.

The system is built on top of web scraping (Playwright), OpenAI / Freepik AI integrations, AWS S3 image storage, RabbitMQ-based messaging, Quartz.NET scheduled workflows, and a YARP-based API Gateway.

DB script : https://github.com/ernkny/Unbiased.News/blob/main/UnbiasedNews.DbScript.sql
---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Tech Stack](#tech-stack)
- [Project Layout](#project-layout)
- [API Services](#api-services)
  - [Unbiased.ApiGateway](#1-unbiasedapigateway)
  - [Unbiased.Identity.Api](#2-unbiasedidentityapi)
  - [Unbiased.News.Api](#3-unbiasednewsapi)
  - [Unbiased.Dashboard.Api](#4-unbiaseddashboardapi)
  - [Unbiased.Playwright.Api](#5-unbiasedplaywrightapi)
  - [Unbiased.Log.Api](#6-unbiasedlogapi)
- [Shared Libraries](#shared-libraries)
- [Per-Service Layered Architecture](#per-service-layered-architecture)
- [Data Flow & Integrations](#data-flow--integrations)
- [Setup & Running](#setup--running)
- [Configuration](#configuration)
- [Port Map](#port-map)
- [Security](#security)

---

## Architecture Overview

Unbiased consists of several independent microservices written following **Clean Architecture** principles. Clients (frontend, mobile, admin panel) send all of their requests through the **API Gateway**, which forwards them to the appropriate microservice. Asynchronous inter-service communication is handled via **RabbitMQ / MassTransit**.

```
                      ┌──────────────────────────┐
                      │        Clients (UI)      │
                      │   Web / Mobile / Admin   │
                      └────────────┬─────────────┘
                                   │ HTTPS + ApiKey
                                   ▼
                      ┌──────────────────────────┐
                      │   Unbiased.ApiGateway    │  (YARP Reverse Proxy)
                      └────────────┬─────────────┘
                                   │
        ┌────────────┬─────────────┼─────────────┬──────────────┐
        ▼            ▼             ▼             ▼              ▼
   ┌─────────┐  ┌─────────┐  ┌──────────┐  ┌───────────┐  ┌──────────┐
   │Identity │  │  News   │  │Dashboard │  │Playwright │  │   Log    │
   │   API   │  │   API   │  │   API    │  │    API    │  │   API    │
   └────┬────┘  └────┬────┘  └─────┬────┘  └─────┬─────┘  └────▲─────┘
        │            │             │             │              │
        │            │             │             │   RabbitMQ   │
        └────────────┴─────────────┴─────────────┴──────────────┘
                                   │
                                   ▼
                ┌──────────────────────────────────┐
                │  MSSQL  ·  AWS S3  ·  OpenAI API │
                │  Freepik API  ·  Quartz Store    │
                └──────────────────────────────────┘
```

---

## Tech Stack

| Layer | Technologies |
| --- | --- |
| Runtime | .NET 8 |
| API Gateway | YARP (Yet Another Reverse Proxy) |
| Data Access | Dapper + Microsoft.Data.SqlClient (MSSQL) |
| Messaging | RabbitMQ + MassTransit |
| Scheduling | Quartz.NET (Persistent + Clustered) |
| Web Scraping | Microsoft.Playwright |
| AI Integration | OpenAI GPT, OpenAI DALL·E, Freepik Flux-Dev |
| Cloud Storage | AWS S3 (news images) |
| Authentication | JWT (Access + Refresh Token) + ApiKey Middleware |
| Validation | FluentValidation |
| Mediator / CQRS | MediatR |
| Resilience | Polly (Retry + Timeout) |
| Logging | Custom `EventAndActivityLog` (flows to Log.Api via RabbitMQ) |
| Documentation | Swagger / OpenAPI |

---

## Project Layout

Each microservice is modelled as a group of five projects following the **Api → Application → Domain → Infrastructure → Common** layering. The solution can be grouped as follows:

### 1) Microservice Groups

- **Unbiased.ApiGateway / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Identity.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.News.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Dashboard.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Playwright.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Log.Api / .Application / .Common / .Domain / .Infrastructure**

### 2) Shared Libraries

- `Unbiased.Shared.Dtos` — Common DTOs and configuration models used across services (`CustomTokenOption`, `Client`, etc.).
- `Unbiased.Shared.Extensions` — Shared middlewares (`GlobalExceptionMiddleware`, `ApiKeyAuthorizeMiddleware`), JWT extensions (`AddCustomTokenAuth`), `EventAndActivityLog` infrastructure.
- `Unbiased.Shared.ExceptionHandler.Middleware` — Global exception handling middleware.

---

## API Services

### 1. Unbiased.ApiGateway

The single entry point for all traffic — a **YARP-based reverse proxy**.

**Responsibilities:**

- Routes incoming requests to the correct microservice based on path templates (`/api/v1/News/...`, `/api/v1/playwright/...`, `/api/v1/identity/...`, `/api/v1/dashboard/...`, `/api/v1/logs/...`).
- Performs **API Key validation** (`ApiKeyAuthorizeMiddleware`). API keys are stored in the database and periodically refreshed by a hosted service called `ApiKeyRefresherService`.
- Applies CORS policies (`UnbiasedCorsPolicy`); origins can be loaded from configuration or from the `CORS_ORIGINS` environment variable.
- Forwards event / activity logs to Log.Api via RabbitMQ.
- Provides global exception handling.

**Key components:**

- `Program.cs` — YARP reverse proxy setup, CORS, MassTransit, ApiKey services.
- `ApiKeyService` / `ApiKeyRepository` — Management of API keys stored in the database.
- `ApiKeyRefresherService` — Background hosted service refreshing keys periodically.

### 2. Unbiased.Identity.Api

The **identity service** responsible for user, role and authorization management.

**Controllers and example endpoints:**

| Controller | Endpoint | Description |
| --- | --- | --- |
| `AuthenticationController` | `POST /Login` | Issues access + refresh tokens based on username / password. |
|  | `POST /refresh-token` | Issues a new access token using a refresh token. |
| `UserManagementController` | `GET /GetAllUsers`, `GET /GetAllUsersCount` | Paginated user list. |
|  | `GET /GetUserWithRoles` | Returns a user with their assigned roles. |
|  | `POST /InsertUserWithRoles` / `POST /UpdateUserWithRoles` / `DELETE /DeleteUserWithRoles` | User CRUD operations. |
| `RoleManagementController` | `GET /GetAllPagesWithPermissions` | All page & permission definitions. |
|  | `GET /GetAllRoles`, `GET /GetRoleById` | Role queries. |
|  | `POST /InsertRole` / `PUT /UpdateRole` / `DELETE /DeleteRole` | Role CRUD. |

**Features:**

- JWT (access + refresh) generation and validation (`TokenService`, `CustomTokenOption`).
- Policy-based authorization (`Access Control Get/Add/Update/Delete`, `News Get`, `Content Management Get`, etc.).
- All admin panel operations are secured by JWTs issued by this service.

### 3. Unbiased.News.Api

**Read-heavy public news API** that serves end users (public site).

**Controllers and example endpoints:**

| Controller | Primary Responsibility |
| --- | --- |
| `NewsController` | Listing generated news, detail pages, banner news, sitemap, statistics. (`/GetAllGeneratedNews`, `/GetAllGeneratedNewsWithImage`, `/GetGeneratedNewById`, `/GetGeneratedNewByUniqUrl`, `/GetBannerGeneratedNews`, `/GetAllNewsStatistics`, `/GetAllGeneratedNewsForSiteMap`, etc.) |
| `CategoryController` | Category list, article counts per category, most visited news, categories enriched with random news. |
| `ContentController` | Daily horoscope, daily content, subheadings, home page content block, content detail page, sitemap. |
| `BlogController` | Blog list with images, total count, blog detail by unique URL. |
| `ContactController` | `POST /SaveContactFormInformations` — Contact form submissions (FluentValidation). |

**Features:**

- Read performance optimization with `IMemoryCache`.
- Multi-language content support via the `language` parameter (TR / EN etc.).
- Pagination support (`pageNumber` / `pageSize`).
- All endpoints run under ApiKey (+ JWT where required).

### 4. Unbiased.Dashboard.Api

**Write-heavy content management API** for the admin panel. All endpoints are protected by policy-based JWT authorization.

**Controllers:**

| Controller | Primary Responsibility |
| --- | --- |
| `NewsDashboardController` | Listing generated news, updating them (`/UpdateGeneratedNews`), inserting new ones, image-based news operations. |
| `ContentDashboardController` | Content management, categories, content updates. |
| `BlogDashboardController` | Blog CRUD (`/GetAllBlogs`, `/InsertBlogs`, `/UpdateBlogs`, `/DeleteBlogs`). |
| `CustomerDashboardController` | Listing contact form submissions, detail and deletion. |
| `EngineDashboardController` | Configuration of the scraping / content generation engine. Activating / deactivating the engine (`/DeActiveOrActive`), triggering immediately (`/ActivateEngineImmediatly`), generating content from a URL (`/GenerateContent`). |

**Features:**

- **AWS S3 integration** (`AwsCredentials`, `S3Settings`) — Uploads images to an S3 bucket.
- `SocialMediaImageGenerator` — An HttpClient-based service that generates social media images triggered from the dashboard.
- Strong DTO validation via FluentValidation (`InsertNewsWithImageDtoValidator`, `UpdateGeneratedNewsWithImageDtoValidator`, `UpdateGeneratedContentValidator`).
- GPT-powered content generation flows initiated from the dashboard.

### 5. Unbiased.Playwright.Api

The **engine** of the system. Responsible for web scraping, content generation, image generation and scheduled jobs.

**Controller:** `NewsController`

| Endpoint | Description |
| --- | --- |
| `POST /InsertNews` | Manually insert a news item. |
| `GET /GetNewsWithPlaywright` | Scrapes the configured source URLs using Playwright. |
| `POST /GenerateNewsWithAI` | Takes raw scraped news and regenerates / summarizes / enriches them using OpenAI GPT. |
| `POST /GenerateImagesWhenAllNewsHasGenerated` | After all news are generated, creates images via DALL·E / Freepik, applies watermarks and uploads to S3. |

**Quartz.NET jobs** (automated workflows):

- `GetNewsWithPlaywrightWithSearchUrlJob` — Continuously scrapes news from registered search URLs.
- `ConsumeUnprocessedNewsJob` — Regenerates unprocessed news with AI.
- `ConsumeUnprocessedImagesJob` — Creates images for unprocessed news.
- `GetDailyContentDataJob` — Aggregates daily content (e.g. home page blocks).
- `GetDailyHoroscopeDataJob` — Generates daily horoscope commentary.
- `GetContentSubheadingsJob` / `ConsumeUnprocessContentSubheadings` — Generates and processes content subheadings.
- `UpdateScrappingRunTimes` (hosted service) — Updates scraping schedules.

**Features:**

- Quartz runs in **Persistent + Clustered** mode (SQL Server backed), so jobs can be shared across multiple instances.
- Retry + timeout strategies via Polly.
- **Chain of Responsibility** pipeline built on top of `AbstractHandlerChain` and `AbstractImageProcess` (e.g. `GetImageProcess`).
- External AI calls via `GptApiExternalService`, `GptDalleApiExternalService`, `FreepikApiExternalService`.
- API limit handling via custom exception types such as `TooManyRequestsException`.
- Generated news are uploaded to AWS S3 and stored as URLs in the database.

### 6. Unbiased.Log.Api

The **centralized log service.** Collects event and activity logs coming from all other microservices over RabbitMQ into a single database.

**Components:**

- `EventLogConsumer` — Consumes system event logs from the `EventLogMessageQueue` queue.
- `ActivityLogConsumer` — Consumes user / action logs from the `ActivityLogMessageQueue` queue.
- `LogService` / `LogRepository` — Used for querying logs.

**Features:**

- Source services push messages to RabbitMQ via `EventAndActivityLog` (Shared.Extensions); Log.Api listens and persists them.
- Protected by the ApiKey middleware.

---

## Shared Libraries

| Project | Description |
| --- | --- |
| `Unbiased.Shared.Dtos` | Shared configuration / DTO models used across services (e.g. `CustomTokenOption`, `Client`). |
| `Unbiased.Shared.Extensions` | `AddCustomTokenAuth` (JWT), `GlobalExceptionMiddleware`, `ApiKeyAuthorizeMiddleware`, `EventAndActivityLog`. |
| `Unbiased.Shared.ExceptionHandler.Middleware` | Standard exception handling middleware. |

---

## Per-Service Layered Architecture

All microservices share the following Clean Architecture skeleton:

```
Unbiased.<Service>.Api           → Presentation layer (Controllers, Program.cs, middleware wiring)
Unbiased.<Service>.Application   → Business rules (Services, Validators, Mappings, MediatR handlers)
Unbiased.<Service>.Domain        → Entities, DTOs, domain models
Unbiased.<Service>.Infrastructure→ Repositories (Dapper), DB connections, external service clients
Unbiased.<Service>.Common        → Helpers, shared abstractions, constants
```

In addition, the Playwright service contains:

- `Unbiased.Playwright.Application/Jobs` → Quartz jobs
- `Unbiased.Playwright.Application/Playwright` → Chain of Responsibility based scraping pipeline
- `Unbiased.Playwright.Infrastructure/Concrete/ExternalServices` → GPT / DALL·E / Freepik clients

---

## Data Flow & Integrations

### 1) News Generation Flow (Engine)

```
Quartz Job (GetNewsWithPlaywrightWithSearchUrlJob)
      │
      ▼
Target sites are scraped with Playwright → raw news is persisted (IsProcessed=false)
      │
      ▼
ConsumeUnprocessedNewsJob → OpenAI GPT → Rewritten / unbiased content is produced
      │
      ▼
ConsumeUnprocessedImagesJob → DALL·E or Freepik → Image generated, watermark applied → uploaded to S3
      │
      ▼
News.Api & Dashboard.Api → Serve the generated content to consumers
```

### 2) Log Flow

```
Any service → EventAndActivityLog.Log(...) → RabbitMQ → Log.Api consumer → LogDB
```

### 3) Authentication Flow

```
Admin Panel → POST /Login (Identity.Api) → JWT
     │
     ▼
For every request: Authorization: Bearer <token> → API Gateway → target microservice
     │
     ▼
Policy check via [Authorize(Policy = "...")] on the target endpoint
```

---

## Setup & Running

### Requirements

- .NET 8 SDK
- SQL Server (local or remote)
- RabbitMQ (default: `localhost`, user `guest` / password `guest`)
- `Microsoft.Playwright` browser dependencies: `pwsh bin/Debug/net8.0/playwright.ps1 install`
- AWS S3 bucket (for images) — optional
- OpenAI API key + Freepik API key — optional (required for AI features)

### Quick Start

The PowerShell script at the repository root starts every service in separate windows:

```powershell
./run-local.ps1
```

To stop:

```powershell
./stop-local.ps1
```

Alternatively you can start each service manually:

```bash
dotnet run --project Unbiased.ApiGateway --launch-profile http
dotnet run --project Unbiased.News.Api --launch-profile http
dotnet run --project Unbiased.Playwright.Api --launch-profile http
dotnet run --project Unbiased.Log.Api --launch-profile http
dotnet run --project Unbiased.Identity.Api --launch-profile http
dotnet run --project Unbiased.Dashboard.Api --launch-profile http
```

---

## Configuration

Every service contains two files at its root:

- `appsettings.json` — Production / shared settings
- `appsettings.Development.json` — Local development settings

> **Note:** Sensitive fields (connection strings, JWT `SecurityKey`, OpenAI / Freepik / AWS keys, client secret, etc.) in the committed `appsettings.*.json` files are **intentionally left empty**. When running locally, fill them for your environment or use User Secrets / environment variables.

### Key Configuration Settings

| Section | Example Key | Usage |
| --- | --- | --- |
| `ConnectionStrings` | `UnbiasedSqlConnection` | MSSQL connection string |
|  | `RabbitMqUrl` | RabbitMQ host |
|  | `CorsApi` (Gateway) | Comma-separated list of CORS origins |
| `TokenOption` | `Audience`, `Issuer`, `AccessTokenExpiration`, `RefreshTokenExpiration`, `SecurityKey` | JWT configuration |
| `Clients.Unbiased` | `Id`, `Secret`, `Audience` | Trusted client configuration |
| `FilePaths.ApiKeyPath` | File path | Local API key file |
| `Keys` | `GptApiKey`, `FreepikApiKey` | AI service keys |
| `S3Settings` | `AccessKey`, `SecretKey`, `BucketName` | AWS S3 access |
| `Urls` | `GptApi`, `GptDalleApi`, `FreePikApi` | External AI endpoints |
| `ReverseProxy` (Gateway) | `Routes`, `Clusters` | YARP routing definitions |

---

## Port Map

Ports used during local development (HTTP):

| Service | Port |
| --- | --- |
| Unbiased.ApiGateway | `5000` |
| Unbiased.News.Api | `5001` |
| Unbiased.Playwright.Api | `5002` |
| Unbiased.Log.Api | `5003` |
| Unbiased.Identity.Api | `5004` |
| Unbiased.Dashboard.Api | `5005` |

Clients should only send traffic through **`http://localhost:5000`** (the Gateway). The Gateway routes paths to services as follows:

| Gateway Path | Target Service |
| --- | --- |
| `/api/v1/News/{**catch-all}` | News.Api (5001) |
| `/api/v1/playwright/{**catch-all}` | Playwright.Api (5002) |
| `/api/v1/logs/{**catch-all}` | Log.Api (5003) |
| `/api/v1/identity/{**catch-all}` | Identity.Api (5004) |
| `/api/v1/dashboard/{**catch-all}` | Dashboard.Api (5005) |

---

## Security

The system uses **two layers of security**:

1. **API Key Middleware** (`ApiKeyAuthorizeMiddleware`) — Every request arriving at any service is validated against an API key defined under `FilePaths.ApiKeyPath` and stored in the database. The `ApiKeyRefresherService` in the Gateway refreshes these keys periodically.
2. **JWT + Policy-based Authorization** — Admin panel endpoints are protected with `[Authorize(Policy = "...")]`. Policies are defined at page / permission level by `RoleManagement` in the Identity service.

Additionally:

- All APIs call `UseHttpsRedirection()`.
- `GlobalExceptionMiddleware` catches every unexpected exception and transforms it into a standard JSON error body.
- Logs are centralized in Log.Api; events are propagated asynchronously via RabbitMQ so that source services are never blocked by log writes.

---

## License & Contact

This repository belongs to a private project. For contributions, bug reports or questions, please reach out to the project owner.
