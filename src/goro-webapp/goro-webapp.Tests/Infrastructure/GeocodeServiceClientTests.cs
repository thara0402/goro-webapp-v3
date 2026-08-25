using System.Net;
using System.Text;
using goro_webapp.Infrastructure;
using goro_webapp.Models;
using Microsoft.Extensions.Options;

namespace goro_webapp.Tests.Infrastructure;

/// <summary>
/// GeocodeServiceClient の入力検証、API エラー処理、座標変換を検証するテストクラスです。
/// </summary>
[TestClass]
public sealed class GeocodeServiceClientTests
{


    /// <summary>
    /// 住所が空の場合に null を返すことを確認します。
    /// </summary>
    [TestMethod]
    public async Task GeocodeAsync_AddressIsEmpty_ReturnsNull()
    {
        var client = new GeocodeServiceClient(CreateHttpClient(HttpStatusCode.OK, "{}"), Options.Create(new MySettings
        {
            GoogleGeocodingApiKey = "dummy"
        }));

        var result = await client.GeocodeAsync(" ");

        Assert.IsNull(result);
    }

    /// <summary>
    /// HTTP レスポンスが失敗の場合に HttpRequestException をスローすることを確認します。
    /// </summary>


    [TestMethod]
    public async Task GeocodeAsync_ResponseIsFailure_ThrowsHttpRequestException()
    {
        var client = new GeocodeServiceClient(CreateHttpClient(HttpStatusCode.BadRequest, "{}"), Options.Create(new MySettings
        {
            GoogleGeocodingApiKey = "dummy"
        }));

        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => client.GeocodeAsync("tokyo"));
    }

    /// <summary>
    /// API ステータスが OK でない場合に null を返すことを確認します。
    /// </summary>


    [TestMethod]
    public async Task GeocodeAsync_StatusIsNotOk_ReturnsNull()
    {
        var json = """
        {
          "status": "ZERO_RESULTS",
          "results": []
        }
        """;

        var client = new GeocodeServiceClient(CreateHttpClient(HttpStatusCode.OK, json), Options.Create(new MySettings
        {
            GoogleGeocodingApiKey = "dummy"
        }));

        var result = await client.GeocodeAsync("tokyo");

        Assert.IsNull(result);
    }

    /// <summary>
    /// API が正常な座標を返した場合に緯度と経度を取得できることを確認します。
    /// </summary>


    [TestMethod]
    public async Task GeocodeAsync_StatusOkWithLocation_ReturnsCoordinates()
    {
        var json = """
        {
          "status": "OK",
          "results": [
            {
              "geometry": {
                "location": {
                  "lat": 35.681236,
                  "lng": 139.767125
                }
              }
            }
          ]
        }
        """;

        var client = new GeocodeServiceClient(CreateHttpClient(HttpStatusCode.OK, json), Options.Create(new MySettings
        {
            GoogleGeocodingApiKey = "dummy"
        }));

        var result = await client.GeocodeAsync("tokyo station");

        Assert.IsNotNull(result);
        Assert.AreEqual(35.681236, result.Value.Latitude);
        Assert.AreEqual(139.767125, result.Value.Longitude);
    }

    /// <summary>
    /// 指定した HTTP ステータスと JSON を返すテスト用 HTTP クライアントを作成します。
    /// </summary>
    private static HttpClient CreateHttpClient(HttpStatusCode statusCode, string content)
    {
        var handler = new StubHttpMessageHandler(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        });

        return new HttpClient(handler);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public StubHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
