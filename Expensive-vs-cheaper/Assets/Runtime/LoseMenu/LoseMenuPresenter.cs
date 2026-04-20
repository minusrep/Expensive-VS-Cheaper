using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.Router;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.LoseMenu
{
    public class LoseMenuPresenter : IPresenter
    {
        private readonly UIRouterModel _model;
        private readonly LoseMenuView _view;

        private Button _continueForAdvButton;
        private Button _exitToMainMenuButton;
        private Button _restartButton;
        
        public LoseMenuPresenter(UIRouterModel model, LoseMenuView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _exitToMainMenuButton = _view.Root.Q<Button>(UIConstants.ExitButton);
            _restartButton = _view.Root.Q<Button>(UIConstants.RetryButton);
            _continueForAdvButton = _view.Root.Q<Button>(UIConstants.ContinueButton);
                
            _exitToMainMenuButton.clicked += ExitButton;
            _restartButton.clicked += RestartButton;
            _continueForAdvButton.clicked += ContinueButton;
        }

        public void Disable()
        {
            _exitToMainMenuButton.clicked -= ExitButton;
            _restartButton.clicked -= RestartButton;
            _continueForAdvButton.clicked -= ContinueButton;
        }

        private void ExitButton()
        {
            _model.RequestExitToMainMenu();
        }

        private void RestartButton()
        {
            _model.RequestRestart();
        }

        private void ContinueButton()
        {
            _model.RequestRewardedContinue();
        }
    }
}