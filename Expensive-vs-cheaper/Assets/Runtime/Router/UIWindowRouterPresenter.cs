using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.MainMenu;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Router
{
    public class UIWindowRouterPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        
        private readonly GameModel _model;
        private readonly UIWindowLayerView _view;
        
        private IPresenter _currentWindowPresenter;

        public UIWindowRouterPresenter(GameModel model, UIWindowLayerView view, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
        }

        public void Enable()
        {
            _model.UIRouterModel.WindowRouterModel.OnChangeState += HandleChangeWindow;
            
            HandleChangeWindow();
        }

        public void Disable()
        {
            _model.UIRouterModel.WindowRouterModel.OnChangeState -= HandleChangeWindow;
            _currentWindowPresenter?.Disable();
            _currentWindowPresenter = null;
        }

        private void HandleChangeWindow()
        {
            _currentWindowPresenter?.Disable();
            
            var asset = _uiAssetCollection.Get(_model.UIRouterModel.WindowRouterModel.CurrentState).Value;
            var root = asset.CloneTree().Q<VisualElement>(UIConstants.Root);
            
            _view.Root.Clear();
            _view.Root.Add(root);

            switch (_model.UIRouterModel.WindowRouterModel.CurrentState)
            {
                case UIConstants.Windows.MainMenu:
                    _currentWindowPresenter = new MainMenuPresenter(_model.UIRouterModel.WindowRouterModel, new MainMenuView(root));
                    break;
                
                case UIConstants.Windows.Gameplay:
                    _currentWindowPresenter = new GameplayPresenter(_model.GameplayModel, new GameplayView(root), _uiAssetCollection);
                    break;
            }
            
            _currentWindowPresenter?.Enable();
        }
    }
}