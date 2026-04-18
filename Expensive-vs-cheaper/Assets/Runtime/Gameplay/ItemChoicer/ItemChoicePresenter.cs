namespace DoubleB.Runtime.Gameplay
{
    public class ItemChoicePresenter : IPresenter
    {
        private readonly GameplayModel _model;
        private readonly ItemChoiceView _view;

        public ItemChoicePresenter(GameplayModel model, ItemChoiceView view)
        {
            _view = view;
            _model = model;
        }

        public void Enable()
        {
            _model.OnInteractionChanged += OnInteractionChanged;
            _view.SelectMoreExpensiveButton.clicked += SelectMoreExpensive;
            _view.SelectCheaperButton.clicked += SelectCheaper;
            
            _model.UnlockInteraction();
            
            OnInteractionChanged(_model.CanInteract);
        }

        public void Disable()
        {
            _model.OnInteractionChanged -= OnInteractionChanged;
            _view.SelectMoreExpensiveButton.clicked -= SelectMoreExpensive;
            _view.SelectCheaperButton.clicked -= SelectCheaper;
        }

        private void OnInteractionChanged(bool newValue)
        {
            _view.SelectCheaperButton.SetEnabled(newValue);
            _view.SelectMoreExpensiveButton.SetEnabled(newValue);
        }
        
        private void SelectMoreExpensive()
        {
            _model.SelectMoreExpensive();
        }
        
        private void SelectCheaper()
        {
            _model.SelectCheaper();
        }
    }
}