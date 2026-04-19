using System.Collections.Generic;
using UnityEngine;

namespace DoubleB.Runtime.Runtime.Descriptions
{
    [CreateAssetMenu(fileName = "ItemSequenceDescription", menuName = "Description/ItemSequenceDescription")]
    public class ItemSequenceDescription : ScriptableObject
    {
        public int NextIndex => CurrentIndex + 1;

        public int Capacity => Positions.Count;

        [Range(1, 3)] [field: SerializeField] public int CurrentIndex { get; private set; }

        [field: SerializeField] public List<int> Positions { get; private set; }
        
        [field: SerializeField] public List<Color> Colors { get; private set; }
        
        [field: SerializeField] public ItemDescriptionCollection Items { get; private set; }
        
        public int GetViewPosition(int index)
        {
            if (index < 0 || index >= Positions.Count) return -1;
            
            return Positions[index];
        }

        public Color GetViewColor(int index)
        {
            if (index < 0 || index >= Colors.Count) return Colors[-1];
            
            return Colors[index];
        }
    }
}