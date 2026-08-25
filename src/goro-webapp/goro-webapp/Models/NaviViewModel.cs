namespace goro_webapp.Models
{
    /// <summary>
    /// 店舗ナビゲーション画面に表示する検索条件と店舗一覧を保持するビューモデルです。
    /// </summary>
    public class NaviViewModel
    {
        /// <summary>
        /// 入力された住所または場所の検索文字列です。
        /// </summary>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// 検索地点の周辺にある店舗一覧です。
        /// </summary>
        public IEnumerable<Gourmet> Gourmets { get; set; } = [];
    }
}