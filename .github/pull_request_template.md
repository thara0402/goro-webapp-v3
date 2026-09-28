## 関連 Issue

<!-- 例: Closes #123 -->

## 変更内容

## 設計変更の有無

<!-- 更新した設計書と内容。なければ「なし」。Cloud Agent では設計判断と前提もここに記載する -->

## テスト結果

<!-- 実行コマンドと、dotnet test の集計行を加工せずに貼り付ける。合計 0 件は成功とみなさない -->

```text
dotnet test src/goro-webapp/goro-webapp.slnx --no-restore

```

## レビュー結果

<!-- reviewer の指摘（blocking：N 件、warning 以下の残った指摘）。orchestrator を使わない例外の変更のみの PR は「対象外」 -->

## 工程実行記録

<!-- src/goro-webapp/ または design/（workflow.md を除く）を変更する PR では必須（orchestrator 経由で作業する）。空欄や「未実行」の工程がある PR、記録がない PR はマージしない。例外の変更（README、.github/、design/workflow.md、CI など）のみの PR は「対象外」 -->

| 工程 | 担当 | 呼び出し回数 | 判定 | 根拠（報告の該当箇所） |
| --- | --- | --- | --- | --- |
| 1. Issue 受付 | orchestrator | - | | |
| 2. 設計 | architect | | | |
| 3. 設計承認ゲート | orchestrator / 人間 | - | | |
| 4. 実装・テスト | developer | | | |
| 5. レビュー・テスト再実行 | reviewer | | | |
| 6. 差し戻し | | | | |
| 7. 完了報告 | orchestrator | - | | |

## 未確認事項

<!-- なければ「なし」 -->

## マージ前の確認（レビュアー）

- [ ] `src/goro-webapp/` または `design/`（workflow.md を除く）を変更する場合、工程実行記録があり、空欄や「未実行」がない
- [ ] テスト結果が集計行の原文で、失敗 0 件かつ合計 1 件以上である
- [ ] 設計変更がある場合、`design/` の更新が差分に含まれている
- [ ] CI が成功している
