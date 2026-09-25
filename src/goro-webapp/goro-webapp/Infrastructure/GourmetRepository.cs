using goro_webapp.Infrastructure.Entity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace goro_webapp.Infrastructure
{
    /// <summary>
    /// Cosmos DB に保存された店舗データを取得するリポジトリです。
    /// </summary>
    public class GourmetRepository : IGourmetRepository
    {
        private const string PartitionKeyValue = "gourmet";

        private readonly Container _container;

        /// <summary>
        /// Cosmos DB クライアントから店舗コンテナーを取得します。
        /// </summary>
        /// <param name="cosmosClient">Cosmos DB へ接続するクライアント。</param>
        public GourmetRepository(CosmosClient cosmosClient)
        {
            _container = cosmosClient.GetContainer("goro-database", "gourmet");
        }

        /// <summary>
        /// 店舗データをシーズン、話数、店舗番号の順に取得します。
        /// </summary>
        /// <returns>並び順を適用した店舗一覧。</returns>
        public async Task<IList<Gourmet>> GetAsync()
        {
            var result = new List<Gourmet>();

            // 単一パーティション（pk="gourmet"）を明示することでクロスパーティションクエリを回避する
            var queryRequestOptions = new QueryRequestOptions { PartitionKey = new PartitionKey(PartitionKeyValue) };

            // Cosmos DB の継続トークンを使って、すべての結果をページ単位で読み取る。
            using (var iterator = _container.GetItemLinqQueryable<Gourmet>(requestOptions: queryRequestOptions)
                .OrderBy(x => x.Season)
                .ThenBy(x => x.Episode)
                .ThenBy(x => x.RestaurantNumber)
                .ToFeedIterator())
            {
                do
                {
                    result.AddRange(await iterator.ReadNextAsync());

                } while (iterator.HasMoreResults);
            }
            return result;
        }

        /// <summary>
        /// 指定された ID の店舗を取得します。
        /// </summary>
        /// <param name="id">取得する店舗の ID。</param>
        /// <returns>該当する店舗。存在しない場合は null。</returns>
        public async Task<Gourmet?> GetByIdAsync(string id)
        {
            // パーティションキーを指定して、対象店舗を効率よく読み取る。
            try
            {
                var response = await _container.ReadItemAsync<Gourmet>(id, new PartitionKey(PartitionKeyValue));
                return response.Resource;
            }
            // Cosmos DB の 404 は通常の未検出として null に変換する。
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <summary>
        /// 指定された座標から近い順に店舗を取得します。
        /// </summary>
        /// <param name="latitude">検索地点の緯度。</param>
        /// <param name="longitude">検索地点の経度。</param>
        /// <param name="count">取得する店舗数。省略時は 10。</param>
        /// <returns>指定地点から近い順に並んだ店舗一覧。</returns>
        public async Task<IList<Gourmet>> GetNearestAsync(double latitude, double longitude, int count = 10)
        {
            var result = new List<Gourmet>();

            // ORDER BY ST_DISTANCE は空間インデックスのみで解決でき、複合インデックスは不要
            // 座標が登録されている店舗だけを対象に、指定件数まで距離順で取得する。
            var queryDefinition = new QueryDefinition(
                "SELECT TOP @count * FROM c " +
                "WHERE c.pk = @pk AND IS_DEFINED(c.geo) " +
                "ORDER BY ST_DISTANCE(c.geo, {\"type\": \"Point\", \"coordinates\": [@longitude, @latitude]})")
                .WithParameter("@count", count)
                .WithParameter("@pk", PartitionKeyValue)
                .WithParameter("@longitude", longitude)
                .WithParameter("@latitude", latitude);

            var queryRequestOptions = new QueryRequestOptions { PartitionKey = new PartitionKey(PartitionKeyValue) };

            // クエリ結果をページ単位で読み取り、一覧へ追加する。
            using (var iterator = _container.GetItemQueryIterator<Gourmet>(queryDefinition, requestOptions: queryRequestOptions))
            {
                do
                {
                    result.AddRange(await iterator.ReadNextAsync());

                } while (iterator.HasMoreResults);
            }
            return result;
        }
    }
}
