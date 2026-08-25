using Newtonsoft.Json;

namespace goro_webapp.Infrastructure.Entity
{
    /// <summary>
    /// GeoJSON 形式の地点情報を表します。
    /// </summary>
    public class GeoPoint
    {
        /// <summary>
        /// GeoJSON のジオメトリ種別。地点情報では Point を使用します。
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = "Point";
        /// <summary>
        /// 地点の座標。GeoJSON の仕様に従い [経度, 緯度] の順で保持します。
        /// </summary>
        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; } = [];
    }
}
