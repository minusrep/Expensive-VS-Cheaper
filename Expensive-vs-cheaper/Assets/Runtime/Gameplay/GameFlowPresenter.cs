using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;

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
            _model.GameplayModel.OnLose += HandleLose;
            _model.UIRouterModel.OnRestartRequested += HandleRestartRequested;
            _model.UIRouterModel.OnExitToMainMenuRequested += HandleExitRequested;
        }

        public void Disable()
        {
            _model.GameplayModel.OnLose -= HandleLose;
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
    }
}