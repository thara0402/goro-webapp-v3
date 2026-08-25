using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using goro_webapp.Infrastructure.Entity;

namespace goro_webapp.Models
{
    /// <summary>
    /// 店舗詳細画面や店舗一覧画面で使用する表示用モデルです。
    /// </summary>
    public class Gourmet
    {
        /// <summary>
        /// 店舗の地理座標です。
        /// </summary>
        [DisplayName("位置情報")]
        public GeoPoint? Geo { get; set; }

        /// <summary>
        /// 店舗を一意に識別する ID です。
        /// </summary>
        [DisplayName("ID")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// ドラマのシーズン番号です。
        /// </summary>
        [DisplayName("シーズン")]
        public int Season { get; set; }

        /// <summary>
        /// ドラマの話数です。
        /// </summary>
        [DisplayName("エピソード")]
        public int Episode { get; set; }

        /// <summary>
        /// エピソードのタイトルです。
        /// </summary>
        [DisplayName("タイトル")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 店舗名です。
        /// </summary>
        [DisplayName("店舗名")]
        public string Restaurant { get; set; } = string.Empty;

        /// <summary>
        /// 店舗へのアクセス情報です。
        /// </summary>
        [DisplayName("アクセス")]
        public string Access { get; set; } = string.Empty;

        /// <summary>
        /// 店舗の電話番号です。
        /// </summary>
        [DisplayName("電話番号")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 店舗の住所です。
        /// </summary>
        [DisplayName("住所")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 店舗の営業状態です。
        /// </summary>
        [DisplayName("営業状態")]
        public GourmetStatus Status { get; set; }
    }
}
