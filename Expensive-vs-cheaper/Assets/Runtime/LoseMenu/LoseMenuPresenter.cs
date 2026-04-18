using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class LoseMenuPresenter : IPresenter
    {
        private readonly UIRouterModel _model;
        private readonly LoseMenuView _view;

        private Button _openMainMenuButton;
        private Button _continueGameplayButton;
        
        public LoseMenuPresenter(UIRouterModel model, LoseMenuView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _openMainMenuButton = _view.Root.Q<Button>(UIConstants.ExitButton);
            _continueGameplayButton = _view.Root.Q<Button>(UIConstants.RetryButton);
                
            _openMainMenuButton.clicked += ExitButton;
            _continueGameplayButton.clicked += RestartButton;
        }

        public void Disable()
        {
            _openMainMenuButton.clicked -= ExitButton;
            _continueGameplayButton.clicked -= RestartButton;
        }

        private void ExitButton()
        {
            _model.RequestExitToMainMenu();
        }

        private void RestartButton()
        {
            _model.RequestRestart();
        }
    }
}