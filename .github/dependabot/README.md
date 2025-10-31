# Dependabot 自動相依性更新

本專案使用 GitHub Dependabot 自動檢查並更新 NuGet 套件和 GitHub Actions 版本。

## 配置概覽

配置檔案位置：`.github/dependabot.yml`

### 更新範圍

1. **NuGet 套件** (.NET 專案)
2. **GitHub Actions** (CI/CD 工作流程)

---

## NuGet 套件更新

### 掃描設定

- **掃描目錄**：`/` (根目錄，遞迴掃描所有 `.csproj` 檔案)
- **排程時間**：每週一凌晨 2:00 (台北時間)
- **PR 限制**：最多同時開啟 100 個 PR
- **Rebase 策略**：自動 rebase 過時的 PR

### 標籤

- `dependencies` - 相依性更新
- `automated` - 自動化產生
- `nuget` - NuGet 套件

### Commit 訊息格式

- 前綴：`deps`
- 包含範圍：是
- 範例：`deps: update Microsoft.EntityFrameworkCore to v8.0.1`

### 更新策略

#### ✅ 自動更新（會建立 PR）

- **Minor 版本**：1.0.0 → 1.1.0
- **Patch 版本**：1.0.0 → 1.0.1

這些更新會自動分組為一個 PR（`nuget-minor-patch` 群組）

#### ❌ 忽略更新（不會建立 PR）

- **Major 版本**：1.0.0 → 2.0.0
- 原因：可能包含破壞性變更（Breaking Changes）
- 策略：專案上線期間採用保守策略，避免風險

#### ⚠️ 特殊處理

- `Microsoft.Extensions.Http.Polly`：主要版本更新會被忽略
  - 備註：建議未來遷移到 Resilience 套件

---

## GitHub Actions 更新

### 掃描設定

- **掃描目錄**：`/.github/workflows`
- **排程時間**：每週一凌晨 2:30 (台北時間)
- **PR 限制**：最多同時開啟 5 個 PR
- **Rebase 策略**：自動 rebase 過時的 PR

### 標籤

- `dependencies` - 相依性更新
- `automated` - 自動化產生
- `github-actions` - GitHub Actions

### Commit 訊息格式

- 前綴：`chore`
- 包含範圍：是
- 範例：`chore: update actions/checkout to v4`

### 更新策略

#### ✅ 自動更新（會建立 PR）

- **Minor 版本**：v3.0.0 → v3.1.0
- **Patch 版本**：v3.0.0 → v3.0.1

這些更新會自動分組為一個 PR（`ghactions-minor-patch` 群組）

#### ❌ 忽略更新（不會建立 PR）

- **Major 版本**：v3.0.0 → v4.0.0
- 原因：可能包含破壞性變更
- 策略：保守更新，確保 CI/CD 穩定性

---

## PR 處理流程

### 當 Dependabot 建立 PR 時

1. **自動觸發**：

   - PR 建立後會觸發 `review.yml` 工作流程
   - 執行 CI 檢查（程式碼品質、建置）
   - CI 完成後發送 Teams 通知

2. **審查檢查清單**：

   - ✅ CI 是否通過
   - ✅ 查看 Release Notes 或 Changelog
   - ✅ 確認沒有明顯的相容性問題
   - ✅ 檢查是否有安全性修復

3. **合併決策**：
   - **Patch 版本**：通常為 bug 修復或安全性更新，快速審查後可合併
   - **Minor 版本**：新增功能但向下相容，審查後可合併
   - **Major 版本**：不會自動建立 PR（已忽略）

---

## 保守更新策略說明

### 為什麼採用保守策略？

#### ✅ 優點

1. **穩定性優先**：避免破壞性變更影響生產環境
2. **降低風險**：主要版本更新可能需要程式碼調整
3. **安全更新**：minor/patch 通常包含 bug 修復和安全性修補
4. **向下相容**：minor/patch 版本遵循語義化版本規範，保證向下相容

#### 📋 適用場景

- ✅ 專案正在上線或生產環境運行中
- ✅ 團隊資源有限，無法快速測試主要版本更新
- ✅ 需要確保系統穩定性

### 何時考慮啟用主要版本更新？

- 專案穩定運行 1-2 個月後
- 有充足的測試時間和資源
- 需要使用新版本的重要功能
- 有安全性需求需要更新到最新版本

---

## 如何啟用主要版本更新

如果專案穩定後想要接收主要版本更新的 PR：

### 步驟 1：編輯 `.github/dependabot.yml`

#### NuGet 套件

移除或註解掉第 32-42 行：

```yaml
# 移除這段
# ignore:
#   - dependency-name: '*'
#     update-types:
#       - 'version-update:semver-major'
```

新增 `nuget-major` 群組：

```yaml
groups:
  nuget-minor-patch:
    applies-to: version-updates
    update-types:
      - 'minor'
      - 'patch'
    patterns:
      - '*'
  # 新增這段
  nuget-major:
    applies-to: version-updates
    update-types:
      - 'major'
    patterns:
      - '*'
```

#### GitHub Actions

移除或註解掉第 72-76 行：

```yaml
# 移除這段
# ignore:
#   - dependency-name: '*'
#     update-types:
#       - 'version-update:semver-major'
```

新增 `ghactions-major` 群組：

```yaml
groups:
  ghactions-minor-patch:
    applies-to: version-updates
    update-types:
      - 'minor'
      - 'patch'
    patterns:
      - '*'
  # 新增這段
  ghactions-major:
    applies-to: version-updates
    update-types:
      - 'major'
    patterns:
      - '*'
```

### 步驟 2：提交變更

```bash
git add .github/dependabot.yml
git commit -m "chore: enable major version updates for dependabot"
git push
```

### 步驟 3：等待下次掃描

- 下週一凌晨會自動執行
- 或手動觸發：Settings > Security > Dependabot > Check for updates

---

## 通知整合

### GitHub 通知

- 設定儲存庫為「Watching」
- 在個人設定中開啟 Pull Request 通知

### Teams 通知

當 Dependabot PR 建立後：

1. PR 觸發 `review.yml` 工作流程
2. CI 檢查執行
3. 檢查完成後自動發送 Teams 通知
4. 通知包含：PR 標題、分支、檢查結果、PR 連結

設定方式：參考 `.github/actions/teams-notification/README.md`

---

## 常見問題

### Q1: 為什麼 PR 限制 NuGet 是 100 個，GitHub Actions 是 5 個？

**A**:

- **NuGet**：專案可能有數十個套件，100 個限制確保所有更新都能被追蹤
- **GitHub Actions**：通常只有少數幾個 actions，5 個足夠且避免 PR 過載

### Q2: 為什麼不自動合併 PR？

**A**:

- 即使是 minor/patch 版本，仍需要人工審查
- CI 可能通過但仍有潛在問題
- 需要查看 Release Notes 了解變更內容
- 團隊決策是否需要此更新

### Q3: 如果某個套件一直失敗怎麼辦？

**A**:
在 `dependabot.yml` 中加入忽略規則：

```yaml
ignore:
  - dependency-name: '問題套件名稱'
    # 可選：只忽略特定版本類型
    update-types: ['version-update:semver-major']
```

### Q4: 可以手動觸發 Dependabot 檢查嗎？

**A**:
可以，有兩種方式：

1. **GitHub 介面**：

   - Settings > Security > Dependabot
   - 點擊 "Check for updates"

2. **PR 評論**：
   - 在 Dependabot PR 中評論 `@dependabot rebase`

### Q5: 私有 NuGet 來源（如 nuget.abp.io）如何設定？

**A**:
需要設定 Dependabot secrets：

1. 前往 Settings > Secrets and variables > Dependabot
2. 新增 Secret（例如：`NUGET_AUTH_TOKEN`）
3. 在專案根目錄建立 `NuGet.Config` 並配置私有來源

---

## 最佳實踐

### ✅ 建議做法

1. **定期檢查 PR**：每週至少檢查一次 Dependabot PR
2. **優先安全更新**：有 security 標籤的 PR 應優先處理
3. **分批合併**：不要一次合併太多更新，分批測試
4. **保留 Release Notes**：合併時檢查變更日誌
5. **測試後部署**：合併後在開發環境驗證後再部署

### ❌ 避免做法

1. **盲目合併**：不看內容直接合併所有 PR
2. **忽略 CI 失敗**：CI 失敗就合併
3. **跳過審查**：沒有人審查就自動合併
4. **不測試就部署**：直接部署到生產環境

---

## 監控與維護

### 定期檢查

- **每週**：檢查是否有新的 Dependabot PR
- **每月**：評估是否需要調整策略
- **每季**：檢視被忽略的主要版本更新

### 指標追蹤

- PR 合併率
- 平均審查時間
- 套件版本落後程度
- 安全漏洞修復速度

---

## 相關檔案

- 配置檔案：`.github/dependabot.yml`
- CI/CD 文件：`.github/workflows/CICD_README.md`
- Teams 通知：`.github/actions/teams-notification/README.md`
- PR 審查流程：`.github/workflows/review.yml`

## 參考資源

- [Dependabot 官方文件](https://docs.github.com/en/code-security/dependabot)
- [語義化版本規範](https://semver.org/lang/zh-TW/)
- [NuGet 套件版本管理](https://learn.microsoft.com/zh-tw/nuget/concepts/package-versioning)

---

## 版本歷史

- **v1.0** (2024)：初始配置，採用保守更新策略
- 保守策略：僅自動更新 minor 和 patch 版本
- 忽略 major 版本更新，避免專案上線期間風險
