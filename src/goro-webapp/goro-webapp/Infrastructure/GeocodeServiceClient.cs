using goro_webapp.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace goro_webapp.Infrastructure
{
    /// <summary>
    /// Google Geocoding API を利用して住所を緯度・経度へ変換するサービスです。
    /// </summary>
    public class GeocodeServiceClient : IGeocodeServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        /// <summary>
        /// HTTP クライアントと Google Geocoding API の設定を受け取ります。
        /// </summary>
        /// <param name="httpClient">Geocoding API へリクエストを送信する HTTP クライアント。</param>
        /// <param name="settings">Google Geocoding API キーを含むアプリケーション設定。</param>
        public GeocodeServiceClient(HttpClient httpClient, IOptions<MySettings> settings)
        {
            _httpClient = httpClient;
            _apiKey = settings.Value.GoogleGeocodingApiKey;
        }

        /// <summary>
        /// 指定された住所をジオコーディングし、緯度と経度を返します。
        /// </summary>
        /// <param name="address">緯度・経度へ変換する住所または場所。</param>
        /// <returns>取得した座標。住所が空、または座標を取得できない場合は null。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合。</exception>
        public async Task<(double Latitude, double Longitude)?> GeocodeAsync(string address)
        {
            // 空の住所は API を呼び出さず、座標なしとして扱う。
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            // 住所を URL 用にエンコードして API リクエストを組み立てる。
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            // HTTP レベルで失敗した場合は HttpRequestException をスローする。
            response.EnsureSuccessStatusCode();

            // API レスポンスの JSON から検索結果と座標を読み取る。
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // API が住所を認識できなかった場合は座標を返さない。
            if (root.GetProperty("status").GetString() != "OK")
            {
                return null;
            }

            var location = root
                .GetProperty("results")[0]
                .GetProperty("geometry")
                .GetProperty("location");

            // Google Geocoding API の location から緯度・経度を抽出する。
            var lat = location.GetProperty("lat").GetDouble();
            var lng = location.GetProperty("lng").GetDouble();

            return (lat, lng);
        }
    }
}
