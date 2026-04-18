using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class MainMenuPresenter : IPresenter
    {
        private readonly MainMenuView _view;

        private readonly UIWindowRouterModel _model;
        
        private Button _startSessionButton;
        
        public MainMenuPresenter(UIWindowRouterModel model, MainMenuView view)
        {
            _view = view;
            _model = model;
        }

        public void Enable()
        {
            _startSessionButton = _view.Root.Q<Button>(UIConstants.StartButton);
             
            _startSessionButton.clicked += StartSession;
        }

        public void Disable()
        {
            _startSessionButton.clicked -= StartSession;
        }

        private void StartSession()
        {
            _model.ChangeState(UIConstants.Windows.Gameplay);
        }
    }
}