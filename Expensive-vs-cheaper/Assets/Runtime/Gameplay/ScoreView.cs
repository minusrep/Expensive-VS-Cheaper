using DoubleB.Runtime.Runtime.Constants;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Gameplay
{
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