using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.MainMenu
{
    public class MainMenuView
    {
        public VisualElement Root { get; private set; }
        
        public MainMenuView(VisualElement root)
        {
            Root = root;
        }
    }
}