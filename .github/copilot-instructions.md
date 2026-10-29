# プロジェクト共通方針

このリポジトリは、ドラマ「孤独のグルメ」に登場した店舗情報を整理し、訪問時に役立つ情報を提供する Web アプリケーションです。

## 設計確認

- 実装やテストの前に、関連する最新の設計を確認してください。仕様・設計の正本は `design/architecture.md`、`design/spec.md`、`design/data-model.md`、`design/gourmet.json` です。
- 設計を変更する場合は、実装やテストより先に該当する設計書を更新してください。
- 開発工程と品質ゲートの正本は [`design/workflow.md`](../design/workflow.md) です。これはアプリケーション設計書とは別の文書です。

## 計画

- コード、設定、設計書、データ定義を変更する場合は、Planning ツールで計画を作成・確定してからファイルを変更してください。利用できない場合は、変更対象と目的、関連文書・設定・エージェント定義との整合、変更手順・検証方法・想定される影響を含む計画を提示してから着手してください。要件や対象が変わった場合は計画を更新してください。
- 実装を依頼されていない相談・質問では、計画ツールやファイル変更を行わず、必要に応じて計画案だけを示してください。
- `orchestrator` から担当エージェントとして呼び出された場合は、依頼内容と承認済み設計レポートを確定した計画として扱い、Planning ツールを改めて実行する必要はありません。

## カスタムエージェントと作業工程

- 変更対象に応じたエージェントの選択、担当者の権限、承認ゲート、工程、完了報告は [`design/workflow.md`](../design/workflow.md) を正本とします。ファイル範囲ごとの適用ルールと例外も同文書を参照してください。
- 担当エージェントとして呼び出された場合は、`orchestrator` からの依頼と自分のエージェント定義に従ってください。各担当の編集範囲・参照スキルは `.github/agents/` の定義を確認してください。
- `design/workflow.md` は、ユーザーが明示的に指示した場合を除き、エージェントが編集しないでください。

## Secret 方針

- Unit テストは Secret を必要としません。通常のテスト実行のために、本番用の Cosmos DB、Google Maps / Geocoding API、Application Insights、Azure Key Vault の Secret を要求しないでください。
- Secret、API key、connection string、Key Vault の値をコード、設定ファイル、Issue、Pull Request、ログ、コメントに出力しないでください。
- Secret が必要な統合テストや本番接続確認は、GitHub Actions Environment または Azure 側の管理下で別途実施してください。

## テストと品質ゲート

- テストコマンド、TDD、失敗時の対応、0 件の扱いを含む品質ゲートは [`design/workflow.md`](../design/workflow.md) に従ってください。対象テストを実行できない場合は完了扱いにせず、理由と代替確認内容を報告してください。

## スキル

該当する作業では、次のスキルを使用してください。エージェントごとの割り当ては各定義の「参照スキル」に従います。スキルを追加・削除した場合は、この一覧とエージェント定義の両方を更新してください。

| スキル | 使う場面 |
| --- | --- |
| `cosmosdb-datamodeling` | Cosmos DB のデータモデルを設計するとき |
| `dotnet-best-practices` | .NET / C# のコードを書く・レビューするとき（常に準拠する） |
| `dotnet10` | .NET 10、C# 14、ASP.NET Core 10、EF Core 10 固有の機能を扱うとき |
| `csharp-async` | C# の非同期処理を扱うとき |
| `csharp-mstest` | MSTest 3.x/4.x の単体テストを実装するとき |
| `microsoft-docs` | Microsoft 製品・サービスの仕様や公式コード例を確認するとき |
| `microsoft-code-reference` | Microsoft API、Azure SDK、.NET ライブラリのメソッド、引数、バージョン互換性を確認するとき |
