using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class GameplayPresenter : IPresenter
    {
        private readonly ItemSequenceDescription _itemSequenceDescription;
        
        private readonly UIAssetCollection _uiAssetCollection;
        private readonly GameplayView _view;
        private GameplayModel _model;

        private ItemSequencePresenter _itemSequencePresenter;

        public GameplayPresenter(GameplayModel model, GameplayView view, ItemSequenceDescription itemSequenceDescription, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
            _itemSequenceDescription = itemSequenceDescription;
        }

        public void Enable()
        {
            var itemSequence = new ItemSequenceModel(_itemSequenceDescription);
            _model = new GameplayModel(itemSequence);

            var itemSequenceView = new ItemSequenceView(_view.Root.Q<VisualElement>(UIConstants.Content));
            _itemSequencePresenter = new ItemSequencePresenter(_model.ItemSequence, itemSequenceView, _uiAssetCollection);
            _itemSequencePresenter.Enable();
            
            _view.MoreExpensiveButton.clicked += SelectMoreExpensive;
            _view.CheaperButton.clicked += SelectCheaper;
        }

        public void Disable()
        {
            _view.MoreExpensiveButton.clicked -= SelectMoreExpensive;
            _view.CheaperButton.clicked -= SelectCheaper;
        }

        private void SelectMoreExpensive()
        {
            var success = _model.ItemSequence.CurrentItem.Description.Worth <= _model.ItemSequence.NextItem.Description.Worth;

            if (success)
            {
                _model.ItemSequence.Next();
            }
        }

        private void SelectCheaper()
        {
            var success = _model.ItemSequence.CurrentItem.Description.Worth >= _model.ItemSequence.NextItem.Description.Worth;

            if (success)
            {
                _model.ItemSequence.Next();
            }
        }
    }
}