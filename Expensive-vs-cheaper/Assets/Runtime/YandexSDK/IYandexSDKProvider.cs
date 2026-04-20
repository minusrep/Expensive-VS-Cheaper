using Cysharp.Threading.Tasks;

namespace DoubleB.Runtime.Runtime.YandexSDK
{
    public interface IYandexSDKProvider
    {
        bool IsInitialized { get; }

        UniTask InitializeAsync();
        string GetEnvironmentJson();
        UniTask<bool> IsAvailableMethodAsync(string methodName);

        UniTask LoadingReadyAsync();
        UniTask GameplayStartAsync();
        UniTask GameplayStopAsync();

        UniTask<YandexFullscreenAdResult> ShowFullscreenAdAsync();
        UniTask<YandexRewardedAdResult> ShowRewardedVideoAsync();
        UniTask<string> GetBannerAdStatusJsonAsync();
        UniTask<string> ShowBannerAdAsync();
        UniTask<string> HideBannerAdAsync();

        UniTask<string> GetPlayerJsonAsync();
        UniTask<string> OpenAuthDialogAsync();
        UniTask<string> GetPlayerDataJsonAsync(string keysJson = null);
        UniTask SetPlayerDataAsync(string dataJson, bool flush = true);
        UniTask<string> GetPlayerStatsJsonAsync(string keysJson = null);
        UniTask SetPlayerStatsAsync(string statsJson);
        UniTask<string> IncrementPlayerStatsAsync(string statsJson);

        UniTask SetLeaderboardScoreAsync(string leaderboardName, int score, string extraData = null);
        UniTask<string> GetLeaderboardEntriesJsonAsync(string leaderboardName, int quantityTop = 10, bool includeUser = true);

        UniTask InitializePaymentsAsync();
        UniTask<string> GetCatalogJsonAsync();
        UniTask<string> PurchaseAsync(string productId);
        UniTask<string> GetPurchasesJsonAsync();
        UniTask ConsumePurchaseAsync(string purchaseToken);
    }
}
