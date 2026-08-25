using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace goro_webapp.Infrastructure.Entity
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    /// <summary>
    /// 店舗の営業状態を表します。JSON では文字列としてシリアライズされます。
    /// </summary>
    public enum GourmetStatus
    {
        /// <summary>
        /// 店舗が営業中です。
        /// </summary>
        [Display(Name = "営業中")]
        Active,
        /// <summary>
        /// 店舗が閉店しています。
        /// </summary>
        [Display(Name = "閉店")]
        Closed,
        /// <summary>
        /// 店舗が一時的に休業しています。
        /// </summary>
        [Display(Name = "一時休業")]
        TemporarilyClosed,
        /// <summary>
        /// 店舗の営業状態が不明です。
        /// </summary>
        [Display(Name = "不明")]
        Unknown,
    }
}
