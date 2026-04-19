using DoubleB.Runtime.Runtime.Constants;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Gameplay
{
    public class GameplayView
    {
        public VisualElement Root { get; private set; }
        public Button MoreExpensiveButton { get; private set; }
        public Button CheaperButton { get; private set; }
        
        public GameplayView(VisualElement root)
        {
            Root = root;
            MoreExpensiveButton = root.Q<Button>(UIConstants.MoreExpensiveButton);
            CheaperButton = root.Q<Button>(UIConstants.CheaperButton);
        }
    }
}