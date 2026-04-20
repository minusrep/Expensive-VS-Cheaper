using Cysharp.Threading.Tasks;
using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.YandexSDK;
using UniRx;
using Unity.Plastic.Newtonsoft.Json;

namespace DoubleB.Runtime.Runtime.Save
{
    public class SavePresenter : IPresenter
    {
        private readonly GameModel _model;
        private readonly IYandexSDKProvider _yandexSDKProvider;

        private CompositeDisposable _disposables = new CompositeDisposable();
        
        public SavePresenter(GameModel model, IYandexSDKProvider yandexSDKProvider)
        {
            _model = model;
            _yandexSDKProvider = yandexSDKProvider;
        }

        public void Enable()
        {
            _model.PlayerData.HighScore
                .SkipLatestValueOnSubscribe()
                .Subscribe(_ => SaveData())
                .AddTo(_disposables);
        }

        public void Disable()
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        private void SaveData()
        {
            var playerDataJson = JsonConvert.SerializeObject(_model.PlayerData);
            _yandexSDKProvider.SetPlayerDataAsync(playerDataJson).Forget();
        }
    }
}
