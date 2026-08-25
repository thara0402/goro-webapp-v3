using goro_webapp.Controllers;
using goro_webapp.Infrastructure;
using goro_webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace goro_webapp.Tests.Controllers;

/// <summary>
/// HomeController の一覧表示と詳細表示を検証するテストクラスです。
/// </summary>
[TestClass]
public sealed class HomeControllerTests
{


    /// <summary>
    /// 店舗データが空の場合に、選択シーズンだけを保持したモデルを返すことを確認します。
    /// </summary>
    [TestMethod]
    public async Task Index_RepositoryReturnsEmpty_ReturnsViewModelWithSelectedSeasonOnly()
    {
        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetAsync()).ReturnsAsync(new List<goro_webapp.Infrastructure.Entity.Gourmet>());

        var mapper = new Mock<AutoMapper.IMapper>(MockBehavior.Strict);
        var sut = new HomeController(repository.Object, mapper.Object, Options.Create(new MySettings()));

        var result = await sut.Index(3);

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<GourmetIndexViewModel>(view.Model);

        Assert.AreEqual(3, model.SelectedSeason);
        Assert.IsEmpty(model.Gourmets);
        Assert.IsNull(model.Seasons);
    }

    /// <summary>
    /// 店舗データをシーズンで絞り込み、話数順に並べることを確認します。
    /// </summary>


    [TestMethod]
    public async Task Index_RepositoryHasData_FiltersBySeasonAndOrdersByEpisode()
    {
        var entities = new List<goro_webapp.Infrastructure.Entity.Gourmet>
        {
            new() { Id = "1", Season = 2, Episode = 4, Title = "A", Restaurant = "R1" },
            new() { Id = "2", Season = 2, Episode = 2, Title = "B", Restaurant = "R2" },
            new() { Id = "3", Season = 1, Episode = 8, Title = "C", Restaurant = "R3" }
        };

        var mapped = new List<goro_webapp.Models.Gourmet>
        {
            new() { Id = "1", Season = 2, Episode = 4, Title = "A", Restaurant = "R1" },
            new() { Id = "2", Season = 2, Episode = 2, Title = "B", Restaurant = "R2" },
            new() { Id = "3", Season = 1, Episode = 8, Title = "C", Restaurant = "R3" }
        };

        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetAsync()).ReturnsAsync(entities);

        var mapper = new Mock<AutoMapper.IMapper>();
        mapper.Setup(x => x.Map<IEnumerable<goro_webapp.Models.Gourmet>>(entities)).Returns(mapped);

        var sut = new HomeController(repository.Object, mapper.Object, Options.Create(new MySettings()));

        var result = await sut.Index(2);

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var model = Assert.IsInstanceOfType<GourmetIndexViewModel>(view.Model);

        Assert.AreEqual(2, model.SelectedSeason);
        Assert.IsNotNull(model.Seasons);

        var gourmets = model.Gourmets.ToList();
        Assert.HasCount(2, gourmets);
        Assert.AreEqual(2, gourmets[0].Episode);
        Assert.AreEqual(4, gourmets[1].Episode);
    }

    /// <summary>
    /// 指定した店舗が存在しない場合に 404 を返すことを確認します。
    /// </summary>


    [TestMethod]
    public async Task Details_ItemNotFound_ReturnsNotFound()
    {
        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetByIdAsync("missing")).ReturnsAsync((goro_webapp.Infrastructure.Entity.Gourmet?)null);

        var mapper = new Mock<AutoMapper.IMapper>(MockBehavior.Strict);
        var sut = new HomeController(repository.Object, mapper.Object, Options.Create(new MySettings()));

        var result = await sut.Details("missing", "/back");

        Assert.IsInstanceOfType<NotFoundResult>(result);
    }

    /// <summary>
    /// 店舗が存在する場合に詳細モデルと画面設定を返すことを確認します。
    /// </summary>


    [TestMethod]
    public async Task Details_ItemFound_SetsViewDataAndReturnsView()
    {
        var entity = new goro_webapp.Infrastructure.Entity.Gourmet
        {
            Id = "id-1",
            Season = 1,
            Episode = 1,
            Title = "title",
            Restaurant = "restaurant"
        };

        var model = new goro_webapp.Models.Gourmet
        {
            Id = "id-1",
            Season = 1,
            Episode = 1,
            Title = "title",
            Restaurant = "restaurant"
        };

        var repository = new Mock<IGourmetRepository>();
        repository.Setup(x => x.GetByIdAsync("id-1")).ReturnsAsync(entity);

        var mapper = new Mock<AutoMapper.IMapper>();
        mapper.Setup(x => x.Map<goro_webapp.Models.Gourmet>(entity)).Returns(model);

        var settings = new MySettings
        {
            GoogleMapsApiKey = "maps-key"
        };

        var sut = new HomeController(repository.Object, mapper.Object, Options.Create(settings));

        var result = await sut.Details("id-1", "/return");

        var view = Assert.IsInstanceOfType<ViewResult>(result);
        var returned = Assert.IsInstanceOfType<goro_webapp.Models.Gourmet>(view.Model);

        Assert.AreEqual("id-1", returned.Id);
        Assert.AreEqual("maps-key", sut.ViewData["GoogleMapsApiKey"] as string);
        Assert.AreEqual("/return", sut.ViewData["ReturnUrl"] as string);
    }
}
