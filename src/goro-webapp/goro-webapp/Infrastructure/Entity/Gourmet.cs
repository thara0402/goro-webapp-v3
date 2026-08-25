using Newtonsoft.Json;

namespace goro_webapp.Infrastructure.Entity
{
    /// <summary>
    /// ドラマに登場する飲食店の情報を表すエンティティです。
    /// </summary>
    public class Gourmet
    {
        /// <summary>
        /// 店舗を一意に識別する ID です。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// パーティションキー。常に固定値 "gourmet" を設定する。
        /// </summary>
        [JsonProperty("pk")]
        public string Pk { get; set; } = "gourmet";
        /// <summary>
        /// ドラマのシーズン番号です。
        /// </summary>
        [JsonProperty("season")]
        public int Season { get; set; }
        /// <summary>
        /// ドラマの話数です。
        /// </summary>
        [JsonProperty("episode")]
        public int Episode { get; set; }
        /// <summary>
        /// エピソードのタイトルです。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 店舗名です。
        /// </summary>
        [JsonProperty("restaurant")]
        public string Restaurant { get; set; } = string.Empty;
        /// <summary>
        /// 同一 episode 内の店舗連番。1以上の整数。
        /// </summary>
        [JsonProperty("restaurantNumber")]
        public int RestaurantNumber { get; set; }
        /// <summary>
        /// 店舗へのアクセス情報です。
        /// </summary>
        [JsonProperty("access")]
        public string Access { get; set; } = string.Empty;
        /// <summary>
        /// 店舗の電話番号です。
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// 店舗の住所です。
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// 店舗の営業状態。
        /// </summary>
        [JsonProperty("status")]
        public GourmetStatus Status { get; set; }
        /// <summary>
        /// 店舗の地理座標です。未登録の場合は null です。
        /// </summary>
        [JsonProperty("geo")]
        public GeoPoint? Geo { get; set; }
    }
}
