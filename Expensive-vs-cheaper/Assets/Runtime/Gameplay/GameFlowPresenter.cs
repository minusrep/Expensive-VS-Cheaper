using System;
using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.YandexSDK;
using UnityEngine;

namespace DoubleB.Runtime.Runtime.Gameplay
{
    public class GameFlowPresenter : IPresenter
    {
        private readonly GameModel _model;
        private readonly IYandexSDKProvider _yandexSDK;

        public GameFlowPresenter(GameModel model, IYandexSDKProvider yandexSDK)
        {
            _model = model;
            _yandexSDK = yandexSDK;
        }

        public void Enable()
        {
            _model.GameplayModel.OnLose += HandleLose;
            _model.GameplayModel.OnScoreChange += HandleScoreChange;
            _model.UIRouterModel.OnRestartRequested += HandleRestartRequested;
            _model.UIRouterModel.OnExitToMainMenuRequested += HandleExitRequested;
            _model.UIRouterModel.OnRewardedContinueRequested += HandleContinueForRewardRequest;
        }

        public void Disable()
        {
            _model.GameplayModel.OnLose -= HandleLose;
            _model.GameplayModel.OnScoreChange -= HandleScoreChange;
            _model.UIRouterModel.OnRestartRequested -= HandleRestartRequested;
            _model.UIRouterModel.OnExitToMainMenuRequested -= HandleExitRequested;
            _model.UIRouterModel.OnRewardedContinueRequested -= HandleContinueForRewardRequest;
        }

        private void HandleLose()
        {
            _model.UIRouterModel.PopupRouterModel.Invoke(UIConstants.Popups.LoseMenu);          
        }

        private void HandleRestartRequested()
        {
            _model.UIRouterModel.PopupRouterModel.Hide();            
            _model.GameplayModel.Reset();
        }

        private void HandleExitRequested()
        {
            _model.UIRouterModel.WindowRouterModel.ChangeState(UIConstants.Windows.MainMenu);    
            _model.UIRouterModel.PopupRouterModel.Hide();
            _model.GameplayModel.Reset();
        }

        private async void HandleContinueForRewardRequest()
        {
            try
            {
                var result = await _yandexSDK.ShowRewardedVideoAsync();

                if (result is { Rewarded: true })
                {
                    _model.UIRouterModel.PopupRouterModel.Hide();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private void HandleScoreChange()
        {
            if (_model.GameplayModel.Score > _model.PlayerData.HighScore.Value)
            {
                _model.PlayerData.HighScore.Value = _model.GameplayModel.Score;
            }
        }
    }
}
