using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class MainMenuPresenter : IPresenter
    {
        private readonly MainMenuView _view;

        private readonly UIWindowRouterModel _model;
        
        public MainMenuPresenter(UIWindowRouterModel model, MainMenuView view)
        {
            _view = view;
            _model = model;
        }

        public void Enable()
        {
            _view.Root.Q<Button>(UIConstants.StartButton).clicked += StartSession;
        }

        public void Disable()
        {
            _view.Root.Q<Button>(UIConstants.StartButton).clicked -= StartSession;
        }

        private void StartSession()
        {
            _model.ChangeState(UIConstants.Gameplay);
        }
    }
}