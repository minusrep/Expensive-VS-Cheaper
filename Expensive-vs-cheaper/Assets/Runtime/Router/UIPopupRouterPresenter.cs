using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.LoseMenu;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Router
{
    public class UIPopupRouterPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly DescriptionCollection _descriptionCollection;
        
        private readonly GameModel _model;
        private readonly UIPopupLayerView _view;
        
        private IPresenter _currentPopupPresenter;

        public UIPopupRouterPresenter(GameModel model, UIPopupLayerView view,
            DescriptionCollection descriptionCollection, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _descriptionCollection = descriptionCollection;
            _uiAssetCollection = uiAssetCollection;
        }

        public void Enable()
        {
            _model.UIRouterModel.PopupRouterModel.OnChangeState += HandleChangePopup;
            
            HandleChangePopup();
        }

        public void Disable()
        {
            _model.UIRouterModel.PopupRouterModel.OnChangeState -= HandleChangePopup;
        }

        private void HandleChangePopup()
        {
            _currentPopupPresenter?.Disable();
            
            _view.Root.pickingMode = string.IsNullOrEmpty(_model.UIRouterModel.PopupRouterModel.CurrentState) ? PickingMode.Ignore : PickingMode.Position;
            _view.Root.style.display = string.IsNullOrEmpty(_model.UIRouterModel.PopupRouterModel.CurrentState) ? DisplayStyle.None : DisplayStyle.Flex;

            if (string.IsNullOrEmpty(_model.UIRouterModel.PopupRouterModel.CurrentState))
            {
                return;
            }
            
            var asset =  _uiAssetCollection.Get(_model.UIRouterModel.PopupRouterModel.CurrentState).Value;
            var root = asset.CloneTree().Q<VisualElement>(UIConstants.Root);
            
            _view.Root.Clear();
            _view.Root.Add(root);

            switch (_model.UIRouterModel.PopupRouterModel.CurrentState)
            {
                case  UIConstants.Popups.LoseMenu:
                    _currentPopupPresenter = new LoseMenuPresenter(_model.UIRouterModel, new LoseMenuView(root));
                    break;
            }
            
            _currentPopupPresenter?.Enable();
        }
    }
}