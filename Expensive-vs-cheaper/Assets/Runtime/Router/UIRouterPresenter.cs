using DoubleB.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class UIWindowRouterPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly DescriptionCollection _descriptionCollection;
        
        private readonly UIWindowRouterModel _model;
        private readonly UIWindowLayerView _view;
        private readonly GameplayModel _gameplayModel;
        
        private IPresenter _currentWindowPresenter;

        public UIWindowRouterPresenter(UIWindowRouterModel model, UIWindowLayerView view, GameplayModel gameplayModel, 
            DescriptionCollection descriptionCollection, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _gameplayModel = gameplayModel;
            _descriptionCollection = descriptionCollection;
            _uiAssetCollection = uiAssetCollection;
        }


        public void Enable()
        {
            _model.OnChangeState += HandleChangeWindow;
            
            HandleChangeWindow();
        }

        public void Disable()
        {
            _model.OnChangeState -= HandleChangeWindow;
            _currentWindowPresenter?.Disable();
            _currentWindowPresenter = null;
        }

        private void HandleChangeWindow()
        {
            var asset = _uiAssetCollection.Get(_model.CurrentState).Value;
            var root = asset.CloneTree().Q<VisualElement>(UIConstants.Root);
            
            _view.Root.Clear();
            _view.Root.Add(root);

            switch (_model.CurrentState)
            {
                case UIConstants.MainMenu:
                    _currentWindowPresenter = new MainMenuPresenter(_model, new MainMenuView(root));
                    break;
                
                case UIConstants.Gameplay:
                    _currentWindowPresenter = new GameplayPresenter(_gameplayModel, new GameplayView(root), _descriptionCollection.ItemSequence, _uiAssetCollection);
                    break;
            }
            
            _currentWindowPresenter?.Enable();
        }
    }
    
    public class UIPopupRouterPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly DescriptionCollection _descriptionCollection;
        
        private readonly UIPopupRouterModel _model;
        private readonly UIPopupLayerView _view;
        private readonly GameplayModel _gameplayModel;
        
        private IPresenter _currentWindowPresenter;

        public UIPopupRouterPresenter(UIPopupRouterModel model, UIPopupLayerView view, GameplayModel gameplayModel,
            DescriptionCollection descriptionCollection, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _gameplayModel = gameplayModel;
            _descriptionCollection = descriptionCollection;
            _uiAssetCollection = uiAssetCollection;
        }

        public void Enable()
        {
            _model.OnChangeState += HandleChangePopup;
            
            HandleChangePopup();
        }

        public void Disable()
        {
            _model.OnChangeState -= HandleChangePopup;
        }

        private void HandleChangePopup()
        {
            _view.Root.pickingMode = string.IsNullOrEmpty(_model.CurrentState) ? PickingMode.Ignore : PickingMode.Position;
        }
    }

    public class UIRouterPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly DescriptionCollection _descriptionCollection;
        private readonly GameView _view;
        private readonly GameModel _model;

        private UIWindowRouterPresenter _windowRouterPresenter;
        private UIPopupRouterPresenter _popupRouterPresenter;
        
        private IPresenter _currentPresenter;

        public UIRouterPresenter(GameModel model, GameView view, 
            UIAssetCollection uiAssetCollection, DescriptionCollection descriptionCollection)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
            _descriptionCollection = descriptionCollection;
        }

        public void Enable()
        {
            _windowRouterPresenter = new UIWindowRouterPresenter(_model.UIRouterModel.WindowRouterModel, _view.UIWindowLayerView, 
                _model.GameplayModel,
                _descriptionCollection, _uiAssetCollection);

            _popupRouterPresenter = new UIPopupRouterPresenter(_model.UIRouterModel.PopupRouterModel,
                _view.UIPopupLayerView, _model.GameplayModel, _descriptionCollection, _uiAssetCollection);
            
            _windowRouterPresenter.Enable();
            _popupRouterPresenter.Enable();
        }

        public void Disable()
        {
            _windowRouterPresenter?.Disable();
            _popupRouterPresenter?.Disable();
        }
    }
}