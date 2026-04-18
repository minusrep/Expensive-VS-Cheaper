using DoubleB.Runtime.Runtime.Constants;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Gameplay.ItemSequence
{
    public class ItemView
    {
        public VisualElement Root { get; private set; }
        
        public TextElement Title { get; private set; }
        
        public TextElement Worth { get; private set; }
        
        public VisualElement Icon { get; private set; }
        
        public ItemView(VisualElement root)
        {
            Root = root;
            
            Title = root.Q<TextElement>(UIConstants.Title);
            Worth = root.Q<TextElement>(UIConstants.Worth);
            Icon = root.Q<VisualElement>(UIConstants.Icon);
        }
    }
}