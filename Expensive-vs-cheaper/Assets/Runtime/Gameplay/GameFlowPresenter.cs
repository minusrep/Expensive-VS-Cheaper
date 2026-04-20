using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using UnityEngine;

namespace DoubleB.Runtime.Runtime.Gameplay
{
    public class GameFlowPresenter : IPresenter
    {
        private readonly GameModel _model;

        public GameFlowPresenter(GameModel model)
        {
            _model = model;
        }

        public void Enable()
        {
            _model.GameplayModel.OnGetResult += HandleChoiceResult;
            _model.GameplayModel.OnLose += HandleLose;
            _model.GameplayModel.OnScoreChange += HandleScoreChange;
            _model.UIRouterModel.OnRestartRequested += HandleRestartRequested;
            _model.UIRouterModel.OnExitToMainMenuRequested += HandleExitRequested;
        }

        public void Disable()
        {
            _model.GameplayModel.OnGetResult -= HandleChoiceResult;
            _model.GameplayModel.OnLose -= HandleLose;
            _model.GameplayModel.OnScoreChange -= HandleScoreChange;
            _model.UIRouterModel.OnRestartRequested -= HandleRestartRequested;
            _model.UIRouterModel.OnExitToMainMenuRequested -= HandleExitRequested;
        }

        private void HandleLose()
        {
            _model.UIRouterModel.PopupRouterModel.Invoke(UIConstants.Popups.LoseMenu);          
        }

        private void HandleRestartRequested()
        {
            _model.UIRouterModel.PopupRouterModel.Hide();            
        }

        private void HandleExitRequested()
        {
            _model.UIRouterModel.WindowRouterModel.ChangeState(UIConstants.Windows.MainMenu);    
            _model.UIRouterModel.PopupRouterModel.Hide();
        }

        private void HandleScoreChange()
        {
            if (_model.GameplayModel.Score > _model.PlayerData.HighScore.Value)
            {
                _model.PlayerData.HighScore.Value = _model.GameplayModel.Score;
            }
        }

        private void HandleChoiceResult(ItemChoiceResult choiceResult)
        {
            if (choiceResult == ItemChoiceResult.Success)
            {
                _model.GameplayModel.AddScore();
            }

            Debug.Log($"HighScore: {_model.PlayerData.HighScore} | Score: {_model.GameplayModel.Score}");
        }
    }
}
