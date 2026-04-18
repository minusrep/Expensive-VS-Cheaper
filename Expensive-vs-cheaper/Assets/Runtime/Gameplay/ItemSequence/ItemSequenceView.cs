using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Gameplay.ItemSequence
{
    public class ItemSequenceView
    {
        public VisualElement Root { get; private set; }

        public ItemSequenceView(VisualElement root)
        {
            Root = root;
        }
    }
}