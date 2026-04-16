using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemSequencePresenter : IPresenter
    {
        private const int SequenceCount = 6;
        private const int CurrentIndex = 2;
        
        private readonly ItemView[] _itemViews = new ItemView[SequenceCount];
        private readonly int[] _itemPositions = new int[SequenceCount] { -100, -50, 0, 50, 100, 150 };

        private readonly UIAssetCollection _uiAssetCollection;
        private readonly ItemSequenceView _view;
        private readonly ItemSequenceModel _model;

        public ItemSequencePresenter(ItemSequenceModel model, ItemSequenceView view, UIAssetCollection uiAssetCollection)
        {
            _model = model;
            _view = view;
            _uiAssetCollection = uiAssetCollection;
        }

        public void Enable()
        {
            var asset = _uiAssetCollection.Get(UIConstants.Item).Value;
            
            for (var i = 0; i < _itemViews.Length; i++)
            {
                var itemElement = asset.CloneTree().Q<VisualElement>(UIConstants.Item);
                
                _itemViews[i] = new ItemView(itemElement);
                
                AnimateToPosition(_itemViews[i].Root, _itemPositions[i]);
                
                _view.Root.Add(itemElement);
            }

            SetupItems();
            
            _model.OnChange += Shift;
        }

        public void Disable()
        {
            _model.OnChange -= Shift;
        }

        private void SetupItems()
        {
            SetupCurrentItem(_itemViews[CurrentIndex], _model.CurrentItem);
            SetupNextItem(_itemViews[CurrentIndex + 1], _model.NextItem);
        }

        private void SetupCurrentItem(ItemView itemView, ItemDescription itemDescription)
        {
            itemView.Title.text = itemDescription.Title;
            itemView.Icon.style.backgroundImage = new StyleBackground(itemDescription.Icon);
            itemView.Worth.text = itemDescription.Worth.ToString("$0");
        }

        private void SetupNextItem(ItemView itemView, ItemDescription itemDescription)
        {
            itemView.Title.text = itemDescription.Title;
            itemView.Icon.style.backgroundImage = new StyleBackground(itemDescription.Icon);
            itemView.Worth.text = string.Empty;
        }

        private void Shift()
        {
            var first = _itemViews[0];
            
            for (var i = 0; i < _itemViews.Length - 1; i++)
            {
                _itemViews[i] = _itemViews[i + 1];
            }

            for (var i = 0; i < _itemViews.Length; i++)
            {
                AnimateToPosition(_itemViews[i].Root, _itemPositions[i]);
            }
            
            _itemViews[^1] = first;
        
            SetupItems();
        }

        private void AnimateToPosition(VisualElement element, float targetPercent)
        {
            element.style.left = new Length(targetPercent, LengthUnit.Percent);
        }
    }
}