# Teams 通知 Action

可重複使用的 GitHub Actions composite action，用於發送 CI/CD 執行結果通知到 Microsoft Teams。

## 功能特色

- ✅ **自動狀態識別**：根據執行結果顯示對應的圖示和顏色
  - 成功：✅ 綠色
  - 失敗：❌ 紅色
  - 取消：⚠️ 橘色
- 📊 **豐富的訊息內容**：包含工作流程、分支、觸發者、提交訊息、執行時間等
- 🎨 **Teams 卡片格式**：使用 MessageCard 格式，支援直接點擊連結查看執行詳情
- 🔧 **高度可自訂**：支援自訂標題和額外資訊
- 🚀 **輕量高效**：使用 PowerShell 腳本，無需額外依賴

## 使用方式

### 基本用法

```yaml
- name: Notify Teams
  if: always() # 確保無論成功或失敗都會通知
  uses: ./.github/actions/teams-notification
  with:
    webhook_url: ${{ secrets.TEAMS_WEBHOOK_URL }}
    workflow_name: ${{ github.workflow }}
    status: ${{ job.status }}
    title: 'CI/CD 通知'
```

### 完整範例

```yaml
- name: Notify Teams
  if: always()
  uses: ./.github/actions/teams-notification
  with:
    webhook_url: ${{ secrets.TEAMS_WEBHOOK_URL }}
    workflow_name: ${{ github.workflow }}
    status: ${{ job.status }}
    title: 'PortalApi 部署完成'
    extra_info: |
      **部署環境**: Production
      **專案**: PortalApi.HttpApi.Host
      **版本**: v1.2.3
```

## 輸入參數

| 參數            | 必填 | 預設值       | 說明                                              |
| --------------- | ---- | ------------ | ------------------------------------------------- |
| `webhook_url`   | ✅   | -            | Microsoft Teams Incoming Webhook URL              |
| `workflow_name` | ✅   | -            | 工作流程名稱（通常使用 `${{ github.workflow }}`） |
| `status`        | ✅   | -            | 執行狀態：success / failure / cancelled           |
| `title`         | ❌   | 'CI/CD 通知' | 通知卡片標題                                      |
| `extra_info`    | ❌   | ''           | 額外要顯示的資訊（支援 Markdown）                 |

## 設定 Teams Webhook

### 步驟 1：在 Teams 建立 Incoming Webhook

1. 開啟 Microsoft Teams
2. 選擇要接收通知的頻道
3. 點擊頻道名稱旁的「...」
4. 選擇「連接器」(Connectors) 或「工作流程」(Workflows)
5. 搜尋「Incoming Webhook」
6. 點擊「設定」或「新增」
7. 為 Webhook 命名（例如：PortalApi CI/CD）
8. 可選擇上傳圖示
9. 點擊「建立」並複製產生的 Webhook URL

### 步驟 2：設定 GitHub Secret

1. 前往 GitHub 儲存庫
2. 點擊 **Settings** > **Secrets and variables** > **Actions**
3. 點擊 **New repository secret**
4. 名稱：`TEAMS_WEBHOOK_URL`
5. 值：貼上剛才複製的 Webhook URL
6. 點擊 **Add secret**

## 通知內容

通知卡片會包含以下資訊：

- **工作流程**：執行的工作流程名稱
- **狀態**：執行結果（附圖示）
- **分支**：觸發的分支名稱
- **觸發者**：執行者的 GitHub 帳號
- **提交訊息**：最新的 commit 訊息
- **執行時間**：工作流程執行的時間戳記
- **執行連結**：可點擊直接查看 GitHub Actions 執行詳情
- **額外資訊**：自訂的補充內容

## 已整合的工作流程

本 action 已整合至以下工作流程：

- `.github/workflows/dev_CICD.yml` - 開發環境 CI/CD
- `.github/workflows/ManualAndSchedule_Deployment.yml` - 手動/排程部署
- `.github/workflows/review.yml` - PR 審查與程式碼品質檢查

## 進階用法

### 條件式通知

只在特定條件下發送通知：

```yaml
# 只在失敗時通知
- name: Notify on Failure
  if: failure()
  uses: ./.github/actions/teams-notification
  with:
    webhook_url: ${{ secrets.TEAMS_WEBHOOK_URL }}
    workflow_name: ${{ github.workflow }}
    status: ${{ job.status }}
    title: '⚠️ 建置失敗警告'

# 只在成功且為 main 分支時通知
- name: Notify on Main Success
  if: success() && github.ref == 'refs/heads/main'
  uses: ./.github/actions/teams-notification
  with:
    webhook_url: ${{ secrets.TEAMS_WEBHOOK_URL }}
    workflow_name: ${{ github.workflow }}
    status: ${{ job.status }}
    title: '🎉 Production 部署成功'
```

### 多個環境使用不同 Webhook

```yaml
- name: Notify to Dev Channel
  if: always() && github.ref == 'refs/heads/develop'
  uses: ./.github/actions/teams-notification
  with:
    webhook_url: ${{ secrets.TEAMS_WEBHOOK_DEV }}
    workflow_name: ${{ github.workflow }}
    status: ${{ job.status }}
    title: 'Development 環境部署'

- name: Notify to Prod Channel
  if: always() && github.ref == 'refs/heads/main'
  uses: ./.github/actions/teams-notification
  with:
    webhook_url: ${{ secrets.TEAMS_WEBHOOK_PROD }}
    workflow_name: ${{ github.workflow }}
    status: ${{ job.status }}
    title: 'Production 環境部署'
```

## 故障排除

### 通知未發送

1. 確認 `TEAMS_WEBHOOK_URL` Secret 已正確設定
2. 檢查 Webhook URL 是否仍然有效（Teams Webhook 可能會過期）
3. 確認 `if: always()` 條件，確保步驟會執行
4. 查看 GitHub Actions 執行日誌中的錯誤訊息

### Webhook URL 過期

Teams Incoming Webhook 有時會因為頻道設定變更而失效：

1. 回到 Teams 頻道的連接器設定
2. 刪除舊的 Incoming Webhook
3. 重新建立新的 Incoming Webhook
4. 更新 GitHub Secret 中的 `TEAMS_WEBHOOK_URL`

### 訊息格式問題

如果訊息顯示不正常：

1. 確認 `extra_info` 使用正確的 Markdown 語法
2. 特殊字元可能需要跳脫
3. 檢查 Teams 是否支援該 Markdown 語法

## 版本歷史

- **v1.0** (2024)
  - 初始版本
  - 支援基本通知功能
  - 自動狀態識別與顏色設定
  - Teams MessageCard 格式

## 授權

此 action 為 PortalApi 專案的一部分，僅供內部使用。

## 維護者

PortalApi Team

## 相關文件

- [GitHub Actions 文件](https://docs.github.com/en/actions)
- [Microsoft Teams Incoming Webhook 文件](https://learn.microsoft.com/zh-tw/microsoftteams/platform/webhooks-and-connectors/how-to/add-incoming-webhook)
- [PortalApi CI/CD 使用手冊](../../workflows/CICD_README.md)
