using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemPresenter : IPresenter
    {
        private readonly ItemModel _model;
        private readonly ItemView _view;
        
        public ItemPresenter(ItemModel model, ItemView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _view.Title.text = _model.Description.Title;
            _view.Icon.style.backgroundImage = new StyleBackground(_model.Description.Icon);
            _view.Worth.text = _model.Description.Worth.ToString("$0");
            
            _model.OnChangePosition += HandlePosition;
            _model.OnChangeDescription += HandleDescription;
            
            HandlePosition();
            HandleDescription();
        }

        public void Disable()
        {
            _model.OnChangePosition -= HandlePosition;
            _model.OnChangeDescription -= HandleDescription;
        }

        private void HandlePosition()
        {
            _view.Root.style.left = new Length(_model.Position, LengthUnit.Percent);
        }

        private void HandleDescription()
        {
            _view.Title.text = _model.Description.Title;
            _view.Icon.style.backgroundImage = new StyleBackground(_model.Description.Icon);
            _view.Worth.text = _model.Description.Worth.ToString("$0");
        }
    }
}