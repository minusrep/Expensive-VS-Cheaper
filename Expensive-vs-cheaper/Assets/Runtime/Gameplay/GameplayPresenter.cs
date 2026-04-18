using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class GameplayPresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly GameplayView _view;
        private readonly GameplayModel _model;

        private ItemSequencePresenter _itemSequencePresenter;
        private ItemChoicePresenter _itemChoicePresenter;

        public GameplayPresenter(GameplayModel model, GameplayView view, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
        }

        public void Enable()
        {
            _model.Reset();
            
            var itemSequenceView = new ItemSequenceView(_view.Root.Q<VisualElement>(UIConstants.Content));
            var itemChoiceView = new ItemChoiceView(_view.Root.Q<VisualElement>(UIConstants.Choicer));
            
            _itemSequencePresenter = new ItemSequencePresenter(_model.ItemSequence, itemSequenceView, _uiAssetCollection);
            _itemChoicePresenter = new ItemChoicePresenter(_model,  itemChoiceView);
            
            _itemSequencePresenter.Enable();
            _itemChoicePresenter.Enable();
        }

        public void Disable()
        {
            _itemSequencePresenter.Disable();
            _itemChoicePresenter.Disable();
        }
    }
}