namespace goro_webapp.Models
{
    /// <summary>
    /// エラー画面に表示する情報を保持するビューモデルです。
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// エラーが発生したリクエストの ID です。
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// リクエスト ID を画面に表示するかどうかを示します。
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}