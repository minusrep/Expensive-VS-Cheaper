using UnityEngine.UIElements;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemChoiceView
    {
        public Button SelectMoreExpensiveButton { get; private set; }
        public Button SelectCheaperButton { get; private set; }
        
        private VisualElement _root;
        
        public ItemChoiceView(VisualElement root)
        {
            _root = root;
            
            SelectMoreExpensiveButton = root.Q<Button>(UIConstants.MoreExpensiveButton);
            SelectCheaperButton = root.Q<Button>(UIConstants.CheaperButton);
        }
    }
}