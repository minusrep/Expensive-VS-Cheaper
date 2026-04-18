using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemChoicePresenter : IPresenter
    {
        private const float ButtonAnimationDuration = 0.12f;
        private const float ButtonAnimationInterval = 0.06f;

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

        public async UniTask HideAsync()
        {
            await AnimateAsync(_view.SelectMoreExpensiveButton, false);
            await UniTask.Delay(System.TimeSpan.FromSeconds(ButtonAnimationInterval));
            await AnimateAsync(_view.SelectCheaperButton, false);
        }

        public async UniTask ShowAsync()
        {
            await AnimateAsync(_view.SelectMoreExpensiveButton, true);
            await UniTask.Delay(System.TimeSpan.FromSeconds(ButtonAnimationInterval));
            await AnimateAsync(_view.SelectCheaperButton, true);
        }
        
        private void SelectMoreExpensive()
        {
            _model.SelectMoreExpensive();
        }
        
        private void SelectCheaper()
        {
            _model.SelectCheaper();
        }

        private static UniTask AnimateAsync(VisualElement element, bool visible)
        {
            DOTween.Kill(element);

            var targetOpacity = visible ? 1f : 0f;
            var targetScale = visible ? 1f : 0.9f;
            var targetTranslateY = visible ? 0f : 16f;

            var sequence = DOTween.Sequence().SetId(element);

            sequence.Join(DOTween.To(
                () => element.resolvedStyle.opacity,
                x => element.style.opacity = x,
                targetOpacity,
                ButtonAnimationDuration));

            sequence.Join(DOTween.To(
                () => element.resolvedStyle.scale.value.x,
                x => element.style.scale = new Scale(new UnityEngine.Vector2(x, x)),
                targetScale,
                ButtonAnimationDuration));

            sequence.Join(DOTween.To(
                () => element.resolvedStyle.translate.y,
                x => element.style.translate = new Translate(0, x, 0),
                targetTranslateY,
                ButtonAnimationDuration));

            return sequence.SetEase(Ease.OutQuad).ToUniTask();
        }
    }
}
