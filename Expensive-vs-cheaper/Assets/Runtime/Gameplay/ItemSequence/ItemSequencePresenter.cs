using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemSequencePresenter : IPresenter
    {
        private readonly UIAssetCollection _uiAssetCollection;
        
        private readonly ItemSequenceView _view;
        private readonly ItemSequenceModel _model;
        
        private readonly ItemPresenter[] _itemPresenters;
        
        public ItemSequencePresenter(ItemSequenceModel model, ItemSequenceView view, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
            _itemPresenters = new ItemPresenter[_model.Description.Capacity];
        }

        public void Enable()
        {
            var asset = _uiAssetCollection.Get(UIConstants.Item).Value;
            
            for (var i = 0; i < _itemPresenters.Length; i++)
            {
                var element = asset.CloneTree().Q<VisualElement>(UIConstants.Item);
                var model = new ItemModel(_model.Description.Items.GetRandom());
                var view = new ItemView(element);
                var presenter = new ItemPresenter(model, view);
                
                _view.Root.Add(element);
                
                _itemPresenters[i] = presenter;
                _model.Items[i] = model;
                _model.Items[i].Position.Value = _model.Description.GetViewPosition(i);
                _model.Items[i].Color.Value = _model.Description.GetViewColor(i);
                presenter.Enable();
            }
        }

        public void Disable()
        {
            foreach (var presenter in _itemPresenters)
            {
                presenter.Disable();
            }
        }
    }
}