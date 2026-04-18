using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Common
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