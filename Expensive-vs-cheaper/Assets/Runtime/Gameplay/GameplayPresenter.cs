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
            else
            {
                _model.Lose();
            }
        }

        private void SelectCheaper()
        {
            var success = _model.ItemSequence.CurrentItem.Description.Worth >= _model.ItemSequence.NextItem.Description.Worth;

            if (success)
            {
                _model.ItemSequence.Next();
            }
            else
            {
                _model.Lose();
            }
        }
    }
}