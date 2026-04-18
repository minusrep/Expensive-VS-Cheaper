using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.ViewDescriptions;

namespace DoubleB.Runtime.Runtime.Router
{
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
            _windowRouterPresenter = new UIWindowRouterPresenter(_model, _view.UIWindowLayerView, _uiAssetCollection);

            _popupRouterPresenter = new UIPopupRouterPresenter(_model,
                _view.UIPopupLayerView, _descriptionCollection, _uiAssetCollection);
            
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