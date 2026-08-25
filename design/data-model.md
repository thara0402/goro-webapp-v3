# データモデル

## Design Philosophy & Approach

- 「孤独のグルメ」に登場した店舗を、登場回単位の1ドキュメントとして管理する。
- 1つの `episode` に複数店舗が登場する場合がある。
- システムの利用規模は、個人利用を想定しており、読み取りのみ。
- `season` は最大20件、1つの `season` あたりの店舗数は12件程度。
- GEO レプリケーションは、単一リージョン。マルチリージョン書き込みは不要。
- Consistency Level は、初期値の Session を設定する。単一リージョンの一般的な Web 閲覧では十分なため。

## Access Patterns

- すべての店舗を全件取得する。`season` の指定なし。画面上では、`season` を指定して一覧表示する。
- 店舗詳細を取得する。`id` で1ドキュメントを取得する。
- 指定地点から近い店舗を最大10件取得する。`geo` がある店舗のみ対象とし、距離順にソートする。
- システムから店舗情報を新規登録・更新しない。管理者が手動で登録・更新する。頻度は低く、月1回程度。

## Container Design

### `gourmet`

```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "pk": "gourmet",
    "season": 1,
    "episode": 1,
    "restaurantNumber": 1,
    "title": "江東区門前仲町のやきとりと焼きめし",
    "restaurant": "庄助",
    "access": "門前仲町駅　徒歩2分",
    "phoneNumber": "03-3643-9648",
    "address": "東京都江東区富岡1-2-8",
    "status": "active",
    "geo": {
      "type": "Point",
      "coordinates": [
        139.796387,
        35.6710663
      ]
    }
  }
]
```

- `season` と `episode` の組み合わせで、通常は登場回を識別する。1話に複数店舗が登場する場合は、同じ組み合わせのオブジェクトを複数登録できる。
- Partition keyは、`pk`（string、固定値 gourmet）。全店舗取得と近隣検索が主経路で、データ量も約240件と小さいため、全アイテムを同一論理パーティションに配置してクロスパーティション検索を避ける。
- `phoneNumber`、`address`、`access` は、情報がない場合も項目を省略せず空文字列で保持する。
- `status` は `active`、`closed`、`temporarilyClosed`、`unknown` のいずれかを設定する。
- 位置情報が不明な場合は `geo` を省略する。`0.0, 0.0` の仮座標は保存しない。地図表示や距離計算では、`geo` が存在する店舗だけを対象にする。
- `geo.coordinates` は必ず `[経度, 緯度]` の順にする。

## Attributes

### `gourmet`

| 項目 | 型 | 必須 | 内容 |
| --- | --- | --- | --- |
| `id` | string | 必須 | Cosmos DBでアイテムを一意に識別するGUID。 |
| `pk` | string | 必須 | パーティションキー。常に固定文字列 `"gourmet"` を設定する。 |
| `season` | number | 必須 | 登場したシーズン番号。1以上の整数。 |
| `episode` | number | 必須 | 登場したエピソード番号。1以上の整数。 |
| `restaurantNumber` | number | 必須 | 同一 `episode` 内の店舗連番。1以上の整数。 |
| `title` | string | 必須 | エピソードの題名。 |
| `restaurant` | string | 必須 | 店舗名。 |
| `access` | string | 必須 | 最寄り駅からの徒歩時間など。不明時は空文字列。 |
| `phoneNumber` | string | 必須 | ハイフンを含む電話番号。不明時は空文字列。 |
| `address` | string | 必須 | 店舗住所。不明時は空文字列。 |
| `status` | string | 必須 | `active`、`closed`、`temporarilyClosed`、`unknown` のいずれか。 |
| `geo` | object | 任意 | 空間検索と地図表示用のGeoJSON `Point`。位置不明時は省略する。 |

### `geo`

| 項目 | 型 | 必須 | 内容 |
| --- | --- | --- | --- |
| `type` | string | 必須 | 常に `Point`。 |
| `coordinates` | array[number] | 必須 | `[経度, 緯度]` の順。 |

## Indexing Strategy

データ更新頻度が月1回程度で、読み取りが主であるため、インデックス作成ポリシーは初期値とする。
ただし、空間検索が全件スキャンにならないように、`spatialIndexes` インデックスのみ追加で設定する。

```json
{
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
        {
            "path": "/*"
        }
    ],
    "excludedPaths": [
        {
            "path": "/\"_etag\"/?"
        }
    ],
    "compositeIndexes": [
        [
            { "path": "/season", "order": "ascending" },
            { "path": "/episode", "order": "ascending" },
            { "path": "/restaurantNumber", "order": "ascending" }
        ]
    ],
    "spatialIndexes": [
        {
            "path": "/geo/?",
            "types": ["Point"]
        }
    ]    
}
```

