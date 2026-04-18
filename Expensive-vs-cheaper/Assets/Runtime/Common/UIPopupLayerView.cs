using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Common
{
    public class UIPopupLayerView
    {
        public VisualElement Root { get; private set;}
        public UIPopupLayerView(VisualElement root)
        {
            Root = root;
        }
    }
}