using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace DoubleB.Runtime.Runtime.YandexSDK
{
    [DisallowMultipleComponent]
    public sealed class YandexSDKProvider : MonoBehaviour, IYandexSDKProvider
    {
        private const string CallbackObjectName = "YandexSDKProvider";

        private readonly Dictionary<string, UniTaskCompletionSource<string>> _requests = new Dictionary<string, UniTaskCompletionSource<string>>();

        private static YandexSDKProvider _instance;
        private int _requestId;

        public static YandexSDKProvider Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var gameObject = new GameObject(CallbackObjectName);
                DontDestroyOnLoad(gameObject);
                _instance = gameObject.AddComponent<YandexSDKProvider>();
                return _instance;
            }
        }

        public bool IsInitialized
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return YandexSDK_IsInitialized() == 1;
#else
                LogEditorCall(nameof(IsInitialized));
                return true;
#endif
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            gameObject.name = CallbackObjectName;
            DontDestroyOnLoad(gameObject);
        }

        public UniTask InitializeAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync(YandexSDK_Init);
#else
            LogEditorCall(nameof(InitializeAsync));
            return UniTask.CompletedTask;
#endif
        }

        public string GetEnvironmentJson()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return YandexSDK_GetEnvironment();
#else
            LogEditorCall(nameof(GetEnvironmentJson));
            return "{\"app\":{\"id\":\"editor\"},\"i18n\":{\"lang\":\"ru\",\"tld\":\"ru\"}}";
#endif
        }

        public async UniTask<bool> IsAvailableMethodAsync(string methodName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var payload = await CallAsync((gameObjectName, callbackId) =>
                YandexSDK_IsAvailableMethod(methodName, gameObjectName, callbackId));
            var result = JsonConvert.DeserializeObject<YandexSDKAvailableMethodResult>(payload);
            return result != null && result.Available;
#else
            LogEditorCall(nameof(IsAvailableMethodAsync), $"methodName={methodName}");
            await UniTask.Yield();
            return true;
#endif
        }

        public UniTask LoadingReadyAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync(YandexSDK_LoadingReady);
#else
            LogEditorCall(nameof(LoadingReadyAsync));
            return UniTask.CompletedTask;
#endif
        }

        public UniTask GameplayStartAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync(YandexSDK_GameplayStart);
#else
            LogEditorCall(nameof(GameplayStartAsync));
            return UniTask.CompletedTask;
#endif
        }

        public UniTask GameplayStopAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync(YandexSDK_GameplayStop);
#else
            LogEditorCall(nameof(GameplayStopAsync));
            return UniTask.CompletedTask;
#endif
        }

        public async UniTask<YandexFullscreenAdResult> ShowFullscreenAdAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var payload = await CallAsync(YandexSDK_ShowFullscreenAdv);
            return JsonConvert.DeserializeObject<YandexFullscreenAdResult>(payload);
#else
            LogEditorCall(nameof(ShowFullscreenAdAsync));
            await UniTask.Yield();
            return new YandexFullscreenAdResult { WasShown = false };
#endif
        }

        public async UniTask<YandexRewardedAdResult> ShowRewardedVideoAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var payload = await CallAsync(YandexSDK_ShowRewardedVideo);
            return JsonConvert.DeserializeObject<YandexRewardedAdResult>(payload);
#else
            LogEditorCall(nameof(ShowRewardedVideoAsync));
            await UniTask.Yield();
            return new YandexRewardedAdResult { WasShown = false, Rewarded = true };
#endif
        }

        public UniTask<string> GetBannerAdStatusJsonAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_GetBannerAdvStatus);
#else
            LogEditorCall(nameof(GetBannerAdStatusJsonAsync));
            return UniTask.FromResult("{\"stickyAdvIsShowing\":false}");
#endif
        }

        public UniTask<string> ShowBannerAdAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_ShowBannerAdv);
#else
            LogEditorCall(nameof(ShowBannerAdAsync));
            return UniTask.FromResult("{\"stickyAdvIsShowing\":false}");
#endif
        }

        public UniTask<string> HideBannerAdAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_HideBannerAdv);
#else
            LogEditorCall(nameof(HideBannerAdAsync));
            return UniTask.FromResult("{\"stickyAdvIsShowing\":false}");
#endif
        }

        public UniTask<string> GetPlayerJsonAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_GetPlayer);
#else
            LogEditorCall(nameof(GetPlayerJsonAsync));
            return UniTask.FromResult("{\"authorized\":true,\"id\":\"editor\",\"name\":\"Editor\"}");
#endif
        }

        public UniTask<string> OpenAuthDialogAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_OpenAuthDialog);
#else
            LogEditorCall(nameof(OpenAuthDialogAsync));
            return UniTask.FromResult("{\"authorized\":true}");
#endif
        }

        public UniTask<string> GetPlayerDataJsonAsync(string keysJson = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync((gameObjectName, callbackId) =>
                YandexSDK_GetPlayerData(keysJson ?? string.Empty, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(GetPlayerDataJsonAsync), $"keysJson={keysJson ?? "null"}");
            return UniTask.FromResult("{}");
#endif
        }

        public UniTask SetPlayerDataAsync(string dataJson, bool flush = true)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync((gameObjectName, callbackId) =>
                YandexSDK_SetPlayerData(dataJson ?? "{}", flush ? 1 : 0, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(SetPlayerDataAsync), $"flush={flush}, dataJson={dataJson ?? "null"}");
            return UniTask.CompletedTask;
#endif
        }

        public UniTask<string> GetPlayerStatsJsonAsync(string keysJson = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync((gameObjectName, callbackId) =>
                YandexSDK_GetPlayerStats(keysJson ?? string.Empty, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(GetPlayerStatsJsonAsync), $"keysJson={keysJson ?? "null"}");
            return UniTask.FromResult("{}");
#endif
        }

        public UniTask SetPlayerStatsAsync(string statsJson)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync((gameObjectName, callbackId) =>
                YandexSDK_SetPlayerStats(statsJson ?? "{}", gameObjectName, callbackId));
#else
            LogEditorCall(nameof(SetPlayerStatsAsync), $"statsJson={statsJson ?? "null"}");
            return UniTask.CompletedTask;
#endif
        }

        public UniTask<string> IncrementPlayerStatsAsync(string statsJson)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync((gameObjectName, callbackId) =>
                YandexSDK_IncrementPlayerStats(statsJson ?? "{}", gameObjectName, callbackId));
#else
            LogEditorCall(nameof(IncrementPlayerStatsAsync), $"statsJson={statsJson ?? "null"}");
            return UniTask.FromResult(statsJson ?? "{}");
#endif
        }

        public UniTask SetLeaderboardScoreAsync(string leaderboardName, int score, string extraData = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync((gameObjectName, callbackId) =>
                YandexSDK_SetLeaderboardScore(leaderboardName, score, extraData ?? string.Empty, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(SetLeaderboardScoreAsync), $"leaderboardName={leaderboardName}, score={score}, extraData={extraData ?? "null"}");
            return UniTask.CompletedTask;
#endif
        }

        public UniTask<string> GetLeaderboardEntriesJsonAsync(string leaderboardName, int quantityTop = 10, bool includeUser = true)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync((gameObjectName, callbackId) =>
                YandexSDK_GetLeaderboardEntries(leaderboardName, quantityTop, includeUser ? 1 : 0, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(GetLeaderboardEntriesJsonAsync), $"leaderboardName={leaderboardName}, quantityTop={quantityTop}, includeUser={includeUser}");
            return UniTask.FromResult("{}");
#endif
        }

        public UniTask InitializePaymentsAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync(YandexSDK_InitPayments);
#else
            LogEditorCall(nameof(InitializePaymentsAsync));
            return UniTask.CompletedTask;
#endif
        }

        public UniTask<string> GetCatalogJsonAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_GetCatalog);
#else
            LogEditorCall(nameof(GetCatalogJsonAsync));
            return UniTask.FromResult("[]");
#endif
        }

        public UniTask<string> PurchaseAsync(string productId)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync((gameObjectName, callbackId) =>
                YandexSDK_Purchase(productId, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(PurchaseAsync), $"productId={productId}");
            return UniTask.FromResult("{\"productID\":\"" + productId + "\",\"purchaseToken\":\"editor\"}");
#endif
        }

        public UniTask<string> GetPurchasesJsonAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallAsync(YandexSDK_GetPurchases);
#else
            LogEditorCall(nameof(GetPurchasesJsonAsync));
            return UniTask.FromResult("[]");
#endif
        }

        public UniTask ConsumePurchaseAsync(string purchaseToken)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return CallVoidAsync((gameObjectName, callbackId) =>
                YandexSDK_ConsumePurchase(purchaseToken, gameObjectName, callbackId));
#else
            LogEditorCall(nameof(ConsumePurchaseAsync), $"purchaseToken={purchaseToken}");
            return UniTask.CompletedTask;
#endif
        }

        [Preserve]
        public void HandleYandexSDKCallback(string messageJson)
        {
            LogEditorCall(nameof(HandleYandexSDKCallback), $"messageJson={messageJson}");

            var message = JsonConvert.DeserializeObject<YandexSDKCallbackMessage>(messageJson);
            if (message == null || string.IsNullOrEmpty(message.Id))
            {
                Debug.LogWarning($"Invalid Yandex SDK callback: {messageJson}");
                return;
            }

            if (!_requests.TryGetValue(message.Id, out var completionSource))
            {
                Debug.LogWarning($"Yandex SDK callback request was not found: {message.Id}");
                return;
            }

            _requests.Remove(message.Id);

            if (message.Success)
            {
                completionSource.TrySetResult(message.Payload);
            }
            else
            {
                completionSource.TrySetException(new InvalidOperationException(message.Error));
            }
        }

        private UniTask<string> CallAsync(Action<string, string> call)
        {
            LogEditorCall(nameof(CallAsync));

            var id = (++_requestId).ToString();
            var completionSource = new UniTaskCompletionSource<string>();

            _requests.Add(id, completionSource);
            call(CallbackObjectName, id);

            return completionSource.Task;
        }

        private async UniTask CallVoidAsync(Action<string, string> call)
        {
            LogEditorCall(nameof(CallVoidAsync));
            await CallAsync(call);
        }

        private static void LogEditorCall(string methodName, string details = null)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(details))
            {
                Debug.Log($"[YandexSDKProvider][Editor] {methodName}");
                return;
            }

            Debug.Log($"[YandexSDKProvider][Editor] {methodName}: {details}");
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern int YandexSDK_IsInitialized();

        [DllImport("__Internal")]
        private static extern void YandexSDK_Init(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern string YandexSDK_GetEnvironment();

        [DllImport("__Internal")]
        private static extern void YandexSDK_IsAvailableMethod(string methodName, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_LoadingReady(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GameplayStart(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GameplayStop(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_ShowFullscreenAdv(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_ShowRewardedVideo(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetBannerAdvStatus(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_ShowBannerAdv(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_HideBannerAdv(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetPlayer(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_OpenAuthDialog(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetPlayerData(string keysJson, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_SetPlayerData(string dataJson, int flush, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetPlayerStats(string keysJson, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_SetPlayerStats(string statsJson, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_IncrementPlayerStats(string statsJson, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_SetLeaderboardScore(string leaderboardName, int score, string extraData, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetLeaderboardEntries(string leaderboardName, int quantityTop, int includeUser, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_InitPayments(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetCatalog(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_Purchase(string productId, string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_GetPurchases(string gameObjectName, string callbackId);

        [DllImport("__Internal")]
        private static extern void YandexSDK_ConsumePurchase(string purchaseToken, string gameObjectName, string callbackId);
#endif
    }
}
