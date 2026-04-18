using UnityEngine;
using UnityEngine.Serialization;

namespace DoubleB.Runtime.Runtime.Descriptions
{
    [CreateAssetMenu(fileName = "DescriptionCollection", menuName = "Description/DescriptionCollection")]    
    public class DescriptionCollection : ScriptableObject
    {
        [field: SerializeField] public ItemSequenceDescription ItemSequence { get; private set; }
    }
}