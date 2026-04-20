using Cysharp.Threading.Tasks;
using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.YandexSDK;

namespace DoubleB.Runtime.Runtime.Advertisement
{
    public class AdvertisementPresenter : IPresenter
    {
        private readonly GameModel _model;
        private readonly IYandexSDKProvider _yandexSDK;

        public AdvertisementPresenter(GameModel model, IYandexSDKProvider yandexSDK)
        {
            _model = model;
            _yandexSDK = yandexSDK;
        }

        public void Enable()
        {
            _model.GameplayModel.OnLose += ShowFullscreenAdv;
        }

        public void Disable()
        {
            _model.GameplayModel.OnLose += ShowFullscreenAdv;
        }

        private void ShowFullscreenAdv()
        {
            _yandexSDK.ShowFullscreenAdAsync().Forget();
        }
    }
}