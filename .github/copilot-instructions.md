# プロジェクト開発方針と Copilot 指示

## プロジェクト概要

> このリポジトリは、松重豊が主演のドラマ「孤独のグルメ」に登場した店舗情報を整理し、訪問時に役立つ情報を提供する Web アプリケーションです。

## 技術スタック

- `.NET 10` / `C#`
- `ASP.NET Core MVC`
- `Azure App Service`
- `Application Insights`
- `Azure Cosmos DB`
- `AutoMapper`
- `Google Maps Platform`（Geocoding API / Maps API）

## リポジトリ構成

```text
goro-webapp-v3/                      # リポジトリルート
├── .github/
│   ├── skills/                      # GitHub Copilot スキル
│   ├── workflows/                   # GitHub Actions などのワークフロー
│   └── copilot-instructions.md      # Copilot 用指示書
├── design/                          # 設計書
├── README.md                        # プロジェクト概要と利用方法
│
└── src/                             # ソースコード・ビルド設定
    └── goro-webapp/                 # .NET ソリューションルート
        └── goro-webapp/             # ASP.NET Core MVC アプリケーション本体
            ├── Controllers/         # MVC コントローラー
            ├── Infrastructure/      # DB アクセス、外部 API 呼び出し
            ├── Models/              # ドメインモデル、ビュー関連モデル
            ├── Views/               # Razor ビュー
            └── wwwroot/             # CSS、JavaScript、画像などの静的ファイル
        └── goro-webapp.Tests/       # MSTest による単体テストプロジェクト
```

## 設計

仕様と設計の詳細は、`design/` 配下の Markdown ファイルを参照してください。**実装やテストの前に、必ず最新の設計を確認してください。**

### 設計書

- **アーキテクチャ**: `design/architecture.md`
- **機能仕様書**: `design/spec.md`
- **データモデル設計**: `design/data-model.md`
- **店舗データ定義**: `design/gourmet.json`

> 設計に変更が入った場合は、必ず該当する設計書を先に更新してください。実装とテストは、更新後の設計書に基づいて行います。

## 制約

- **Planning ツールが利用可能な環境では、Planning ツールを実行して計画が確定するまで、ファイルを作成・編集・削除しないでください。**
- コード、設定、設計書、データ定義を変更する可能性がある場合は、実装前に必ず Planning ツールで計画を作成してください。Planning ツールが利用できない環境に限り、チャットの回答に計画を記載することで代用できます。
- ユーザーが実装を明示的に依頼していない相談・質問では、Planning ツールやファイル変更を行わず、必要に応じて計画案だけを説明してください。
- Planning ツールが利用できない場合は、ファイル変更を開始する前に、Planning の必須項目を満たす実装計画をチャットの回答に記載してください。

## Planning の必須項目

Planning ツールでは、少なくとも次の項目を確認してください。

1. 変更対象のファイルと担当するコード領域
2. 仕様・設計書との関係、および必要な設計変更
3. 実装手順、検証方法、想定される影響

### 設計変更を含む場合

1. Planning の計画に設計書の更新を含める。
2. 設計書を先に更新する。
3. 実装とテストを更新後の設計書に基づいて行う。

> 計画後に要件、対象ファイル、設計が変わった場合は、実装を続けず、Planning ツールを再実行して計画を更新してください。

## 実装手順

1. 確定した実装計画と最新の設計書を確認する。
2. 計画に基づいて実装する。
3. 既存のコードスタイルに合わせて実装する。
4. 必要な Unit テストも追加または更新する。
5. 実装完了後、変更内容を簡潔に報告する。

## テスト

1. ビルドとテストを実行して確認する。
2. 失敗した場合は原因を調査して修正する。
3. テスト完了後、確認結果を簡潔に報告する。

## スキル

設計から実装・テストまで、該当するフェーズの Copilot スキルを使用してください。

### 設計・実装・リファクタリング

| スキル | 用途 |
| --- | --- |
| `cosmosdb-datamodeling` | Cosmos DB のデータモデルを設計する |
| `dotnet-best-practices` | .NET / C# のベストプラクティスに準拠する |
| `dotnet10` | .NET 10 / C# 14 の機能や ASP.NET Core 10、EF Core 10 を扱う |
| `csharp-async` | C# の非同期プログラミングに関するベストプラクティスを適用する |
| `microsoft-docs` | Microsoft Learn の公式ドキュメントで概念、仕様、チュートリアル、コード例を確認する |
| `microsoft-code-reference` | Microsoft API、Azure SDK、.NET ライブラリの仕様や公式コード例を検証する |

- データ層の設計が必要な場合は `cosmosdb-datamodeling` を使用してください。
- .NET / C# コードは `dotnet-best-practices` に準拠してください。
- .NET 10、C# 14、ASP.NET Core 10、または EF Core 10 固有の機能を扱う場合は `dotnet10` を使用してください。
- C# の非同期処理を扱う場合は `csharp-async` を使用してください。
- Microsoft 製品・サービスの仕様や公式コード例を確認する必要がある場合は `microsoft-docs` を使用してください。
- Microsoft API、Azure SDK、または .NET ライブラリのメソッド、引数、バージョン互換性を確認する必要がある場合は `microsoft-code-reference` を使用してください。

### テスト

| スキル | 用途 |
| --- | --- |
| `csharp-mstest` | MSTest 3.x/4.x の単体テスト、最新のアサーション API、データ駆動テストのベストプラクティスを適用する |

- MSTest 3.x/4.x を使用した単体テストを実装する場合は `csharp-mstest` を使用してください。
