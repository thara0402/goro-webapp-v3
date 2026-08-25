using AutoMapper;

namespace goro_webapp.Models
{
    /// <summary>
    /// Entity と画面表示用モデルのマッピング設定です。
    /// </summary>
    public class GourmetProfile : Profile
    {
        /// <summary>
        /// Gourmet Entity から表示用 Gourmet へのマッピングを設定します。
        /// </summary>
        public GourmetProfile()
        {
            // データアクセス層の Entity を画面表示用モデルへ変換する。
            CreateMap<Infrastructure.Entity.Gourmet, Gourmet>();
        }
    }
}