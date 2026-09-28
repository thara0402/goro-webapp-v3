using System.IO;

namespace goro_webapp.Tests.Views;

/// <summary>
/// 受け入れ条件 4 に関わる Razor ビューの構造を検証するテストクラスです。
/// </summary>
[TestClass]
public sealed class RazorViewStructureTests
{
    /// <summary>
    /// 店舗詳細ビューに、座標がある場合の地図描画要素と Google Maps 初期化スクリプトが定義されていることを確認します。
    /// </summary>
    [TestMethod]
    public void DetailsView_WithGeoCoordinates_ContainsMapContainerAndGoogleMapsScript()
    {
        var content = ReadViewContent("Home", "Details.cshtml");

        Assert.Contains("@if (Model.Geo != null && Model.Geo.Coordinates.Length == 2)", content);
        Assert.Contains("<div id=\"map\" class=\"map-container\"></div>", content);
        Assert.Contains("function initMap()", content);
        Assert.Contains("new google.maps.Map(document.getElementById(\"map\")", content);
        Assert.Contains("https://maps.googleapis.com/maps/api/js?key=@ViewData[\"GoogleMapsApiKey\"]&callback=initMap", content);
    }

    /// <summary>
    /// 近くの店舗検索ビューに、検索結果から詳細ページへ戻り先付きで遷移するリンクが定義されていることを確認します。
    /// </summary>
    [TestMethod]
    public void NaviIndexView_SearchResultsContainDetailsLinkWithReturnUrl()
    {
        var content = ReadViewContent("Navi", "Index.cshtml");

        Assert.Contains("asp-controller=\"Home\"", content);
        Assert.Contains("asp-action=\"Details\"", content);
        Assert.Contains("asp-route-id=\"@item.Id\"", content);
        Assert.Contains("asp-route-returnUrl=\"@Url.Action(\"Index\", \"Navi\", new { query = Model.Query })\"", content);
    }

    private static string ReadViewContent(string viewFolder, string fileName)
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "goro-webapp",
            "Views",
            viewFolder,
            fileName));

        Assert.IsTrue(File.Exists(path), $"ビュー ファイルが見つかりません: {path}");

        return File.ReadAllText(path);
    }
}
