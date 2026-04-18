# English Read Me File : https://github.com/ernkny/Unbiased.News/blob/main/README.en.md

# Unbiased - Tarafsız Haber ve İçerik Platformu - Turkish

Unbiased, haber kaynaklarını otomatik olarak tarayan, yapay zekâ ile yeniden üreten ve birden fazla kanal üzerinden tüketicilere sunan **.NET 8 tabanlı, mikroservis mimarisinde** bir haber ve içerik platformudur.

Sistem; web scraping (Playwright), OpenAI/Freepik AI entegrasyonları, AWS S3 görsel depolama, RabbitMQ tabanlı mesajlaşma, Quartz.NET ile zamanlanmış iş akışları ve YARP tabanlı bir API Gateway üzerine inşa edilmiştir.

DB script : https://github.com/ernkny/Unbiased.News/blob/main/UnbiasedNews.DbScript.sql
---

## İçindekiler

- [Mimari Genel Bakış](#mimari-genel-bakış)
- [Teknoloji Yığını](#teknoloji-yığını)
- [Proje Yapısı](#proje-yapısı)
- [API Servisleri](#api-servisleri)
  - [Unbiased.ApiGateway](#1-unbiasedapigateway)
  - [Unbiased.Identity.Api](#2-unbiasedidentityapi)
  - [Unbiased.News.Api](#3-unbiasednewsapi)
  - [Unbiased.Dashboard.Api](#4-unbiaseddashboardapi)
  - [Unbiased.Playwright.Api](#5-unbiasedplaywrightapi)
  - [Unbiased.Log.Api](#6-unbiasedlogapi)
- [Ortak (Shared) Katmanlar](#ortak-shared-katmanlar)
- [Her Servisin Katman Mimarisi](#her-servisin-katman-mimarisi)
- [Veri Akışı ve Entegrasyonlar](#veri-akışı-ve-entegrasyonlar)
- [Kurulum ve Çalıştırma](#kurulum-ve-çalıştırma)
- [Konfigürasyon](#konfigürasyon)
- [Port Haritası](#port-haritası)
- [Güvenlik](#güvenlik)

---

## Mimari Genel Bakış

Unbiased, **Clean Architecture** prensiplerine uygun olarak yazılmış birden fazla bağımsız mikroservisten oluşur. İstemciler (frontend, mobil, yönetim paneli) tüm isteklerini **API Gateway** üzerinden yapar; Gateway istekleri ilgili mikroservise yönlendirir. Servisler arası asenkron iletişim **RabbitMQ / MassTransit** ile sağlanır.

```
                      ┌──────────────────────────┐
                      │     İstemciler (UI)      │
                      │  Web / Mobil / Dashboard │
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

## Teknoloji Yığını

| Katman | Teknolojiler |
| --- | --- |
| Runtime | .NET 8 |
| API Gateway | YARP (Yet Another Reverse Proxy) |
| Veri Erişimi | Dapper + Microsoft.Data.SqlClient (MSSQL) |
| Mesajlaşma | RabbitMQ + MassTransit |
| Zamanlayıcı | Quartz.NET (Persistent + Clustered) |
| Web Scraping | Microsoft.Playwright |
| AI Entegrasyonu | OpenAI GPT, OpenAI DALL·E, Freepik Flux-Dev |
| Bulut Depolama | AWS S3 (haber görselleri) |
| Kimlik Doğrulama | JWT (Access + Refresh Token) + ApiKey Middleware |
| Doğrulama | FluentValidation |
| Mediator / CQRS | MediatR |
| Resilience | Polly (Retry + Timeout) |
| Logging | Custom `EventAndActivityLog` (RabbitMQ üzerinden Log.Api'ye akar) |
| Dokümantasyon | Swagger / OpenAPI |

---

## Proje Yapısı

Her mikroservis; **Api → Application → Domain → Infrastructure → Common** katmanlarını barındıran 5'li bir grup hâlinde modellenmiştir. Çözüm içindeki tüm projeler şu şekilde gruplanabilir:

### 1) Mikroservis Grupları

- **Unbiased.ApiGateway / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Identity.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.News.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Dashboard.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Playwright.Api / .Application / .Common / .Domain / .Infrastructure**
- **Unbiased.Log.Api / .Application / .Common / .Domain / .Infrastructure**

### 2) Paylaşılan (Shared) Kütüphaneler

- `Unbiased.Shared.Dtos` — Tüm servislerin kullandığı ortak DTO'lar ve konfigürasyon modelleri (`CustomTokenOption`, `Client` vb.).
- `Unbiased.Shared.Extensions` — Ortak middleware'ler (`GlobalExceptionMiddleware`, `ApiKeyAuthorizeMiddleware`), JWT eklentileri (`AddCustomTokenAuth`), `EventAndActivityLog` altyapısı.
- `Unbiased.Shared.ExceptionHandler.Middleware` — Global hata yakalama middleware'i.

---

## API Servisleri

### 1. Unbiased.ApiGateway

Tüm trafiğin tek giriş noktası olan **YARP tabanlı ters proxy**.

**Sorumlulukları:**

- Gelen istekleri path şablonlarına göre doğru mikroservise yönlendirir (`/api/v1/News/...`, `/api/v1/playwright/...`, `/api/v1/identity/...`, `/api/v1/dashboard/...`, `/api/v1/logs/...`).
- **API Key doğrulaması** yapar (`ApiKeyAuthorizeMiddleware`). API anahtarları veritabanında tutulur ve `ApiKeyRefresherService` adlı hosted service ile periyodik olarak yenilenir.
- CORS politikalarını (`UnbiasedCorsPolicy`) uygular; origin'ler hem konfigürasyondan hem `CORS_ORIGINS` env değişkeninden okunabilir.
- RabbitMQ üzerinden olay/aktivite loglarını Log.Api'ye iletir.
- Global exception handling sağlar.

**Önemli bileşenler:**

- `Program.cs` — YARP reverse proxy yapılandırması, CORS, MassTransit, ApiKey servisleri.
- `ApiKeyService` / `ApiKeyRepository` — Veritabanındaki API key'lerin yönetimi.
- `ApiKeyRefresherService` — Arka planda key'leri yenileyen hosted service.

### 2. Unbiased.Identity.Api

Kullanıcı, rol ve yetkilendirme yönetiminden sorumlu **kimlik servisi**.

**Controller'lar ve endpoint örnekleri:**

| Controller | Endpoint | Açıklama |
| --- | --- | --- |
| `AuthenticationController` | `POST /Login` | Kullanıcı adı/şifre ile access + refresh token üretir. |
|  | `POST /refresh-token` | Refresh token ile yeni access token üretir. |
| `UserManagementController` | `GET /GetAllUsers`, `GET /GetAllUsersCount` | Sayfalı kullanıcı listesi. |
|  | `GET /GetUserWithRoles` | Belirli kullanıcıyı rolleriyle getirir. |
|  | `POST /InsertUserWithRoles` / `POST /UpdateUserWithRoles` / `DELETE /DeleteUserWithRoles` | CRUD işlemleri. |
| `RoleManagementController` | `GET /GetAllPagesWithPermissions` | Tüm sayfa & yetki tanımları. |
|  | `GET /GetAllRoles`, `GET /GetRoleById` | Rol sorguları. |
|  | `POST /InsertRole` / `PUT /UpdateRole` / `DELETE /DeleteRole` | Rol CRUD. |

**Özellikler:**

- JWT (access + refresh) üretim ve doğrulaması (`TokenService`, `CustomTokenOption`).
- Policy tabanlı yetkilendirme (`Access Control Get/Add/Update/Delete`, `News Get`, `Content Management Get` vb.).
- Tüm admin panel işlemleri bu servisin ürettiği JWT ile güvenlik altına alınır.

### 3. Unbiased.News.Api

**Son kullanıcıya (public site) servis veren okuma ağırlıklı haber API'si.**

**Controller'lar ve endpoint örnekleri:**

| Controller | Temel Görev |
| --- | --- |
| `NewsController` | Üretilmiş haberlerin listelenmesi, detay sayfaları, banner haberler, site map, istatistikler. (`/GetAllGeneratedNews`, `/GetAllGeneratedNewsWithImage`, `/GetGeneratedNewById`, `/GetGeneratedNewByUniqUrl`, `/GetBannerGeneratedNews`, `/GetAllNewsStatistics`, `/GetAllGeneratedNewsForSiteMap` vb.) |
| `CategoryController` | Kategori listesi, kategori bazlı haber sayıları, en çok okunan haberler, rastgele haberlerle zenginleştirilmiş kategori listesi. |
| `ContentController` | Günlük burç, günlük içerik, alt başlıklar, ana sayfa içerik bloğu, içerik detay sayfası, site map. |
| `BlogController` | Görsel ekli blog listesi, toplam sayı, uniq URL ile blog detayı. |
| `ContactController` | `POST /SaveContactFormInformations` — İletişim formu kayıtları (FluentValidation). |

**Özellikler:**

- `IMemoryCache` ile okuma performansı optimizasyonu.
- `language` parametresiyle çok dilli içerik desteği (TR / EN vb.).
- Sayfalama (pageNumber / pageSize) desteği.
- Tüm endpoint'ler ApiKey + (gerektiğinde) JWT altında çalışır.

### 4. Unbiased.Dashboard.Api

Admin/yönetim paneli için **yazma ağırlıklı içerik yönetimi API'si**. Tüm endpoint'ler policy tabanlı JWT yetkilendirmesi ile korunur.

**Controller'lar:**

| Controller | Temel Görev |
| --- | --- |
| `NewsDashboardController` | Üretilmiş haberlerin listelenmesi, güncellenmesi (`/UpdateGeneratedNews`), yeni haber eklenmesi, görselli haber operasyonları. |
| `ContentDashboardController` | İçerik yönetimi, kategoriler, içerik güncelleme. |
| `BlogDashboardController` | Blog CRUD (`/GetAllBlogs`, `/InsertBlogs`, `/UpdateBlogs`, `/DeleteBlogs`). |
| `CustomerDashboardController` | İletişim formlarının listelenmesi, detay ve silme. |
| `EngineDashboardController` | Scraping / içerik üretim motorunun konfigürasyonu. Motoru aktif/pasif yapma (`/DeActiveOrActive`), anında tetikleme (`/ActivateEngineImmediatly`), URL bazlı içerik üretme (`/GenerateContent`). |

**Özellikler:**

- **AWS S3 entegrasyonu** (`AwsCredentials`, `S3Settings`) — Görselleri S3 bucket'a yükler.
- `SocialMediaImageGenerator` — Dashboard üzerinden sosyal medya görselleri oluşturan HttpClient tabanlı servis.
- FluentValidation ile güçlü DTO doğrulamaları (`InsertNewsWithImageDtoValidator`, `UpdateGeneratedNewsWithImageDtoValidator`, `UpdateGeneratedContentValidator`).
- GPT entegrasyonu ile dashboard'dan başlatılan içerik üretim akışları.

### 5. Unbiased.Playwright.Api

Sistemin **"motoru"** (engine). Web scraping, içerik üretimi, görsel üretimi ve zamanlanmış işlerden sorumludur.

**Controller:** `NewsController`

| Endpoint | Açıklama |
| --- | --- |
| `POST /InsertNews` | Manuel haber ekleme. |
| `GET /GetNewsWithPlaywright` | Tanımlı kaynak URL'lerinden Playwright ile scraping yapar. |
| `POST /GenerateNewsWithAI` | Scraping ile alınan ham haberleri OpenAI GPT ile yeniden yazdırır, özetletir ve zenginleştirir. |
| `POST /GenerateImagesWhenAllNewsHasGenerated` | Tüm haberler üretildikten sonra DALL·E / Freepik ile görselleri oluşturur, watermark ekler ve S3'e yükler. |

**Quartz.NET Job'ları** (otomatik çalışan iş akışları):

- `GetNewsWithPlaywrightWithSearchUrlJob` — Kayıtlı arama URL'lerinden sürekli haber çeker.
- `ConsumeUnprocessedNewsJob` — İşlenmemiş haberleri AI ile yeniden üretir.
- `ConsumeUnprocessedImagesJob` — İşlenmemiş haberlere görsel oluşturur.
- `GetDailyContentDataJob` — Günlük içerikleri toplar (örn. ana sayfa blokları).
- `GetDailyHoroscopeDataJob` — Günlük burç yorumlarını üretir.
- `GetContentSubheadingsJob` / `ConsumeUnprocessContentSubheadings` — İçerik alt başlıklarını oluşturur ve işler.
- `UpdateScrappingRunTimes` (hosted service) — Scraping zamanlamalarını günceller.

**Özellikler:**

- Quartz **Persistent + Clustered** modda (SQL Server destekli) çalışır; birden fazla instance arasında job paylaşımı yapılabilir.
- Polly ile retry + timeout stratejileri.
- `AbstractHandlerChain` ve `AbstractImageProcess` üzerine kurulu **Chain of Responsibility** mimarisi (`GetImageProcess` gibi).
- `GptApiExternalService`, `GptDalleApiExternalService`, `FreepikApiExternalService` üzerinden harici AI çağrıları.
- `TooManyRequestsException` gibi özel exception türleri ile API limiti yönetimi.
- Üretilen haberler AWS S3'e yüklenir ve veritabanında URL olarak saklanır.

### 6. Unbiased.Log.Api

**Merkezi log servisi.** Diğer tüm mikroservislerden RabbitMQ aracılığıyla gelen olay ve aktivite loglarını tek bir veritabanında toplar.

**Bileşenler:**

- `EventLogConsumer` — `EventLogMessageQueue` kuyruğundaki sistem olay loglarını tüketir.
- `ActivityLogConsumer` — `ActivityLogMessageQueue` kuyruğundaki kullanıcı/aksiyon loglarını tüketir.
- `LogService` / `LogRepository` — Logların sorgulanması için kullanılır.

**Özellikler:**

- Kaynak servisler `EventAndActivityLog` (Shared.Extensions) üzerinden RabbitMQ'ya mesaj gönderir; Log.Api dinler ve DB'ye yazar.
- ApiKey middleware ile korunur.

---

## Ortak (Shared) Katmanlar

| Proje | Açıklama |
| --- | --- |
| `Unbiased.Shared.Dtos` | Tüm servisler arası paylaşılan konfigürasyon / DTO modelleri (ör. `CustomTokenOption`, `Client`). |
| `Unbiased.Shared.Extensions` | `AddCustomTokenAuth` (JWT), `GlobalExceptionMiddleware`, `ApiKeyAuthorizeMiddleware`, `EventAndActivityLog`. |
| `Unbiased.Shared.ExceptionHandler.Middleware` | Standart hata yakalama middleware'i. |

---

## Her Servisin Katman Mimarisi

Tüm mikroservisler aşağıdaki Clean Architecture iskeletine sahiptir:

```
Unbiased.<Service>.Api           → Sunum katmanı (Controllers, Program.cs, Middleware kurulumu)
Unbiased.<Service>.Application   → İş kuralları (Services, Validators, Mappings, MediatR handlers)
Unbiased.<Service>.Domain        → Entity'ler, DTO'lar, domain modelleri
Unbiased.<Service>.Infrastructure→ Repository'ler (Dapper), DB bağlantıları, harici servis istemcileri
Unbiased.<Service>.Common        → Helpers, ortak abstraction'lar, sabitler
```

Playwright servisinde ek olarak:

- `Unbiased.Playwright.Application/Jobs` → Quartz job'ları
- `Unbiased.Playwright.Application/Playwright` → Chain of Responsibility temelli scraping pipeline
- `Unbiased.Playwright.Infrastructure/Concrete/ExternalServices` → GPT / DALL·E / Freepik istemcileri

---

## Veri Akışı ve Entegrasyonlar

### 1) Haber Üretim Akışı (Engine)

```
Quartz Job (GetNewsWithPlaywrightWithSearchUrlJob)
      │
      ▼
Playwright ile hedef siteler taranır → ham haber DB'ye kaydedilir (IsProcessed=false)
      │
      ▼
ConsumeUnprocessedNewsJob → OpenAI GPT → Yeniden yazılmış / tarafsız içerik üretilir
      │
      ▼
ConsumeUnprocessedImagesJob → DALL·E veya Freepik → Görsel üretilir, watermark eklenir → S3'e yüklenir
      │
      ▼
News.Api & Dashboard.Api → Üretilmiş içerikleri tüketicilere sunar
```

### 2) Log Akışı

```
Herhangi bir servis → EventAndActivityLog.Log(...) → RabbitMQ → Log.Api Consumer → LogDB
```

### 3) Kimlik Doğrulama Akışı

```
Admin Panel → POST /Login (Identity.Api) → JWT
     │
     ▼
Her istekte Authorization: Bearer <token> → API Gateway → ilgili mikroservis
     │
     ▼
İlgili endpoint'teki [Authorize(Policy = "...")] policy kontrolü
```

---

## Kurulum ve Çalıştırma

### Gereksinimler

- .NET 8 SDK
- SQL Server (local veya uzak)
- RabbitMQ (default: `localhost`, kullanıcı `guest` / şifre `guest`)
- `Microsoft.Playwright` tarayıcı bağımlılıkları: `pwsh bin/Debug/net8.0/playwright.ps1 install`
- AWS S3 bucket (görseller için) — opsiyonel
- OpenAI API Key + Freepik API Key — opsiyonel (AI özellikleri için)

### Hızlı Başlangıç

Kök dizindeki PowerShell betiği tüm servisleri ayrı pencerelerde ayağa kaldırır:

```powershell
./run-local.ps1
```

Durdurmak için:

```powershell
./stop-local.ps1
```

Alternatif olarak her servisi manuel başlatabilirsiniz:

```bash
dotnet run --project Unbiased.ApiGateway --launch-profile http
dotnet run --project Unbiased.News.Api --launch-profile http
dotnet run --project Unbiased.Playwright.Api --launch-profile http
dotnet run --project Unbiased.Log.Api --launch-profile http
dotnet run --project Unbiased.Identity.Api --launch-profile http
dotnet run --project Unbiased.Dashboard.Api --launch-profile http
```

### Docker

Her API projesi kendi `Dockerfile` ile gelir; containerize deployment için uygundur.

---

## Konfigürasyon

Her servisin kökünde iki dosya bulunur:

- `appsettings.json` — Prod/ortak ayarlar
- `appsettings.Development.json` — Lokal geliştirme ayarları

> **Not:** Repoya commitlenen `appsettings.*.json` dosyalarındaki hassas alanlar (connection string, JWT `SecurityKey`, OpenAI / Freepik / AWS anahtarları, client secret vb.) **bilinçli olarak boş bırakılmıştır**. Lokal çalışırken bu değerleri kendi ortamınıza göre doldurun veya User Secrets / environment variables kullanın.

### Önemli Ayar Anahtarları

| Bölüm | Örnek Anahtar | Kullanım |
| --- | --- | --- |
| `ConnectionStrings` | `UnbiasedSqlConnection` | MSSQL bağlantı cümlesi |
|  | `RabbitMqUrl` | RabbitMQ host |
|  | `CorsApi` (Gateway) | Virgülle ayrılmış CORS origin listesi |
| `TokenOption` | `Audience`, `Issuer`, `AccessTokenExpiration`, `RefreshTokenExpiration`, `SecurityKey` | JWT konfigürasyonu |
| `Clients.Unbiased` | `Id`, `Secret`, `Audience` | Trusted client konfigürasyonu |
| `FilePaths.ApiKeyPath` | Dosya yolu | Lokal API key dosyası |
| `Keys` | `GptApiKey`, `FreepikApiKey` | AI servis anahtarları |
| `S3Settings` | `AccessKey`, `SecretKey`, `BucketName` | AWS S3 erişimi |
| `Urls` | `GptApi`, `GptDalleApi`, `FreePikApi` | Harici AI endpoint'leri |
| `ReverseProxy` (Gateway) | `Routes`, `Clusters` | YARP yönlendirme tanımları |

---

## Port Haritası

Lokal geliştirme sırasında (HTTP) kullanılan portlar:

| Servis | Port |
| --- | --- |
| Unbiased.ApiGateway | `5000` |
| Unbiased.News.Api | `5001` |
| Unbiased.Playwright.Api | `5002` |
| Unbiased.Log.Api | `5003` |
| Unbiased.Identity.Api | `5004` |
| Unbiased.Dashboard.Api | `5005` |

İstemciler yalnızca **`http://localhost:5000`** (Gateway) üzerinden trafik üretmelidir. Gateway path'lerini aşağıdaki gibi servislere yönlendirir:

| Gateway Path | Hedef Servis |
| --- | --- |
| `/api/v1/News/{**catch-all}` | News.Api (5001) |
| `/api/v1/playwright/{**catch-all}` | Playwright.Api (5002) |
| `/api/v1/logs/{**catch-all}` | Log.Api (5003) |
| `/api/v1/identity/{**catch-all}` | Identity.Api (5004) |
| `/api/v1/dashboard/{**catch-all}` | Dashboard.Api (5005) |

---

## Güvenlik

Sistem **iki katmanlı güvenlik** kullanır:

1. **API Key Middleware** (`ApiKeyAuthorizeMiddleware`) — Her servise gelen her istek, `FilePaths.ApiKeyPath` altında tanımlı olan ve veritabanında tutulan API key ile doğrulanır. Gateway'deki `ApiKeyRefresherService` bu key'leri periyodik yeniler.
2. **JWT + Policy-based Authorization** — Admin panel uçları `[Authorize(Policy = "...")]` ile korunur. Policy'ler Identity servisinde `RoleManagement` tarafından sayfa/izin düzeyinde tanımlanır.

Ek olarak:

- Tüm API'ler `UseHttpsRedirection()` kullanır.
- `GlobalExceptionMiddleware` tüm beklenmeyen istisnaları yakalayıp standart bir JSON hata gövdesine dönüştürür.
- Loglar Log.Api üzerinde merkezî olarak saklanır; olaylar RabbitMQ ile asenkron iletilir, böylece kaynak servis log yazma yüküyle bloklanmaz.

---

## Lisans ve İletişim

Bu depo özel bir projeye aittir. Katkı, hata bildirimi veya sorular için proje sahibine ulaşın.
