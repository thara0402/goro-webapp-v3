namespace goro_webapp.Models
{
    /// <summary>
    /// アプリケーションで使用する外部サービスと接続設定を保持します。
    /// </summary>
    public class MySettings
    {
        /// <summary>
        /// Azure Cosmos DB の接続文字列です。
        /// </summary>
        public string CosmosConnection { get; set; } = null!;

        /// <summary>
        /// Application Insights の接続文字列です。
        /// </summary>
        public string AppInsightsConnectionString { get; set; } = null!;

        /// <summary>
        /// Google Maps API のキーです。
        /// </summary>
        public string GoogleMapsApiKey { get; set; } = null!;

        /// <summary>
        /// Google Geocoding API のキーです。
        /// </summary>
        public string GoogleGeocodingApiKey { get; set; } = null!;
    }
}