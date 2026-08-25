using Azure.Identity;
using goro_webapp.Infrastructure;
using goro_webapp.Models;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Azure.Cosmos.Fluent;

// アプリケーションの設定とサービス登録を行うビルダーを作成する。
var builder = WebApplication.CreateBuilder(args);

// MVC、外部サービス、データアクセスに必要な依存関係を登録する。
if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultUrl"] ?? ""), new DefaultAzureCredential());
}
builder.Services.AddControllersWithViews();
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("WebApp"));
builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions { ConnectionString = builder.Configuration["WebApp:AppInsightsConnectionString"] });
builder.Services.AddSingleton(new CosmosClientBuilder(builder.Configuration["WebApp:CosmosConnection"])
    .WithConnectionModeDirect()
    .Build());
builder.Services.AddTransient<IGourmetRepository, GourmetRepository>();
builder.Services.AddHttpClient<IGeocodeServiceClient, GeocodeServiceClient>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<GourmetProfile>());

var app = builder.Build();

// HTTP リクエストの処理パイプラインを構成する。
// 本番環境では例外画面と HSTS を有効にする。
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
