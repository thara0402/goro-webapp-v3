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
        /// <returns>取得した座標。住所が空、または座標を取得できない場合は null。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合。</exception>
        Task<(double Latitude, double Longitude)?> GeocodeAsync(string address);
    }
}
