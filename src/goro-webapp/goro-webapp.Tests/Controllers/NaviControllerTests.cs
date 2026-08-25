using goro_webapp.Controllers;
using goro_webapp.Infrastructure;
using goro_webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace goro_webapp.Tests.Controllers;

/// <summary>
/// NaviController の住所検索と周辺店舗検索を検証するテストクラスです。
/// </summary>
[TestClass]
public sealed class NaviControllerTests
{


    /// <summary>
    /// 検索条件が空の場合に外部サービスを呼び出さず画面を返すことを確認します。
    /// </summary>
    [TestMethod]
    public async Task Index_QueryIsEmpty_ReturnsViewWithoutServiceCalls()
    {
        var repository = new Mock<IGourmetRepository>(MockBehavior.Strict);
        var geocodeService = new Mock<IGeocodeServiceClient>(MockBehavior.Strict);
        var mapper = new Mock<AutoMapper.IMapper>(MockBehavior.Strict);

        var sut = new NaviController(repository.Object, geocodeService.Object, mapper.Object);

        var result = await sut.Index(" ");

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<NaviViewModel>(view.Model);

        Assert.AreEqual(" ", model.Query);
        Assert.IsEmpty(model.Gourmets);
        repository.VerifyNoOtherCalls();
        geocodeService.VerifyNoOtherCalls();
        mapper.VerifyNoOtherCalls();
    }

    /// <summary>
    /// ジオコーディングで座標を取得できなかった場合にリポジトリを呼び出さないことを確認します。
    /// </summary>


    [TestMethod]
    public async Task Index_GeocodeFails_ReturnsViewWithoutRepositoryCall()
    {
        var repository = new Mock<IGourmetRepository>(MockBehavior.Strict);
        var geocodeService = new Mock<IGeocodeServiceClient>();
        geocodeService.Setup(x => x.GeocodeAsync("tokyo")).ReturnsAsync(((double Latitude, double Longitude)?)null);
        var mapper = new Mock<AutoMapper.IMapper>(MockBehavior.Strict);

        var sut = new NaviController(repository.Object, geocodeService.Object, mapper.Object);

        var result = await sut.Index("tokyo");

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<NaviViewModel>(view.Model);

        Assert.AreEqual("tokyo", model.Query);
        Assert.IsEmpty(model.Gourmets);
        repository.VerifyNoOtherCalls();
        mapper.VerifyNoOtherCalls();
    }

    /// <summary>
    /// ジオコーディング成功時に周辺店舗を取得して画面モデルへ変換することを確認します。
    /// </summary>


    [TestMethod]
    public async Task Index_GeocodeSucceeds_ReturnsMappedNearestGourmets()
    {
        var entities = new List<goro_webapp.Infrastructure.Entity.Gourmet>
        {
            new() { Id = "a", Season = 1, Episode = 3, Title = "T1", Restaurant = "R1" },
            new() { Id = "b", Season = 2, Episode = 4, Title = "T2", Restaurant = "R2" }
        };

        var mapped = new List<goro_webapp.Models.Gourmet>
        {
            new() { Id = "a", Season = 1, Episode = 3, Title = "T1", Restaurant = "R1" },
            new() { Id = "b", Season = 2, Episode = 4, Title = "T2", Restaurant = "R2" }
        };

        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetNearestAsync(35.1, 139.2, 10)).ReturnsAsync(entities);

        var geocodeService = new Mock<IGeocodeServiceClient>();
        geocodeService.Setup(x => x.GeocodeAsync("shibuya")).ReturnsAsync((35.1, 139.2));

        var mapper = new Mock<AutoMapper.IMapper>();
        mapper.Setup(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities)).Returns(mapped);

        var sut = new NaviController(repository.Object, geocodeService.Object, mapper.Object);

        var result = await sut.Index("shibuya");

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<NaviViewModel>(view.Model);

        var gourmets = model.Gourmets.ToList();
        Assert.HasCount(2, gourmets);
        Assert.AreEqual("a", gourmets[0].Id);
        Assert.AreEqual("b", gourmets[1].Id);

        repository.Verify(x => x.GetNearestAsync(35.1, 139.2, 10), Times.Once);
        mapper.Verify(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities), Times.Once);
    }
}

