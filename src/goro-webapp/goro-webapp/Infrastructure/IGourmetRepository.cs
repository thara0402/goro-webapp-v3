using goro_webapp.Infrastructure.Entity;

namespace goro_webapp.Infrastructure
{
    /// <summary>
    /// 店舗データを取得するリポジトリのインターフェースです。
    /// </summary>
    public interface IGourmetRepository
    {
        /// <summary>
        /// すべての店舗データを取得します。
        /// </summary>
        /// <returns>店舗データの一覧。</returns>
        Task<IList<Gourmet>> GetAsync();
        /// <summary>
        /// 指定された ID の店舗を取得します。
        /// </summary>
        /// <param name="id">取得する店舗の ID。</param>
        /// <returns>該当する店舗。存在しない場合は null。</returns>
        Task<Gourmet?> GetByIdAsync(string id);
        /// <summary>
        /// 指定された座標から近い順に店舗を取得します。
        /// </summary>
        /// <param name="latitude">検索地点の緯度。</param>
        /// <param name="longitude">検索地点の経度。</param>
        /// <param name="count">取得する店舗数。省略時は 10。</param>
        /// <returns>指定地点から近い順に並んだ店舗一覧。</returns>
        Task<IList<Gourmet>> GetNearestAsync(double latitude, double longitude, int count = 10);
    }
}
