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
    /// 該当店舗が 10 件未満の場合に、取得した件数だけを距離順のまま表示することを確認します。
    /// </summary>
    [TestMethod]
    public async Task Index_GeocodeSucceeds_WithFewerThanTenResults_ReturnsAllGourmetsInDistanceOrder()
    {
        var entities = CreateEntities(9);
        var mapped = CreateModels(9);

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

        Assert.HasCount(9, gourmets);
        CollectionAssert.AreEqual(mapped.Select(x => x.Id).ToList(), gourmets.Select(x => x.Id).ToList());

        repository.Verify(x => x.GetNearestAsync(35.1, 139.2, 10), Times.Once);
        mapper.Verify(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities), Times.Once);
    }

    /// <summary>
    /// 該当店舗がちょうど 10 件の場合に、10 件すべてを距離順のまま表示することを確認します。
    /// </summary>
    [TestMethod]
    public async Task Index_GeocodeSucceeds_WithTenResults_ReturnsTenGourmetsInDistanceOrder()
    {
        var entities = CreateEntities(10);
        var mapped = CreateModels(10);

        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetNearestAsync(35.1, 139.2, 10)).ReturnsAsync(entities);

        var geocodeService = new Mock<IGeocodeServiceClient>();
        geocodeService.Setup(x => x.GeocodeAsync("meguro")).ReturnsAsync((35.1, 139.2));

        var mapper = new Mock<AutoMapper.IMapper>();
        mapper.Setup(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities)).Returns(mapped);

        var sut = new NaviController(repository.Object, geocodeService.Object, mapper.Object);

        var result = await sut.Index("meguro");

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<NaviViewModel>(view.Model);
        var gourmets = model.Gourmets.ToList();

        Assert.HasCount(10, gourmets);
        CollectionAssert.AreEqual(mapped.Select(x => x.Id).ToList(), gourmets.Select(x => x.Id).ToList());

        repository.Verify(x => x.GetNearestAsync(35.1, 139.2, 10), Times.Once);
        mapper.Verify(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities), Times.Once);
    }

    /// <summary>
    /// 該当店舗が 11 件以上ある場合でも、画面表示用の結果は距離順の先頭 10 件に制限されることを確認します。
    /// </summary>
    [TestMethod]
    public async Task Index_GeocodeSucceeds_WithMoreThanTenResults_ReturnsOnlyFirstTenGourmetsInDistanceOrder()
    {
        var entities = CreateEntities(11);
        var mapped = CreateModels(11);

        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetNearestAsync(35.1, 139.2, 10)).ReturnsAsync(entities);

        var geocodeService = new Mock<IGeocodeServiceClient>();
        geocodeService.Setup(x => x.GeocodeAsync("ueno")).ReturnsAsync((35.1, 139.2));

        var mapper = new Mock<AutoMapper.IMapper>();
        mapper.Setup(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities)).Returns(mapped);

        var sut = new NaviController(repository.Object, geocodeService.Object, mapper.Object);

        var result = await sut.Index("ueno");

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<NaviViewModel>(view.Model);
        var gourmets = model.Gourmets.ToList();

        Assert.HasCount(10, gourmets);
        CollectionAssert.AreEqual(mapped.Take(10).Select(x => x.Id).ToList(), gourmets.Select(x => x.Id).ToList());
        Assert.DoesNotContain(mapped[10].Id, gourmets.Select(x => x.Id).ToList());

        repository.Verify(x => x.GetNearestAsync(35.1, 139.2, 10), Times.Once);
        mapper.Verify(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities), Times.Once);
    }

    private static List<goro_webapp.Infrastructure.Entity.Gourmet> CreateEntities(int count)
    {
        var result = new List<goro_webapp.Infrastructure.Entity.Gourmet>(count);
        for (var index = 1; index <= count; index++)
        {
            result.Add(new goro_webapp.Infrastructure.Entity.Gourmet
            {
                Id = $"id-{index}",
                Season = index,
                Episode = index,
                Title = $"Title {index}",
                Restaurant = $"Restaurant {index}"
            });
        }

        return result;
    }

    private static List<goro_webapp.Models.Gourmet> CreateModels(int count)
    {
        var result = new List<goro_webapp.Models.Gourmet>(count);
        for (var index = 1; index <= count; index++)
        {
            result.Add(new goro_webapp.Models.Gourmet
            {
                Id = $"id-{index}",
                Season = index,
                Episode = index,
                Title = $"Title {index}",
                Restaurant = $"Restaurant {index}"
            });
        }

        return result;
    }
}
