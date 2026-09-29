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
│   ├── agents/                      # GitHub Copilot カスタムエージェント
│   ├── skills/                      # GitHub Copilot スキル
│   ├── workflows/                   # GitHub Actions などのワークフロー
│   ├── commit-instructions.md       # コミットメッセージ規約
│   ├── copilot-instructions.md      # Copilot 用指示書
│   └── pull_request_template.md     # Pull Request テンプレート
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

### その他

- **開発ワークフロー**: `design/workflow.md`（開発工程と品質ゲートの定義。アプリケーションの設計ではないため、実装やテストの前に確認する設計書には含まれません）

## 制約

- **コード、設定、設計書、データ定義を変更する可能性がある場合は、Planning ツールで計画を作成し、確定するまでファイルを作成・編集・削除しないでください。** Planning ツールが利用できない環境では、「計画（Planning の必須項目）」を満たす計画をチャットの回答に記載してから変更を開始してください。
- ユーザーが実装を明示的に依頼していない相談・質問では、Planning ツールやファイル変更を行わず、必要に応じて計画案だけを説明してください。
- `orchestrator` から担当エージェント（`architect`、`developer`、`reviewer`）として呼び出された場合は、受け取った依頼内容と承認済みの設計レポートを確定した計画として扱い、Planning ツールを改めて実行する必要はありません。

## Secret 方針

- 現時点の Unit テストは Secret を必要としません。
- ローカル実行・Cloud Agent のいずれでも、通常の Unit テスト実行のために、本番用の Cosmos DB、Google Maps / Geocoding API、Application Insights、Azure Key Vault の Secret を要求しないでください。
- Secret、API key、connection string、Key Vault の値をコード、設定ファイル、Issue、Pull Request、ログ、コメントに出力しないでください。
- Secret が必要な統合テストや本番接続確認は、別途 GitHub Actions Environment または Azure 側の管理下で実施してください。

## 作業手順

この手順は、「カスタムエージェント」の節で `orchestrator` を使わなくてよいとされた変更（例外）にだけ適用します。アプリケーションの変更には適用しません。

### 計画（Planning の必須項目）

Planning ツールでは、少なくとも次の項目を確認してください。

1. 変更対象のファイルと目的
2. 関連するドキュメント・設定・エージェント定義との整合
3. 変更手順、検証方法、想定される影響

> 計画後に要件や対象ファイルが変わった場合は、変更を続けず、Planning ツールを再実行して計画を更新してください。変更が `src/goro-webapp/` または `design/`（`design/workflow.md` を除く）に及ぶ場合は、例外の対象外になるため `orchestrator` に切り替えてください。

### 変更

1. Issue または依頼内容の目的、対象ファイル、完了条件を確認する。
2. 確定した計画に基づいて、既存の記述スタイルに合わせて必要な最小限の変更を行う。
3. 設定、CI、エージェント定義など、ビルドやテストに影響し得る変更の場合は、「テスト」の節に従ってテストを実行する。
4. 変更完了後、変更内容を簡潔に報告する。

### 実行環境ごとの違い

| 環境 | 開始方法 | 計画の確定 | 完了時 |
| --- | --- | --- | --- |
| ローカル実行（GitHub Copilot App / VS Code） | チャットで依頼 | Planning ツールで確定する | チャットで報告する。PR 作成依頼時の引き継ぎは `design/workflow.md` の「完了報告」に従う。 |
| GitHub Copilot Cloud Agent | Issue の割り当て | 計画を Pull Request 本文に記載する | 作業用ブランチと Pull Request を作成し、本文は `.github/pull_request_template.md` に従って変更内容、設計変更の有無、テスト結果、未確認事項を記載する。最終的な review / merge は人間が行う |

## カスタムエージェント

Issue とチャットのどちらを起点にする場合でも、アプリケーションの変更は `.github/agents/` の `orchestrator` を選択して行います。工程、担当エージェント（`architect`、`developer`、`reviewer`）の役割と権限、承認ゲート、工程実行記録は `design/workflow.md` に定義しています。

| 変更対象 | 進め方 |
| --- | --- |
| `src/goro-webapp/` と `design/`（`design/workflow.md` を除く） | 必ず `orchestrator` を使う |
| `README.md`、`.github/`、`design/workflow.md`（CI を含む） | 例外として、「作業手順」の節に従って進めてよい |

- カスタムエージェントを選択していない状態でアプリケーションの変更を依頼された場合は、自分で実装せず、`orchestrator` を選択し直すようユーザーに案内してください。
- 担当エージェントとして呼び出された場合は、`orchestrator` からの依頼内容と自分のエージェント定義に従ってください。
- ローカルの `orchestrator` は読み取り専用です。PR 作成依頼時は `design/workflow.md` の「完了報告」に従い、Default Agent への引き継ぎを案内してください。Cloud Agent の PR 作成は従来どおりです。
- `design/workflow.md` は、ユーザーが明示的に指示した場合を除き、エージェントが編集しないでください。

## テスト

品質ゲートの正式な定義は `design/workflow.md` の「Quality Gates」です。コード、設定、設計書、テストを変更した場合は、完了報告または Pull Request 作成前に次のコマンドを実行してください（Windows / Linux / macOS 共通）。

```bash
dotnet test src/goro-webapp/goro-webapp.slnx --no-restore
```

- テストが失敗した場合、または合計が 0 件の場合は完了扱いにしない。0 件の場合は依存関係が未復元の可能性があるため、`dotnet restore src/goro-webapp/goro-webapp.slnx` を実行してから再実行する。
- テストを実行できない場合は完了扱いにせず、理由と代替確認内容を完了報告および Pull Request 本文に明記する。

## スキル

該当する作業では、次のスキルを使用してください。この表はリポジトリ全体のスキル一覧です。カスタムエージェントごとの割り当ては各エージェント定義の「参照スキル」にあります。スキルを追加・削除した場合は、両方を更新してください。

| スキル | 使う場面 |
| --- | --- |
| `cosmosdb-datamodeling` | Cosmos DB のデータモデルを設計するとき |
| `dotnet-best-practices` | .NET / C# のコードを書く・レビューするとき（常に準拠する） |
| `dotnet10` | .NET 10、C# 14、ASP.NET Core 10、EF Core 10 固有の機能を扱うとき |
| `csharp-async` | C# の非同期処理を扱うとき |
| `csharp-mstest` | MSTest 3.x/4.x の単体テストを実装するとき |
| `microsoft-docs` | Microsoft 製品・サービスの仕様や公式コード例を確認するとき |
| `microsoft-code-reference` | Microsoft API、Azure SDK、.NET ライブラリのメソッド、引数、バージョン互換性を確認するとき |
