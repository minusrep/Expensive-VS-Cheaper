using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemPresenter : IPresenter
    {
        private readonly ItemModel _model;
        private readonly ItemView _view;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
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
            
            _model.Position.Pairwise().Subscribe(x => HandlePosition(x.Previous, x.Current)).AddTo(_disposables);
            _model.OnChangeDescription += HandleDescription;
            
            HandlePosition();
            HandleDescription();
        }

        public void Disable()
        {
            _disposables.Dispose();
            _model.OnChangeDescription -= HandleDescription;
        }

        private void HandlePosition()
        {
            _view.Root.style.left = new Length(_model.Position.Value, LengthUnit.Percent);
        }
        
        private void HandlePosition(int previousValue, int currentValue)
        {
            DOTween.Kill(_view.Root);
            
            var withAnimation = currentValue < previousValue;
            var currentColor = _view.Root.resolvedStyle.backgroundColor;
            var targetColor = _model.Color.Value;
            
            if (withAnimation)
            {
                var moveTween = DOTween.To(
                        () => _view.Root.style.left.value.value,
                        x => _view.Root.style.left = new Length(x, LengthUnit.Percent),
                        currentValue,
                        0.25f)
                    .SetEase(Ease.Linear);

                var colorTween = DOTween.To(
                    () => currentColor,
                    x =>
                    {
                        currentColor = x;
                        _view.Root.style.backgroundColor = new StyleColor(x);
                    },
                    targetColor,
                    0.25f
                ).SetEase(Ease.Linear);

                DOTween.Sequence().SetId(_view.Root).Append(moveTween).Join(colorTween);
            }
            else
            {
                _view.Root.style.left = new Length(currentValue, LengthUnit.Percent);
                _view.Root.style.backgroundColor = new StyleColor(targetColor);
            }
        }

        private void HandleDescription()
        {
            _view.Title.text = _model.Description.Title;
            _view.Icon.style.backgroundImage = new StyleBackground(_model.Description.Icon);
            _view.Worth.text = _model.Description.Worth.ToString("$0");
        }
    }
}