using DoubleB.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class UIRouterPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly ItemSequenceDescription _itemSequenceDescription;
        private readonly GameView _view;
        private readonly GameModel _model;

        private IPresenter _currentPresenter;
        
        public UIRouterPresenter(GameModel model, GameView view, 
            UIAssetCollection uiAssetCollection, ItemSequenceDescription itemSequenceDescription)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
            _itemSequenceDescription = itemSequenceDescription;
        }

        public void Enable()
        {
            Build();
            
            _model.UIRouterModel.OnChangeState += Build;
        }

        public void Disable()
        {
            _model.UIRouterModel.OnChangeState -= Build;
        }

        private void Build()
        {
            _currentPresenter?.Disable();
            
            var asset = _uiAssetCollection.Get(_model.UIRouterModel.CurrentState).Value;
            var root = asset.CloneTree().Q<VisualElement>(UIConstants.Root);
            
            _view.Root.Clear();
            _view.Root.Add(root);

            switch (_model.UIRouterModel.CurrentState)
            {
                case UIConstants.MainMenu:
                    _currentPresenter = new MainMenuPresenter(_model, new MainMenuView(root));
                    break;
                
                case UIConstants.Gameplay:
                    _currentPresenter = new GameplayPresenter(_model.GameplayModel, new GameplayView(root), _itemSequenceDescription, _uiAssetCollection);
                    break;
            }
            
            _currentPresenter?.Enable();
        }
    }
}