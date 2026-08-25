using AutoMapper;
using goro_webapp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using goro_webapp.Models;

namespace goro_webapp.Controllers
{
    /// <summary>
    /// 店舗一覧と店舗詳細画面へのリクエストを処理するコントローラーです。
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IGourmetRepository _repository;
        private readonly IMapper _mapper;
        private readonly MySettings _settings;

        /// <summary>
        /// 店舗リポジトリ、マッピング設定、アプリケーション設定を受け取ります。
        /// </summary>
        /// <param name="repository">店舗データを取得するリポジトリ。</param>
        /// <param name="mapper">データモデルを画面モデルへ変換するマッパー。</param>
        /// <param name="settings">アプリケーション設定。</param>
        public HomeController(IGourmetRepository repository, IMapper mapper, IOptions<MySettings> settings)
        {
            _repository = repository;
            _mapper = mapper;
            _settings = settings.Value;
        }

        /// <summary>
        /// 指定されたシーズンの店舗一覧画面を表示します。
        /// </summary>
        /// <param name="season">表示するシーズン番号。省略時は 1。</param>
        /// <returns>店舗一覧画面。</returns>
        public async Task<ActionResult> Index(int season = 1)
        {
            var items = await _repository.GetAsync();
            // 店舗データがない場合も、選択中のシーズンを保持した空の画面を表示する。
            if (items == null || items.Count == 0)
            {
                return View(new GourmetIndexViewModel { SelectedSeason = season });
            }
            var gourmetList = _mapper.Map<IEnumerable<Gourmet>>(items);
            // セレクトボックスには、登録されているシーズンを重複なく昇順で表示する。
            var seasons = gourmetList.Select(x => x.Season).Distinct().OrderBy(x => x).ToList();
            var viewModel = new GourmetIndexViewModel
            {
                SelectedSeason = season,
                Seasons = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                    seasons.Select(s => new { Value = s, Text = $"Season {s}" }),
                    "Value", "Text", season),
                // 一覧には選択されたシーズンの店舗だけを、話数順で渡す。
                Gourmets = gourmetList.Where(x => x.Season == season).OrderBy(x => x.Episode)
            };
            return View(viewModel);
        }

        /// <summary>
        /// 指定された ID の店舗詳細画面を表示します。
        /// </summary>
        /// <param name="id">表示する店舗の ID。</param>
        /// <param name="returnUrl">一覧画面へ戻るための URL。</param>
        /// <returns>店舗詳細画面。店舗が存在しない場合は 404。</returns>
        public async Task<ActionResult> Details(string id, string? returnUrl)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var gourmet = _mapper.Map<Gourmet>(item);
            // 詳細画面の地図表示に必要な設定値と、一覧へ戻るための URL をビューへ渡す。
            ViewData["GoogleMapsApiKey"] = _settings.GoogleMapsApiKey;
            ViewData["ReturnUrl"] = returnUrl;
            return View(gourmet);
        }
    }
}
