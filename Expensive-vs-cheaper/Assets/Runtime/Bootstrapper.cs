using System;
using Cysharp.Threading.Tasks;
using DoubleB.Runtime.Runtime.Audio;
using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Gameplay.ItemSequence;
using DoubleB.Runtime.Runtime.Router;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using DoubleB.Runtime.Runtime.YandexSDK;
using UniRx;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string PlayerDataKeysJson = "[\"highScore\"]";

        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private AssetCollection _assetCollection;
        [SerializeField] private DescriptionCollection _descriptionCollection;

        [SerializeField] private AudioView _audioView;
        
        private GameFlowPresenter _gameFlowPresenter;
        private AudioPresenter _audioPresenter;
        
        private async void Start()
        {
            var yandexSDK = await InitializeYandexSDKAsync();
            var playerData = await LoadPlayerDataAsync(yandexSDK);
            
            var uiRouterModel = new UIRouterModel(UIConstants.Windows.MainMenu);
            var itemSequenceModel = new ItemSequenceModel(_descriptionCollection.ItemSequence);
            var gameplayModel = new GameplayModel(itemSequenceModel);
            
            
            var gameModel = new GameModel(uiRouterModel, gameplayModel, playerData);
            var gameView = new GameView(_uiDocument);
            
            var uiRouter = new UIRouterPresenter(gameModel, gameView,  _assetCollection.UIAssetCollection, _descriptionCollection);
            
            _gameFlowPresenter = new GameFlowPresenter(gameModel);
            _audioPresenter = new AudioPresenter(gameModel, _audioView, _assetCollection.AudioAssetCollection);

            
            _gameFlowPresenter.Enable();
            _audioPresenter.Enable();
            uiRouter.Enable();

            await SetYandexLoadingReadyAsync(yandexSDK);
        }

        private async UniTask<IYandexSDKProvider> InitializeYandexSDKAsync()
        {
            var yandexSDK = YandexSDKProvider.Instance;

            try
            {
                await yandexSDK.InitializeAsync();
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Yandex SDK initialization failed: {exception.Message}");
            }

            return yandexSDK;
        }

        private async UniTask<PlayerData> LoadPlayerDataAsync(IYandexSDKProvider yandexSDK)
        {
            try
            {
                var playerDataJson = await yandexSDK.GetPlayerDataJsonAsync(PlayerDataKeysJson);
                return JsonConvert.DeserializeObject<PlayerData>(playerDataJson) ?? new PlayerData();
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Player data loading failed: {exception.Message}");
                return new PlayerData();
            }
        }

        private async UniTask SetYandexLoadingReadyAsync(IYandexSDKProvider yandexSDK)
        {
            try
            {
                await yandexSDK.LoadingReadyAsync();
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Yandex SDK loading ready failed: {exception.Message}");
            }
        }
    }
}
