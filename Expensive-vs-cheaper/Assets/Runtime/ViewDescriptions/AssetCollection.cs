using UnityEngine;

namespace DoubleB.Runtime.Runtime.ViewDescriptions
{
    [CreateAssetMenu(fileName = "AssetCollection", menuName = "Asset/AssetCollection")]
    public class AssetCollection : ScriptableObject
    {
        [field: SerializeField] public UIAssetCollection UIAssetCollection { get; private set;}
        
        [field: SerializeField] public AudioAssetCollection AudioAssetCollection { get; private set;}
    }
}