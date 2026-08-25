# アーキテクチャ


## Layer Architecture

```
┌───────────────────────────────────┐
│         Presentation (UI)         │  MVC パターン
│  Models / Views / Controllers     │
├───────────────────────────────────┤
│           Data Access             │  DB アクセスと外部 API 呼び出し
│         Infrastructure            │
└───────────────────────────────────┘
```
依存方向は Presentation（Controller → Model、Controller → View） → Infrastructure のみ。逆方向の参照は禁止。  
Business Logic 層は、必要に応じて追加することも可能だが、現状のアプリケーションでは複雑なビジネスロジックが存在しないため、省略している。

## Directory Roles
### Models/
**役割**：ドメインモデル、ビュー関連モデル
**依存先**：
- ✅ 下層（DB Access、外部API）に依存可能
- ✅ 他の Model クラスに依存可能
- ❌ Controller や View に依存しない（独立性を保つ）

```csharp
public class Gourmet
{
    public string Id { get; set; } = string.Empty;
    public int Season { get; set; }
    public GeoPoint? Geo { get; set; }  // 他 Model に依存
}
```

### Views/
**役割**：HTML レンダリング、UI 表示
**依存先**：
- ✅ Model（ViewModel）からデータを受け取る
- ✅ Controller から呼び出される
- ❌ Controller のロジックに直接依存しない

```html
@model Gourmet
<h1>@Model.Season</h1>   <!-- Model のみ使用 -->
```

### Controllers/
**役割**：リクエスト処理、Model と View の調整、ビジネスロジック実行
**依存先**：
- ✅ Model（Repository 経由でアクセス）に依存
- ✅ View の選択と ViewModel 提供
- ✅ Infrastructure（Repository、Service）に依存

```csharp
public ActionResult Index()
{
    var items = _repository.GetAll();      // Infrastructure 層呼び出し
    var viewModel = _mapper.Map<IEnumerable<Gourmet>>(items);
    return View(viewModel);                // View に Model 渡す
}
```

### Infrastructure/
**役割**：データアクセス、外部 API 呼び出し、リポジトリパターンの実装
**依存先**：
- ✅ 外部サービス（DB Access、外部API）に依存
- ❌ Presentation に依存しない（独立性を保つ）

```csharp
public class GourmetRepository
{
    private readonly CosmosDbContext _context;

    public GourmetRepository(CosmosDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Gourmet> GetAll()
    {
        return _context.Gourmets.ToList();
    }
}
```

## Secret Management

- シークレットはソースコードや Git 管理対象の設定ファイルに直接記載しない。
- 本番環境では Azure Key Vault を利用し、`DefaultAzureCredential` と `AddAzureKeyVault` により秘密情報を取得する。
- ローカル開発では ASP.NET Core の User Secrets を利用し、秘密情報は `secrets.json` に保存する。
- 既存のプロジェクトでは `UserSecretsId` を定義しており、秘密情報をローカルユーザー環境に隔離して管理する設計とする。
- アプリで利用する秘密項目は、`CosmosConnection`、`AppInsightsConnectionString`、`GoogleMapsApiKey`、`GoogleGeocodingApiKey` の 4 つとし、`WebApp` セクションから読み込む。
- 取得した値は `builder.Services.Configure<MySettings>(builder.Configuration.GetSection("WebApp"));` により `MySettings` クラスへバインドし、アプリケーションの各サービスで型安全に利用する。
- これにより、開発用と本番用の設定を分離し、API キーや接続文字列の漏えいリスクを低減する。

```json
{
  "WebApp": {
    "CosmosConnection": "AccountEndpoint=...;AccountKey=...",
    "AppInsightsConnectionString": "InstrumentationKey=...;IngestionEndpoint=...",
    "GoogleMapsApiKey": "xxxxxxxxxxxxxxxx",
    "GoogleGeocodingApiKey": "xxxxxxxxxxxxxxxx"
  }
}
```

## Entity-to-Model Mapping


データベースの保存形式と画面表示形式を分離するため、Cosmos DB 用の Entity と画面表示用の Model を分けて定義する。

```text
Cosmos DB
  ↓
Infrastructure.Entity.Gourmet
  ↓ AutoMapper（GourmetProfile）
Models.Gourmet
  ↓
Razor View
```

- `GourmetRepository` は Cosmos DB から `Infrastructure.Entity.Gourmet` を取得する。
- Controller は `IMapper` を使って Entity を `Models.Gourmet` に変換し、View に渡す。
- AutoMapper は Entity から Model への一方向マッピングとする。
- 同名かつ同じ型のプロパティは規約により変換する。
- `Pk` や `RestaurantNumber` など、データベース処理専用のプロパティは画面 Model に含めない。
- `DisplayName` や `DataType` などの画面表示用属性は Model 側に定義する。

なお、現状の `Models.Gourmet.Geo` は `Infrastructure.Entity.GeoPoint` を使用しているため、Model から Infrastructure への型依存が存在する。将来、層を完全に分離する必要が生じた場合は、画面用の `GeoPoint` と `GourmetStatus` を Models 側に定義し、AutoMapper で明示的に変換する。
