namespace goro_webapp.Infrastructure
{
    /// <summary>
    /// 住所や場所をジオコーディングするサービスのインターフェースです。
    /// </summary>
    public interface IGeocodeServiceClient
    {
        /// <summary>
        /// 指定された住所を緯度と経度へ変換します。
        /// </summary>
        /// <param name="address">緯度・経度へ変換する住所または場所。</param>
        /// <returns>取得した座標。変換できない場合は null。</returns>
        Task<(double Latitude, double Longitude)?> GeocodeAsync(string address);
    }
}
