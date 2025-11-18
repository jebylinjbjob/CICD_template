# CICD_template
給C# 專案使用的CICD 模板 

![Alt](https://repobeats.axiom.co/api/embed/471b719a2db89bdf73223d371c9ecc340211b790.svg "Repobeats analytics image")

## 專案簡介

本專案使用 ABP Framework 建立，採用官方推薦的分層架構（Layered Architecture）。

### 技術堆疊

- **框架**: ABP Framework 8.2.1
- **資料庫**: PostgreSQL
- **ORM**: Entity Framework Core with PostgreSQL Provider
- **依賴注入**: Autofac
- **.NET 版本**: .NET 8.0
- **架構**: 領域驅動設計 (DDD) + 分層架構

### ABP 官方分層架構

專案按照 ABP 官方模板結構組織，分為以下各層：

```
PortalApi/
├── src/
│   ├── PortalApi.Domain                    # 領域層 - 實體、領域服務
│   ├── PortalApi.Application.Contracts     # 應用服務合約層 - DTOs、介面
│   ├── PortalApi.Application               # 應用層 - 應用服務實作
│   ├── PortalApi.EntityFrameworkCore       # 資料訪問層 - DbContext、Repository
│   ├── PortalApi.HttpApi                   # HTTP API層 - Controllers
│   └── PortalApi.HttpApi.Host              # Web Host - 啟動入口
└── PortalApi.sln                           # 解決方案檔案
```

#### 各層職責

**Domain Layer (領域層)**
- 定義核心業務實體（Entities）
- 領域服務（Domain Services）
- 倉儲介面（Repository Interfaces）
- 領域事件

**Application.Contracts Layer (應用服務合約層)**
- 定義應用服務介面（IAppService）
- 資料傳輸物件（DTOs）
- 應用服務輸入/輸出定義

**Application Layer (應用層)**
- 實作應用服務
- 處理業務邏輯
- 協調領域物件
- 實現 DTO 與實體之間的映射（AutoMapper）

**EntityFrameworkCore Layer (資料訪問層)**
- DbContext 定義
- 實體配置（Entity Configurations）
- 資料庫遷移（Migrations）
- Repository 實作

**HttpApi Layer (HTTP API層)**
- 定義 REST API 控制器
- HTTP 路由配置
- API 版本控制

**HttpApi.Host Layer (主機層)**
- 應用程式啟動
- 中介軟體配置
- 依賴注入容器配置
- 資料庫連接配置

## 開始使用

### 先決條件

- .NET 8.0 SDK 或更新版本
- PostgreSQL 資料庫伺服器

### 資料庫設定

1. 確保 PostgreSQL 正在運行
2. 更新 `PortalApi/src/PortalApi.HttpApi.Host/appsettings.json` 中的連接字串：

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres"
  }
}
```

### 建置和執行

```bash
# 還原套件
dotnet restore PortalApi.sln

# 建置專案
dotnet build PortalApi.sln --configuration Release

# 執行應用程式（從 Host 專案）
cd PortalApi/src/PortalApi.HttpApi.Host
dotnet run
```

應用程式將在 `http://localhost:5000` (或 `https://localhost:5001`) 上啟動。

### 資料庫遷移

```bash
# 從 EntityFrameworkCore 專案目錄執行
cd PortalApi/src/PortalApi.EntityFrameworkCore

# 建立遷移
dotnet ef migrations add InitialCreate --startup-project ../PortalApi.HttpApi.Host

# 更新資料庫
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

# 取得產品列表
curl https://localhost:5001/api/app/products
```

## 專案特色

✅ **ABP 官方分層架構** - 遵循 ABP Framework 最佳實踐
✅ **領域驅動設計 (DDD)** - 清晰的領域邏輯分離
✅ **自動 Repository** - ABP 自動生成基本 CRUD 操作
✅ **AutoMapper 整合** - 自動化 DTO 與實體映射
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

