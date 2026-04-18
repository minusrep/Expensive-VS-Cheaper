using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime.Descriptions
{
    [CreateAssetMenu(fileName = "Item", menuName = "Description/Item")]
    public class ItemDescription : ScriptableObject
    {
        public string Id;
        public string Title;
        public ulong Worth;
        public Sprite Icon;
    }
}