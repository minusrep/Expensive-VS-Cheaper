using System.Collections.Generic;
using DoubleB.Runtime.Runtime.Descriptions;
using UnityEngine;
using UnityEngine.Serialization;

namespace DoubleB.Runtime.Gameplay
{
    [CreateAssetMenu(fileName = "ItemSequenceDescription", menuName = "Description/ItemSequenceDescription")]
    public class ItemSequenceDescription : ScriptableObject
    {
        public int NextIndex => CurrentIndex + 1;

        public int Capacity => Positions.Count;

        [Range(1, 3)] [field: SerializeField] public int CurrentIndex { get; private set; }

        [field: SerializeField] public List<int> Positions { get;  private set; }
        
        [field: SerializeField] public ItemDescriptionCollection Items { get; private set; }
        
        public int GetViewPosition(int position)
        {
            if (position < 0 || position >= Positions.Count) return -1;
            
            return Positions[position];
        } 
    }
}