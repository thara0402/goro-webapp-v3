using Microsoft.AspNetCore.Mvc.Rendering;

namespace goro_webapp.Models
{
    /// <summary>
    /// 店舗一覧画面に表示するシーズンと店舗情報を保持するビューモデルです。
    /// </summary>
    public class GourmetIndexViewModel
    {
        /// <summary>
        /// 現在選択されているシーズン番号です。
        /// </summary>
        public int SelectedSeason { get; set; }

        /// <summary>
        /// シーズン選択用のリストです。
        /// </summary>
        public SelectList Seasons { get; set; } = default!;

        /// <summary>
        /// 選択されたシーズンに該当する店舗一覧です。
        /// </summary>
        public IEnumerable<Gourmet> Gourmets { get; set; } = [];
    }
}