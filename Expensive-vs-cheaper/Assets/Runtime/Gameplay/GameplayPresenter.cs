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
        private ScorePresenter _scorePresenter;
        
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
            var scoreView = new ScoreView(_view.Root.Q<VisualElement>(UIConstants.ScorePanel));
            
            _itemSequencePresenter = new ItemSequencePresenter(_model.ItemSequence, itemSequenceView, _uiAssetCollection);
            _itemChoicePresenter = new ItemChoicePresenter(_model,  itemChoiceView);
            _scorePresenter = new ScorePresenter(_model, scoreView);
            
            _itemSequencePresenter.Enable();
            _itemChoicePresenter.Enable();
            _scorePresenter.Enable();
            
            _model.OnSelect += HandleChoice;
        }

        public void Disable()
        {
            _itemSequencePresenter.Disable();
            _itemChoicePresenter.Disable();
            _scorePresenter.Disable();
            
            _model.OnSelect -= HandleChoice;
        }

        private async void HandleChoice(ItemChoice choice)
        {
            var success = IsCorrect(choice);

            _model.RegisterResult(success ? ItemChoiceResult.Success : ItemChoiceResult.Fail);
            
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

    public class ScorePresenter : IPresenter
    {
        private readonly GameplayModel _model;
        private readonly ScoreView _view;

        public ScorePresenter(GameplayModel model, ScoreView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _model.OnScoreChange += HandleScore;
            _model.OnGetResult += HandleGetResult;
            
            HandleScore();
        }

        public void Disable()
        {
            _model.OnScoreChange -= HandleScore;
            _model.OnGetResult -= HandleGetResult;
        }

        
        private void HandleGetResult(ItemChoiceResult choiceResult)
        {
            if (choiceResult == ItemChoiceResult.Success)
            {
                _model.AddScore();
            }            
        }

        private void HandleScore()
        {
            _view.Value.text = _model.Score.ToString();            
        }
    }
    
    public class ScoreView
    {
        public VisualElement Root { get; }
        public TextElement Value { get; }
        
        public ScoreView(VisualElement root)
        {
            Root = root;
            Value = root.Q<TextElement>(UIConstants.ScoreValue);
        }
    }
}
