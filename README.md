# CICD_template
給C# 專案使用的CICD 模板 

![Alt](https://repobeats.axiom.co/api/embed/471b719a2db89bdf73223d371c9ecc340211b790.svg "Repobeats analytics image")

## 專案簡介

本專案使用 ABP Framework 建立，採用官方推薦的完整分層架構（Layered Architecture），包含所有標準專案層級。

### 技術堆疊

- **框架**: ABP Framework 8.2.1
- **資料庫**: PostgreSQL
- **ORM**: Entity Framework Core with PostgreSQL Provider
- **依賴注入**: Autofac
- **.NET 版本**: .NET 8.0
- **架構**: 領域驅動設計 (DDD) + 完整分層架構

### ABP 官方完整分層架構（9個專案層）

專案按照 ABP 官方模板結構組織，包含完整的 9 個專案層：

```
PortalApi/
├── src/
│   ├── PortalApi.Domain.Shared              # 1. 領域共享層 - 常數、列舉
│   ├── PortalApi.Domain                     # 2. 領域層 - 實體、領域服務
│   ├── PortalApi.Application.Contracts      # 3. 應用服務合約層 - DTOs、介面
│   ├── PortalApi.Application                # 4. 應用層 - 應用服務實作
│   ├── PortalApi.EntityFrameworkCore        # 5. 資料訪問層 - DbContext、Repository
│   ├── PortalApi.HttpApi                    # 6. HTTP API層 - Controllers
│   ├── PortalApi.HttpApi.Client             # 7. HTTP 客戶端 - 客戶端代理
│   ├── PortalApi.HttpApi.Host               # 8. Web Host - 啟動入口
│   └── PortalApi.DbMigrator                 # 9. 資料庫遷移工具
└── PortalApi.sln                            # 解決方案檔案
```

#### 各層詳細說明

**1. PortalApi.Domain.Shared** (領域共享層)
- **用途**: 跨模組共用的 Domain 物件
- **內容**: Enum、常數、基礎介面
- **依賴**: 無

**2. PortalApi.Domain** (領域層)
- **用途**: 核心業務邏輯
- **內容**: 實體（Entities）、聚合（Aggregates）、Domain Services
- **依賴**: Domain.Shared

**3. PortalApi.Application.Contracts** (應用服務合約層)
- **用途**: 定義應用服務的介面與 DTO
- **內容**: 介面（IAppService）、DTO
- **依賴**: Domain.Shared

**4. PortalApi.Application** (應用層)
- **用途**: 實作應用服務，負責業務邏輯的協調
- **內容**: 應用服務類別、DTO 轉換（AutoMapper）
- **依賴**: Domain, Application.Contracts

**5. PortalApi.EntityFrameworkCore** (資料訪問層)
- **用途**: 資料存取層，實作 Repository
- **內容**: DbContext、Migration 設定、實體配置
- **依賴**: Domain

**6. PortalApi.HttpApi** (HTTP API層)
- **用途**: 定義 API Controller，將 Application Service 暴露給外部
- **內容**: Controller 類別、API 路由
- **依賴**: Application.Contracts

**7. PortalApi.HttpApi.Client** (HTTP 客戶端層)
- **用途**: 提供呼叫 API 的客戶端程式碼
- **內容**: 代理類別、介面
- **依賴**: Application.Contracts

**8. PortalApi.HttpApi.Host** (主機層)
- **用途**: API 的啟動專案
- **內容**: Program.cs、Startup、DI 註冊、Middleware
- **依賴**: 所有其他層

**9. PortalApi.DbMigrator** (資料庫遷移工具)
- **用途**: 資料庫遷移工具，執行 EF Core Migration
- **內容**: 啟動程式、連線設定、資料初始化
- **依賴**: EntityFrameworkCore

## 開始使用

### 先決條件

- .NET 8.0 SDK 或更新版本
- PostgreSQL 資料庫伺服器

### 資料庫設定

1. 確保 PostgreSQL 正在運行
2. 更新連接字串（在 `PortalApi/src/PortalApi.HttpApi.Host/appsettings.json` 和 `PortalApi/src/PortalApi.DbMigrator/appsettings.json`）：

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=PortalApiDb;Username=postgres;******"
  }
}
```

### 建置和執行

```bash
# 還原套件
dotnet restore PortalApi.sln

# 建置專案
dotnet build PortalApi.sln --configuration Release

# 執行資料庫遷移（首次執行）
cd PortalApi/src/PortalApi.DbMigrator
dotnet run

# 執行應用程式（從 Host 專案）
cd ../PortalApi.HttpApi.Host
dotnet run
```

應用程式將在 `http://localhost:5000` (或 `https://localhost:5001`) 上啟動。

### 資料庫遷移

有兩種方式執行資料庫遷移：

**方式 1: 使用 DbMigrator 工具（推薦）**
```bash
cd PortalApi/src/PortalApi.DbMigrator
dotnet run
```

**方式 2: 使用 EF Core CLI**
```bash
cd PortalApi/src/PortalApi.EntityFrameworkCore
dotnet ef migrations add InitialCreate --startup-project ../PortalApi.HttpApi.Host
dotnet ef database update --startup-project ../PortalApi.HttpApi.Host
```

## API 端點

### Products API

- `GET /api/app/products` - 取得產品列表（分頁）
- `GET /api/app/products/{id}` - 取得特定產品
- `POST /api/app/products` - 建立新產品
- `PUT /api/app/products/{id}` - 更新產品
- `DELETE /api/app/products/{id}` - 刪除產品

### 範例請求

```bash
# 建立產品
curl -X POST https://localhost:5001/api/app/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Sample Product",
    "description": "This is a sample product",
    "price": 99.99,
    "stock": 100
  }'

# 取得產品列表（分頁）
curl "https://localhost:5001/api/app/products?SkipCount=0&MaxResultCount=10"
```

## 專案特色

✅ **ABP 官方完整分層架構** - 包含所有 9 個標準層級  
✅ **領域驅動設計 (DDD)** - 清晰的領域邏輯分離  
✅ **自動 Repository** - ABP 自動生成基本 CRUD 操作  
✅ **AutoMapper 整合** - 自動化 DTO 與實體映射  
✅ **HTTP Client SDK** - 自動生成的客戶端代理  
✅ **資料庫遷移工具** - 獨立的 DbMigrator 專案  
✅ **模組化設計** - 易於擴展和維護  
✅ **依賴注入** - 使用 Autofac 容器  
✅ **日誌記錄** - 整合 Serilog

## CI/CD

本專案包含以下 GitHub Actions 工作流程：

- **CodeQL 安全掃描**: 自動化安全漏洞掃描（支援 C# 和 JavaScript/TypeScript）
- **Pull Request 檢查**: 程式碼品質檢查和自動化審查
- **Dependabot**: 自動依賴更新

## 授權

本專案採用 MIT 授權 - 詳見 [LICENSE](LICENSE) 檔案。
