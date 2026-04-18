using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.Gameplay.ItemChoicer;
using DoubleB.Runtime.Runtime.Gameplay.ItemSequence;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Gameplay
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

            _model.OnSelected += HandleChoice;
        }

        public void Disable()
        {
            _itemSequencePresenter.Disable();
            _itemChoicePresenter.Disable();
            
            _model.OnSelected -= HandleChoice;
        }

        private async void HandleChoice(ItemChoice choice)
        {
            var success = IsCorrect(choice);

            if (!success)
            {
                _model.Lose();
                return;
            }

            _model.LockInteraction();

            await _itemChoicePresenter.HideAsync();
            await _itemSequencePresenter.NextAsync();
            await _itemChoicePresenter.ShowAsync();

            _model.UnlockInteraction();
        }

        private bool IsCorrect(ItemChoice choice)
        {
            var currentWorth = _model.ItemSequence.CurrentItem.Description.Worth;
            var nextWorth = _model.ItemSequence.NextItem.Description.Worth;

            return choice switch
            {
                ItemChoice.MoreExpensive => currentWorth <= nextWorth,
                ItemChoice.Cheaper => currentWorth >= nextWorth,
                _ => false
            };
        }
    }
}
