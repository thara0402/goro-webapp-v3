using AutoMapper;
using goro_webapp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace goro_webapp.Tests.Models;

/// <summary>
/// GourmetProfile の AutoMapper 設定と Entity から画面モデルへの変換を検証するテストクラスです。
/// </summary>
[TestClass]
public sealed class GourmetProfileTests
{


    /// <summary>
    /// AutoMapper のマッピング設定が有効であることを確認します。
    /// </summary>
    [TestMethod]
    public void Configuration_IsValid()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<GourmetProfile>());

        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    /// <summary>
    /// Entity の主要なプロパティが画面モデルへ変換されることを確認します。
    /// </summary>


    [TestMethod]
    public void Map_EntityToModel_MapsMainFields()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<GourmetProfile>());

        var mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();

        var entity = new goro_webapp.Infrastructure.Entity.Gourmet
        {
            Id = "id-1",
            Season = 3,
            Episode = 7,
            Title = "title",
            Restaurant = "restaurant",
            Access = "station",
            PhoneNumber = "000-0000",
            Address = "tokyo"
        };

        var model = mapper.Map<goro_webapp.Models.Gourmet>(entity);

        Assert.AreEqual(entity.Id, model.Id);
        Assert.AreEqual(entity.Season, model.Season);
        Assert.AreEqual(entity.Episode, model.Episode);
        Assert.AreEqual(entity.Title, model.Title);
        Assert.AreEqual(entity.Restaurant, model.Restaurant);
        Assert.AreEqual(entity.Access, model.Access);
        Assert.AreEqual(entity.PhoneNumber, model.PhoneNumber);
        Assert.AreEqual(entity.Address, model.Address);
    }
}
