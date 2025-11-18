# PortalApi - ABP Framework 官方分層架構應用

這是一個使用 ABP Framework 8.2.1 和 PostgreSQL 建立的 ASP.NET Core 應用程式，採用官方推薦的分層架構（Layered Architecture）模式。

## 專案結構（官方 ABP 分層）

```
PortalApi/
├── src/
│   ├── PortalApi.Domain/                      # 領域層
│   │   ├── Products/Product.cs                # 領域實體
│   │   └── PortalApiDomainModule.cs          # 領域模組
│   │
│   ├── PortalApi.Application.Contracts/       # 應用服務合約層
│   │   ├── Products/
│   │   │   ├── ProductDto.cs                  # 資料傳輸物件
│   │   │   ├── CreateUpdateProductDto.cs     # 輸入 DTO
│   │   │   └── IProductAppService.cs         # 應用服務介面
│   │   └── PortalApiApplicationContractsModule.cs
│   │
│   ├── PortalApi.Application/                 # 應用層
│   │   ├── Products/ProductAppService.cs      # 應用服務實作
│   │   ├── PortalApiApplicationAutoMapperProfile.cs  # AutoMapper 設定
│   │   └── PortalApiApplicationModule.cs
│   │
│   ├── PortalApi.EntityFrameworkCore/         # 資料訪問層
│   │   ├── EntityFrameworkCore/
│   │   │   └── PortalApiDbContext.cs         # DbContext
│   │   └── PortalApiEntityFrameworkCoreModule.cs
│   │
│   ├── PortalApi.HttpApi/                     # HTTP API層
│   │   ├── Products/ProductController.cs      # API 控制器
│   │   └── PortalApiHttpApiModule.cs
│   │
│   └── PortalApi.HttpApi.Host/                # 主機層
│       ├── Program.cs                         # 應用程式入口
│       ├── PortalApiHttpApiHostModule.cs      # 主機模組
│       └── appsettings.json                   # 配置檔案
└── PortalApi.sln
```

## 功能特色

- ✅ **ABP Framework 8.2.1** - 遵循官方分層架構
- ✅ **領域驅動設計 (DDD)** - 清晰的業務邏輯分離
- ✅ **Entity Framework Core with PostgreSQL** - 資料持久化
- ✅ **Autofac 依賴注入** - 模組化容器
- ✅ **RESTful API** - 標準化 HTTP 端點
- ✅ **AutoMapper** - 自動物件映射
- ✅ **Serilog** - 結構化日誌記錄
- ✅ **EF Core Migrations** - 資料庫版本管理

## 分層架構說明

### 1. Domain Layer (領域層)
- **職責**: 定義核心業務實體和領域邏輯
- **依賴**: 無（最核心層）
- **包含**: 實體（Entities）、值物件（Value Objects）、領域服務

### 2. Application.Contracts Layer (應用服務合約層)
- **職責**: 定義應用服務的輸入/輸出契約
- **依賴**: Domain Layer
- **包含**: 介面定義、DTOs、應用服務介面

### 3. Application Layer (應用層)
- **職責**: 實作業務用例和應用邏輯
- **依賴**: Domain Layer, Application.Contracts Layer
- **包含**: 應用服務實作、AutoMapper 設定

### 4. EntityFrameworkCore Layer (資料訪問層)
- **職責**: 實作資料持久化
- **依賴**: Domain Layer
- **包含**: DbContext、Entity Configurations、Migrations

### 5. HttpApi Layer (HTTP API層)
- **職責**: 提供 RESTful API 端點
- **依賴**: Application.Contracts Layer
- **包含**: Controllers、API 配置

### 6. HttpApi.Host Layer (主機層)
- **職責**: 應用程式啟動和配置
- **依賴**: 所有其他層
- **包含**: 啟動配置、中介軟體、DI 容器配置

## 資料庫設定

### 連接字串

在 `src/PortalApi.HttpApi.Host/appsettings.json` 中配置：

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres"
  }
}
```

### 執行資料庫遷移

```bash
# 從 EntityFrameworkCore 專案目錄
cd src/PortalApi.EntityFrameworkCore

# 建立遷移
dotnet ef migrations add InitialCreate --startup-project ../PortalApi.HttpApi.Host

# 更新資料庫
dotnet ef database update --startup-project ../PortalApi.HttpApi.Host
```

## API 端點

應用程式提供以下 API 端點：

### Products API

| 方法 | 端點 | 描述 |
|------|------|------|
| GET | `/api/app/products` | 取得產品列表（支援分頁和排序） |
| GET | `/api/app/products/{id}` | 取得特定產品 |
| POST | `/api/app/products` | 建立新產品 |
| PUT | `/api/app/products/{id}` | 更新產品 |
| DELETE | `/api/app/products/{id}` | 刪除產品 |

### 範例請求

**建立產品:**
```bash
curl -X POST https://localhost:5001/api/app/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Sample Product",
    "description": "This is a sample product",
    "price": 99.99,
    "stock": 100
  }'
```

**取得產品列表（分頁）:**
```bash
curl "https://localhost:5001/api/app/products?SkipCount=0&MaxResultCount=10"
```

**取得特定產品:**
```bash
curl https://localhost:5001/api/app/products/{id}
```

## 開發

### 執行應用程式

```bash
# 從 Host 專案目錄
cd src/PortalApi.HttpApi.Host
dotnet run
```

應用程式將在以下位址啟動：
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

### 建置專案

```bash
# 從解決方案根目錄
dotnet build PortalApi.sln --configuration Release
```

### 執行測試

```bash
dotnet test
```

## 套件依賴

### 核心套件
- `Volo.Abp.Ddd.Domain` - ABP DDD 領域層支援
- `Volo.Abp.Ddd.Application` - ABP 應用層支援
- `Volo.Abp.EntityFrameworkCore.PostgreSql` - PostgreSQL 資料庫提供者
- `Volo.Abp.AspNetCore.Mvc` - ABP MVC 整合
- `Volo.Abp.Autofac` - Autofac 依賴注入
- `Volo.Abp.AutoMapper` - AutoMapper 整合
- `Volo.Abp.AspNetCore.Serilog` - Serilog 日誌整合

### 工具套件
- `Microsoft.EntityFrameworkCore.Design` - EF Core 設計時工具
- `Serilog.Extensions.Hosting` - Serilog 主機整合
- `Serilog.Sinks.Console` - 控制台日誌輸出
- `Serilog.Sinks.Async` - 非同步日誌處理

## 環境需求

- .NET 8.0 SDK
- PostgreSQL 12 或更新版本

## ABP Framework 資源

- [ABP 官方文檔](https://docs.abp.io/)
- [ABP 框架 GitHub](https://github.com/abpframework/abp)
- [ABP Community](https://community.abp.io/)

## 授權

MIT License
