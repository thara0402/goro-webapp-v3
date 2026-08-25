using AutoMapper;
using goro_webapp.Infrastructure;
using goro_webapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace goro_webapp.Controllers
{
    /// <summary>
    /// 住所検索に基づく店舗ナビゲーションを処理するコントローラーです。
    /// </summary>
    public class NaviController : Controller
    {
        private readonly IGourmetRepository _repository;
        private readonly IGeocodeServiceClient _serviceClient;
        private readonly IMapper _mapper;

        /// <summary>
        /// 店舗リポジトリ、ジオコーディングサービス、マッピング設定を受け取ります。
        /// </summary>
        /// <param name="repository">近隣店舗を取得するリポジトリ。</param>
        /// <param name="serviceClient">検索文字列を緯度・経度へ変換するサービス。</param>
        /// <param name="mapper">データモデルを画面モデルへ変換するマッパー。</param>
        public NaviController(IGourmetRepository repository, IGeocodeServiceClient serviceClient, IMapper mapper)
        {
            _repository = repository;
            _serviceClient = serviceClient;
            _mapper = mapper;
        }

        /// <summary>
        /// 指定された住所や場所の周辺にある店舗一覧画面を表示します。
        /// </summary>
        /// <param name="query">検索する住所または場所。未指定の場合は検索しません。</param>
        /// <returns>近隣店舗の検索画面。</returns>
        // GET: NaviController
        public async Task<ActionResult> Index(string? query)
        {
            // null の検索文字列でも、画面には空文字として保持する。
            var viewModel = new NaviViewModel { Query = query ?? string.Empty };

            // 検索条件が未入力の場合は、検索を実行せず初期画面を表示する。
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(viewModel);
            }

            // 検索文字列を緯度・経度へ変換し、周辺店舗を検索できるようにする。
            var location = await _serviceClient.GeocodeAsync(query);
            // 位置情報を取得できない場合は、検索条件を保持した画面を表示する。
            if (location is null)
            {
                return View(viewModel);
            }

            // 取得した座標を使って、周辺の店舗を検索する。
            var items = await _repository.GetNearestAsync(location.Value.Latitude, location.Value.Longitude);
            // 検索結果を画面表示用のモデルへ変換する。
            viewModel.Gourmets = _mapper.Map<IEnumerable<Gourmet>>(items);

            return View(viewModel);
        }
    }
}
