using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class UIWindowLayerView
    {
        public VisualElement Root { get; private set;}
        public UIWindowLayerView(VisualElement root)
        {
            Root = root;
        }
    }
}