using DoubleB.Runtime.Runtime.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Gameplay
{
    public class ScorePresenter : IPresenter
    {
        private const float HighlightDuration = 0.12f;
        private const float RestoreDuration = 0.16f;
        private const float HighlightScale = 1.1f;
        private const float HighlightFontSizeBoost = 8f;

        private readonly GameplayModel _model;
        private readonly ScoreView _view;

        private readonly Color _highlightColor = new Color32(255, 221, 89, 255);

        private Color _baseColor = Color.white;
        private float _baseScale = 1f;
        private float _baseFontSize = 56f;
        private bool _hasVisualSnapshot;
        private int _lastScore;
        private bool _hasScoreSnapshot;

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

            DOTween.Kill(_view.Value);
            RestoreValueVisualState();
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
            var score = _model.Score;
            var hasScoreChanged = _hasScoreSnapshot && _lastScore != score;

            _view.Value.text = score.ToString();
            CacheValueVisualState();

            if (hasScoreChanged)
            {
                PlayValueChangeAnimation();
            }

            _lastScore = score;
            _hasScoreSnapshot = true;
        }

        private void CacheValueVisualState()
        {
            if (_hasVisualSnapshot)
            {
                return;
            }

            _baseColor = _view.Value.resolvedStyle.color;
            _baseScale = _view.Value.resolvedStyle.scale.value.x;
            _baseFontSize = _view.Value.resolvedStyle.fontSize;

            if (_baseScale <= 0f)
            {
                _baseScale = 1f;
            }

            if (_baseFontSize <= 0f)
            {
                _baseFontSize = 56f;
            }

            _hasVisualSnapshot = true;
        }

        private void PlayValueChangeAnimation()
        {
            DOTween.Kill(_view.Value);

            var currentScale = _baseScale;
            var currentColor = _baseColor;
            var currentFontSize = _baseFontSize;
            var highlightFontSize = _baseFontSize + HighlightFontSizeBoost;

            var sequence = DOTween.Sequence().SetId(_view.Value);

            sequence.Append(DOTween.To(
                    () => currentScale,
                    x =>
                    {
                        currentScale = x;
                        _view.Value.style.scale = new Scale(new Vector2(x, x));
                    },
                    HighlightScale,
                    HighlightDuration)
                .SetEase(Ease.OutQuad));

            sequence.Join(DOTween.To(
                    () => currentColor,
                    x =>
                    {
                        currentColor = x;
                        _view.Value.style.color = new StyleColor(x);
                    },
                    _highlightColor,
                    HighlightDuration)
                .SetEase(Ease.OutQuad));

            sequence.Join(DOTween.To(
                    () => currentFontSize,
                    x =>
                    {
                        currentFontSize = x;
                        _view.Value.style.fontSize = x;
                    },
                    highlightFontSize,
                    HighlightDuration)
                .SetEase(Ease.OutQuad));

            sequence.Append(DOTween.To(
                    () => currentScale,
                    x =>
                    {
                        currentScale = x;
                        _view.Value.style.scale = new Scale(new Vector2(x, x));
                    },
                    _baseScale,
                    RestoreDuration)
                .SetEase(Ease.InOutQuad));

            sequence.Join(DOTween.To(
                    () => currentColor,
                    x =>
                    {
                        currentColor = x;
                        _view.Value.style.color = new StyleColor(x);
                    },
                    _baseColor,
                    RestoreDuration)
                .SetEase(Ease.InOutQuad));

            sequence.Join(DOTween.To(
                    () => currentFontSize,
                    x =>
                    {
                        currentFontSize = x;
                        _view.Value.style.fontSize = x;
                    },
                    _baseFontSize,
                    RestoreDuration)
                .SetEase(Ease.InOutQuad));
        }

        private void RestoreValueVisualState()
        {
            if (!_hasVisualSnapshot)
            {
                return;
            }

            _view.Value.style.scale = new Scale(new Vector2(_baseScale, _baseScale));
            _view.Value.style.color = new StyleColor(_baseColor);
            _view.Value.style.fontSize = _baseFontSize;
        }
    }
}
